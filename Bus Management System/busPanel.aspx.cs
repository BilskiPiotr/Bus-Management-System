using System;
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
            User loggedUser = (User)Session["loggedUser"];

            ScriptManager.RegisterClientScriptBlock(this, GetType(), "pobierzLokalizacje", "getLocation();", true);

            if (!IsPostBack)
            {
                MenuItemCollection menuItems = mineMenu.Items;

                if (loggedUser != null)
                {
                    lb_loggedUser.Text = "";
                    lb_loggedUser.Text += (string)loggedUser.FirstName + " " + (string)loggedUser.LastName + "       ID: " + ((int)loggedUser.CompanyId).ToString();

                    SetActiveView(loggedUser, menuItems);
                }
            }
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

        protected void bt_GetPosition_Click(object sender, EventArgs e)
        {

        }
    }
}