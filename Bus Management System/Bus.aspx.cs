using System;
using System.Data;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Bus_Management_System
{
    public partial class Bus : System.Web.UI.Page
    {
        public static BusinessLayer bl = new BusinessLayer();
        private static DataAccessLayer dal = new DataAccessLayer();
        public static double speed = 0.0d;
        public static double accuracy = 0.0d;
        public static double currentLat = 0.0d;
        public static double currentLon = 0.0d;
        User loggedUser;

        protected void Page_Load(object sender, EventArgs e)
        {
            string userId = "";
            if (Request.Cookies["Bus"] != null)
            {
                userId = Convert.ToString(Request.Cookies["Bus"].Values["userId"]);
            }
            else
            {
                Response.Redirect("global.aspx");
            }
            loggedUser = (User)Session[userId];
            SetButtonsStatus(loggedUser.OperationStatus);

            if (!IsPostBack)
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "przeliczOdleglosc", "getLocation();", true);

                MenuItemCollection menuItems = busMenu.Items;

                if (loggedUser != null)
                {
                    lb_loggedUser.Text = "";
                    lb_loggedUser.Text += (string)loggedUser.FirstName + " " + (string)loggedUser.LastName + "       ID: " + ((int)loggedUser.CompanyId).ToString();

                    // załadowanie listy dostępnych pojazdów do listy
                    BindBusDDL();

                    Reporting rp = new Reporting();
                    rp.PrepareLogFile(loggedUser, DateTime.Now.ToString("yyyyMMdd_HH_mm_ss"));
                    rp.Dispose();
                    Cleaning cl = new Cleaning();
                    cl.ClearSessionObject(loggedUser);
                    cl.Dispose();
                    CheckOperations();
                }
            }
        }

        [System.Web.Services.WebMethod/*(EnableSession = true)*/]
        public static string PrzeliczArray(string[] arrayIn)
        {
            double currentSpeed = 0.0d;
            double currentAccuracy = 0.0d;
            double latitude = double.Parse(arrayIn[0], CultureInfo.InvariantCulture);
            double longitude = double.Parse(arrayIn[1], CultureInfo.InvariantCulture);
            if (arrayIn[2] != "")
            {
                currentAccuracy = double.Parse(arrayIn[2], CultureInfo.InvariantCulture);
            }
            else
            {
                currentAccuracy = 0.0d;
            }
            if (arrayIn[3] != "")
            {
                currentSpeed = double.Parse(arrayIn[3], CultureInfo.InvariantCulture);
            }
            else
            {
                currentSpeed = 0.0d;
            }
            speed = currentSpeed;
            accuracy = currentAccuracy;
            currentLat = latitude;
            currentLon = longitude;

            return currentSpeed.ToString();
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
            HttpCookie cookie = Request.Cookies["Bus"];
            if (cookie != null)
            {
                // zerowanie potęcjalnego komunikatu głosowego
                loggedUser.Alert = 0;
                dm.UpdateGPSData(loggedUser);

                // jeśli nie ma zlecenia
                if (loggedUser.OperationStatus == 0)
                {
                    // sprawdzenie, czy pojawiła się operacja
                    ds = bl.GetOperations(loggedUser.Bus);

                    // jeśli pojawiło sie zlecenie
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        dm.GetPPSData(loggedUser, ds.Tables[0].Rows[0].Field<int>("PPS"));
                        dm.GetGateData(loggedUser, ds.Tables[0].Rows[0].Field<int>("Gate"));
                        dm.SetOperationStatus(loggedUser, ds);
                        SetButtonsStatus(loggedUser.OperationStatus);
                        dm.GetOperationData(loggedUser, ds);
                        InWorkBusControls(loggedUser.OperationStatus);
                    }
                    else
                    {
                        InWorkBusControls(loggedUser.OperationStatus);
                    }
                }
                else
                {
                    loggedUser.Interval = loggedUser.Interval + 1;
                    // żeby nie zapychać łącza, odświeżanie danych co 10s
                    if (loggedUser.Interval == 5)
                    {
                        ds = bl.GetOperations(loggedUser.Bus);
                        dm.GetPPSData(loggedUser, ds.Tables[0].Rows[0].Field<int>("PPS"));
                        dm.GetGateData(loggedUser, ds.Tables[0].Rows[0].Field<int>("Gate"));
                        dm.GetOperationData(loggedUser, ds);
                        dm.SetOperationStatus(loggedUser, ds);
                        SetButtonsStatus(loggedUser.OperationStatus);
                        InWorkBusControls(loggedUser.OperationStatus);
                        loggedUser.Interval = 0;
                    }
                    else
                    {
                        InWorkBusControls(loggedUser.OperationStatus);
                    }
                }
            }
            // "buss" cookie nie istnieje, wiedz na wszelki wypadek koniec sesji i wylogowanie
            else
            {
                // wylogować użytkownika zapisanego w sesji oraz zwolnic BUS
                if (Request.Cookies["Bus"] != null)
                {
                    Response.Cookies["Bus"].Expires = DateTime.Now.AddDays(-1);
                }
                bl.UserLogOut(loggedUser);
                Session.Abandon();
                Response.Redirect("global.aspx");
            }

            // obsługa zmiennych testowych
            stop = Environment.TickCount & Int32.MaxValue;
            loggedUser.LoopTime = stop - start;
            Reporting rp = new Reporting();
            rp.SaveUserFieldsValues(loggedUser);
            rp.Dispose();
            dm.Dispose();
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
                        HttpCookie cookie = Request.Cookies["Bus"];
                        if (cookie != null)
                        {
                            bool result = bl.UserLogOut(loggedUser);

                            if (Request.Cookies["Bus"] != null)
                            {
                                Response.Cookies["Bus"].Expires = DateTime.Now.AddDays(-1);
                            }
                            
                            if(result)
                            {
                                Session.Abandon();
                                Response.Redirect("global.aspx");
                            }
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
            if (Request.Cookies["Bus"] != null)
            {
                DataSet ds = bl.GetBus(3, loggedUser.Al_Bu);
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

            HttpCookie cookie = Request.Cookies["Bus"];
            if (cookie != null)
            {
                // dodanie wybranewgo autobusu do ciasteczka z operatorem
                loggedUser.Bus = ddl_busSelect.SelectedItem.ToString();

                // naniesienie zmiany statusu wybranego pojazdu
                bl.UpdateVehicleStatus(2, loggedUser);
            }
            else
            {
                // użytkownik nie jest zalogowany, albo ciasteczko z jakiegos powodu znikło
                //bl.UserLogOut(loggedUser.CompanyId);
                Response.Redirect("global.aspx");
            }
            // dodać do zalogowanego usera wybrany autobus
            BusManagement.SetActiveView(Home);
        }

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
            DataSet ds = bl.GetOperations(loggedUser.Bus);
            if (ds.Tables[0].Rows.Count > 0)
            {
                dm.GetPPSData(loggedUser, ds.Tables[0].Rows[0].Field<int>("PPS"));
                dm.GetGateData(loggedUser, ds.Tables[0].Rows[0].Field<int>("Gate"));
                dm.SetOperationStatus(loggedUser, ds);
                SetButtonsStatus(loggedUser.OperationStatus);
                dm.UpdateGPSData(loggedUser);
                if (loggedUser.OperationStatus == 2)
                    dm.TranslateColToDegree(loggedUser);
                dm.GetOperationData(loggedUser, ds);
                InWorkBusControls(loggedUser.OperationStatus);
                dm.Dispose();
            }
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
            dm.UpdateGPSData(loggedUser);
            if (loggedUser.OperationStatus == 2 && (loggedUser.StartLocLatDegree == "" || loggedUser.StartLocLonDegree == ""))
                dm.TranslateColToDegree(loggedUser);
            dm.CheckDistance(loggedUser);
            dm.SetPredictedDistance(loggedUser);
            SetGraficElements();
            SetColorControls();
            SetDataControls();
            CheckPosition();
            SetAlert();
            BusAlert(loggedUser.Alert);
            dm.Dispose();
        }

        // ustawienie aktywności kontrolek na panelu
        private void SetButtonsStatus(int operationStatus)
        {
            switch (operationStatus)
            {
                case 0:
                    {
                        loggedUser.OperationStatus = 0;
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
        private void SetGraficElements()
        {
            if (loggedUser.OperationStatus == 0)
            {
                R1C2.Style.Add(HtmlTextWriterStyle.BackgroundImage, "");
                R1C4.Style.Add(HtmlTextWriterStyle.BackgroundImage, "");
            }
            if (loggedUser.OperationStatus == 1)            // jest zadanie, ale jeszcze nie zaakceptowane
            {
                if (loggedUser.Operation == 1)              // przylot
                {
                    R1C2.Style.Add(HtmlTextWriterStyle.BackgroundImage, "pictures/ssp.png");
                    R1C4.Style.Add(HtmlTextWriterStyle.BackgroundImage, "pictures/ssp.png");
                }
                else
                if (loggedUser.Operation == 2)              // odlot
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
            if (loggedUser.OperationStatus == 2 || loggedUser.OperationStatus == 4)             // zaakceptowana operacja
            {
                switch (loggedUser.Operation)
                {
                    case 1:                                 // przylot
                        {
                            if (loggedUser.Shengen == 0)    // shengen
                            {
                                Dr1C2.Style.Add(HtmlTextWriterStyle.BackgroundImage, "pictures/sz.png");
                                Dr1C4.Style.Add(HtmlTextWriterStyle.BackgroundImage, "pictures/sz.png");
                            }
                            else                            // non shengen
                            {
                                Dr1C2.Style.Add(HtmlTextWriterStyle.BackgroundImage, "pictures/sc.png");
                                Dr1C4.Style.Add(HtmlTextWriterStyle.BackgroundImage, "pictures/sc.png");
                            }
                        }
                        break;
                    case 2:                                 // odlot
                        {
                            Dr1C2.Style.Add(HtmlTextWriterStyle.BackgroundImage, "pictures/sn.png");
                            Dr1C4.Style.Add(HtmlTextWriterStyle.BackgroundImage, "pictures/sn.png");
                        }
                        break;
                }
                Dr1C2.Style.Add("background-repeat", "no-repeat");
                Dr1C4.Style.Add("background-repeat", "no-repeat");
                Dr1C2.Style.Add("background-size", "100% 100%");
                Dr1C4.Style.Add("background-size", "100% 100%");
            }
            else
            if (loggedUser.OperationStatus == 3 || loggedUser.OperationStatus == 5)            // nie ma żadnej operacji 
            {
                switch (loggedUser.Operation)
                {
                    case 1:                                                                 // przylot
                        {
                            if (loggedUser.Shengen == 0)                                    // shengen
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
            switch (loggedUser.OperationStatus)
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
                        if (loggedUser.Operation == 1)
                        {
                            if (loggedUser.Shengen == 0)
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
                case 3:
                    {
                        if (loggedUser.Operation == 1)
                        {
                            if (loggedUser.Shengen == 0)
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
                            R4C2.Style.Add("color", "Blue");
                            R4C3.Style.Add("color", "Black");
                            R4C4.Style.Add("color", "Blue");
                            R5C3.Style.Add("color", "Purple");
                        }
                    }
                    break;
                case 4:
                    {
                        if (loggedUser.Operation == 1)
                        {
                            if (loggedUser.Shengen == 0)
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
                        if (loggedUser.Operation == 1)
                        {
                            if (loggedUser.Shengen == 0)
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
                            R4C2.Style.Add("color", "Blue");
                            R4C3.Style.Add("color", "Black");
                            R4C4.Style.Add("color", "Blue");
                            R5C3.Style.Add("color", "Purple");
                        }
                    }
                    break;
            }

        }

        // ustawienie wyświetlania odpowiednich danych w zależności od operacji i strefy
        private void SetDataControls()
        {
            switch (loggedUser.OperationStatus)
            {
                case 0:
                    {
                        R1C3.Text = loggedUser.Bus;
                        R5C3.Text = "oczekiwanie...";
                    }
                    break;
                case 1:
                    {
                        if (loggedUser.Operation == 1)
                        {
                            R4C2.Text = loggedUser.Pps;
                            R4C4.Text = loggedUser.Gate;
                        }
                        else
                        {
                            R4C2.Text = "GATE " + loggedUser.Gate;
                            R4C4.Text = "PPS " + loggedUser.Pps;
                        }
                        R1C3.Text = loggedUser.AirPort;
                        R3C2.Text = loggedUser.FlightNb;
                        R3C3.Text = loggedUser.GodzinaRozkladowa;
                        R3C4.Text = loggedUser.Pax;
                        R5C3.Text = "WAW";
                    }
                    break;
                case 2:
                    {
                        if (loggedUser.Operation == 1)
                        {
                            if (loggedUser.Shengen == 0)
                            {
                                Dr5C3.Text = "SHENGEN";
                            }
                            else
                            {
                                Dr5C3.Text = "NON SHENGEN";
                            }
                            Dr5C3.Text = loggedUser.Pps;
                        }
                        else
                        {
                            Dr5C3.Text = loggedUser.Gate;
                        }
                        Dr2C2.Text = loggedUser.StartLocLatDegree;
                        Dr2C3.Text = "";
                        Dr2C4.Text = loggedUser.StartLocLonDegree;
                        Dr3C3.Text = Math.Round(loggedUser.PredictedDistance, 2, MidpointRounding.AwayFromZero).ToString() + " m";
                    }
                    break;
                case 3:
                    {
                        if (loggedUser.Operation == 1)
                        {
                            R4C2.Text = loggedUser.Pps;
                            R4C4.Text = loggedUser.Gate;
                            R1C3.Text = "PRZYLOT";
                            R5C3.Text = "...debording...";
                        }
                        else
                        {
                            R4C2.Text = "GATE " + loggedUser.Gate;
                            R4C4.Text = "PPS " + loggedUser.Pps;
                            R1C3.Text = "ODLOT";
                            R5C3.Text = "...loading...";
                        }
                        R3C2.Text = loggedUser.FlightNb;
                        R3C3.Text = loggedUser.GodzinaRozkladowa;
                        R3C4.Text = loggedUser.Pax;
                    }
                    break;
                case 4:
                    {
                        if (loggedUser.Operation == 1)
                        {
                            if (loggedUser.Shengen == 0)
                            {
                                R5C3.Text = "SHENGEN";
                            }
                            else
                            {
                                R5C3.Text = "NON SHENGEN";
                            }
                            Dr5C3.Text = loggedUser.Gate;
                        }
                        else
                        {
                            Dr5C3.Text = loggedUser.Pps;
                        }
                        Dr2C2.Text = loggedUser.StartLocLatDegree;
                        Dr2C3.Text = "";
                        Dr2C4.Text = loggedUser.StartLocLonDegree;
                        Dr3C3.Text = Math.Round(loggedUser.PredictedDistance, 2, MidpointRounding.AwayFromZero).ToString() + " m";
                    }
                    break;
                case 5:
                    {
                        if (loggedUser.Operation == 1)
                        {
                            R4C2.Text = loggedUser.Pps;
                            R4C4.Text = loggedUser.Gate;
                            R1C3.Text = "PRZYLOT";
                            R5C3.Text = "...unload...";
                        }
                        else
                        {
                            R4C2.Text = "GATE " + loggedUser.Gate;
                            R4C4.Text = "PPS " + loggedUser.Pps;
                            R1C3.Text = "ODLOT";
                            R5C3.Text = "...boarding...";
                        }
                        R3C2.Text = loggedUser.FlightNb;
                        R3C3.Text = loggedUser.GodzinaRozkladowa;
                        R3C4.Text = loggedUser.Pax;
                    }
                    break;
            }

        }

        // sprawdzenie aktualnej pozycji w zależności od celu zadania i ustawnie odpowienich akcji
        private void CheckPosition()
        {
            if (loggedUser.OperationStatus == 2 || loggedUser.Operation == 4)
            {
                if (loggedUser.PredictedDistance > 30.0d)
                {
                    Dr3C3.Text = Math.Round(loggedUser.DistanceT - (loggedUser.Speed * 3), 2, MidpointRounding.AwayFromZero).ToString() + " m";
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
            if (loggedUser.Operation == 1)      // przylot
            {
                switch (loggedUser.Shengen)
                {
                    case 0:                     // shengen
                        {
                            if (loggedUser.PredictedDistance > 30.0d)
                            {
                                if (loggedUser.DistanceN >= 100.0d)
                                    loggedUser.Alert = 0;
                                else
                                {
                                    if (loggedUser.Speed <= 0.88d)
                                    {
                                        loggedUser.Alert = 1;
                                        SetColorAlert();
                                    }
                                    else
                                    {
                                        loggedUser.Alert = 3;
                                        SetColorAlert();
                                    }
                                }
                            }   
                        }
                        break;
                    case 1:                     // nonShengen
                        {
                            if (loggedUser.PredictedDistance > 30.0d)
                            {
                                if (loggedUser.DistanceS >= 100.0d)
                                    loggedUser.Alert = 0;
                                else
                                {
                                    if (loggedUser.Speed <= 0.88d)
                                    {
                                        loggedUser.Alert = 1;
                                        SetColorAlert();
                                    }
                                    else
                                    {
                                        loggedUser.Alert = 2;
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
            bl.BusOperationAction(loggedUser, 1);
            loggedUser.OperationStatus = 2;
            DataManipulate dm = new DataManipulate();
            dm.TranslateColToDegree(loggedUser);
            dm.Dispose();
        }

        // zaznaczenie początku operacji odbioru pasażerów z samolotu lub Gate
        protected void BusStartLoad_Click(object sender, EventArgs e)
        {
            bl.BusOperationAction(loggedUser, 2);
            loggedUser.OperationStatus = 3;
            DataManipulate dm = new DataManipulate();
            dm.TranslateColToDegree(loggedUser);
            dm.Dispose();
        }

        protected void BusStartDrive_Click(object sender, EventArgs e)
        {
            bl.BusOperationAction(loggedUser, 3);
            loggedUser.OperationStatus = 4;
            DataManipulate dm = new DataManipulate();
            dm.TranslateColToDegree(loggedUser);
            dm.Dispose();
        }

        protected void BusStartUnload_Click(object sender, EventArgs e)
        {
            bl.BusOperationAction(loggedUser, 4);
            loggedUser.OperationStatus = 5;
            DataManipulate dm = new DataManipulate();
            dm.TranslateColToDegree(loggedUser);
            dm.Dispose();
        }

        protected void BusEndOp_Click(object sender, EventArgs e)
        {
            bl.BusOperationAction(loggedUser, 5);
            loggedUser.OperationStatus = 0;
            Cleaning cl = new Cleaning();
            cl.ClearSessionObject(loggedUser);
            cl.Dispose();
        }

        protected void busPause_Click(object sender, EventArgs e)
        {

        }
    }
}