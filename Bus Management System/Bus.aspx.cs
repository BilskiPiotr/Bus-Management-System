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
                        Session.Abandon();
                        Response.Redirect("global.aspx");
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
                SqlCommand cmd = new SqlCommand("SELECT * FROM Operations WHERE Bus=(SELECT Id FROM Vehicles WHERE VehicleNb = @busNb)");
                cmd.Parameters.AddWithValue("@busNb", cookie.Values["busNb"].ToString());
                int shengen = 0;
                string dt = "";
                try
                {
                    DataSet ds = dal.GetDataSet(cmd);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                         dt = ds.Tables[0].Rows[0].Field<DateTime>("Accepted").ToString();
                        if (dt == "1999-01-01 00:00:00")
                        {
                            GreyScreen(ds, shengen);
                        }
                        else
                        {
                            int operation = bl.GetOperations(ds.Tables[0].Rows[0].Field<int>("Operation"));

                            R1C3.Text = bl.GetPPS(ds.Tables[0].Rows[0].Field<int>("PPS"));
                            R3C2.Text = ds.Tables[0].Rows[0].Field<string>("FlightNb");
                            R3C2.Style.Add("color", "Black");
                            R3C3.Text = "00:00";
                            R3C3.Style.Add("color", "Black");
                            R3C4.Text = ds.Tables[0].Rows[0].Field<int>("Pax").ToString();
                            R3C4.Style.Add("color", "Black");
                            switch (operation)
                            {
                                case 0:
                                    Przylot(ds, shengen);
                                    break;
                                case 1:
                                    Odlot(ds, shengen);
                                    break;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    R1C3.Text = DateTime.Now.ToString("hh:mm");
                    IddleBusControls();
                }
            }
            else
            {
                Session.Abandon();
                Response.Redirect("global.aspx");
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
            //td1.Style.Add(HtmlTextWriterStyle.BackgroundImage, "Images/7.jpg");
        }

        protected void BusAccept_Click(object sender, EventArgs e)
        {
            HttpCookie cookie = Request.Cookies["Bus"];
            string bus = cookie.Values["busNb"].ToString();
            SqlCommand cmd = new SqlCommand("UPDATE Operations SET Accepted = @accepted WHERE Bus = (SELECT Id FROM Vehicles WHERE VehicleNb = @busNb)");
            cmd.Parameters.AddWithValue("@busNb", bus);
            cmd.Parameters.AddWithValue("@accepted", DateTime.Now);
            dal.QueryExecution(cmd);
        }

        private void GreyScreen(DataSet ds, int shengen)
        {
            R1C2.Style.Add(HtmlTextWriterStyle.BackgroundImage, "");
            R1C2.Style.Add("background-color", "grey");
            R1C3.Text = bl.GetPPS(ds.Tables[0].Rows[0].Field<int>("PPS"));
            R1C3.Style.Add("color", "grey");
            R1C4.Style.Add(HtmlTextWriterStyle.BackgroundImage, "");
            R1C4.Style.Add("background-color", "grey");
            R3C2.Text = ds.Tables[0].Rows[0].Field<string>("FlightNb");
            R3C2.Style.Add("color", "grey");
            R3C3.Text = "00:00";
            R3C3.Style.Add("color", "grey");
            R3C4.Text = ds.Tables[0].Rows[0].Field<int>("Pax").ToString();
            R3C4.Style.Add("color", "grey");
            R4C2.Text = "WAW";
            R4C2.Style.Add("color", "grey");
            R4C3.Style.Add("color", "grey");
            R4C4.Text = bl.GetAirPort(ds.Tables[0].Rows[0].Field<int>("AirPort"), ref shengen);
            R4C4.Style.Add("color", "grey");
            R5C3.Text = bl.GetGate(ds.Tables[0].Rows[0].Field<int>("Gate")).ToString();
            R5C3.Style.Add("color", "grey");
            busAccept.Enabled = true;
            busAccept.Style.Add("background-color", "Green");
            busAccept.Style.Add("color", "White");
        }

        private void Odlot(DataSet ds, int shengen)
        {
            R1C2.Style.Add(HtmlTextWriterStyle.BackgroundImage, "pictures/sn.png");
            R1C2.Style.Add("background-repeat", "no-repeat");
            R1C2.Style.Add("background-size", "100% 100%");
            R1C4.Style.Add(HtmlTextWriterStyle.BackgroundImage, "pictures/sn.png");
            R1C4.Style.Add("background-repeat", "no-repeat");
            R1C4.Style.Add("background-size", "100% 100%");
            R4C2.Text = "WAW";
            R4C4.Text = bl.GetAirPort(ds.Tables[0].Rows[0].Field<int>("AirPort"), ref shengen);
            R5C3.Text = bl.GetGate(ds.Tables[0].Rows[0].Field<int>("Gate")).ToString();
        }

        private void Przylot(DataSet ds, int shengen)
        {
            R4C2.Text = bl.GetAirPort(ds.Tables[0].Rows[0].Field<int>("AirPort"), ref shengen);
            R4C2.Style.Add("color", "Black");
            if (shengen == 0)
            {
                R1C2.Style.Add(HtmlTextWriterStyle.BackgroundImage, "pictures/sz.png");
                R1C2.Style.Add("background-repeat", "no-repeat");
                R1C2.Style.Add("background-size", "100% 100%");
                R4C2.Text = bl.GetAirPort(ds.Tables[0].Rows[0].Field<int>("AirPort"), ref shengen);
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
    }
}