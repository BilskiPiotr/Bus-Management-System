using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Web;
using System.Web.UI.WebControls;

namespace Bus_Management_System
{
    public partial class Alocator : System.Web.UI.Page
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
                }

                // dodać sprawdzenie, czy taka sesja już istnieje, a jeśli nie - to dodać
                User loggedUser = (User)Session[userId];

                MenuItemCollection menuItems = alocatorMenu.Items;

                if (loggedUser != null)
                {
                    lb_loggedUser.Text = "";
                    lb_loggedUser.Text += (string)loggedUser.FirstName + " " + (string)loggedUser.LastName + "       ID: " + ((int)loggedUser.CompanyId).ToString();
                    // załadowanie danych do GridView Alocator
                    BindGrid();
                }
            }
        }



        //obsługva zdażenia MenuItem Click
        protected void MineMenu_MenuItemClick(object sender, MenuEventArgs e)
        {
            User loggedUser = (User)Session["loggedUser"];
            MenuItem mnu = (MenuItem)e.Item;

            switch (mnu.Value)
            {
                case "1":
                    {
                        Response.Redirect("Alocator.aspx");
                    }
                    break;
                case "2":
                    {
                        if (Request.Cookies["Bus"] != null)
                        {
                            Response.Cookies["Bus"].Expires = DateTime.Now.AddDays(-1);
                        }
                        Session.Abandon();
                        Response.Redirect("global.aspx");
                    }
                    break;
            }
        }

        // pobranie aktualnych danych
        private void BindGrid()
        {
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT a.Id, a.RadioGate, a.Created, a.GodzinaRozkladowa, a.FlightNb, a.Pax, a.RadioNeon, " +
                                                "a.Accepted, a.StartLoad, a.StartDrive, a.StartUnload, a.EndOp, " +
                                                "b.Operation, c.StationNb, d.IATA_Name, e.GateNb, f.VehicleNb, g.Shengen " +
                                                "FROM Operations AS a " +
                                                "INNER JOIN OperationType AS b ON a.Operation=b.Id " +
                                                "INNER JOIN Stations AS c ON a.PPS = c.Id " +
                                                "INNER JOIN AirPorts AS d ON a.AirPort = d.Id " +
                                                "INNER JOIN Gates AS e ON a.Gate = e.Id " +
                                                "INNER JOIN Vehicles AS f ON a.Bus = f.Id " +
                                                "INNER JOIN Countries AS g ON a.AirPort = d.Id  AND d.Country_Id = g.Id");

                DataSet ds = dal.GetDataSet(cmd);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    gv_Alocator.DataSource = ds;
                    gv_Alocator.DataBind();
                }
                else
                {
                    DataTable dt = GridViewStructCreate();
                    gv_Alocator.DataSource = dt;
                    gv_Alocator.DataBind();
                }

                SetBusStatus();
            }
            catch  // powstaje gdzieś błąd odwołania do obiektu podczazs przeładowania przy pomocy funkcji Edytuj
            {
                //Response.Write("<script> alert('Linia 93 - Błąd pobierania listy operacji') </script>");
            }
        }



        // wypełnienie kontrolek DropDownList wewnątrz GridView
        protected void Gv_Alocator_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //utworzenie pustej instancji ciasteczka
            HttpCookie getData = null;
            string operacjaCookie = "";
            string ppsCookie = "";
            string airPortCookie = "";
            string gateCookie = "";
            string busCookie = "";

            // jeśli istnieje przyisanie danych z cookie Alocator do nowej punstej instancji
            try
            {
                if (Request.Cookies["Alocator"] != null)
                {
                    getData = Request.Cookies["Alocator"];
                    operacjaCookie = Convert.ToString(getData.Values["operation"]);
                    ppsCookie = Convert.ToString(getData.Values["pps"]);
                    airPortCookie = Convert.ToString(getData.Values["airPort"]);
                    gateCookie = Convert.ToString(getData.Values["gate"]);
                    busCookie = Convert.ToString(getData.Values["bus"]);
                }
            }
            catch // jeśli ciasteczko już wygadło
            {
                Response.Write("<script> alert('Linia 172 - Za długi czas dostu do serwera danych') </script>");
                // dodać potęcjalnie inne porządane zachowania, po testach
            }

            // pobranie i wypełnienie kontrolek DropDownList listą możliwych operacji
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM OperationType");
                DataSet ds = dal.GetDataSet(cmd);
                DropDownList ddl_operationEdit = (DropDownList)e.Row.FindControl("ddl_operationEdit");
                if (ddl_operationEdit != null)
                {
                    ddl_operationEdit.DataSource = ds;
                    ddl_operationEdit.DataValueField = "Id";
                    ddl_operationEdit.DataTextField = "Operation";
                    ddl_operationEdit.DataBind();

                    // ustawnienie wartości startowej wyświetlania na podstawie danych z ciasteczka Alocator
                    ddl_operationEdit.SelectedIndex = ddl_operationEdit.Items.IndexOf(ddl_operationEdit.Items.FindByText(operacjaCookie));
                    //ddl_operationEdit.Text = operacjaCookie;
                    ddl_operationEdit.Dispose();
                }
                if (e.Row.RowType == DataControlRowType.Footer)
                {
                    DropDownList ddl_OperationAdd = (DropDownList)e.Row.FindControl("ddl_operationAdd");
                    ddl_OperationAdd.DataSource = ds;
                    ddl_OperationAdd.DataValueField = "Id";
                    ddl_OperationAdd.DataTextField = "Operation";
                    ddl_OperationAdd.DataBind();
                    ddl_OperationAdd.Dispose();

                }
                //DropDownList ddl_OperationEdit = (DropDownList)e.Row.FindControl("ddl_operationEdit");
                ds.Dispose();
            }
            catch
            {
                Response.Write("<script> alert('Linia 173 - Błąd ładowania listy typów operacji') </script>");
            }


            // pobranie i wypełnienie kontrolek DropDownList numerami stanowisk postojowych samolotów
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT Id, StationNb FROM Stations");
                DataSet ds = dal.GetDataSet(cmd);
                DropDownList ddl_ppsEdit = (DropDownList)e.Row.FindControl("ddl_ppsEdit");
                if (ddl_ppsEdit != null)
                {
                    ddl_ppsEdit.DataSource = ds;
                    ddl_ppsEdit.DataValueField = "Id";
                    ddl_ppsEdit.DataTextField = "StationNb";
                    ddl_ppsEdit.DataBind();

                    // ustawnienie wartości startowej wyświetlania na podstawie danych z ciasteczka Alocator
                    ddl_ppsEdit.SelectedIndex = ddl_ppsEdit.Items.IndexOf(ddl_ppsEdit.Items.FindByText(ppsCookie));
                    //ddl_ppsEdit.SelectedItem.Text = ppsCookie;
                    ddl_ppsEdit.Dispose();
                }
                if (e.Row.RowType == DataControlRowType.Footer)
                {
                    DropDownList ddl_ppsAdd = (DropDownList)e.Row.FindControl("ddl_ppsAdd");
                    ddl_ppsAdd.DataSource = ds;
                    ddl_ppsAdd.DataValueField = "Id";
                    ddl_ppsAdd.DataTextField = "StationNb";
                    ddl_ppsAdd.DataBind();
                    ddl_ppsAdd.Dispose();
                }
                ds.Dispose();
            }
            catch
            {
                Response.Write("<script> alert('Linia 244 - Błąd ładowania listy stanowisk postojowych') </script>");
            }


            // pobranie i wypełnienie kontrolek DropDownList listą obsługiwanych portów lotniczych
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT Id, IATA_Name FROM AirPorts");
                DataSet ds = dal.GetDataSet(cmd);
                DropDownList ddl_airPortEdit = (DropDownList)e.Row.FindControl("ddl_airPortEdit");
                if (ddl_airPortEdit != null)
                {
                    ddl_airPortEdit.DataSource = ds;
                    ddl_airPortEdit.DataValueField = "Id";
                    ddl_airPortEdit.DataTextField = "IATA_Name";
                    ddl_airPortEdit.DataBind();

                    // ustawnienie wartości startowej wyświetlania na podstawie danych z ciasteczka Alocator
                    ddl_airPortEdit.SelectedIndex = ddl_airPortEdit.Items.IndexOf(ddl_airPortEdit.Items.FindByText(airPortCookie));
                    //ddl_airPortEdit.SelectedItem.Text = airPortCookie;
                    ddl_airPortEdit.Dispose();
                }
                if (e.Row.RowType == DataControlRowType.Footer)
                {
                    DropDownList ddl_airPortAdd = (DropDownList)e.Row.FindControl("ddl_airPortAdd");
                    ddl_airPortAdd.DataSource = ds;
                    ddl_airPortAdd.DataValueField = "Id";
                    ddl_airPortAdd.DataTextField = "IATA_Name";
                    ddl_airPortAdd.DataBind();
                    ddl_airPortAdd.Dispose();
                }
                ds.Dispose();
            }
            catch
            {
                Response.Write("<script> alert('Linia 279 - Błąd ładowania listy obsługiwanych portów lotniczych') </script>");
            }


            // pobranie i wypełnienie kontrolek DropDownList numerami gate
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT Id, GateNb FROM Gates");
                DataSet ds = dal.GetDataSet(cmd);
                DropDownList ddl_gateEdit = (DropDownList)e.Row.FindControl("ddl_gateEdit");
                if (ddl_gateEdit != null)
                {
                    ddl_gateEdit.DataSource = ds;
                    ddl_gateEdit.DataValueField = "Id";
                    ddl_gateEdit.DataTextField = "GateNb";
                    ddl_gateEdit.DataBind();

                    // ustawnienie wartości startowej wyświetlania na podstawie danych z ciasteczka Alocator
                    ddl_gateEdit.SelectedIndex = ddl_gateEdit.Items.IndexOf(ddl_gateEdit.Items.FindByText(gateCookie));
                    //ddl_gateEdit.SelectedItem.Text = gateCookie;
                    ddl_gateEdit.Dispose();
                }
                if (e.Row.RowType == DataControlRowType.Footer)
                {
                    DropDownList ddl_gateAdd = (DropDownList)e.Row.FindControl("ddl_gateAdd");
                    ddl_gateAdd.DataSource = ds;
                    ddl_gateAdd.DataValueField = "Id";
                    ddl_gateAdd.DataTextField = "GateNb";
                    ddl_gateAdd.DataBind();
                    ddl_gateAdd.Dispose();
                }
                ds.Dispose();
            }
            catch
            {
                Response.Write("<script> alert('Linia 314 - Błąd ładowania listy gate') </script>");
            }


            // pobranie i wypełnienie kontrolek DropDownList numerami autobusów
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT Id, VehicleNb FROM Vehicles WHERE Bus_Status = '" + 2 + "' AND Work_Status = '" + 0 + "'");
                DataSet ds = dal.GetDataSet(cmd);
                DropDownList ddl_busEdit = (DropDownList)e.Row.FindControl("ddl_busEdit");
                if (ddl_busEdit != null && ds.Tables[0].Rows.Count == 0)
                {
                    cmd = new SqlCommand("SELECT Id, VehicleNb FROM Vehicles WHERE VehicleNb = '" + busCookie + "'");
                    ds.Clear();
                    ds = dal.GetDataSet(cmd);
                    ddl_busEdit.DataSource = ds;
                    ddl_busEdit.DataValueField = "Id";
                    ddl_busEdit.DataTextField = "VehicleNb";
                    ddl_busEdit.DataBind();
                }
                else
                {
                    if (ddl_busEdit != null)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            ddl_busEdit.DataSource = ds;
                            ddl_busEdit.DataValueField = "Id";
                            ddl_busEdit.DataTextField = "VehicleNb";
                            ddl_busEdit.DataBind();

                            // ustawnienie wartości startowej wyświetlania na podstawie danych z ciasteczka Alocator
                            ddl_busEdit.SelectedIndex = ddl_busEdit.Items.IndexOf(ddl_busEdit.Items.FindByText(busCookie));
                            //ddl_busEdit.SelectedItem.Text = busCookie;
                            ddl_busEdit.Dispose();
                        }
                    }
                    if (e.Row.RowType == DataControlRowType.Footer)
                    {
                        DropDownList ddl_busAdd = (DropDownList)e.Row.FindControl("ddl_busAdd");
                        ddl_busAdd.DataSource = ds;
                        ddl_busAdd.DataValueField = "Id";
                        ddl_busAdd.DataTextField = "VehicleNb";
                        ddl_busAdd.DataBind();
                        ddl_busAdd.Dispose();
                    }
                    ds.Dispose();
                }
            }
            catch
            {
                Response.Write("<script> alert('Linia 352 - Błąd ładowania listy Autobusów') </script>");
            }
        }


        // ustalenie typu operacji i strefy ze względu na wczytane dane
        protected void Gv_CheckSecurityZone(object sender, EventArgs e)
        {
            for (int i = 0; i <= gv_Alocator.Rows.Count - 1; i++)
            {
                Label lb_Zone = (Label)gv_Alocator.Rows[i].FindControl("lb_zone");
                Label lb_Operation = (Label)gv_Alocator.Rows[i].FindControl("lb_operation");

                if (lb_Operation.Text == "Odlot")
                {
                    gv_Alocator.Rows[i].Cells[3].BackColor = Color.Blue;
                    gv_Alocator.Rows[i].Cells[3].ForeColor = Color.White;
                }
                else
                {
                    if (lb_Zone.Text == "0")
                    {
                        gv_Alocator.Rows[i].Cells[3].BackColor = Color.Green;
                        gv_Alocator.Rows[i].Cells[3].ForeColor = Color.White;
                    }
                    else
                    if (lb_Zone.Text == "1")
                    {
                        gv_Alocator.Rows[i].Cells[3].BackColor = Color.Red;
                        gv_Alocator.Rows[i].Cells[3].ForeColor = Color.White;
                    }
                }
            }
        }


        // usunięcie błędnego wpisu do rozważenia, czy to na pewno jest potrzebne
        protected void Gv_Alocator_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int Id = Convert.ToInt32(gv_Alocator.DataKeys[e.RowIndex].Value);
            GridViewRow row = gv_Alocator.Rows[e.RowIndex];
            Label bus = (Label)row.FindControl("lb_bus");

            SqlCommand cmd1 = new SqlCommand("UPDATE Vehicles SET Work_Status = @work_status WHERE VehicleNb = @vehicleNb");
            cmd1.Parameters.AddWithValue("@work_status", 0);
            cmd1.Parameters.AddWithValue("@vehicleNb", bus.Text);
            dal.QueryExecution(cmd1);

            SqlCommand cmd = new SqlCommand("DELETE FROM Operations WHERE Id=@Id");
            cmd.Parameters.AddWithValue("@Id", Id);
            dal.QueryExecution(cmd);

            bus.Dispose();
            BindGrid();
        }


        // wywołanie edycji istniejącej operacji
        // poprawić tak, żeby po uruchomieniu w kontrolkach wyświetliły się dane przed poprawkami a nie defaultowe
        protected void Gv_Alocator_RowEditing(object sender, GridViewEditEventArgs e)
        {
            btn_addNewOperation.Enabled = false;
            gv_Alocator.EditIndex = e.NewEditIndex;

            try
            {
                if (Request.Cookies["Alocator"] != null)
                    Response.Cookies["Alocator"].Expires = DateTime.Now.AddDays(-1);
                else
                {
                    //String value = gv_Alocator.Rows[e.NewEditIndex].Cells[5].Text;
                    HttpCookie EditOperationValues = new HttpCookie("Alocator");

                    //pobranie danych dla edycji list ddl i ciasteczka Alocator
                    GridViewRow row = gv_Alocator.Rows[e.NewEditIndex];

                    Label oper = (Label)row.FindControl("lb_operation");
                    string operation = oper.Text;
                    Label iataName = (Label)row.FindControl("lb_airPort");
                    string airPort = iataName.Text;
                    Label gateNb = (Label)row.FindControl("lb_gate");
                    string gate = gateNb.Text;
                    Label ppsNb = (Label)row.FindControl("lb_pps");
                    string pps = ppsNb.Text;
                    Label busNb = (Label)row.FindControl("lb_bus");
                    string bus = busNb.Text;

                    EditOperationValues.Values["operation"] = operation;
                    EditOperationValues.Values["pps"] = pps;
                    EditOperationValues.Values["airPort"] = airPort;
                    EditOperationValues.Values["gate"] = gate;
                    EditOperationValues.Values["bus"] = bus;

                    EditOperationValues.Expires = DateTime.Now.AddMilliseconds(6000);
                    Response.Cookies.Add(EditOperationValues);
                }
                BindGrid();
            }
            catch // powstaje gdzieś błąd odwołania do obiektu podczazs przeładowania przy pomocy funkcji Edytuj
            {
                Response.Write("<script> alert('Linia 486 - Błąd pobierania listy operacji') </script>");
            }
        }


        //Dodanie nowej operacji
        protected void Gv_Alocator_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("Insert"))
            {
                DropDownList ddl_OperationAdd = (DropDownList)gv_Alocator.FooterRow.FindControl("ddl_operationAdd");
                TextBox tb_FlightNbAdd = (TextBox)gv_Alocator.FooterRow.FindControl("tb_flightNbAdd");
                TextBox tb_GodzinaRozkładowaAdd = (TextBox)gv_Alocator.FooterRow.FindControl("tb_godzinaRozkładowaAdd");
                DropDownList ddl_AirPortAdd = (DropDownList)gv_Alocator.FooterRow.FindControl("ddl_airPortAdd");
                TextBox tb_PaxAdd = (TextBox)gv_Alocator.FooterRow.FindControl("tb_paxAdd");
                DropDownList ddl_GateAdd = (DropDownList)gv_Alocator.FooterRow.FindControl("ddl_gateAdd");
                DropDownList ddl_PpsAdd = (DropDownList)gv_Alocator.FooterRow.FindControl("ddl_ppsAdd");
                DropDownList ddl_BusAdd = (DropDownList)gv_Alocator.FooterRow.FindControl("ddl_busAdd");
                TextBox tb_RadioGateAdd = (TextBox)gv_Alocator.FooterRow.FindControl("tb_radioGateAdd");
                TextBox tb_RadioNeonAdd = (TextBox)gv_Alocator.FooterRow.FindControl("tb_radioNeonAdd");

                string bus = ddl_BusAdd.SelectedItem.ToString();
                DateTime zeroDate = new DateTime(1999, 01, 01);
                DateTime dt = new DateTime();
                if (!DateTime.TryParseExact(tb_GodzinaRozkładowaAdd.Text, "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out dt))
                {
                    // w przypadku błędu konwersji daty
                    Response.Write("<script> alert('Błąd - format daty wydaje się być nieprawidłowy' ) </script>");
                    return;
                }
                //else 
                //    if(dt.ToString() == "0001-01-01 00:00:00")
                //{
                //    Response.Write("<script> alert('Błąd - wprowadzona godzina wydaje się być nieprawidłowa' ) </script>");
                //    return;
                //}
                    
                // a jeśli będzie potrzebny tylko czas to...
                // TimeSpan time = dt.TimeOfDay;

                try
                {
                    int finished = 0;
                    SqlCommand cmd = new SqlCommand("INSERT INTO Operations (Employee_Id, Operation, GodzinaRozkladowa, FlightNb, AirPort, Pax, Gate, PPS, Bus, RadioGate, RadioNeon, Created, Accepted, StartLoad, StartDrive, StartUnload, EndOp, Finished) " +
                                "VALUES (" + 2 + ", " +
                                "" + ddl_OperationAdd.SelectedValue + ", " +
                                "'" + dt + "', " +
                                "'" + tb_FlightNbAdd.Text + "', " +
                                "" + ddl_AirPortAdd.SelectedValue + ", " +
                                "'" + tb_PaxAdd.Text + "', " +
                                "" + ddl_GateAdd.SelectedValue + ", " +
                                "" + ddl_PpsAdd.SelectedValue + ", " +
                                "" + ddl_BusAdd.SelectedValue + ", " +
                                "'" + tb_RadioGateAdd.Text + "', " +
                                "'" + tb_RadioNeonAdd.Text + "', getdate()," +
                                "'" + zeroDate + "', '" + zeroDate + "', '" + zeroDate + "', '" + zeroDate + "', '" + zeroDate + "', '" + finished + "')");
                    dal.QueryExecution(cmd);

                    SqlCommand cmd1 = new SqlCommand("UPDATE Vehicles SET Work_Status = 1 WHERE VehicleNb = '" + bus + "' ");
                    dal.QueryExecution(cmd1);

                    btn_addNewOperation.Visible = true;
                    gv_Alocator.ShowFooter = false;
                    BindGrid();
                }
                catch
                {
                    Response.Write("<script> alert('Błąd - nie udało się dodać nowej operacji' ) </script>");
                }
            }
            SetBusStatus();
        }


        // Poprawienie danych w istniejącym rekordzie
        protected void Gv_Alocator_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int id = int.Parse(((Label)(gv_Alocator.Rows[e.RowIndex].Cells[1].FindControl("lb_id"))).Text);
            int operation = int.Parse(((DropDownList)(gv_Alocator.Rows[e.RowIndex].Cells[1].FindControl("ddl_operationEdit"))).SelectedValue);
            string godzinaRozkladowa = (((TextBox)(gv_Alocator.Rows[e.RowIndex].Cells[1].FindControl("tb_godzinaRozkładowa"))).Text);
            string flightNb = (((TextBox)(gv_Alocator.Rows[e.RowIndex].Cells[1].FindControl("tb_flightNbEdit"))).Text);
            int airPort = int.Parse(((DropDownList)(gv_Alocator.Rows[e.RowIndex].Cells[1].FindControl("ddl_airPortEdit"))).SelectedValue);
            string pax = (((TextBox)(gv_Alocator.Rows[e.RowIndex].Cells[1].FindControl("tb_paxEdit"))).Text);
            int gate = int.Parse(((DropDownList)(gv_Alocator.Rows[e.RowIndex].Cells[1].FindControl("ddl_gateEdit"))).SelectedValue);
            int pps = int.Parse(((DropDownList)(gv_Alocator.Rows[e.RowIndex].Cells[1].FindControl("ddl_ppsEdit"))).SelectedValue);
            int bus = int.Parse(((DropDownList)(gv_Alocator.Rows[e.RowIndex].Cells[1].FindControl("ddl_busEdit"))).SelectedValue);
            string radioGate = ((TextBox)(gv_Alocator.Rows[e.RowIndex].Cells[1].FindControl("tb_radioGateEdit"))).Text;
            string radioNeon = ((TextBox)(gv_Alocator.Rows[e.RowIndex].Cells[1].FindControl("tb_radioNeonEdit"))).Text;

            DateTime editDate = DateTime.Now;
            DateTime opTime = CheckTimeFormat(godzinaRozkladowa);

                SqlCommand cmd = new SqlCommand("UPDATE Operations SET Operation = @operation, " +
                                "FlightNb = @flightNb, " +
                                "GodzinaRozkladowa = @opTime, " +
                                "AirPort = @airPort, " +
                                "Pax = @pax, " +
                                "Gate = @gate, " +
                                "PPS = @pps, " +
                                "Bus = @bus, " +
                                "RadioGate = @radioGate, " +
                                "RadioNeon = @radioNeon, " +
                                "Created = @editDate " +
                                "WHERE Id=@id ");
                cmd.Parameters.AddWithValue("@operation", operation);
                cmd.Parameters.AddWithValue("@flightNb", flightNb);
                cmd.Parameters.AddWithValue("@opTime", opTime);
                cmd.Parameters.AddWithValue("@airPort", airPort);
                cmd.Parameters.AddWithValue("@pax", pax);
                cmd.Parameters.AddWithValue("@gate", gate);
                cmd.Parameters.AddWithValue("@pps", pps);
                cmd.Parameters.AddWithValue("@bus", bus);
                cmd.Parameters.AddWithValue("@radioGate", radioGate);
                cmd.Parameters.AddWithValue("@radioNeon", radioNeon);
                cmd.Parameters.AddWithValue("@editDate", editDate);
                cmd.Parameters.AddWithValue("@id", id);

            try
            {
                dal.QueryExecution(cmd);
                gv_Alocator.EditIndex = -1;
                BindGrid();
            }
            catch
            {
                Response.Write("<script> alert('Błąd - nie udało się poprawić istniejącej operacji') </script>");
            }

            try
            {
                dal.QueryExecution(cmd);

                SqlCommand cmd1 = new SqlCommand("UPDATE Vehicles SET Work_Status = 1 WHERE Id = '" + bus + "' ");
                dal.QueryExecution(cmd1);

                gv_Alocator.EditIndex = -1;
                btn_addNewOperation.Enabled = true;
                BindGrid();
            }
            catch
            {
                Response.Write("<script> alert('Błąd - nie udało się poprawić istniejącej operacji') </script>");
            }
        }


        // Rezygnacja z poprawienia operacjhi
        protected void Gv_Alocator_CancelEdit(object sender, GridViewCancelEditEventArgs e)
        {
            btn_addNewOperation.Enabled = true;
            gv_Alocator.EditIndex = -1;
            BindGrid();
        }


        // Wyświetlenie "Footer" kontrolki GridView
        protected void Gv_Alocator_NewOperation(object sender, EventArgs e)
        {
            btn_addNewOperation.Visible = false;
            btn_cancelNewOperation.Visible = true;
            gv_Alocator.ShowFooter = true;
            BindGrid();
        }


        // Anulowanie operacji dodawania nowej operacji
        protected void Gv_Alocator_CancelNewOperation(object sender, EventArgs e)
        {
            btn_addNewOperation.Visible = true;
            btn_cancelNewOperation.Visible = false;
            gv_Alocator.ShowFooter = false;
            BindGrid();
        }


        private void SetBusStatus()
        {
            //Bus status: 0 - Not Available, 1 - Empty, 2 - Free, 3 - In Work

            DataSet buses = bl.GetBus();
            string numer = "";
            string str = "";
            int status = -1;
            int work_Status = -1;

            for (int i = 0; i < buses.Tables[0].Rows.Count; i++)
            {
                // pobranie numeru autobusu
                numer = buses.Tables[0].Rows[i].Field<string>("VehicleNb");
                // pobranie statusu tego autobusu
                status = buses.Tables[0].Rows[i].Field<int>("Bus_Status");
                work_Status = buses.Tables[0].Rows[i].Field<int>("Work_Status");

                //określenie klucza poszukiwań nazewnictwa kontrolek dla kolejnych wpisów w kontenerze DataSet
                str = "lb_Vehicle" + (i + 1).ToString();

                if (this.FindControl(str) is Label label)
                {
                    // wypisanie numeru autobusu
                    label.Text = numer.ToString();

                    // ustalenie koloru tła kontrolki ze względu na status pojazdu
                    switch (status)
                    {
                        case 0:
                            label.BackColor = Color.Black;
                            label.ForeColor = Color.Yellow;
                            label.Font.Bold = true;
                            break;
                        case 1:
                            label.BackColor = Color.Gray;
                            label.ForeColor = Color.Yellow;
                            break;
                        case 2:
                            switch (work_Status)
                            {
                                case 0:
                                    label.BackColor = Color.Green;
                                    label.ForeColor = Color.White;
                                    label.Font.Bold = true;
                                    break;
                                case 1:
                                    label.BackColor = Color.Red;
                                    label.ForeColor = Color.White;
                                    label.Font.Bold = true;
                                    break;
                            }
                            break;
                    }

                    status = -1;
                    work_Status = -1;
                    // wyświetlenie kontrolki, jeśli dodano do niej dane
                    label.Visible = true;
                    label.Dispose();
                }


            }
            buses.Dispose();
        }


        private DataTable GridViewStructCreate()
        {
            DataTable dt = new DataTable();

            // konstruktor kolumn zawierający wszystkie informacje 
            // jak w poprawnie pobranym DataSource, ponieważ jakieś operacje już istniały
            dt.Columns.Add("Created");
            dt.Columns.Add("Operation");
            dt.Columns.Add("GodzinaRozkladowa");
            dt.Columns.Add("FlightNb");
            dt.Columns.Add("IATA_Name");
            dt.Columns.Add("Shengen");
            dt.Columns.Add("Pax");
            dt.Columns.Add("GateNb");
            dt.Columns.Add("id");
            dt.Columns.Add("StationNb");
            dt.Columns.Add("VehicleNb");
            dt.Columns.Add("RadioGate");
            dt.Columns.Add("RadioNeon");
            dt.Columns.Add("Accepted");
            dt.Columns.Add("StartLoad");
            dt.Columns.Add("StartDrive");
            dt.Columns.Add("StartUnload");
            dt.Columns.Add("EndOp");

            DataRow dr = dt.NewRow();
            dt.Rows.Add(dr);

            return dt;
        }


        // Sprawdzenie formatu wprowadzonej godziny operacji
        private DateTime CheckTimeFormat(string time)
        {
            DateTime dt = new DateTime();
            if (!DateTime.TryParseExact(time, "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out dt))
            {
                // w przypadku błędu konwersji daty
                Response.Write("<script> alert('Błąd - format daty wydaje się być nieprawidłowy' ) </script>");
                Response.Redirect("Alocator.aspx");
            }
            return dt;
        }
        
    }
}