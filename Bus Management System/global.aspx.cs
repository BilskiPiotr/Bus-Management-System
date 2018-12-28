using System;
using System.Data;

namespace Bus_Management_System
{
    public partial class global : System.Web.UI.Page
    {
        private BusinessLayer bl = new BusinessLayer();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

            }

            Server.ClearError();
        }

        // zresetowanie kontrolek na panelu logowania
        protected void Bt_resetLogin_Click(object sender, EventArgs e)
        {
            ClearTextbox();
            inp_name.Focus();
        }

        // weryfikacja logowania
        protected void Bt_submitLogin_Click(object sender, EventArgs e)
        {
            VerifyLayer vl = new VerifyLayer
            {
                Imię = inp_name.Text.Trim(),
                Nazwisko = inp_2ndName.Text.Trim(),
                Pesel = inp_pesel.Text.Trim()
            };
            int iD = 0;
            //int CompanyId = 0;

            // user nie istnieje, albo wprowadzono dane z błedem
            if (!bl.VerifyUser(vl, ref iD))
            {
                ClearTextbox();
                lb_errorMsg.Visible = true;
                lb_errorMsg.Text = "Błędny Login lub Hasło! ";
            }

            // user istnieje - wiedz tworzenie danych sesji
            else
            {
                DataTable dt = new DataTable();
                bl.GetUserData(iD, ref dt);

                // Konstruktor obiektu SESJA
                if (dt.Rows.Count > 0)
                {
                    bool result = PrepareSession(dt, iD);

                    if (result)
                    {
                        DateTime loginDate = DateTime.Now;
                        if (bl.UserLogIn(iD, loginDate))
                        {
                            HomeView((string)Session["AdminPrivileges"]);
                        }
                        else
                        {
                            result = false;
                        }
                    }
                    if (!result)
                    {
                        bl.UserLogOut(iD, "");
                        Session.Abandon();
                        Response.Redirect("global.aspx");
                    }
                }
                else
                {
                    ClearTextbox();
                    Response.Write("<script> alert('Błąd - proszę wprowadzić inne poświadczenia' ) </script>");
                }
                dt.Dispose();
            }
        }

        // ustalenie który panel należy wyświetlić i przekierowanie
        private void HomeView(string AdminPrivileges)
        {
            if (Convert.ToInt32(AdminPrivileges) == 1)
                Response.Redirect("Alocator.aspx");
            else
            if (Convert.ToInt32(AdminPrivileges) == 2)
                Response.Redirect("Bus.aspx");
            else
            {
                Session.Abandon();
                Response.Redirect("Global.aspx");
            }
        }

        // wyczyszczenie textboxów na panelu logowania
        private void ClearTextbox()
        {
            inp_name.Text = "";
            inp_2ndName.Text = "";
            inp_pesel.Text = "";
        }

        private bool PrepareSession(DataTable dt, int iD)
        {
            bool result = false;
            try
            {
                Session["Name"] = dt.Rows[0][0].ToString();
                Session["FirstName"] = dt.Rows[0][1].ToString();
                Session["LastName"] = dt.Rows[0][2].ToString();
                Session["AdminPrivileges"] = dt.Rows[0][3].ToString();
                Session["Interval"] = 0;
                Session["Bus"] = "";
                Session["Id"] = iD;
                if ((string)Session["AdminPrivileges"] == "2")
                {
                    Session["OperationStatus"] = 0;
                    Session["Shengen"] = 0;
                    Session["Operation"] = 2;
                    Session["GateLat"] = 0.0d;
                    Session["GateLon"] = 0.0d;
                    Session["PpsLat"] = 0.0d;
                    Session["PpsLon"] = 0.0d;
                    Session["DistanceT"] = 0.0d;
                    Session["OldDistanceT"] = 0.0d;
                    Session["DistanceS"] = 0.0d;
                    Session["OldDistanceS"] = 0.0d;
                    Session["DistanceN"] = 0.0d;
                    Session["OldDistanceN"] = 0.0d;
                    Session["CurrentLat"] = 0.0d;
                    Session["CurrentLon"] = 0.0d;
                    Session["PredictedDistance"] = 0.0d;
                    Session["Interval"] = 0;
                    Session["StartLat"] = 0.0d;
                    Session["StartLon"] = 0.0d;
                    result = true;
                }
                else
                    if ((string)Session["AdminPrivileges"] == "1")
                {
                    Session["Operation"] = 0;
                    Session["Pps"] = "";
                    Session["AirPort"] = 0;
                    Session["Gate"] = "";
                    Session["GodzinaRozkladowa"] = "";
                    Session["FlightNb"] = "";
                    Session["Pax"] = "";
                    Session["RadioGate"] = "";
                    Session["RadioNeon"] = "";
                    result = true;
                }


            }
            catch (Exception ex)
            {
                result = false;
            }
            return result;
        }
    }
}