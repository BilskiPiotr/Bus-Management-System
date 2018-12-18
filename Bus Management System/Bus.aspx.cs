using System;
using System.Data;
using System.Data.SqlClient;
using System.Device.Location;
using System.Globalization;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Bus_Management_System
{
    public partial class Bus : System.Web.UI.Page
    {
        private static BusinessLayer bl = new BusinessLayer();
        private static DataAccessLayer dal = new DataAccessLayer();
        private static double speed = 0.0d;
        private static double accuracy = 0.0d;
        private static double currentLat = 0.0d;
        private static double currentLon = 0.0d;
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

                    PrepareLogFile(DateTime.Now.ToString("yyyyMMdd_HH_mm_ss"));
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
                            bool result = bl.UserLogOut(cookie);

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
                string bus = ddl_busSelect.SelectedItem.ToString();
                loggedUser.Bus = bus;
                cookie.Values["busNb"] = bus;
                // i nadpisujemy ciasteczko
                Response.Cookies.Add(cookie);

                // naniesienie zmiany statusu wybranego pojazdu
                bl.UpdateVehicleStatus(2, cookie);

                // na koniec pobranie informacji czy operator ma jakieś zlecenie
                CheckOperations();
            }
            else
            {
                // użytkownik nie jest zalogowany, albo ciasteczko z jakiegos powodu znikło
                // dopisać zamykanie sesji
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
            DataSet ds = bl.GetOperations(loggedUser.Bus);
            if (ds.Tables[0].Rows.Count > 0)
            {
                GetPPSData(ds.Tables[0].Rows[0].Field<int>("PPS"));
                GetGateData(ds.Tables[0].Rows[0].Field<int>("Gate"));
                SetOperationStatus(ds);
                UpdateGPSData();
                GetSpecyficAirPortData(ds.Tables[0].Rows[0].Field<int>("AirPort"));

            }
        }

        // pobranie danych lokalizacyjnych stanowiska postojowego samolotu
        private void GetPPSData(int ppsId)
        {
            DataSet pps = bl.GetPPS(ppsId);
            loggedUser.Pps = pps.Tables[0].Rows[0].Field<string>("StationNb");
            loggedUser.PpsLat = Convert.ToDouble(pps.Tables[0].Rows[0].Field<string>("GPS_Latitude"), CultureInfo.InvariantCulture);
            loggedUser.PpsLon = Convert.ToDouble(pps.Tables[0].Rows[0].Field<string>("GPS_Longitude"), CultureInfo.InvariantCulture);
            pps.Dispose();
        }

        // pobranie danych lokalizacyjnych wyjścia pasażerskiego
        private void GetGateData(int gateId)
        {
            DataSet gate = bl.GetGate(gateId);
            loggedUser.Gate = gate.Tables[0].Rows[0].Field<string>("GateNb");
            loggedUser.GateLat = Convert.ToDouble(gate.Tables[0].Rows[0].Field<string>("GPS_Latitude"), CultureInfo.InvariantCulture);
            loggedUser.GateLon = Convert.ToDouble(gate.Tables[0].Rows[0].Field<string>("GPS_Longitude"), CultureInfo.InvariantCulture);
            gate.Dispose();
        }

        //ustawienie wartości aktualnej operacji w zmiennej sesyjnej
        private void GetOperationData(DataSet ds)
        {
            loggedUser.Operation = ds.Tables[0].Rows[0].Field<int>("Operation");
            loggedUser.FlightNb = ds.Tables[0].Rows[0].Field<string>("FlightNb");
            loggedUser.Pax = ds.Tables[0].Rows[0].Field<int>("Pax").ToString();
            loggedUser.RadioGate = ds.Tables[0].Rows[0].Field<string>("RadioGate").ToString();
            loggedUser.RadioNeon = ds.Tables[0].Rows[0].Field<string>("RadioNeon").ToString();
            loggedUser.GodzinaRozkladowa = (ds.Tables[0].Rows[0].Field<DateTime>("GodzinaRozkladowa")).ToString("HH:mm");

            GetSpecyficAirPortData(ds.Tables[0].Rows[0].Field<int>("AirPort"));
        }

        // ustalenie stref bezpieczeństwa dla obsługiwanego portu
        private void GetSpecyficAirPortData(int airPort)
        {
            DataSet ds = bl.GetAirPort(airPort);
            loggedUser.AirPort = ds.Tables[0].Rows[0].Field<string>("IATA_Name");
            loggedUser.Shengen = Convert.ToInt32(bl.GetCountry(ds.Tables[0].Rows[0].Field<int>("Shengen")));
            loggedUser.PortName = ds.Tables[0].Rows[0].Field<string>("Full_Name");
            loggedUser.Country = ds.Tables[0].Rows[0].Field<string>("Country_name");
        }

        // Ustalenie na jakim etapie jest aktualna operacja
        private void SetOperationStatus(DataSet ds)
        {
            /* operationStatus wartości możliwe:
             * 0 - brak zlecenia       <-
             * 1 - zlecenie utworzone    |
             * 2 - zlecenie przyjete     |
             * 3 - rozpoczęty załadunek  |
             * 4 - dowóz pasażerów       |
             * 5 - rozpoczęty wyładunek >|
             */
            int operationStatus = 0;
            loggedUser.Created = (ds.Tables[0].Rows[0].Field<DateTime>("Created")).ToString("HH:mm");
            loggedUser.Accepted = (ds.Tables[0].Rows[0].Field<DateTime>("Accepted")).ToString("HH:mm");
            loggedUser.StartLoad = (ds.Tables[0].Rows[0].Field<DateTime>("StartLoad")).ToString("HH:mm");
            loggedUser.StartDrive = (ds.Tables[0].Rows[0].Field<DateTime>("StartDrive")).ToString("HH:mm");
            loggedUser.StartUnload = (ds.Tables[0].Rows[0].Field<DateTime>("StartUnload")).ToString("HH:mm");
            loggedUser.EndOp = (ds.Tables[0].Rows[0].Field<DateTime>("EndOp")).ToString("HH:mm");
            if (loggedUser.Created != "00:00")
                operationStatus = 1;
            if (loggedUser.Accepted != "00:00")
                operationStatus = operationStatus + 1;
            if (loggedUser.StartLoad != "00:00")
                operationStatus = operationStatus + 1;
            if (loggedUser.StartDrive != "00:00")
                operationStatus = operationStatus + 1;
            if (loggedUser.StartUnload != "00:00")
                operationStatus = operationStatus + 1;
            if (loggedUser.EndOp != "00:00")
                operationStatus = 0;

            loggedUser.OperationStatus = operationStatus;
        }

        // naniesienie aktualnych danych lokalizacyjnych
        private void UpdateGPSData()
        {
            loggedUser.Speed = speed;
            loggedUser.Accuracy = accuracy;
            loggedUser.CurrentLat = currentLat;
            loggedUser.CurrentLon = currentLon;
        }

        protected void BusHomeTimer_Tick(object sender, EventArgs e)
        {
            // pomiar czasu wykonywania timera
            int start, stop;
            start = Environment.TickCount & Int32.MaxValue;

            // sprawdzenie, czy użytkownik jest poprawnie zalogowany
            HttpCookie cookie = Request.Cookies["Bus"];
            if (cookie != null)
            {
                // zerowanie potęcjalnego komunikatu głosowego
                loggedUser.Alert = 0;
                UpdateGPSData();

                int operationStatus = loggedUser.OperationStatus;

                if (operationStatus == 0)
                {
                    DataSet ds = bl.GetOperations(loggedUser.Bus);

                    // sprawdzenie, czy pojawiła się operacja
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        int shengen = 0;
                        string portName = "";
                        string country = "";

                        GetPPSData(ds.Tables[0].Rows[0].Field<int>("PPS"));
                        GetGateData(ds.Tables[0].Rows[0].Field<int>("Gate"));
                        SetOperationStatus(ds);
                        GetSpecyficAirPortData(ds.Tables[0].Rows[0].Field<int>("AirPort"));
                    }
                    else
                    {
                        IddleBusControls();
                    }
                }
                //else
                InWorkBusControls(operationStatus);
            }
            // "buss" cookie nie istnieje, wiedz na wszelki wypadek koniec sesji i wylogowanie
            else
            {
                // wylogować użytkownika zapisanego w sesji oraz zwolnic BUS
                if (Request.Cookies["Bus"] != null)
                {
                    Response.Cookies["Bus"].Expires = DateTime.Now.AddDays(-1);
                }
                Session.Abandon();
                Response.Redirect("global.aspx");
            }

            // sprawdzenie stanu alertu, i ewentualne odtworzenie go
            BusAlert(loggedUser.Alert);

            stop = Environment.TickCount & Int32.MaxValue;

            loggedUser.LoopTime = stop - start;

            SaveUserFieldsValues();
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
            if (operationStatus !=0)
            {
                int shengen = loggedUser.Shengen;

                if (operationStatus == 1)
                {
                    GreyScreen(loggedUser.Operation, shengen);
                }
                else
                {
                    switch (loggedUser.Operation)
                    {
                        case 1:
                            Przylot(shengen);
                            break;
                        case 2:
                            Odlot(shengen);
                            break;
                    }
                }
            }
        }



        private void SetButtonsStatus(int operationStatus)
        {
            switch (operationStatus)
            {
                case 0:
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
                case 6:
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
                default:
                    break;
            }
        }



        private void IddleBusControls()
        {
            R1C2.Style.Add("background-color", "#FFFFCC");
            R1C4.Style.Add("background-color", "#FFFFCC");
            R3C2.Style.Add("color", "#FFFFCC");
            R3C3.Style.Add("color", "#FFFFCC");
            R3C4.Style.Add("color", "#FFFFCC");
            R4C2.Style.Add("color", "#FFFFCC");
            R4C3.Style.Add("color", "#FFFFCC");
            R4C4.Style.Add("color", "#FFFFCC");
            R5C3.Text = "oczekiwanie...";
        }


        private void GreyScreen(int operation, int shengen)
        {
            if (operation == 1)
            {
                R1C2.Style.Add(HtmlTextWriterStyle.BackgroundImage, "pictures/sso.png");
                R1C4.Style.Add(HtmlTextWriterStyle.BackgroundImage, "pictures/sso.png");
                R1C3.Text = loggedUser.AirPort;
                R4C2.Text = loggedUser.Gate;
                R4C4.Text = loggedUser.Pps;
                R5C3.Text = "WAW";
            }
            else
            {
                R1C2.Style.Add(HtmlTextWriterStyle.BackgroundImage, "pictures/ssp.png");
                R1C4.Style.Add(HtmlTextWriterStyle.BackgroundImage, "pictures/ssp.png");
                R1C3.Text = "WAW";
                R4C2.Text = loggedUser.Pps;
                R4C4.Text = loggedUser.Gate;
                R5C3.Text = loggedUser.AirPort;
            }

            R1C2.Style.Add("background-repeat", "no-repeat");
            R1C4.Style.Add("background-repeat", "no-repeat");
            R1C2.Style.Add("background-size", "100% 100%");
            R1C4.Style.Add("background-size", "100% 100%");

            R1C3.Style.Add("color", "Grey");
            R3C2.Text = loggedUser.FlightNb;
            R3C2.Style.Add("color", "Grey");
            R3C3.Text = loggedUser.GodzinaRozkladowa;
            R3C3.Style.Add("color", "Grey");
            R3C4.Text = loggedUser.Pax;
            R3C4.Style.Add("color", "Grey");
            R4C2.Style.Add("color", "Grey");
            R4C3.Style.Add("color", "Grey");
            R4C4.Style.Add("color", "Grey");
            R5C3.Style.Add("color", "Grey");
        }


        private void DisplayWork(int operation, int shengen)
        {
            if (operation == 1)
            {
                R1C2.Style.Add(HtmlTextWriterStyle.BackgroundImage, "pictures/sn.png");
                R1C4.Style.Add(HtmlTextWriterStyle.BackgroundImage, "pictures/sn.png");
                R4C2.Text = loggedUser.Gate;
                R4C4.Text = loggedUser.Pps;
                R1C3.Style.Add("color", "DarkBlue");
                R5C3.Style.Add("color", "DarkBlue");
            }
            else
            {
                if (shengen == 0)
                {
                    R1C2.Style.Add(HtmlTextWriterStyle.BackgroundImage, "pictures/sz.png");
                    R1C4.Style.Add(HtmlTextWriterStyle.BackgroundImage, "pictures/sz.png");
                    R1C3.Style.Add("color", "Green");
                    R5C3.Style.Add("color", "Green");
                    R5C3.Text = "SHENGEN";
                }
                else
                {
                    R1C2.Style.Add(HtmlTextWriterStyle.BackgroundImage, "pictures/sc.png");
                    R1C4.Style.Add(HtmlTextWriterStyle.BackgroundImage, "pictures/sc.png");
                    R1C3.Style.Add("color", "Red");
                    R5C3.Style.Add("color", "Red");
                    R5C3.Text = "NON SHENGEN";
                }
                R4C2.Text = loggedUser.Pps;
                R4C4.Text = loggedUser.Gate;
            }

            R1C3.Text = loggedUser.AirPort;


            R1C2.Style.Add("background-repeat", "no-repeat");
            R1C4.Style.Add("background-repeat", "no-repeat");
            R1C2.Style.Add("background-size", "100% 100%");
            R1C4.Style.Add("background-size", "100% 100%");
            R3C2.Text = loggedUser.FlightNb;
            R3C3.Text = loggedUser.GodzinaRozkladowa;
            R3C4.Text = loggedUser.Pax;

        }

        private void Odlot(int shengen)
        {
            if (busMINEtable.Visible == false)
            {
                Dr1C2.Style.Add(HtmlTextWriterStyle.BackgroundImage, "pictures/3snl.png");
                Dr1C2.Style.Add("background-repeat", "no-repeat");
                Dr1C2.Style.Add("background-size", "100% 100%");
                Dr1C4.Style.Add(HtmlTextWriterStyle.BackgroundImage, "pictures/3snp.png");
                Dr1C4.Style.Add("background-repeat", "no-repeat");
                Dr1C4.Style.Add("background-size", "100% 100%");

                if (loggedUser.OperationStatus == 2 || loggedUser.OperationStatus == 4)
                {
                    if (loggedUser.StartLocLatDegree == null || loggedUser.StartLocLonDegree == null)
                    {
                        string lat = "";
                        string lon = "";
                        TranslateColToDegree(ref lat, ref lon);
                        loggedUser.StartLocLatDegree = lat;
                        loggedUser.StartLocLonDegree = lon;
                    }
                    Dr2C2.Text = loggedUser.StartLocLatDegree;
                    Dr2C3.Text = "";
                    Dr2C4.Text = loggedUser.StartLocLonDegree;
                    Dr5C3.Text = loggedUser.Gate;

                    double distanceT = 0.0;

                    if (loggedUser.OperationStatus == 2)
                    {
                        distanceT = CheckDistance(loggedUser.CurrentLat, loggedUser.CurrentLon, loggedUser.GateLat, loggedUser.GateLon);
                    }
                    else
                    {
                        distanceT = CheckDistance(loggedUser.CurrentLat, loggedUser.CurrentLon, loggedUser.PpsLat, loggedUser.PpsLon);

                    }

                    /*  to jest niedorobiona metoda MUSZĘ TO KIEDYŚ POPRAWIC!!! */

                    if (distanceT > 15.0d)
                    {
                        // jesli poprzedni dystans do celu byl wiekszy (mniejszy) to
                        Dr3C3.Text = Math.Round(loggedUser.DistanceT - (loggedUser.Speed * 3), 2, MidpointRounding.AwayFromZero).ToString() + " m";
                        Dr3C3.Style.Add("color", "Violet");
                    }
                    else
                    {
                        Dr3C3.Text = "OK!";
                        Dr3C3.Style.Add("color", "Green");
                    }
                }
                else
                {
                    Dr2C3.Text = loggedUser.Gate;
                    Dr5C3.Text = loggedUser.Pps;
                }
            }
        }



        private void Przylot(int shengen)
        {
            double distanceT = 0.0d;

            // ustalenie kolorów w zależności od strefy
            if (shengen == 0)
            {
                Dr1C2.Style.Add(HtmlTextWriterStyle.BackgroundImage, "pictures/sz.png");
                Dr1C2.Style.Add("background-repeat", "no-repeat");
                Dr1C2.Style.Add("background-size", "100% 100%");

                Dr1C4.Style.Add(HtmlTextWriterStyle.BackgroundImage, "pictures/sz.png");
                Dr1C4.Style.Add("background-repeat", "no-repeat");
                Dr1C4.Style.Add("background-size", "100% 100%");

                Dr5C3.Style.Add("color", "Green");
                Dr5C3.Text = "SHENGEN";
            }
            else
            {
                Dr1C2.Style.Add(HtmlTextWriterStyle.BackgroundImage, "pictures/sc.png");
                Dr1C2.Style.Add("background-repeat", "no-repeat");
                Dr1C2.Style.Add("background-size", "100% 100%");
                ;
                Dr1C4.Style.Add(HtmlTextWriterStyle.BackgroundImage, "pictures/sc.png");
                Dr1C4.Style.Add("background-repeat", "no-repeat");
                Dr1C4.Style.Add("background-size", "100% 100%");

                R5C3.Style.Add("color", "Red");
                R5C3.Text = "NON SHENGEN";
            }


            // ustalenie zawartości pozostałych komurek tabeli w zależności od postępu zadania
            if (busMINEtable.Visible == false)
            {
                if (loggedUser.OperationStatus == 2 || loggedUser.OperationStatus == 4)
                {
                    //wyświetlenie aktualnej prędkości
                    Dr4C3.Style.Add("font-size", "16px");
                    Dr4C3.Text = Math.Round(loggedUser.Speed, 2, MidpointRounding.AwayFromZero).ToString() + " m/s   |   " + Math.Round(((loggedUser.Speed * 3600) / 1000), 2, MidpointRounding.AwayFromZero).ToString() + " km/h";

                    // ustalenie, gdzie operacja została przyjęta
                    if (loggedUser.StartLocLatDegree == "0° 0' 0'' N" || loggedUser.StartLocLonDegree == "0° 0' 0'' E")
                    {
                        string lat = "";
                        string lon = "";
                        TranslateColToDegree(ref lat, ref lon);
                        loggedUser.StartLocLatDegree = lat;
                        loggedUser.StartLocLonDegree = lon;
                    }

                    // sprawdzenie wszystkich odległości w zależności od celu
                    if (loggedUser.OperationStatus == 2)
                    {
                        distanceT = CheckDistance(loggedUser.CurrentLat, loggedUser.CurrentLon, loggedUser.PpsLat, loggedUser.PpsLon);
                        Dr5C3.Text = loggedUser.Pps;
                    }
                    else
                    {
                        distanceT = CheckDistance(loggedUser.CurrentLat, loggedUser.CurrentLon, loggedUser.GateLat, loggedUser.GateLon);
                        Dr5C3.Text = loggedUser.Gate;
                    }

                    Dr2C2.Text = loggedUser.StartLocLatDegree;
                    Dr2C3.Text = "";
                    Dr2C4.Text = loggedUser.StartLocLonDegree;

                    if (loggedUser.OldDistanceT > loggedUser.DistanceT)
                    {
                        loggedUser.PredictedDistance = distanceT - (loggedUser.Speed * 3);
                    }
                    else
                    {
                        loggedUser.PredictedDistance = distanceT + (loggedUser.Speed * 3);
                    }

                    Dr3C3.Text = Math.Round(loggedUser.PredictedDistance, 2, MidpointRounding.AwayFromZero).ToString() + " m";



                    // interakcja z użytkownikiem w zależności od odległości do punktów szczegulnych
                    bool danger = false;

                    if (distanceT != 0.0d && Math.Round(loggedUser.PredictedDistance, 2, MidpointRounding.AwayFromZero) > 100.0d)
                    {
                        danger = CheckSecurityZone(loggedUser.Shengen);
                    }
                    else
                    {
                        // metoda normalne kolody
                        Dr3C3.Style.Add("background-color", "#FFFFCC");
                        Dr4C3.Style.Add("background-color", "#FFFFCC");
                        Dr5C3.Style.Add("background-color", "#FFFFCC");
                        Dr3C3.Style.Add("color", "Violet");
                        Dr4C3.Style.Add("color", "Black");

                        // metoda dojazd do strefy przeznaczenia
                        if (shengen == 0)
                            Dr5C3.Style.Add("color", "Green");
                        else
                        if (shengen == 1)
                            Dr5C3.Style.Add("color", "Red");



                        // dojechano do strefy przeznaczenia
                        if (Math.Round(loggedUser.PredictedDistance, 2, MidpointRounding.AwayFromZero) <= 15.0d)
                        {
                            Dr3C3.Text = "OK!";
                        }
                        else
                        {
                            Dr3C3.Text = Math.Round(loggedUser.PredictedDistance, 2, MidpointRounding.AwayFromZero).ToString() + " m";
                        }
                    }

                    if (!danger)
                    {
                        // metoda normalne kolory 
                        Dr3C3.Style.Add("background-color", "#FFFFCC");
                        Dr4C3.Style.Add("background-color", "#FFFFCC");
                        Dr5C3.Style.Add("background-color", "#FFFFCC");
                        Dr3C3.Style.Add("color", "Violet");
                        Dr4C3.Style.Add("color", "Black");

                        // metoda dojazd do strefy przeznaczenia
                        if (shengen == 0)
                            Dr5C3.Style.Add("color", "Green");
                        else
                        if (shengen == 1)
                            Dr5C3.Style.Add("color", "Red");
                    }
                    else
                    {
                        // metoda kolory alarmowe
                        Dr3C3.Style.Add("background-color", "Red");
                        Dr4C3.Style.Add("background-color", "Red");
                        Dr5C3.Style.Add("background-color", "Red");
                        Dr3C3.Style.Add("color", "White");
                        Dr4C3.Style.Add("color", "Black");
                        Dr5C3.Style.Add("color", "Black");
                    }
                }
                else
                {
                    // jeśli autobus aktualnie nie przewozi ludzi
                }
            }
        }



        private bool CheckSecurityZone(int securityZone)
        {
            bool danger = false;
            double predictedDistance = 0.0d;

            switch (securityZone)
            {
                case 0:
                    {
                        if (loggedUser.OldDistanceN == loggedUser.DistanceN)
                            predictedDistance = Math.Round(loggedUser.DistanceN, 2, MidpointRounding.AwayFromZero);
                        else
                        if (loggedUser.OldDistanceN > loggedUser.DistanceN)
                            predictedDistance = Math.Round((loggedUser.DistanceN - (loggedUser.Speed * 3)), 2, MidpointRounding.AwayFromZero);
                        else
                        if (loggedUser.OldDistanceN < loggedUser.DistanceN)
                            predictedDistance = Math.Round((loggedUser.DistanceN + (loggedUser.Speed * 3)), 2, MidpointRounding.AwayFromZero);

                        // jeśli pojazd jadąc do shengen jest w pobliżu non shengen
                        if (predictedDistance < 75.0d && predictedDistance > 25.0d)
                        {
                            // w odległości mniejszej niż 75m 
                            loggedUser.Alert = 3;
                            danger = true;
                        }
                        else
                        // odległość mniejsza niż 25m, i prędkość mniejsza od 1,5 km/h
                        if (predictedDistance <= 25.0d && predictedDistance >= 0.0d)
                        {
                            if (loggedUser.Speed <= 0.83d)
                            {
                                loggedUser.Alert = 1;
                                danger = true;
                            }
                            else
                                if (loggedUser.Speed > 0.83d)
                            {
                                loggedUser.Alert = 3;
                                danger = true;
                            }
                        }
                        else
                            danger = false;
                    }
                    break;
                case 1:
                    {
                        if (loggedUser.OldDistanceS == loggedUser.DistanceS)
                            predictedDistance = Math.Round(loggedUser.DistanceS, 2, MidpointRounding.AwayFromZero);
                        else
                        if (loggedUser.OldDistanceS > loggedUser.DistanceS)
                            predictedDistance = Math.Round((loggedUser.DistanceS - (loggedUser.Speed * 3)), 2, MidpointRounding.AwayFromZero);
                        else
                        if (loggedUser.OldDistanceS < loggedUser.DistanceS)
                            predictedDistance = Math.Round((loggedUser.DistanceS + (loggedUser.Speed * 3)), 2, MidpointRounding.AwayFromZero);

                        // jeśli pojazd jadąc do NonShengen jest w pobliżu Shengen
                        if (predictedDistance < 75.0d && predictedDistance > 25.0d)
                        {
                            // w odległości mniejszej niż 75m 
                            loggedUser.Alert = 2;
                            danger = true;
                        }
                        else
                        // odległość mniejsza niż 25m, i prędkość mniejsza od 1,5 km/h
                        if (predictedDistance <= 25.0d)
                        {
                            if (loggedUser.Speed <= 0.83d)
                            {
                                loggedUser.Alert = 1;
                                danger = true;
                            }
                            else
                                if (loggedUser.Speed > 0.83d)
                            {
                                loggedUser.Alert = 2;
                                danger = true;
                            }
                        }
                        else
                            danger = false;
                    }
                    break;
            }
            return danger;
        }




        private double CheckDistance(double currentLat, double currentLon, double destinationLat, double destinationLon)
        {
            GeoCoordinate busPosition = new GeoCoordinate(currentLat, currentLon);
            GeoCoordinate targetPosition = new GeoCoordinate(destinationLat, destinationLon);
            GeoCoordinate shengen = new GeoCoordinate(52.17035, 20.97174);
            GeoCoordinate nonShengen = new GeoCoordinate(52.17224, 20.9702);

            // zwrocenie odleglosci miedzy wspolrzednymi z ograniczeniem do 2 miejsc po przecinku
            double distanceT = Math.Round(busPosition.GetDistanceTo(targetPosition), 2, MidpointRounding.AwayFromZero);
            double distanceS = Math.Round(busPosition.GetDistanceTo(shengen), 2, MidpointRounding.AwayFromZero);
            double distanceN = Math.Round(busPosition.GetDistanceTo(nonShengen), 2, MidpointRounding.AwayFromZero);

            loggedUser.OldDistanceT = loggedUser.DistanceT;
            loggedUser.DistanceT = distanceT;

            loggedUser.OldDistanceS = loggedUser.DistanceS;
            loggedUser.DistanceS = distanceS;

            loggedUser.OldDistanceN = loggedUser.DistanceN;
            loggedUser.DistanceN = distanceN;

            return distanceT;
        }




        // konwersja i sprawdzenie czy operacja została rozpoczęta
        private Boolean GetStoredTime(DateTime inputDate)
        {
            TimeSpan time = inputDate.TimeOfDay;
            if (time.ToString() == "00:00:00")
                return false;
            else
                return true;
        }


        // operacja została zaakceptowana
        protected void BusAccept_Click(object sender, EventArgs e)
        {
            HttpCookie cookie = Request.Cookies["Bus"];
            string bus = cookie.Values["busNb"].ToString();
            SqlCommand cmd = new SqlCommand("UPDATE Operations SET Accepted = @accepted WHERE Bus = (SELECT Id FROM Vehicles WHERE VehicleNb = @busNb)");
            cmd.Parameters.AddWithValue("@busNb", bus);
            cmd.Parameters.AddWithValue("@accepted", DateTime.Now);
            dal.QueryExecution(cmd);

            loggedUser.OperationStatus = 2;

            string lat = "";
            string lon = "";

            TranslateColToDegree(ref lat, ref lon);

            int operation = loggedUser.Operation;

            loggedUser.StartLocLatDegree = lat;
            loggedUser.StartLocLonDegree = lon;
        }

        // zaznaczenie początku operacji odbioru pasażerów z samolotu lub Gate
        protected void BusStartLoad_Click(object sender, EventArgs e)
        {
            HttpCookie cookie = Request.Cookies["Bus"];
            string bus = cookie.Values["busNb"].ToString();
            SqlCommand cmd = new SqlCommand("UPDATE Operations SET StartLoad = @startLoad WHERE Bus = (SELECT Id FROM Vehicles WHERE VehicleNb = @busNb)");
            cmd.Parameters.AddWithValue("@busNb", bus);
            cmd.Parameters.AddWithValue("@startLoad", DateTime.Now);
            dal.QueryExecution(cmd);
            cmd.Parameters.Clear();
            loggedUser.OperationStatus = 3;
        }

        protected void BusStartDrive_Click(object sender, EventArgs e)
        {
            HttpCookie cookie = Request.Cookies["Bus"];
            string bus = cookie.Values["busNb"].ToString();
            SqlCommand cmd = new SqlCommand("UPDATE Operations SET StartDrive = @startDrive WHERE Bus = (SELECT Id FROM Vehicles WHERE VehicleNb = @busNb)");
            cmd.Parameters.AddWithValue("@busNb", bus);
            cmd.Parameters.AddWithValue("@startDrive", DateTime.Now);
            dal.QueryExecution(cmd);
            loggedUser.OperationStatus = 4;
        }

        protected void BusStartUnload_Click(object sender, EventArgs e)
        {
            HttpCookie cookie = Request.Cookies["Bus"];
            string bus = cookie.Values["busNb"].ToString();
            SqlCommand cmd = new SqlCommand("UPDATE Operations SET StartUnload = @startUnload WHERE Bus = (SELECT Id FROM Vehicles WHERE VehicleNb = @busNb)");
            cmd.Parameters.AddWithValue("@busNb", bus);
            cmd.Parameters.AddWithValue("@startUnload", DateTime.Now);
            dal.QueryExecution(cmd);
            loggedUser.OperationStatus = 5;
        }

        protected void BusEndOp_Click(object sender, EventArgs e)
        {
            HttpCookie cookie = Request.Cookies["Bus"];
            string bus = cookie.Values["busNb"].ToString();
            SqlCommand cmd = new SqlCommand("UPDATE Operations SET EndOp = @endOp, Finished = @finished WHERE Bus = (SELECT Id FROM Vehicles WHERE VehicleNb = @busNb)");
            cmd.Parameters.AddWithValue("@busNb", bus);
            cmd.Parameters.AddWithValue("@endOp", DateTime.Now);
            cmd.Parameters.AddWithValue("@finished", 1);
            dal.QueryExecution(cmd);
            loggedUser.OperationStatus = 6;
            ClearFields();
        }

        protected void BusPause_Click(object sender, EventArgs e)
        {

        }



        private void ClearFields()
        {
            // wyczyszczenie wszystkich wartości i zakończenie wgrywania danych
        }



        // przeliczenie współrzędnych pobranych z urządzenia GPS do czytelnych współrzędnych w stopniach
        private void TranslateColToDegree(ref string lat, ref string lon)
        {
            double latitude = loggedUser.CurrentLat;
            double longitude = loggedUser.CurrentLon;

            string latitude_Kierunek = (latitude >= 0 ? "N" : "S");

            latitude = Math.Abs(latitude);
            double minutyLat = ((latitude - Math.Truncate(latitude) / 1) * 60);
            double sekundyLat = ((minutyLat - Math.Truncate(minutyLat) / 1) * 60);

            string longitude_Kierunek = (longitude >= 0 ? "E" : "W");
            longitude = Math.Abs(longitude);
            double minutyLon = ((longitude - Math.Truncate(longitude) / 1) * 60);
            double sekundyLon = ((minutyLon - Math.Truncate(minutyLon) / 1) * 60);

            lat = String.Format(Convert.ToString(Math.Truncate(latitude) + "° " + +Math.Truncate(minutyLat) + "' " + Math.Truncate(sekundyLat) + "'' " + latitude_Kierunek));
            lon = String.Format(Convert.ToString(Math.Truncate(longitude) + "° " + Math.Truncate(minutyLon) + "' " + Math.Truncate(sekundyLon) + "'' " + longitude_Kierunek));
        }


        // przygotowanie i obrobka pliku tekstowego z logami operacji
        private void PrepareLogFile(string data)
        {
            // tworzymy nazwę pliku ktory bedzie uzupelniany danymi
            string dataLogic = Server.MapPath("~/logi/" + loggedUser.FirstName + "_" + loggedUser.LastName + "__" + data + ".txt");

            FileStream fs = null;

            if (!File.Exists(dataLogic))
            {
                using (fs = File.Create(dataLogic))
                {

                }
                fs.Close();

                string newLine = Environment.NewLine;
                newLine += "Operation; ";
                newLine += "Operation Status; ";
                newLine += "Created; ";
                newLine += "Accepted; ";
                newLine += "Start Load; ";
                newLine += "Start Drive; ";
                newLine += "Start Unload; ";
                newLine += "End Operation; ";
                newLine += "Start Latitude; ";
                newLine += "Start Longitude; ";
                newLine += "Current Latitude; ";
                newLine += "Current Longitude; ";
                newLine += "Speed; ";
                newLine += "Accuracy; ";
                newLine += "Old Distance to Target; ";
                newLine += "Distance to Target; ";
                newLine += "Old Distance to Shengen; ";
                newLine += "Distance to Shengen; ";
                newLine += "Old Distance to NonShengen; ";
                newLine += "Distance to NonShengen; ";
                newLine += "Predicted Distance; ";
                newLine += "Alert Nb; ";
                newLine += "Loop Time; ";
                newLine += "Measurement Time";

                using (StreamWriter writer = new StreamWriter(dataLogic, true))
                {
                    writer.WriteLine(newLine);
                    writer.Close();
                }

            }
            else
            {
                Response.Write("<script> alert('Błąd - ścieżki lub pliku') </script>");
            }
            // i dodajemy nazwe pliku do zmiennej sesyjnej
            loggedUser.LogFilePath = dataLogic;
        }
        


        // dodajemy do pliku log sesji dane z kontrolek
        private void SaveUserFieldsValues()
        {
            string newLine = Environment.NewLine;
            newLine += loggedUser.Operation.ToString() + "; ";
            newLine += loggedUser.OperationStatus.ToString() + "; ";
            newLine += loggedUser.Created + "; ";
            newLine += loggedUser.Accepted + "; ";
            newLine += loggedUser.StartLoad + "; ";
            newLine += loggedUser.StartDrive + "; ";
            newLine += loggedUser.StartUnload + "; ";
            newLine += loggedUser.EndOp + "; ";
            newLine += loggedUser.StartLat.ToString() + "; ";
            newLine += loggedUser.StartLon.ToString() + "; ";
            newLine += loggedUser.CurrentLat.ToString() + "; ";
            newLine += loggedUser.CurrentLon.ToString() + "; ";
            newLine += loggedUser.Speed.ToString() + "; ";
            newLine += loggedUser.Accuracy.ToString() + "; ";
            newLine += loggedUser.OldDistanceT.ToString() + "; ";
            newLine += loggedUser.DistanceT.ToString() + "; ";
            newLine += loggedUser.OldDistanceS.ToString() + "; ";
            newLine += loggedUser.DistanceS.ToString() + "; ";
            newLine += loggedUser.OldDistanceN.ToString() + "; ";
            newLine += loggedUser.DistanceN.ToString() + "; ";
            newLine += loggedUser.PredictedDistance.ToString() + "; ";
            newLine += loggedUser.Alert.ToString() + "; ";
            newLine += loggedUser.LoopTime.ToString() + "; ";
            newLine += DateTime.Now.ToString("HH:mm:ss");

            string dataLogic = loggedUser.LogFilePath;

            if (File.Exists(dataLogic))
            {
                using (StreamWriter writer = new StreamWriter(loggedUser.LogFilePath, true))
                {
                    writer.WriteLine(newLine);
                    writer.Close();
                }
            }    
        }
    }
}