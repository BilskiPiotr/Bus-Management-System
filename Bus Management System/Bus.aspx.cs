using System;
using System.Data;
using System.Data.SqlClient;
using System.Device.Location;
using System.Globalization;
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
        private static string audioAlert = "";
        User loggedUser;

        protected void Page_Load(object sender, EventArgs e)
        {
            string userId = "";

            if (Request.Cookies["Bus"] != null)
            {
                userId = Convert.ToString(Request.Cookies["Bus"].Values["userId"]);
            }
            else
                Response.Redirect("global.aspx");

            loggedUser = (User)Session[userId];

            if (!IsPostBack)
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "przeliczOdleglosc", "getLocation();", true);

                MenuItemCollection menuItems = busMenu.Items;

                if (loggedUser != null)
                {
                    lb_loggedUser.Text = "";
                    lb_loggedUser.Text += (string)loggedUser.FirstName + " " + (string)loggedUser.LastName + "       ID: " + ((int)loggedUser.CompanyId).ToString();

                    // załadowanie danych do psnelu Operatora
                    BindDdlData();
                }

                HttpCookie locCookie = new HttpCookie("locCookie");

                locCookie.Values["currentLocLat"] = "";
                locCookie.Values["currentLocLon"] = "";
                locCookie.Values["currentSpeed"] = "";
                locCookie.Values["distance"] = "";
                locCookie.Values["startLocLat"] = "";
                locCookie.Values["startLocLon"] = "";

                Response.Cookies.Add(locCookie);
            }
        }


        [System.Web.Services.WebMethod]
        public static string PrzeliczArray(string[] arrayIn)
        {
            double latitude = double.Parse(arrayIn[0], CultureInfo.InvariantCulture);
            double longitude = double.Parse(arrayIn[1], CultureInfo.InvariantCulture);
            string accuracy = arrayIn[2].ToString();
            string speed = arrayIn[3].ToString();
            //double distance = 0.0;

            //dopisanie surowych danych do HttpCookie
            HttpCookie locCookie = HttpContext.Current.Request.Cookies.Get("locCookie");

            if (locCookie != null)
            {
                locCookie.Values["currentLocLat"] = latitude.ToString();
                locCookie.Values["currentLocLon"] = longitude.ToString();
                locCookie.Values["currentSpeed"] = speed;
                HttpContext.Current.Response.Cookies.Add(locCookie);
            }

            //// aktualne współrzędne podane ze ClientSide w tablicy
            //GeoCoordinate objectPosition = new GeoCoordinate(latitude, longitude);

            //// współrzędne docelowe, pobierane z wysłanego zlecenia
            //GeoCoordinate targetPosition = new GeoCoordinate(52.17021166666667, 20.971659999999996);

            //// zwrocenie odleglosci miedzy wspolrzednymi z ograniczeniem do 2 miejsc po przecinku
            //distance = Math.Round(objectPosition.GetDistanceTo(targetPosition), 2, MidpointRounding.AwayFromZero);

            //string latitude_Kierunek = (latitude >= 0 ? "N" : "S");

            //latitude = Math.Abs(latitude);
            //double minutyLat = ((latitude - Math.Truncate(latitude) / 1) * 60);
            //double sekundyLat = ((minutyLat - Math.Truncate(minutyLat) / 1) * 60);

            //string longitude_Kierunek = (longitude >= 0 ? "E" : "W");
            //longitude = Math.Abs(longitude);
            //double minutyLon = ((longitude - Math.Truncate(longitude) / 1) * 60);
            //double sekundyLon = ((minutyLon - Math.Truncate(minutyLon) / 1) * 60);

            //string wsp1 = Convert.ToString(Math.Truncate(latitude) + "° " + Math.Truncate(minutyLat) + "' " + Math.Truncate(sekundyLat) + "'' " + latitude_Kierunek);
            //string wsp2 = Convert.ToString(Math.Truncate(longitude) + "° " + Math.Truncate(minutyLon) + "' " + Math.Truncate(sekundyLon) + "'' " + longitude_Kierunek);

            //string[] wynikowaArray = new string[3];

            //wynikowaArray[0] = wsp1;
            //wynikowaArray[1] = wsp2;
            //wynikowaArray[2] = distance.ToString();

            return accuracy;
        }




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
                            bl.UserLogOut(cookie);
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


        private void BindDdlData()
        {
            if (Request.Cookies["Bus"] != null)
            {
                SqlCommand cmd = new SqlCommand("SELECT Id, VehicleNb FROM Vehicles");
                DataSet ds = dal.GetDataSet(cmd);
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

        protected void Bt_busSelect_Click(object sender, EventArgs e)
        {
            this.busMenu.Items[0].Selectable = true;
            this.busMenu.Items[1].Selectable = true;

            HttpCookie cookie = Request.Cookies["Bus"];
            if (cookie != null)
            {
                // dodanie wybranewgo autobusu do ciasteczka z operatorem
                string bus = ddl_busSelect.SelectedItem.ToString();
                cookie.Values["busNb"] = bus;
                // i nadpisujemy ciasteczko
                Response.Cookies.Add(cookie);

                // zmiana tekstu dla menu Home na numer zalogowanego autobusu
                this.busMenu.Items[0].Text = bus;
                SqlCommand cmd = new SqlCommand("UPDATE Vehicles SET Bus_Status = @busStatus WHERE VehicleNb = @busNb");
                cmd.Parameters.AddWithValue("@busStatus", 2);
                cmd.Parameters.AddWithValue("@busNb", bus);
                dal.QueryExecution(cmd);
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

        protected void BusHomeTimer_Tick(object sender, EventArgs e)
        {
            
            HttpCookie cookie = Request.Cookies["Bus"];

            audioAlert = "/audio/danger.wav";

            HtmlGenericControl sound = new HtmlGenericControl("<embed src=\"" + audioAlert + "\" type=\"audio/wav\" autostart =\"true\" hidden=\"true\"></embed>");
            BusHomeUP.ContentTemplateContainer.Controls.Remove(sound);
            BusHomeUP.ContentTemplateContainer.Controls.Add(sound);

            //Button1_Click(null, null);
            //Button1.OnClientClick();

            if (cookie != null)
            {
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "CallMyFunction", "PlaySound()", true);

                int operationStatus = loggedUser.OperationStatus;
                //int interval = loggedUser.Interval;
                string bus = cookie.Values["busNb"].ToString();

                    DataSet ds = bl.GetOperations(cookie.Values["busNb"].ToString());

                    // sprawdzenie, czy pojawiła się operacja
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        int shengen = 0;
                        string portName = "";
                        string country = "";

                        DataSet pps = bl.GetPPS(ds.Tables[0].Rows[0].Field<int>("PPS"));
                        DataSet gate = bl.GetGate(ds.Tables[0].Rows[0].Field<int>("Gate"));

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

                        loggedUser.Operation = ds.Tables[0].Rows[0].Field<int>("Operation");
                        loggedUser.FlightNb = ds.Tables[0].Rows[0].Field<string>("FlightNb");
                        loggedUser.Pax = ds.Tables[0].Rows[0].Field<int>("Pax").ToString();
                        loggedUser.AirPort = bl.GetAirPort(ds.Tables[0].Rows[0].Field<int>("AirPort"), ref shengen, ref portName, ref country);
                        loggedUser.Pps = pps.Tables[0].Rows[0].Field<string>("StationNb");
                        loggedUser.PpsLat = pps.Tables[0].Rows[0].Field<string>("GPS_Latitude");
                        loggedUser.PpsLon = pps.Tables[0].Rows[0].Field<string>("GPS_Longitude");
                        loggedUser.Gate = gate.Tables[0].Rows[0].Field<string>("GateNb");
                        loggedUser.GateLat = gate.Tables[0].Rows[0].Field<string>("GPS_Latitude");
                        loggedUser.GateLon = gate.Tables[0].Rows[0].Field<string>("GPS_Longitude");
                        loggedUser.RadioGate = ds.Tables[0].Rows[0].Field<string>("RadioGate").ToString();
                        loggedUser.RadioNeon = ds.Tables[0].Rows[0].Field<string>("RadioNeon").ToString();
                        loggedUser.Shengen = shengen;
                        loggedUser.PortName = portName;
                        loggedUser.Country = country;

                        loggedUser.OperationStatus = operationStatus;

                        /* operationStatus wartości możliwe:
                        * 0 - brak zlecenia       <-
                        * 1 - zlecenie utworzone    |
                        * 2 - zlecenie przyjete     |
                        * 3 - rozpoczęty załadunek  |
                        * 4 - dowóz pasażerów       |
                        * 5 - rozpoczęty wyładunek >|
                        */

                        SetButtonsStatus(operationStatus);
                        InWorkBusControls(operationStatus);
                    }
                    else
                    {
                        IddleBusControls();
                    }
            }
            // "buss" cookie nie istnieje, wiedz na wszelki wypadek koniec sesji i wylogowanie
            else
            {
                // wylogować użytkownika zapisanego w sesji oraz zwolnic BUS
                Session.Abandon();
                Response.Redirect("global.aspx");
            }
        }


        // ustawienie kolorów aktywnych dla wszystrkich przycisków na stronie bus
        private void InWorkBusControls(int operationStatus)
        {
            int operation = bl.GetOperations(loggedUser.Operation);
            int shengen = loggedUser.Shengen;

            if (operationStatus == 1)
                GreyScreen(operation, shengen);
            else
            {
                switch (operation)
                {
                    case 0:
                        Przylot(shengen);
                        break;
                    case 1:
                        Odlot(shengen);
                        break;
                }
            }
        }



        private void SetButtonsStatus(int operationStatus)
        {

            if (operationStatus == 1)
            {
                busAccept.Style.Add("background-color", "#1a993d");
                busAccept.Enabled = true;
            }
            else
                if (operationStatus == 2)
            {
                busAccept.Style.Add("background-color", "#a63d40");
                busAccept.Enabled = false;
                busStartLoad.Style.Add("background-color", "#1a993d");
                busStartLoad.Enabled = true;
                busMINEtable.Visible = false;
                busDriveTable.Visible = true;

            }
            else
                if (operationStatus == 3)
            {
                busStartLoad.Style.Add("background-color", "#a63d40");
                busStartLoad.Enabled = false;
                busStartDrive.Style.Add("background-color", "#1a993d");
                busStartDrive.Enabled = true;
                busMINEtable.Visible = true;
                busDriveTable.Visible = false;
            }
            else
                if (operationStatus == 4)
            {
                busStartDrive.Style.Add("background-color", "#a63d40");
                busStartDrive.Enabled = false;
                busStartUnload.Style.Add("background-color", "#1a993d");
                busStartUnload.Enabled = true;
                busMINEtable.Visible = false;
                busDriveTable.Visible = true;
            }
            else
                if (operationStatus == 5)
            {
                busStartUnload.Style.Add("background-color", "#a63d40");
                busStartUnload.Enabled = false;
                busEndOp.Style.Add("background-color", "#1a993d");
                busEndOp.Enabled = true;
                busMINEtable.Visible = true;
                busDriveTable.Visible = false;
            }
            else
            {
                busAccept.Style.Add("background-color", "#a63d40");
                busAccept.Enabled = false;
                busStartLoad.Style.Add("background-color", "#a63d40");
                busStartLoad.Enabled = false;
                busStartDrive.Style.Add("background-color", "#a63d40");
                busStartDrive.Enabled = false;
                busStartUnload.Style.Add("background-color", "#a63d40");
                busStartUnload.Enabled = false;
                busEndOp.Style.Add("background-color", "#a63d40");
                busEndOp.Enabled = false;
                busMINEtable.Visible = true;
                busDriveTable.Visible = false;
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

        private void Odlot(int shengen)
        {
            HttpCookie cookie = Request.Cookies["Bus"];

            if (busMINEtable.Visible == true)
            {
                R1C2.Style.Add(HtmlTextWriterStyle.BackgroundImage, "pictures/sn.png");
                R1C2.Style.Add("background-repeat", "no-repeat");
                R1C2.Style.Add("background-size", "100% 100%");
                R1C4.Style.Add(HtmlTextWriterStyle.BackgroundImage, "pictures/sn.png");
                R1C4.Style.Add("background-repeat", "no-repeat");
                R1C4.Style.Add("background-size", "100% 100%");
                R4C2.Text = "WAW";
                R4C4.Text = "";
                R5C3.Text = "";
            }
            else
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

                    double distance = CheckDistance(loggedUser.GateLat, loggedUser.GateLon, loggedUser.PpsLat, loggedUser.PpsLon, 2);

                    if (loggedUser.OperationStatus == 2)
                    {
                        if (distance > 10.0d)
                        {
                            Dr3C3.Text = distance.ToString() + " m";
                            Dr3C3.Style.Add("color", "Violet");
                        }
                        else
                        {
                            Dr3C3.Text = "OK!";
                            Dr3C3.Style.Add("color", "Green");
                        }
                    }
                    else
                        if (distance > 20.0d)
                    {
                        Dr3C3.Text = distance.ToString() + " m";
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

        private double CheckDistance(string gateLat, string gateLon, string ppsLat, string ppsLon, int operation)
        {
            HttpCookie locCookie = Request.Cookies["locCookie"];
            double busLat = Convert.ToDouble(locCookie.Values["currentLocLat"]);
            double busLon = Convert.ToDouble(locCookie.Values["currentLocLon"]);

            double targetLat = 0.0;
            double targetLon = 0.0;

            if (operation == 1)
            {
                targetLat = Convert.ToDouble(gateLat, CultureInfo.InvariantCulture);
                targetLon = Convert.ToDouble(gateLon, CultureInfo.InvariantCulture);
            }
            else
            {
                targetLat = Convert.ToDouble(ppsLat, CultureInfo.InvariantCulture);
                targetLon = Convert.ToDouble(ppsLon, CultureInfo.InvariantCulture);
            }
            
            GeoCoordinate busPosition = new GeoCoordinate(busLat, busLon);
            GeoCoordinate targetPosition = new GeoCoordinate(targetLat, targetLon);

            // zwrocenie odleglosci miedzy wspolrzednymi z ograniczeniem do 2 miejsc po przecinku
            double distance = Math.Round(busPosition.GetDistanceTo(targetPosition), 2, MidpointRounding.AwayFromZero);

            return distance;
        }


        private void Przylot(int shengen)
        {
            R4C2.Style.Add("color", "Black");
            if (shengen == 0)
            {
                R1C3.Style.Add("color", "Green");
                R1C2.Style.Add(HtmlTextWriterStyle.BackgroundImage, "pictures/sz.png");
                R1C2.Style.Add("background-repeat", "no-repeat");
                R1C2.Style.Add("background-size", "100% 100%");
                R4C2.Text = "";
                R4C3.Style.Add("color", "Green");
                R1C4.Style.Add(HtmlTextWriterStyle.BackgroundImage, "pictures/sz.png");
                R1C4.Style.Add("background-repeat", "no-repeat");
                R1C4.Style.Add("background-size", "100% 100%");
                R5C3.Style.Add("color", "Green");
                R5C3.Text = "SHENGEN";
            }
            else
            {
                R1C2.Style.Add(HtmlTextWriterStyle.BackgroundImage, "pictures/sc.png");
                R1C2.Style.Add("background-repeat", "no-repeat");
                R1C2.Style.Add("background-size", "100% 100%");
                R4C3.Style.Add("color", "Red");
                R1C4.Style.Add(HtmlTextWriterStyle.BackgroundImage, "pictures/sc.png");
                R1C4.Style.Add("background-repeat", "no-repeat");
                R1C4.Style.Add("background-size", "100% 100%");
                R5C3.Style.Add("color", "Red");
                R5C3.Text = "NON SHENGEN";
            }
            R4C4.Text = "WAW";
            R4C4.Style.Add("color", "Black");
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

            cookie.Values["operationStatus"]  = "2";
            Response.Cookies.Add(cookie);

            string lat = "";
            string lon = "";
            
            TranslateColToDegree(ref lat, ref lon);

            HttpCookie locCookie = Request.Cookies["locCookie"];
            int operation = loggedUser.Operation;

            // nie no - trzeba pobrac te współrzędne do ciasteczka podczas tworzenia OpCookie
            switch(operation)
            {
                case 1:
                    {
                        cmd = new SqlCommand("SELECT GPS_Latitude, GPS_Longitude FROM Stations WHERE StationNb = @pps");
                        cmd.Parameters.AddWithValue("@pps", loggedUser.Pps);
                        DataSet ds = dal.GetDataSet(cmd);
                    }
                    break;
                case 2:
                    break;
            }
            loggedUser.StartLat = locCookie.Values["currentLocLat"].ToString();
            loggedUser.StartLon = locCookie.Values["currentLocLon"].ToString();
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
        }

        protected void BusStartDrive_Click(object sender, EventArgs e)
        {
            HttpCookie cookie = Request.Cookies["Bus"];
            string bus = cookie.Values["busNb"].ToString();
            SqlCommand cmd = new SqlCommand("UPDATE Operations SET StartDrive = @startDrive WHERE Bus = (SELECT Id FROM Vehicles WHERE VehicleNb = @busNb)");
            cmd.Parameters.AddWithValue("@busNb", bus);
            cmd.Parameters.AddWithValue("@startDrive", DateTime.Now);
            dal.QueryExecution(cmd);
        }

        protected void BusStartUnload_Click(object sender, EventArgs e)
        {
            HttpCookie cookie = Request.Cookies["Bus"];
            string bus = cookie.Values["busNb"].ToString();
            SqlCommand cmd = new SqlCommand("UPDATE Operations SET StartUnload = @startUnload WHERE Bus = (SELECT Id FROM Vehicles WHERE VehicleNb = @busNb)");
            cmd.Parameters.AddWithValue("@busNb", bus);
            cmd.Parameters.AddWithValue("@startUnload", DateTime.Now);
            dal.QueryExecution(cmd);
        }

        protected void BusEndOp_Click(object sender, EventArgs e)
        {
            HttpCookie cookie = Request.Cookies["Bus"];
            string bus = cookie.Values["busNb"].ToString();
            SqlCommand cmd = new SqlCommand("UPDATE Operations SET EndOp = @endOp WHERE Bus = (SELECT Id FROM Vehicles WHERE VehicleNb = @busNb)");
            cmd.Parameters.AddWithValue("@busNb", bus);
            cmd.Parameters.AddWithValue("@endOp", DateTime.Now);
            dal.QueryExecution(cmd);
        }

        protected void BusPause_Click(object sender, EventArgs e)
        {

        }


        private void TranslateColToDegree(ref string lat, ref string lon)
        {
            HttpCookie locCookie = Request.Cookies["locCookie"];

            double latitude = double.Parse(locCookie.Values["currentLocLat"]);
            double longitude = double.Parse(locCookie.Values["currentLocLon"]);

            string latitude_Kierunek = (latitude >= 0 ? "N" : "S");

            latitude = Math.Abs(latitude);
            double minutyLat = ((latitude - Math.Truncate(latitude) / 1) * 60);
            double sekundyLat = ((minutyLat - Math.Truncate(minutyLat) / 1) * 60);

            string longitude_Kierunek = (longitude >= 0 ? "E" : "W");
            longitude = Math.Abs(longitude);
            double minutyLon = ((longitude - Math.Truncate(longitude) / 1) * 60);
            double sekundyLon = ((minutyLon - Math.Truncate(minutyLon) / 1) * 60);

            lat = String.Format(Convert.ToString(Math.Truncate(latitude) + "° " +  + Math.Truncate(minutyLat) + "' " + Math.Truncate(sekundyLat) + "'' " + latitude_Kierunek));
            lon = String.Format(Convert.ToString(Math.Truncate(longitude) + "° " + Math.Truncate(minutyLon) + "' " + Math.Truncate(sekundyLon) + "'' " + longitude_Kierunek));
        }

        //protected void Button1_Click(object sender, EventArgs e)
        //{
        //    Page.ClientScript.RegisterStartupScript(this.GetType(), "callSound", "PlaySound()", true);
        //}
    }
}