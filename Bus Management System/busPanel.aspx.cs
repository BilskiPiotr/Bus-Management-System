using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Device.Location;
using System.Drawing;
using System.Globalization;
using System.Reflection;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Bus_Management_System
{
    public partial class busPanel : System.Web.UI.Page
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

                MenuItemCollection menuItems = mineMenu.Items;

                if (loggedUser != null)
                {
                    lb_loggedUser.Text = "";
                    lb_loggedUser.Text += (string)loggedUser.FirstName + " " + (string)loggedUser.LastName + "       ID: " + ((int)loggedUser.CompanyId).ToString();

                    LoadGates();
                    LoadStations();
                    LoadAirPorts();
                    LoadBus();
                    SetActiveView(loggedUser, menuItems);
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

        protected void Timer1_Tick(object sender, EventArgs e)
        {
            string dlugosc = lb_Latitude.Text;
            Label6.Text = "A teraz długość to : " + dlugosc;
            //DateTime.Now.ToLongTimeString();
        }

        protected void MineMenu_MenuItemClick(object sender, MenuEventArgs e)
        {
            User loggedUser = (User)Session["loggedUser"];
            MenuItem mnu = (MenuItem)e.Item;

            switch (mnu.Value)
            {
                case "1":
                    {
                        HomeView(loggedUser);
                    }
                    break;
                case "2":
                    {
                        BusManagement.SetActiveView(BusDetail);
                    }
                    break;
                case "3":
                    {
                        BusManagement.SetActiveView(Airport);
                    }
                    break;
                case "4":
                    {
                        BusManagement.SetActiveView(Country);
                    }
                    break;
                case "5":
                    {
                        BusManagement.SetActiveView(Employee);
                    }
                    break;
                case "6":
                    {
                        BusManagement.SetActiveView(Vehicle);
                    }
                    break;
                case "7":
                    {
                        Session.Abandon();
                        Response.Redirect("global.aspx");
                    }
                    break;
                default:
                    {
                        bl.ClearFields(bl);
                        ClearInputs(Page.Controls);
                        HomeView(loggedUser);
                    }
                    break;
            }

        }

        protected void Bt_resetSelfEdit_Click(object sender, EventArgs e)
        {

        }

        protected void Bt_submitSelfEdit_Click(object sender, EventArgs e)
        {

        }

        protected void Bt_abadonSelfEdit_Click(object sender, EventArgs e)
        {

        }

        protected void Bt_applySelfEdit_Click(object sender, EventArgs e)
        {

        }

        protected void Bt_errorConfirm_Click(object sender, EventArgs e)
        {
            Response.Redirect("gloabl.aspx");
        }

        protected void Bt_myPhoto_Click(object sender, EventArgs e)
        {

        }

        protected void Bt_error_Click1(object sender, EventArgs e)
        {

        }

        protected void Imgbt1_Click(object sender, ImageClickEventArgs e)
        {

        }

        protected void Imgbt2_Click(object sender, ImageClickEventArgs e)
        {

        }

        private void ClearInputs(ControlCollection ctrls)
        {
            foreach (Control ctrl in ctrls)
            {
                if (ctrl is TextBox)
                    ((TextBox)ctrl).Text = string.Empty;
                else if (ctrl is DropDownList)
                    ((DropDownList)ctrl).ClearSelection();

                ClearInputs(ctrl.Controls);
            }
        }


        private void SetActiveView(User loggedUser, MenuItemCollection menuItems)
        {
            MenuItem menuItem = new MenuItem();

            if (Convert.ToInt32(loggedUser.AdminPrivileges) == 0)
            {
                foreach (MenuItem item in menuItems)
                {
                    if (item.Value == "2")
                        menuItem = item;
                }
                menuItems.Remove(menuItem);
                menuItem = null;

                HomeView(loggedUser);
            }
            else
            if (Convert.ToInt32(loggedUser.AdminPrivileges) == 1)
            {
                foreach (MenuItem item in menuItems)
                {
                    if (item.Value == "2")
                        menuItem = item;
                }
                menuItems.Remove(menuItem);
                menuItem = null;

                foreach (MenuItem item in menuItems)
                {
                    if (item.Value == "3")
                        menuItem = item;
                }
                menuItems.Remove(menuItem);
                menuItem = null;

                foreach (MenuItem item in menuItems)
                {
                    if (item.Value == "4")
                        menuItem = item;
                }
                menuItems.Remove(menuItem);
                menuItem = null;

                foreach (MenuItem item in menuItems)
                {
                    if (item.Value == "5")
                        menuItem = item;
                }
                menuItems.Remove(menuItem);
                menuItem = null;

                foreach (MenuItem item in menuItems)
                {
                    if (item.Value == "6")
                        menuItem = item;
                }
                menuItems.Remove(menuItem);
                menuItem = null;

                HomeView(loggedUser);
            }
            else
            if (Convert.ToInt32(loggedUser.AdminPrivileges) == 2)
            {
                foreach (MenuItem item in menuItems)
                {
                    if (item.Value == "3")
                        menuItem = item;
                }
                menuItems.Remove(menuItem);
                menuItem = null;

                foreach (MenuItem item in menuItems)
                {
                    if (item.Value == "4")
                        menuItem = item;
                }
                menuItems.Remove(menuItem);
                menuItem = null;

                foreach (MenuItem item in menuItems)
                {
                    if (item.Value == "5")
                        menuItem = item;
                }
                menuItems.Remove(menuItem);
                menuItem = null;

                foreach (MenuItem item in menuItems)
                {
                    if (item.Value == "6")
                        menuItem = item;
                }
                menuItems.Remove(menuItem);
                menuItem = null;

                HomeView(loggedUser);
            }
            else
            if (loggedUser == null || Convert.ToInt32(loggedUser.AdminPrivileges) < 0 || Convert.ToInt32(loggedUser.AdminPrivileges) > 2)
            {
                Response.Redirect("global.aspx");
            }
        }


        private void HomeView(User loggedUser)
        {
            if (loggedUser.AdminPrivileges == 0)
                BusManagement.SetActiveView(Admin);
            else
                if (loggedUser.AdminPrivileges == 1)
                BusManagement.SetActiveView(Alocator);
            else
                if (loggedUser.AdminPrivileges == 2)
                BusManagement.SetActiveView(Bus);
        }

        [System.Web.Services.WebMethod]
        public static string GetCurrentTime(string name)
        {
            return name + DateTime.Now.ToString();
        }


        private void LoadGates()
        {
            DataSet gates = bl.GetGates();
            ddl_Gate.DataSource = gates;
            ddl_Gate.DataTextField = "GateNb";
            ddl_Gate.DataValueField = "Id";
            ddl_Gate.DataBind();

            // dodanie pierwszej linii do ddl
            ListItem gate = new ListItem("---", "-1");
            ddl_Gate.Items.Insert(0, gate);

            gates.Dispose();
        }


        private void LoadStations()
        {
            DataSet stations = bl.GetStations();
            ddl_PPS.DataSource = stations;
            ddl_PPS.DataTextField = "StationNb";
            ddl_PPS.DataValueField = "Id";
            ddl_PPS.DataBind();

            // dodanie pierwszej linii do ddl
            ListItem station = new ListItem("---", "-1");
            ddl_PPS.Items.Insert(0, station);

            stations.Dispose();
        }


        private void LoadAirPorts()
        {
            DataSet airPorts = bl.GetAirPort();
            ddl_Port.DataSource = airPorts;
            ddl_Port.DataTextField = "IATA_Name";
            ddl_Port.DataValueField = "Id";
            ddl_Port.DataBind();

            // dodanie pierwszej linii do ddl
            ListItem airport = new ListItem("---", "-1");
            ddl_Port.Items.Insert(0, airport);

            airPorts.Dispose();
        }

        private void LoadBus()
        {
            DataSet buses = bl.GetBus();
            ddl_Bus.DataSource = buses;
            ddl_Bus.DataTextField = "VehicleNb";
            ddl_Bus.DataValueField = "Id";
            ddl_Bus.DataBind();

            // dodanie pierwszej linii do ddl
            ListItem bus = new ListItem("---", "-1");
            ddl_Bus.Items.Insert(0, bus);

            SetBusStatus(buses);

            buses.Dispose();
        }


        private void SetBusStatus(DataSet buses)
        {
            for (int i = 0; i < buses.Tables[0].Rows.Count; i++)
            {
                string numer = buses.Tables[0].Rows[i].Field<string>("VehicleNb");

                string str = "lb_Vehicle" + (i + 1).ToString();

                Label label = this.FindControl(str) as Label;
                if (label != null)
                {
                    label.Text = numer.ToString();
                    label.Visible = true;
                }


                int status = buses.Tables[0].Rows[i].Field<int>("Status");

                switch (status)
                {
                    case 0:
                        break;
                    case 1:
                        label.BackColor = Color.Gray;
                        break;
                    case 2:
                        label.BackColor = Color.Green;
                        break;
                    case 3:
                        label.BackColor = Color.Red;
                        break;
                }
            }
        }



        protected void Rb_Przylot_CheckedChanged(object sender, EventArgs e)
        {
            if(rb_Przylot.Checked)
            {
                rb_Odlot.Checked = false;
                Clear_Alocator();
            }
            else
            {
                Clear_Alocator();
            }
        }

        protected void Rb_Odlot_CheckedChanged(object sender, EventArgs e)
        {
            if(rb_Odlot.Checked)
            {
                if (rb_Przylot.Checked)
                    rb_Przylot.Checked = false;
                Clear_Alocator();
            }
            else
            {
                Clear_Alocator();
            }
        }


        private void Clear_Alocator()
        {
            ddl_Port.SelectedIndex = 0;
            ddl_PPS.SelectedIndex = 0;
            ddl_Gate.SelectedIndex = 0;
            ddl_Bus.SelectedIndex = 0;
            tb_AlocatorFNb.Text = "";
            tb_Pax.Text = "";
            tb_RadioGate.Text = "";
            tb_RadioNeon.Text = "";

        }


        protected void Bt_AlocatorReset_Click(object sender, EventArgs e)
        {
            Clear_Alocator();
        }


        protected void Bt_AlocatorAccept_Click(object sender, EventArgs e)
        {
            NewOperation newOp = new NewOperation();

            if (rb_Odlot.Checked)
                newOp.OperationType = 1;
            else
                newOp.OperationType = 2;

            newOp.FlightNb = tb_AlocatorFNb.Text;
            newOp.PaxCount = Convert.ToInt32(tb_Pax.Text);
            newOp.Port = Convert.ToInt32(ddl_Port.SelectedItem.Value);
            newOp.GateNb = Convert.ToInt32(ddl_Gate.SelectedItem.Value);
            newOp.PPSNb = Convert.ToInt32(ddl_PPS.SelectedItem.Value);
            newOp.BusSelected = Convert.ToInt32(ddl_Bus.SelectedItem.Value);
            newOp.RadioGate = tb_RadioGate.Text;
            newOp.RadioNeon = tb_RadioNeon.Text;

            int id = 0;

            if (Request.Cookies["BusManagement"] != null)
            {
                id = Convert.ToInt32(Request.Cookies["BusManagement"].Values["Id"]);
            }

            bool result = bl.AddNewOperation(newOp, id);

            if (result)
            {
                ClearAlocatorControls();
                UpdateAlocatorPanel(newOp.BusSelected);
            }
                
            else
            {
                // wyświetlić na panelu że się nie udało i z jakiego powodu!
            }
        }

        private void UpdateAlocatorPanel(int selectedBusStatus)
        {
            for (int i = 0; i <36; i++)
            {
                string str = "lb_Vehicle" + (i + 1).ToString();

                Label label = this.FindControl(str) as Label;
                if (label != null && label.Text == selectedBusStatus.ToString())
                {
                    label.ForeColor = Color.Red;
                    label.BackColor = Color.Yellow;
                }
            }
        }

        //private List<String> getOnlineUsers()
        //{
        //    List<String> activeSessions = new List<String>();
        //    object obj = typeof(HttpRuntime).GetProperty("CacheInternal", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null, null);
        //    object[] obj2 = (object[])obj.GetType().GetField("_caches", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(obj);
        //    for (int i = 0; i < obj2.Length; i++)
        //    {
        //        Hashtable c2 = (Hashtable)obj2[i].GetType().GetField("_entries", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(obj2[i]);
        //        foreach (DictionaryEntry entry in c2)
        //        {
        //            object o1 = entry.Value.GetType().GetProperty("Value", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(entry.Value, null);
        //            if (o1.GetType().ToString() == "System.Web.SessionState.InProcSessionState")
        //            {
        //                SessionStateItemCollection sess = (SessionStateItemCollection)o1.GetType().GetField("_sessionItems", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(o1);
        //                if (sess != null)
        //                {
        //                    if (sess["loggedInUserId"] != null)
        //                    {
        //                        activeSessions.Add(sess["loggedInUserId"].ToString());
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    return activeSessions;
        //}

        private void ClearAlocatorControls()
        {
            rb_Przylot.Checked = true;
            rb_Odlot.Checked = false;
            tb_AlocatorFNb.Text = "";
            tb_Pax.Text = "";
            ddl_Bus.SelectedIndex = -1;
            ddl_Gate.SelectedIndex = -1;
            ddl_Port.SelectedIndex = -1;
            ddl_PPS.SelectedIndex = -1;
            tb_RadioNeon.Text = "";
            tb_RadioGate.Text = "";
        }
    }
}