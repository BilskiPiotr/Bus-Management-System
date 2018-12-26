using System;
using System.Data;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Bus_Management_System
{
    public partial class Bus : System.Web.UI.Page
    {
        public static BusinessLayer bl = new BusinessLayer();

        protected void Page_Load(object sender, EventArgs e)
        {
            string userId = "";
            if (Session["Name"] == null)
                Response.Redirect("global.aspx");
            else
                userId = (string)Session["Name"]; 

            if (Session["OperationStatus"] != null)
                SetButtonsStatus();

            if (!IsPostBack)
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "przeliczOdleglosc", "getLocation();", true);

                MenuItemCollection menuItems = busMenu.Items;

                if (Session["Name"] != null)
                {
                    lb_loggedUser.Text = "";
                    lb_loggedUser.Text += (string)Session["FirstName"] + " " + (string)Session["LastName"] + "       ID: " + (string)Session["Name"];

                    // załadowanie listy dostępnych pojazdów do listy
                    BindBusDDL();

                    // przygotowanie wiersza nagłówkowego do pliku raportu
                    Reporting rp = new Reporting();
                    rp.PrepareLogFile(/*loggedUser, */DateTime.Now.ToString("yyyyMMdd_HH_mm_ss"));
                    rp.Dispose();
                    CheckOperations();
                }
            }
        }

        // pętla odświeżająca Update Panel
        protected void BusHomeTimer_Tick(object sender, EventArgs e)
        {
            DataSet ds = new DataSet();
            DataManipulate dm = new DataManipulate();

            // pomiar czasu wykonywania timera
            int start, stop;
            start = Environment.TickCount & Int32.MaxValue;

            // sprawdzenie, czy użytkownik jest poprawnie zalogowany
            if (Session["Name"] != null)
            {
                // zerowanie potęcjalnego komunikatu głosowego
                Session["Alert"] = 0;
                UpdateGPSData();

                // jeśli nie ma zlecenia
                if ((int)Session["OperationStatus"] == 0)
                {
                    // sprawdzenie, czy pojawiła się operacja
                    bl.GetOperations((string)Session["Bus"], ref ds);

                    // jeśli pojawiło sie zlecenie
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        dm.GetPPSData(ds.Tables[0].Rows[0].Field<int>("PPS"));
                        dm.GetGateData(ds.Tables[0].Rows[0].Field<int>("Gate"));
                        dm.SetOperationStatus(ds);
                        SetButtonsStatus();
                        dm.GetOperationData(ds);
                        InWorkBusControls((int)Session["OperationStatus"]);
                    }
                    else
                    {
                        InWorkBusControls((int)Session["OperationStatus"]);
                    }
                }
                else
                {
                    Session["Interval"] = (int)Session["Interval"] + 1;
                    // żeby nie zapychać łącza, odświeżanie danych co 20s
                    if ((int)Session["Interval"] == 10)
                    {
                        bl.GetOperations((string)Session["Bus"], ref ds);
                        dm.GetPPSData(ds.Tables[0].Rows[0].Field<int>("PPS"));
                        dm.GetGateData(ds.Tables[0].Rows[0].Field<int>("Gate"));
                        dm.GetOperationData(ds);
                        dm.SetOperationStatus(ds);
                        SetButtonsStatus();
                        InWorkBusControls((int)Session["OperationStatus"]);
                        Session["Interval"] = 0;
                    }
                    else
                    {
                        InWorkBusControls((int)Session["OperationStatus"]);
                    }
                }
            }
            // "buss" cookie nie istnieje, wiedz na wszelki wypadek koniec sesji i wylogowanie
            else
            {
                bl.UserLogOut((int)Session["Id"], (string)Session["Bus"]);
                Session.Abandon();
                Response.Redirect("global.aspx");
            }

            // obsługa zmiennych testowych
            stop = Environment.TickCount & Int32.MaxValue;
            Session["LoopTime"] = stop - start;
            Reporting rp = new Reporting();
            rp.SaveUserFieldsValues();
            ds.Dispose();
            rp.Dispose();
            dm.Dispose();
        }

        // naniesienie aktualnych danych lokalizacyjnych
        public void UpdateGPSData()
        {
            if (HiddenField1.Value != "")
                Session["CurrentLat"] = double.Parse(HiddenField1.Value, CultureInfo.InvariantCulture);
            else
                Session["CurrentLat"] = 0.0d;
            if (HiddenField2.Value != "")
                Session["CurrentLon"] = double.Parse(HiddenField2.Value, CultureInfo.InvariantCulture);
            else
                Session["CurrentLon"] = 0.0d;
            if (HiddenField3.Value != "")
                Session["Accuracy"] = double.Parse(HiddenField3.Value, CultureInfo.InvariantCulture);
            else
                Session["Accuracy"] = 0.0d;
            if (HiddenField4.Value != "")
                Session["Speed"] = double.Parse(HiddenField4.Value, CultureInfo.InvariantCulture);
            else
                Session["Speed"] = 0.0d;
        }

        // obsługa zdażeń menu Operatora
        protected void MineMenu_MenuItemClick(object sender, MenuEventArgs e)
        {
            MenuItem mnu = (MenuItem)e.Item;
            switch (mnu.Value)
            {
                case "1":
                    {
                        BusManagement.SetActiveView(Home);
                    }
                    break;
                case "2":
                    {
                        BusManagement.SetActiveView(Detail);
                    }
                    break;
                case "3":
                    {
                        bool result = bl.UserLogOut((int)Session["Id"], (string)Session["Bus"]);

                        if (result)
                        {
                            Session.Abandon();
                            Response.Redirect("global.aspx");
                        }
                        else
                        {
                            Response.Write("<script> alert('Błąd - nie udało się poprawnie wylogować') </script>");
                        }
                    }
                    break;
                default:
                    {
                        BusManagement.SetActiveView(Home);
                    }
                    break;
            }
        }

        // pobranie listy dostępnych autobusów
        private void BindBusDDL()
        {
            if (Session["Name"] != null)
            {
                DataSet ds = new DataSet();
                bl.GetBus(3, "", ref ds);
                if (ddl_busSelect != null)
                {
                    ddl_busSelect.DataSource = ds;
                    ddl_busSelect.DataValueField = "Id";
                    ddl_busSelect.DataTextField = "VehicleNb";
                    ddl_busSelect.DataBind();

                    ddl_busSelect.Items.Insert(0, new ListItem("-----", String.Empty));
                    ddl_busSelect.SelectedIndex = 0;
                }
                ds.Dispose();
            }
            BusManagement.SetActiveView(BusSelection);
        }

        // zatwierdzenie wyboru pojazdu
        protected void Bt_busSelect_Click(object sender, EventArgs e)
        {
            this.busMenu.Items[0].Selectable = true;
            this.busMenu.Items[1].Selectable = true;

            if (Session["Name"] != null)
            {
                // dodanie wybranewgo autobusu do ciasteczka z operatorem
                Session["Bus"] = ddl_busSelect.SelectedItem.ToString();

                // naniesienie zmiany statusu wybranego pojazdu
                bl.UpdateVehicleStatus(2, (string)Session["Bus"]);
            }
            else
            {
                // użytkownik nie jest zalogowany, albo ciasteczko z jakiegos powodu znikło
                bl.UserLogOut((int)Session["Id"], (string)Session["Bus"]);
                Response.Redirect("global.aspx");
            }
            BusManagement.SetActiveView(Home);
        }

        // reakcja na wybranie pojazdu
        protected void Ddl_busSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddl_busSelect.SelectedIndex != 0)
                bt_busSelect.Enabled = true;
            else
                bt_busSelect.Enabled = false;
        }

        // Sprawdzenie stanu zleceń w bazie
        private void CheckOperations()
        {
            DataManipulate dm = new DataManipulate();
            DataSet ds = new DataSet();
            bl.GetOperations((string)Session["Bus"], ref ds);
            if (ds.Tables[0].Rows.Count > 0)
            {
                dm.GetPPSData(ds.Tables[0].Rows[0].Field<int>("PPS"));
                dm.GetGateData(ds.Tables[0].Rows[0].Field<int>("Gate"));
                dm.SetOperationStatus(ds);
                SetButtonsStatus();
                UpdateGPSData();
                if ((int)Session["OperationStatus"] == 2)
                    dm.TranslateCoordToDegree((double)Session["CurrentLat"], (double)Session["CurrentLon"]);
                dm.GetOperationData(ds);
                InWorkBusControls((int)Session["OperationStatus"]);
                dm.Dispose();
            }
            ds.Dispose();
        }

        // podstawienie odpowiedniego dzwieku do alertu
        private void BusAlert(int alert)
        {
            string audioAlert = "";

            switch (alert)
            {
                // dojechałeś na miejsce
                case 0:
                    audioAlert = "";
                    break;
                // odległość mniejsza niż 20m dla non shengen i 40m dla shengen w złej strefie
                case 1:
                    audioAlert = "/audio/alert.mp3";
                    break;
                // autobus znajduje sie w strefie bezpieczeństwa Shengen "Alert - to strefa SHENGEN"
                case 2:
                    audioAlert = "/audio/w_s_z.mp3";
                    break;
                // autobus znajduje sie w strefie bezpieczeństwa Non Shengen "Alert - to strefa NON SHENGEN"
                case 3:
                    audioAlert = "/audio/w_ns_z.mp3";
                    break;
                default:
                    audioAlert = "";
                    break;
            };

            HtmlGenericControl sound = new HtmlGenericControl("<embed src=\"" + audioAlert + "\" type=\"audio/mp3\" autostart =\"true\" hidden=\"true\" showcontrols=\"0\" volume=\"-1\"></embed>");
            BusHomeUP.ContentTemplateContainer.Controls.Remove(sound);
            BusHomeUP.ContentTemplateContainer.Controls.Add(sound);
        }

        // ustawienie kolorów aktywnych dla wszystrkich przycisków na stronie bus
        private void InWorkBusControls(int operationStatus)
        {
            DataManipulate dm = new DataManipulate();
            UpdateGPSData();
            if ((int)Session["OperationStatus"] == 2)
                {
                if ((string)Session["StartLocLatDegree"] == null || (string)Session["StartLocLonDegree"] == null || (string)Session["StartLocLatDegree"] == "" || (string)Session["StartLocLonDegree"] == "")
                    dm.TranslateCoordToDegree((double)Session["CurrentLat"], (double)Session["CurrentLon"]);
                }
            dm.CheckDistance((int)Session["OperationStatus"]);
            dm.SetPredictedDistance();
            SetGraficsElements();
            SetColorControls();
            SetDataControls();
            CheckPosition();
            SetAlert();
            BusAlert((int)Session["Alert"]);
            dm.Dispose();
        }

        // ustawienie aktywności kontrolek na panelu
        private void SetButtonsStatus()
        {
            switch ((int)Session["OperationStatus"])
            {
                case 0:
                    {
                        busEndOp.Click -= new EventHandler(BusEndOp_Click);
                        busAccept.Style.Add("background-color", "Silver");
                        busStartLoad.Style.Add("background-color", "Silver");
                        busStartDrive.Style.Add("background-color", "Silver");
                        busStartUnload.Style.Add("background-color", "Silver");
                        busEndOp.Style.Add("background-color", "Silver");
                    }
                    break;
                case 1:
                    {
                        busAccept.Style.Add("background-color", "#1a993d");
                        busAccept.Click += new EventHandler(BusAccept_Click);
                    }
                    break;
                case 2:
                    {
                        busAccept.Style.Add("background-color", "#a63d40");
                        busAccept.Click -= new EventHandler(BusAccept_Click);
                        busStartLoad.Style.Add("background-color", "#1a993d");
                        busStartLoad.Click += new EventHandler(BusStartLoad_Click);
                        busMINEtable.Visible = false;
                        busDriveTable.Visible = true;
                    }
                    break;
                case 3:
                    {
                        busAccept.Style.Add("background-color", "#a63d40");
                        busStartLoad.Style.Add("background-color", "#a63d40");
                        busStartLoad.Click -= new EventHandler(BusStartLoad_Click);
                        busStartDrive.Style.Add("background-color", "#1a993d");
                        busStartDrive.Click += new EventHandler(BusStartDrive_Click);
                        busMINEtable.Visible = true;
                        busDriveTable.Visible = false;
                    }
                    break;
                case 4:
                    {
                        busAccept.Style.Add("background-color", "#a63d40");
                        busStartLoad.Style.Add("background-color", "#a63d40");
                        busStartDrive.Style.Add("background-color", "#a63d40");
                        busStartDrive.Click -= new EventHandler(BusStartDrive_Click);
                        busStartUnload.Style.Add("background-color", "#1a993d");
                        busStartUnload.Click += new EventHandler(BusStartUnload_Click);
                        busMINEtable.Visible = false;
                        busDriveTable.Visible = true;
                    }
                    break;
                case 5:
                    {
                        busAccept.Style.Add("background-color", "#a63d40");
                        busStartLoad.Style.Add("background-color", "#a63d40");
                        busStartDrive.Style.Add("background-color", "#a63d40");
                        busStartUnload.Style.Add("background-color", "#a63d40");
                        busStartUnload.Click -= new EventHandler(BusStartUnload_Click);
                        busEndOp.Style.Add("background-color", "#1a993d");
                        busEndOp.Click += new EventHandler(BusEndOp_Click);
                        busMINEtable.Visible = true;
                        busDriveTable.Visible = false;
                    }
                    break;
            }
        }

        // ustawienie grafik w zależności od operacji i strefy
        private void SetGraficsElements()
        {
            if ((int)Session["OperationStatus"] == 0)
            {
                R1C2.Style.Add(HtmlTextWriterStyle.BackgroundImage, "");
                R1C4.Style.Add(HtmlTextWriterStyle.BackgroundImage, "");
            }
            if ((int)Session["OperationStatus"] == 1)            // jest zadanie, ale jeszcze nie zaakceptowane
            {
                if ((int)Session["Operation"] == 1)              // przylot
                {
                    R1C2.Style.Add(HtmlTextWriterStyle.BackgroundImage, "pictures/ssp.png");
                    R1C4.Style.Add(HtmlTextWriterStyle.BackgroundImage, "pictures/ssp.png");
                }
                else
                if ((int)Session["Operation"] == 2)              // odlot
                {
                    R1C2.Style.Add(HtmlTextWriterStyle.BackgroundImage, "pictures/sso.png");
                    R1C4.Style.Add(HtmlTextWriterStyle.BackgroundImage, "pictures/sso.png");
                }
                R1C2.Style.Add("background-repeat", "no-repeat");
                R1C4.Style.Add("background-repeat", "no-repeat");
                R1C2.Style.Add("background-size", "100% 100%");
                R1C4.Style.Add("background-size", "100% 100%");
            }
            else
            if ((int)Session["OperationStatus"] == 2 || (int)Session["OperationStatus"] == 4)             // zaakceptowana operacja
            {
                switch ((int)Session["Operation"])
                {
                    case 1:                                 // przylot
                        {
                            if ((int)Session["Shengen"] == 0)    // shengen
                            {
                                Dr1C2.Style.Add(HtmlTextWriterStyle.BackgroundImage, "pictures/3szl.png");
                                Dr1C4.Style.Add(HtmlTextWriterStyle.BackgroundImage, "pictures/3szp.png");
                            }
                            else                            // non shengen
                            {
                                Dr1C2.Style.Add(HtmlTextWriterStyle.BackgroundImage, "pictures/3scl.png");
                                Dr1C4.Style.Add(HtmlTextWriterStyle.BackgroundImage, "pictures/3scp.png");
                            }
                        }
                        break;
                    case 2:                                 // odlot
                        {
                            Dr1C2.Style.Add(HtmlTextWriterStyle.BackgroundImage, "pictures/3snl.png");
                            Dr1C4.Style.Add(HtmlTextWriterStyle.BackgroundImage, "pictures/3snp.png");
                            Dr2C2.Style.Add("font-size", "14px");
                            Dr2C3.Style.Add("font-size", "32px");
                            Dr2C4.Style.Add("font-size", "14px");
                        }
                        break;
                }
                Dr1C2.Style.Add("background-repeat", "no-repeat");
                Dr1C4.Style.Add("background-repeat", "no-repeat");
                Dr1C2.Style.Add("background-size", "100% 100%");
                Dr1C4.Style.Add("background-size", "100% 100%");
            }
            else
            if ((int)Session["OperationStatus"] == 3 || (int)Session["OperationStatus"] == 5)            // nie ma żadnej operacji 
            {
                switch ((int)Session["Operation"])
                {
                    case 1:                                                                 // przylot
                        {
                            if ((int)Session["Shengen"] == 0)                                    // shengen
                            {
                                R1C2.Style.Add(HtmlTextWriterStyle.BackgroundImage, "pictures/sz.png");
                                R1C4.Style.Add(HtmlTextWriterStyle.BackgroundImage, "pictures/sz.png");
                            }
                            else                                                            // non shengen
                            {
                                R1C2.Style.Add(HtmlTextWriterStyle.BackgroundImage, "pictures/sc.png");
                                R1C4.Style.Add(HtmlTextWriterStyle.BackgroundImage, "pictures/sc.png");
                            }
                        }
                        break;
                    case 2:                                                                 // odlot
                        {
                            R1C2.Style.Add(HtmlTextWriterStyle.BackgroundImage, "pictures/sn.png");
                            R1C4.Style.Add(HtmlTextWriterStyle.BackgroundImage, "pictures/sn.png");
                        }
                        break;
                }
                
                R1C2.Style.Add("background-repeat", "no-repeat");
                R1C4.Style.Add("background-repeat", "no-repeat");
                R1C2.Style.Add("background-size", "100% 100%");
                R1C4.Style.Add("background-size", "100% 100%");
                R3C2.Style.Add("font-size", "20px");
                R3C3.Style.Add("font-size", "20px");
                R3C4.Style.Add("font-size", "20px");
                R5C3.Style.Add("font-size", "20px");
            }
        }

        // ustawienie kolorystyki kontrolek w zależności od operacji i strefy
        private void SetColorControls()
        {
            switch ((int)Session["OperationStatus"])
            {
                case 0:
                    {
                        R1C2.Style.Add("background-color", "#FFFFCC");
                        R1C3.Style.Add("color", "Violet");
                        R1C4.Style.Add("background-color", "#FFFFCC");
                        R3C2.Style.Add("color", "#FFFFCC");
                        R3C3.Style.Add("color", "#FFFFCC");
                        R3C4.Style.Add("color", "#FFFFCC");
                        R4C2.Style.Add("color", "#FFFFCC");
                        R4C3.Style.Add("color", "#FFFFCC");
                        R4C4.Style.Add("color", "#FFFFCC");
                    }
                    break;
                case 1:
                    {
                        R1C3.Style.Add("color", "Grey");
                        R3C2.Style.Add("color", "Grey");
                        R3C3.Style.Add("color", "Grey");
                        R3C4.Style.Add("color", "Grey");
                        R4C2.Style.Add("color", "Grey");
                        R4C3.Style.Add("color", "Grey");
                        R4C4.Style.Add("color", "Grey");
                        R5C3.Style.Add("color", "Grey");
                    }
                    break;
                case 2:
                    {
                        if ((int)Session["Operation"] == 1)
                        {
                            if ((int)Session["Shengen"] == 0)
                            {
                                Dr5C3.Style.Add("color", "Green");
                            }
                            else
                            {
                                Dr5C3.Style.Add("color", "Red");
                            }
                        }
                        else
                        {
                            Dr5C3.Style.Add("color", "DarkBlue");
                        }
                        Dr1C3.Style.Add("color", "Black");
                        Dr2C2.Style.Add("Color", "DarkBlue");
                        Dr2C3.Style.Add("Color", "#00008B");
                        Dr2C4.Style.Add("Color", "DarkBlue");
                        Dr3C3.Style.Add("color", "Violet");
                        Dr4C3.Style.Add("color", "Black");
                    }
                    break;
                case 3:
                    {
                        if ((int)Session["Operation"] == 1)
                        {
                            if ((int)Session["Shengen"] == 0)
                            {
                                R1C3.Style.Add("color", "Green");
                                R4C2.Style.Add("color", "Green");
                                R4C4.Style.Add("color", "Green");
                            }
                            else
                            {
                                R1C3.Style.Add("color", "Red");
                                R4C2.Style.Add("color", "Red");
                                R4C4.Style.Add("color", "Red");
                            }
                            R3C2.Style.Add("color", "DarkBlue");
                            R3C3.Style.Add("color", "DarkBlue");
                            R3C4.Style.Add("color", "DarkBlue");
                            R4C3.Style.Add("color", "Black");
                            R5C3.Style.Add("color", "Purple");
                        }
                        else
                        {
                            R1C3.Style.Add("color", "Blue");
                            R3C2.Style.Add("color", "DarkBlue");
                            R3C3.Style.Add("color", "DarkBlue");
                            R3C4.Style.Add("color", "DarkBlue");
                            R4C2.Style.Add("color", "#00008B");
                            R4C3.Style.Add("color", "Black");
                            R4C4.Style.Add("color", "#00008B");
                            R5C3.Style.Add("color", "Purple");
                        }
                    }
                    break;
                case 4:
                    {
                        if ((int)Session["Operation"] == 1)
                        {
                            if ((int)Session["Shengen"] == 0)
                            {
                                Dr5C3.Style.Add("color", "Green");
                            }
                            else
                            {
                                Dr5C3.Style.Add("color", "Red");
                            }
                        }
                        else
                        {
                            Dr5C3.Style.Add("color", "DarkBlue");
                        }
                        Dr1C3.Style.Add("color", "Black");
                        Dr2C2.Style.Add("Color", "DarkBlue");
                        Dr2C4.Style.Add("Color", "DarkBlue");
                        Dr3C3.Style.Add("color", "Violet");
                        Dr4C3.Style.Add("color", "Black");
                    }
                    break;
                case 5:
                    {
                        if ((int)Session["Operation"] == 1)
                        {
                            if ((int)Session["Shengen"] == 0)
                            {
                                R1C3.Style.Add("color", "Green");
                                R4C2.Style.Add("color", "Green");
                                R4C4.Style.Add("color", "Green");
                            }
                            else
                            {
                                R1C3.Style.Add("color", "Red");
                                R4C2.Style.Add("color", "Red");
                                R4C4.Style.Add("color", "Red");
                            }
                            R3C2.Style.Add("color", "DarkBlue");
                            R3C3.Style.Add("color", "DarkBlue");
                            R3C4.Style.Add("color", "DarkBlue");
                            R4C3.Style.Add("color", "Black");
                            R5C3.Style.Add("color", "Purple");
                        }
                        else
                        {
                            R1C3.Style.Add("color", "Blue");
                            R3C2.Style.Add("color", "DarkBlue");
                            R3C3.Style.Add("color", "DarkBlue");
                            R3C4.Style.Add("color", "DarkBlue");
                            R4C2.Style.Add("color", "#00008B");
                            R4C3.Style.Add("color", "Black");
                            R4C4.Style.Add("color", "#00008B");
                            R5C3.Style.Add("color", "Purple");
                        }
                    }
                    break;
            }

        }

        // ustawienie wyświetlania odpowiednich danych w zależności od operacji i strefy
        private void SetDataControls()
        {
            switch ((int)Session["OperationStatus"])
            {
                case 0:
                    {
                        R1C3.Text = (string)Session["Bus"];
                        R5C3.Text = "oczekiwanie...";
                    }
                    break;
                case 1:
                    {
                        if ((int)Session["Operation"] == 1)
                        {
                            R4C2.Text = (string)Session["Pps"];
                            R4C4.Text = (string)Session["Gate"];
                        }
                        else
                        {
                            R4C2.Text = "GATE " + (string)Session["Gate"];
                            R4C4.Text = "PPS " + (string)Session["Pps"];
                        }
                        R1C3.Text = (string)Session["AirPort"];
                        R3C2.Text = (string)Session["FlightNb"];
                        R3C3.Text = (string)Session["GodzinaRozkladowa"];
                        R3C4.Text = (string)Session["Pax"];
                        R5C3.Text = "WAW";
                    }
                    break;
                case 2:
                    {
                        if ((int)Session["Operation"] == 1)
                        {
                            if ((int)Session["Shengen"] == 0)
                            {
                                Dr5C3.Text = "SHENGEN";
                            }
                            else
                            {
                                Dr5C3.Text = "NON SHENGEN";
                            }
                            Dr5C3.Text = (string)Session["Pps"];
                        }
                        else
                        {
                            Dr5C3.Text = (string)Session["Gate"];
                        }
                        Dr2C2.Text = (string)Session["StartLocLatDegree"];
                        Dr2C3.Text = "";
                        Dr2C4.Text = (string)Session["StartLocLonDegree"];
                        Dr3C3.Text = Math.Round((double)Session["PredictedDistance"], 2, MidpointRounding.AwayFromZero).ToString() + " m";
                    }
                    break;
                case 3:
                    {
                        if ((int)Session["Operation"] == 1)
                        {
                            R4C2.Text = (string)Session["Pps"];
                            R4C4.Text = (string)Session["Gate"];
                            R1C3.Text = "PRZYLOT";
                            R5C3.Text = "...debording...";
                        }
                        else
                        {
                            R4C2.Text = "Gate: " + (string)Session["RadioGate"];
                            R4C3.Text = "";
                            R4C4.Text = "Neon: " + (string)Session["RadioNeon"];
                            R1C3.Text = "ODLOT";
                            R5C3.Text = "...loading...";
                        }
                        R3C2.Text = (string)Session["FlightNb"];
                        R3C3.Text = (string)Session["GodzinaRozkladowa"];
                        R3C4.Text = (string)Session["Pax"];
                    }
                    break;
                case 4:
                    {
                        if ((int)Session["Operation"] == 1)
                        {
                            if ((int)Session["Shengen"] == 0)
                            {
                                R5C3.Text = "SHENGEN";
                            }
                            else
                            {
                                R5C3.Text = "NON SHENGEN";
                            }
                            Dr5C3.Text = (string)Session["Gate"];
                        }
                        else
                        {
                            Dr5C3.Text = (string)Session["Pps"];
                        }
                        Dr2C2.Text = (string)Session["StartLocLatDegree"];
                        Dr2C3.Text = (string)Session["Gate"];
                        Dr2C4.Text = (string)Session["StartLocLonDegree"];
                        Dr3C3.Text = Math.Round((double)Session["PredictedDistance"], 2, MidpointRounding.AwayFromZero).ToString() + " m";
                    }
                    break;
                case 5:
                    {
                        if ((int)Session["Operation"] == 1)
                        {
                            R4C2.Text = (string)Session["Pps"];
                            R4C4.Text = (string)Session["Gate"];
                            R1C3.Text = "PRZYLOT";
                            R5C3.Text = "...unload...";
                        }
                        else
                        {
                            R4C2.Text = "Gate: " + (string)Session["RadioGate"];
                            R4C4.Text = "Neon: " + (string)Session["RadioNeon"];
                            R1C3.Text = (string)Session["Pps"];
                            R5C3.Text = "...boarding...";
                        }
                        R3C2.Text = (string)Session["FlightNb"];
                        R3C3.Text = (string)Session["GodzinaRozkladowa"];
                        R3C4.Text = (string)Session["Pax"];
                    }
                    break;
            }

        }

        // sprawdzenie aktualnej pozycji w zależności od celu zadania i ustawnie odpowienich akcji
        private void CheckPosition()
        {
            if ((int)Session["OperationStatus"] == 2 || (int)Session["Operation"] == 4)
            {
                if ((double)Session["PredictedDistance"] > 30.0d)
                {
                    Dr3C3.Text = Math.Round((double)Session["DistanceT"] - ((double)Session["Speed"] * 3), 2, MidpointRounding.AwayFromZero).ToString() + " m";
                }
                else
                {
                    Dr3C3.Text = "OK!";
                    Dr3C3.Style.Add("color", "Green");
                }
            }
        }

        //ustawienie poziomu alertu ze względu na odległości do punktów szczególnych
        private void SetAlert()
        {
            if ((int)Session["Operation"] == 1)      // przylot
            {
                switch ((int)Session["Shengen"])
                {
                    case 0:                     // shengen
                        {
                            if ((double)Session["PredictedDistance"] > 30.0d)
                            {
                                if ((double)Session["DistanceN"] >= 100.0d)
                                    Session["Alert"] = 0;
                                else
                                {
                                    if ((double)Session["Speed"] <= 0.88d)
                                    {
                                        Session["Alert"] = 1;
                                        SetColorAlert();
                                    }
                                    else
                                    {
                                        Session["Alert"] = 3;
                                        SetColorAlert();
                                    }
                                }
                            }   
                        }
                        break;
                    case 1:                     // nonShengen
                        {
                            if ((double)Session["PredictedDistance"] > 30.0d)
                            {
                                if ((double)Session["DistanceS"] >= 100.0d)
                                    Session["Alert"] = 0;
                                else
                                {
                                    if ((double)Session["Speed"] <= 0.88d)
                                    {
                                        Session["Alert"] = 1;
                                        SetColorAlert();
                                    }
                                    else
                                    {
                                        Session["Alert"] = 2;
                                        SetColorAlert();
                                    }
                                }
                            }
                        }
                        break;
                }
            }
        }

        // zmiana koloru panelu jeśli wystąpi sytuacja krytyczna
        private void SetColorAlert()
        {
            Dr1C3.Style.Add("background-color", "Red");
            Dr1C3.Style.Add("color", "Black");
            Dr2C2.Style.Add("background-color", "Red");
            Dr2C2.Style.Add("color", "Black");
            Dr2C3.Style.Add("background-color", "Red");
            Dr2C3.Style.Add("color", "Black");
            Dr2C4.Style.Add("background-color", "Red");
            Dr2C4.Style.Add("color", "Black");
            Dr3C3.Style.Add("background-color", "Red");
            Dr3C3.Style.Add("color", "White");
            Dr4C3.Style.Add("background-color", "Red");
            Dr4C3.Style.Add("color", "Black");
            Dr5C3.Style.Add("background-color", "Red");
            Dr5C3.Style.Add("color", "Black");
        }

        // operacja została zaakceptowana
        protected void BusAccept_Click(object sender, EventArgs e)
        {
            bl.BusOperationAction(1, (string)Session["Bus"]);
            Session["OperationStatus"] = 2;
            DataManipulate dm = new DataManipulate();
            dm.TranslateCoordToDegree((double)Session["CurrentLat"], (double)Session["CurrentLon"]);
            dm.Dispose();
        }

        // zaznaczenie początku operacji odbioru pasażerów z samolotu lub Gate
        protected void BusStartLoad_Click(object sender, EventArgs e)
        {
            bl.BusOperationAction(2, (string)Session["Bus"]);
            Session["OperationStatus"] = 3;
            DataManipulate dm = new DataManipulate();
            dm.TranslateCoordToDegree((double)Session["CurrentLat"], (double)Session["CurrentLon"]);
            dm.Dispose();
        }

        protected void BusStartDrive_Click(object sender, EventArgs e)
        {
            bl.BusOperationAction(3, (string)Session["Bus"]);
            Session["OperationStatus"] = 4;
            DataManipulate dm = new DataManipulate();
            dm.TranslateCoordToDegree((double)Session["CurrentLat"], (double)Session["CurrentLon"]);
            dm.Dispose();
        }

        protected void BusStartUnload_Click(object sender, EventArgs e)
        {
            bl.BusOperationAction(4, (string)Session["Bus"]);
            Session["OperationStatus"] = 5;
            DataManipulate dm = new DataManipulate();
            dm.TranslateCoordToDegree((double)Session["CurrentLat"], (double)Session["CurrentLon"]);
            dm.Dispose();
        }

        protected void BusEndOp_Click(object sender, EventArgs e)
        {
            bl.BusOperationAction(5, (string)Session["Bus"]);
            Session["OperationStatus"] = 0;
        }

        protected void BusPause_Click(object sender, EventArgs e)
        {

        }
    }
}