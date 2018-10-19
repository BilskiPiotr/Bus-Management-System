using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Device.Location;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Bus_Management_System
{
    public partial class Bus : System.Web.UI.Page
    {
        private static BusinessLayer bl = new BusinessLayer();
        private static DataAccessLayer dal = new DataAccessLayer();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string userId = "";

                if (Request.Cookies["Bus"] != null)
                {
                    userId = Convert.ToString(Request.Cookies["Bus"].Values["userId"]);

                    // dodać sprawdzenie, czy taka sesja już istnieje, a jeśli nie - to dodać
                    User loggedUser = (User)Session[userId];

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
                        locCookie.Values["distance"] = "";
                        locCookie.Values["startLocLat"] = "";
                        locCookie.Values["startLocLon"] = "";

                        Response.Cookies.Add(locCookie);

                }
                else
                    Response.Redirect("global.aspx");
            }
        }


        [System.Web.Services.WebMethod]
        public static string[] PrzeliczArray(string[] arrayIn)
        {
            double latitude = double.Parse(arrayIn[0], CultureInfo.InvariantCulture);
            double longitude = double.Parse(arrayIn[1], CultureInfo.InvariantCulture);
            double distance = 0.0;

            //dopisanie surowych danych do HttpCookie
            HttpCookie locCookie = HttpContext.Current.Request.Cookies.Get("locCookie");

            if (locCookie != null)
            {
                locCookie.Values["currentLocLat"] = latitude.ToString();
                locCookie.Values["currentLocLon"] = longitude.ToString();
                HttpContext.Current.Response.Cookies.Add(locCookie);
            }

            // aktualne współrzędne podane ze ClientSide w tablicy
            GeoCoordinate objectPosition = new GeoCoordinate(latitude, longitude);

            // współrzędne docelowe, pobierane z wysłanego zlecenia
            GeoCoordinate targetPosition = new GeoCoordinate(52.17021166666667, 20.971659999999996);

            // zwrocenie odleglosci miedzy wspolrzednymi z ograniczeniem do 2 miejsc po przecinku
            distance = Math.Round(objectPosition.GetDistanceTo(targetPosition), 2, MidpointRounding.AwayFromZero);

            string latitude_Kierunek = (latitude >= 0 ? "N" : "S");

            latitude = Math.Abs(latitude);
            double minutyLat = ((latitude - Math.Truncate(latitude) / 1) * 60);
            double sekundyLat = ((minutyLat - Math.Truncate(minutyLat) / 1) * 60);

            string longitude_Kierunek = (longitude >= 0 ? "E" : "W");
            longitude = Math.Abs(longitude);
            double minutyLon = ((longitude - Math.Truncate(longitude) / 1) * 60);
            double sekundyLon = ((minutyLon - Math.Truncate(minutyLon) / 1) * 60);

            string wsp1 = Convert.ToString(Math.Truncate(latitude) + "° " + Math.Truncate(minutyLat) + "' " + Math.Truncate(sekundyLat) + "'' " + latitude_Kierunek);
            string wsp2 = Convert.ToString(Math.Truncate(longitude) + "° " + Math.Truncate(minutyLon) + "' " + Math.Truncate(sekundyLon) + "'' " + longitude_Kierunek);

            string[] wynikowaArray = new string[3];

            wynikowaArray[0] = wsp1;
            wynikowaArray[1] = wsp2;
            wynikowaArray[2] = distance.ToString();

            return wynikowaArray;
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
            if (cookie != null)
            {
                int operationStatus = Convert.ToInt32(cookie.Values["operationStatus"]);
                int interval = Convert.ToInt32(cookie.Values["interval"]);
                string bus = cookie.Values["busNb"].ToString();


                if (cookie.Values["interval"] == "0")
                {
                    DataSet ds = bl.GetOperations(cookie.Values["busNb"].ToString());

                    // sprawdzenie, czy pojawiła się operacja
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                            string createdStatus = (ds.Tables[0].Rows[0].Field<DateTime>("Created")).ToString("HH:mm");
                            string acceptedStatus = (ds.Tables[0].Rows[0].Field<DateTime>("Accepted")).ToString("HH:mm");
                            string loadStatus = (ds.Tables[0].Rows[0].Field<DateTime>("StartLoad")).ToString("HH:mm");
                            string driveStatus = (ds.Tables[0].Rows[0].Field<DateTime>("StartDrive")).ToString("HH:mm");
                            string unloadStatus = (ds.Tables[0].Rows[0].Field<DateTime>("StartUnload")).ToString("HH:mm");
                            string endStatus = (ds.Tables[0].Rows[0].Field<DateTime>("EndOp")).ToString("HH:mm");
                            if (createdStatus != "00:00")
                                operationStatus = 1;
                            if (acceptedStatus != "00:00")
                                operationStatus = operationStatus + 1;
                            if (loadStatus != "00:00")
                                operationStatus = operationStatus + 1;
                            if (driveStatus != "00:00")
                                operationStatus = operationStatus + 1;
                            if (unloadStatus != "00:00")
                                operationStatus = operationStatus + 1;
                            if (endStatus != "00:00")
                                operationStatus = 0;

                        cookie.Values["operationStatus"] = operationStatus.ToString();
                        Response.Cookies.Add(cookie);

                            /* operationStatus wartości możliwe:
                            * 0 - brak zlecenia       <-
                            * 1 - zlecenie utworzone    |
                            * 2 - zlecenie przyjete     |
                            * 3 - rozpoczęty załadunek  |
                            * 4 - dowóz pasażerów       |
                            * 5 - rozpoczęty wyładunek >|
                            */

                            HttpCookie opCookie = Request.Cookies["opCookie"];
                            if (opCookie == null)
                            {
                                opCookie = OpCookie.CreateCookie(ds);
                                Response.Cookies.Add(opCookie);
                            }
                            else
                        {
                            opCookie = OpCookie.RebiuldCookie(ds);
                            Response.Cookies.Add(opCookie);
                        }

                            InWorkBusControls(operationStatus);

                        interval = interval + 5;
                        cookie.Values["interval"] = Convert.ToString(interval);
                        Response.Cookies.Add(cookie);
                    }
                }
                else
                if (cookie.Values["interval"] == "20")
                {
                    cookie.Values["interval"] = "0";
                    Response.Cookies.Add(cookie);
                }
                else
                {
                    interval = interval + 5;
                    cookie.Values["interval"] = Convert.ToString(interval);
                    Response.Cookies.Add(cookie);
                }
                

                SetButtonsStatus(operationStatus);
                InWorkBusControls(operationStatus);
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
            HttpCookie opCookie = Request.Cookies["opCookie"];

            int operation = bl.GetOperations(Convert.ToInt32(opCookie.Values["operation"]));
            int shengen = Convert.ToInt32(opCookie.Values["shengen"]);

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
            HttpCookie opCookie = Request.Cookies["opCookie"];

            if (operation == 1)
            {
                R1C2.Style.Add(HtmlTextWriterStyle.BackgroundImage, "pictures/sso.png");
                R1C4.Style.Add(HtmlTextWriterStyle.BackgroundImage, "pictures/sso.png");
                R1C3.Text = opCookie.Values["airPort"].ToString();
                R4C2.Text = opCookie.Values["gate"].ToString();
                R4C4.Text = opCookie.Values["pps"].ToString();
                R5C3.Text = "WAW";
            }
            else
            {
                R1C2.Style.Add(HtmlTextWriterStyle.BackgroundImage, "pictures/ssp.png");
                R1C4.Style.Add(HtmlTextWriterStyle.BackgroundImage, "pictures/ssp.png");
                R1C3.Text = "WAW";
                R4C2.Text = opCookie.Values["pps"].ToString();
                R4C4.Text = opCookie.Values["gate"].ToString();
                R5C3.Text = opCookie.Values["airPort"].ToString();
            }

            R1C2.Style.Add("background-repeat", "no-repeat");
            R1C4.Style.Add("background-repeat", "no-repeat");
            R1C2.Style.Add("background-size", "100% 100%");
            R1C4.Style.Add("background-size", "100% 100%");

            R1C3.Style.Add("color", "Grey");
            R3C2.Text = opCookie.Values["flightNb"].ToString();
            R3C2.Style.Add("color", "Grey");
            R3C3.Text = opCookie.Values["godzinaRozkladowa"].ToString();
            R3C3.Style.Add("color", "Grey");
            R3C4.Text = opCookie.Values["pax"].ToString(); ;
            R3C4.Style.Add("color", "Grey");
            R4C2.Style.Add("color", "Grey");
            R4C3.Style.Add("color", "Grey");
            R4C4.Style.Add("color", "Grey");
            R5C3.Style.Add("color", "Grey");
        }

        private void Odlot(int shengen)
        {
            HttpCookie opCookie = Request.Cookies["opCookie"];
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
                Dr1C2.Style.Add(HtmlTextWriterStyle.BackgroundImage, "pictures/sn.png");
                Dr1C2.Style.Add("background-repeat", "no-repeat");
                Dr1C2.Style.Add("background-size", "100% 100%");
                Dr1C4.Style.Add(HtmlTextWriterStyle.BackgroundImage, "pictures/sn.png");
                Dr1C4.Style.Add("background-repeat", "no-repeat");
                Dr1C4.Style.Add("background-size", "100% 100%");

                if (cookie.Values["operationStatus"].ToString() == "2" || cookie.Values["operationStatus"].ToString() == "4")
                {
                    if (opCookie.Values["startLocLatDegree"].ToString() == "" || opCookie.Values["startLocLonDegree"].ToString() == "")
                    {
                        string lat = "";
                        string lon = "";
                        TranslateColToDegree(ref lat, ref lon);
                        opCookie.Values["startLocLatDegree"] = lat;
                        opCookie.Values["startLocLonDegree"] = lon;
                    }
                    Dr2C2.Text = opCookie.Values["startLocLatDegree"].ToString();
                    Dr2C3.Text = "";
                    Dr2C4.Text = opCookie.Values["startLocLonDegree"].ToString();
                    Dr5C3.Text = opCookie.Values["gate"].ToString();

                    double distance = CheckDistance(opCookie, 2);

                    if (cookie.Values["operationStatus"].ToString() == "2")
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
                    Dr2C3.Text = opCookie.Values["gate"].ToString();
                    Dr5C3.Text = opCookie.Values["pps"].ToString();
                }
            }
        }

        private double CheckDistance(HttpCookie opCookie, int operation)
        {
            HttpCookie locCookie = Request.Cookies["locCookie"];
            double busLat = Convert.ToDouble(locCookie.Values["currentLocLat"]);
            double busLon = Convert.ToDouble(locCookie.Values["currentLocLon"]);

            double targetLat = 0.0;
            double targetLon = 0.0;

            if (operation == 1)
            {
                targetLat = Convert.ToDouble(opCookie.Values["gateLat"], CultureInfo.InvariantCulture);
                targetLon = Convert.ToDouble(opCookie.Values["gateLon"], CultureInfo.InvariantCulture);
            }
            else
            {
                targetLat = double.Parse(opCookie.Values["ppsLat"], CultureInfo.InvariantCulture);
                targetLon = Convert.ToDouble(opCookie.Values["ppsLon"], CultureInfo.InvariantCulture);
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
            HttpCookie opCookie = Request.Cookies["opCookie"];

            // nie no - trzeba pobrac te współrzędne do ciasteczka podczas tworzenia OpCookie
            switch (opCookie.Values["operation"].ToString())
            {
                case "1":
                    {
                        cmd = new SqlCommand("SELECT GPS_Latitude, GPS_Longitude FROM Stations WHERE StationNb = @pps");
                        cmd.Parameters.AddWithValue("@pps", opCookie.Values["pps"].ToString());
                        DataSet ds = dal.GetDataSet(cmd);
                    }
                    break;
                case "2":
                    break;
            }

            opCookie.Values["startLocLat"] = locCookie.Values["currentLocLat"].ToString();
            opCookie.Values["startLocLon"] = locCookie.Values["currentLocLon"].ToString();
            opCookie.Values["startLocLatDegree"] = lat;
            opCookie.Values["startLocLonDegree"] = lon;
            Response.Cookies.Add(opCookie);
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
    }
}