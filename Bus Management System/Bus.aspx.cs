﻿using System;
using System.Collections.Generic;
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

                if (Request.Cookies["BusManagement"] != null)
                {
                    userId = Convert.ToString(Request.Cookies["BusManagement"].Values["userId"]);
                }

                // dodać sprawdzenie, czy taka sesja już istnieje, a jeśli nie - to dodać
                User loggedUser = (User)Session[userId];

                ScriptManager.RegisterClientScriptBlock(this, GetType(), "przeliczOdleglosc", "getLocation();", true);

                MenuItemCollection menuItems = busMenu.Items;

                if (loggedUser != null)
                {
                    lb_loggedUser.Text = "";
                    lb_loggedUser.Text += (string)loggedUser.FirstName + " " + (string)loggedUser.LastName + "       ID: " + ((int)loggedUser.CompanyId).ToString();

                    // załadowanie danych do psnelu Operatora
                    BindData();
                }
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


        private void BindData()
        {
            BusManagement.SetActiveView(Home);
        }
    }
}