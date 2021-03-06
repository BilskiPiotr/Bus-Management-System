﻿using System;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Web.UI.WebControls;

namespace Bus_Management_System
{
    public partial class Alocator : System.Web.UI.Page
    {
        private static BusinessLayer bl = new BusinessLayer();

        protected void Page_Load(object sender, EventArgs e)
        {
            if ((string)Session["Name"] == "")
            {
                Session.Abandon();
                Response.Redirect("global.aspx");
            }

            if (!IsPostBack)
            {
                MenuItemCollection menuItems = alocatorMenu.Items;

                lb_loggedUser.Text = "";
                lb_loggedUser.Text += (string)Session["FirstName"] + " " + (string)Session["LastName"] + "       ID: " + ((string)Session["Name"]);
                // załadowanie danych do GridView Alocator
                 BindGrid();
            }
        }

        //obsługva zdażenia MenuItem Click
        protected void MineMenu_MenuItemClick(object sender, MenuEventArgs e)
        {
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
                        bl.UserLogOut((int)Session["Id"], "");
                        Session.Abandon();
                        Response.Redirect("global.aspx");
                    }
                    break;
            }
        }

        // pętla odświeżająca Update Panel
        protected void AlocatorHomeTimer_Tick(object sender, EventArgs e)
        {
            BindGrid();
        }

        // pobranie aktualnych danych
        private void BindGrid()
        {
            DataSet ds = new DataSet();

            bl.GetGridData(ref ds);
             
            if (ds.Tables[0].Rows.Count > 0)
            {
                gv_Alocator.DataSource = null;
                gv_Alocator.DataSource = ds;
                gv_Alocator.DataBind();
                ds.Dispose();
            }
            else
            {
                DataTable dt = GridViewStructCreate();
                gv_Alocator.DataSource = dt;
                gv_Alocator.DataBind();
                dt.Dispose();
            }
            SetBusStatus();
        }

        // wypełnienie kontrolek DropDownList wewnątrz GridView
        protected void Gv_Alocator_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            bool success;

            success = FillOperationDDL(sender, e);
            if (success)
            {
                success = FillPPSDDL(sender, e);
            }
            if (success)
            {
                success = FillAirPortDDL(sender, e);
            }
            if (success)
            {
                success = FillGateDDL(sender, e);
            }
            if (success)
            {
                success = FillBusDDL(sender, e);
            }
        }

        // pobranie i wypełnienie kontrolek DropDownList listą możliwych operacji
        private bool FillOperationDDL(object sender, GridViewRowEventArgs e)
        {
            bool success = false;
            try
            {
                DropDownList ddl_operationEdit = (DropDownList)e.Row.FindControl("ddl_operationEdit");
                if (ddl_operationEdit != null)
                {
                    DataSet ds = new DataSet();
                    bl.GetOperationList(ref ds);
                    ddl_operationEdit.DataSource = ds;
                    ddl_operationEdit.DataValueField = "Id";
                    ddl_operationEdit.DataTextField = "Operation";
                    ddl_operationEdit.DataBind();

                    // ustawnienie wartości startowej wyświetlania na podstawie danych z ciasteczka Alocator
                    ddl_operationEdit.SelectedIndex = ddl_operationEdit.Items.IndexOf(ddl_operationEdit.Items.FindByText((string)Session["Operation"]));
                    ddl_operationEdit.Dispose();
                    ds.Dispose();
                    success = true;
                }
                if (e.Row.RowType == DataControlRowType.Footer)
                {
                    DataSet ds = new DataSet();
                    DropDownList ddl_OperationAdd = (DropDownList)e.Row.FindControl("ddl_operationAdd");
                    bl.GetOperationList(ref ds);
                    ddl_OperationAdd.DataSource = ds;
                    ddl_OperationAdd.DataValueField = "Id";
                    ddl_OperationAdd.DataTextField = "Operation";
                    ddl_OperationAdd.DataBind();
                    ddl_OperationAdd.Dispose();
                    ds.Dispose();
                    success = true;
                }
            }
            catch (Exception FillOperationDDL_ex)
            {
                Response.Write("<script> alert('Błąd - FillOperationDDL()') </script>");
                success = false;
            }
            return success;
        }

        // pobranie i wypełnienie kontrolek DropDownList numerami stanowisk postojowych samolotów
        private bool FillPPSDDL(object sender, GridViewRowEventArgs e)
        {
            bool success = false;
            try
            {
                DropDownList ddl_ppsEdit = (DropDownList)e.Row.FindControl("ddl_ppsEdit");
                if (ddl_ppsEdit != null)
                {
                    DataSet ds = new DataSet();
                    bl.GetStations(ref ds);
                    ddl_ppsEdit.DataSource = ds;
                    ddl_ppsEdit.DataValueField = "Id";
                    ddl_ppsEdit.DataTextField = "StationNb";
                    ddl_ppsEdit.DataBind();

                    // ustawnienie wartości startowej wyświetlania na podstawie danych z ciasteczka Alocator
                    ddl_ppsEdit.SelectedIndex = ddl_ppsEdit.Items.IndexOf(ddl_ppsEdit.Items.FindByText((string)Session["Pps"]));
                    ddl_ppsEdit.Dispose();
                    ds.Dispose();
                    success = true;
                }
                if (e.Row.RowType == DataControlRowType.Footer)
                {
                    DataSet ds = new DataSet();
                    DropDownList ddl_ppsAdd = (DropDownList)e.Row.FindControl("ddl_ppsAdd");
                    bl.GetStations(ref ds);
                    ddl_ppsAdd.DataSource = ds;
                    ddl_ppsAdd.DataValueField = "Id";
                    ddl_ppsAdd.DataTextField = "StationNb";
                    ddl_ppsAdd.DataBind();
                    ddl_ppsAdd.Dispose();
                    ds.Dispose();
                    success = true;
                }
            }
            catch (Exception FillPPSDDL_ex)
            {
                Response.Write("<script> alert('Błąd - FillPPSDDL()') </script>");
                success = false;
            }
            return success;
        }

        // pobranie i wypełnienie kontrolek DropDownList listą obsługiwanych portów lotniczych
        private bool FillAirPortDDL(object sender, GridViewRowEventArgs e)
        {
            bool success = false;
            try
            {
                DropDownList ddl_airPortEdit = (DropDownList)e.Row.FindControl("ddl_airPortEdit");
                if (ddl_airPortEdit != null)
                {
                    DataSet ds = new DataSet();
                    bl.GetAirPort(ref ds);
                    ddl_airPortEdit.DataSource = ds;
                    ddl_airPortEdit.DataValueField = "Id";
                    ddl_airPortEdit.DataTextField = "IATA_Name";
                    ddl_airPortEdit.DataBind();

                    // ustawnienie wartości startowej wyświetlania na podstawie danych z ciasteczka Alocator
                    ddl_airPortEdit.SelectedIndex = ddl_airPortEdit.Items.IndexOf(ddl_airPortEdit.Items.FindByText((string)Session["AirPort"]));
                    ddl_airPortEdit.Dispose();
                    ds.Dispose();
                    success = true;
                }
                if (e.Row.RowType == DataControlRowType.Footer)
                {
                    DataSet ds = new DataSet();
                    DropDownList ddl_airPortAdd = (DropDownList)e.Row.FindControl("ddl_airPortAdd");
                    bl.GetAirPort(ref ds);
                    ddl_airPortAdd.DataSource = ds;
                    ddl_airPortAdd.DataValueField = "Id";
                    ddl_airPortAdd.DataTextField = "IATA_Name";
                    ddl_airPortAdd.DataBind();
                    ddl_airPortAdd.Dispose();
                    ds.Dispose();
                    success = true;
                }
            }
            catch (Exception FillAirPortDDL_ex)
            {
                Response.Write("<script> alert('Błąd - FillAirPortDDL()') </script>");
                success = false;
            }
            return success;
        }

        // pobranie i wypełnienie kontrolek DropDownList numerami gate
        private bool FillGateDDL(object sender, GridViewRowEventArgs e)
        {
            bool success = false;
            try
            {
                DropDownList ddl_gateEdit = (DropDownList)e.Row.FindControl("ddl_gateEdit");
                if (ddl_gateEdit != null)
                {
                    DataSet ds = new DataSet();
                    bl.GetGates(ref ds);
                    ddl_gateEdit.DataSource = ds;
                    ddl_gateEdit.DataValueField = "Id";
                    ddl_gateEdit.DataTextField = "GateNb";
                    ddl_gateEdit.DataBind();

                    // ustawnienie wartości startowej wyświetlania na podstawie danych z ciasteczka Alocator
                    ddl_gateEdit.SelectedIndex = ddl_gateEdit.Items.IndexOf(ddl_gateEdit.Items.FindByText((string)Session["Gate"]));
                    ddl_gateEdit.Dispose();
                    ds.Dispose();
                    success = true;
                }
                if (e.Row.RowType == DataControlRowType.Footer)
                {
                    DataSet ds = new DataSet();
                    DropDownList ddl_gateAdd = (DropDownList)e.Row.FindControl("ddl_gateAdd");
                    bl.GetGates(ref ds);
                    ddl_gateAdd.DataSource = ds;
                    ddl_gateAdd.DataValueField = "Id";
                    ddl_gateAdd.DataTextField = "GateNb";
                    ddl_gateAdd.DataBind();
                    ddl_gateAdd.Dispose();
                    ds.Dispose();
                    success = true;
                }
            }
            catch (Exception FillGateDDL_ex)
            {
                Response.Write("<script> alert('Błąd - FillGateDDL()') </script>");
                success = false;
            }
            return success;
        }

        // pobranie i wypełnienie kontrolek DropDownList numerami autobusów
        private bool FillBusDDL(object sender, GridViewRowEventArgs e)
        {
            bool success = false;
            DataSet ds = new DataSet();
            DropDownList ddl = new DropDownList();

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ddl = (DropDownList)e.Row.FindControl("ddl_busEdit");
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                ddl = (DropDownList)e.Row.FindControl("ddl_busAdd");
            }
            try
            {
                bl.GetBus(1, (string)Session["Bus"], ref ds);
                if (ds.Tables[0].Rows.Count == 0 && (string)Session["Bus"] != "")
                {
                    ds.Clear();
                    bl.GetBus(2, (string)Session["Bus"], ref ds);
                    ddl.DataSource = ds;
                    ddl.DataValueField = "Id";
                    ddl.DataTextField = "VehicleNb";
                    ddl.DataBind();
                    success = true;
                }
                else
                {
                    ddl.DataSource = ds;
                    ddl.DataValueField = "Id";
                    ddl.DataTextField = "VehicleNb";
                    ddl.DataBind();
                    // ustawnienie wartości startowej wyświetlania na podstawie danych z ciasteczka Alocator
                    ddl.SelectedIndex = ddl.Items.IndexOf(ddl.Items.FindByText((string)Session["Bus"]));
                    success = true;
                }
            }
            catch (Exception FillBusDDL_ex)
            {
                Response.Write("<script> alert('Błąd - FillBusDDL()') </script>");
            }
            ddl.Dispose();
            ds.Dispose();
            return success;
        }

        // ustalenie typu operacji i strefy ze względu na wczytane dane
        protected void Gv_CheckSecurityZone(object sender, EventArgs e)
        {
            for (int i = 0; i <= gv_Alocator.Rows.Count - 1; i++)
            {
                Label lb_Zone = (Label)gv_Alocator.Rows[i].FindControl("lb_zone");
                Label lb_Operation = (Label)gv_Alocator.Rows[i].FindControl("lb_operation");

                if (lb_Operation != null && lb_Operation.Text == "Odlot")
                {
                    gv_Alocator.Rows[i].Cells[3].BackColor = Color.Blue;
                    gv_Alocator.Rows[i].Cells[3].ForeColor = Color.White;
                }
                else
                {
                    if (lb_Zone != null && lb_Zone.Text == "0")
                    {
                        gv_Alocator.Rows[i].Cells[3].BackColor = Color.Green;
                        gv_Alocator.Rows[i].Cells[3].ForeColor = Color.White;
                    }
                    else
                    if (lb_Zone != null && lb_Zone.Text == "1")
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
            int iD = Convert.ToInt32(gv_Alocator.DataKeys[e.RowIndex].Value);
            GridViewRow row = gv_Alocator.Rows[e.RowIndex];
            Label bus = (Label)row.FindControl("lb_bus");

            bool result = bl.DeleteOperation(bus.Text, iD);

            row.Dispose();
            bus.Dispose();
            BindGrid();

            if (!result)
                Response.Write("<script> alert('Błąd - Gv_Alocator_RowDeleting()') </script>");
        }

        // Edycja istniejącej operacji
        protected void Gv_Alocator_RowEditing(object sender, GridViewEditEventArgs e)
        {
            btn_addNewOperation.Enabled = false;
            gv_Alocator.EditIndex = e.NewEditIndex;
            try
            {
                GridViewRow row = gv_Alocator.Rows[e.NewEditIndex];
                Label oper = (Label)row.FindControl("lb_operation");
                Session["Operation"] = oper.Text;

                Label iataName = (Label)row.FindControl("lb_airPort");
                Session["AirPort"] = iataName.Text;
                Label gateNb = (Label)row.FindControl("lb_gate");
                Session["Gate"] = gateNb.Text;
                Label ppsNb = (Label)row.FindControl("lb_pps");
                Session["Pps"] = ppsNb.Text;
                Label busNb = (Label)row.FindControl("lb_bus");
                Session["Bus"] = busNb.Text;

                row.Dispose();
                oper.Dispose();
                iataName.Dispose();
                gateNb.Dispose();
                ppsNb.Dispose();
                busNb.Dispose();
            }
            catch (Exception Gv_Alocator_RowEditing_ex)
            {
                Response.Write("<script> alert('Błąd Gv_Alocator_RowEditing()') </script>");
            }
            BindGrid();
        }

        //Dodanie nowej operacji
        protected void Gv_Alocator_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            bool success = false;

            if (e.CommandName.Equals("Insert"))
            {
                NewOperation newOp = new NewOperation();
                DateTime zeroDate = new DateTime(1999, 01, 01);
                DateTime dateTime = new DateTime();

                DropDownList ddl_OperationAdd = (DropDownList)gv_Alocator.FooterRow.FindControl("ddl_operationAdd");
                newOp.Operation = Convert.ToInt32(ddl_OperationAdd.SelectedValue);
                ddl_OperationAdd.Dispose();

                TextBox tb_FlightNbAdd = (TextBox)gv_Alocator.FooterRow.FindControl("tb_flightNbAdd");
                newOp.FlightNb = tb_FlightNbAdd.Text;
                tb_FlightNbAdd.Dispose();

                TextBox tb_GodzinaRozkładowaAdd = (TextBox)gv_Alocator.FooterRow.FindControl("tb_godzinaRozkładowaAdd");
                if (!DateTime.TryParseExact(tb_GodzinaRozkładowaAdd.Text, "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out dateTime))
                {
                    // w przypadku błędu konwersji daty
                    Response.Write("<script> alert('Błąd - format daty wydaje się być nieprawidłowy' ) </script>");
                    return;
                }
                newOp.GodzinaRozkładowa = dateTime;
                tb_GodzinaRozkładowaAdd.Dispose();

                DropDownList ddl_AirPortAdd = (DropDownList)gv_Alocator.FooterRow.FindControl("ddl_airPortAdd");
                newOp.AirPort = Convert.ToInt32(ddl_AirPortAdd.SelectedValue);
                ddl_AirPortAdd.Dispose();

                TextBox tb_PaxAdd = (TextBox)gv_Alocator.FooterRow.FindControl("tb_paxAdd");
                newOp.Pax = Convert.ToInt32(tb_PaxAdd.Text);
                tb_PaxAdd.Dispose();

                DropDownList ddl_GateAdd = (DropDownList)gv_Alocator.FooterRow.FindControl("ddl_gateAdd");
                newOp.Gate = Convert.ToInt32(ddl_GateAdd.SelectedValue);
                ddl_GateAdd.Dispose();

                DropDownList ddl_PpsAdd = (DropDownList)gv_Alocator.FooterRow.FindControl("ddl_ppsAdd");
                newOp.PPS = Convert.ToInt32(ddl_PpsAdd.SelectedValue);
                ddl_PpsAdd.Dispose();

                DropDownList ddl_BusAdd = (DropDownList)gv_Alocator.FooterRow.FindControl("ddl_busAdd");
                newOp.Bus = Convert.ToInt32(ddl_BusAdd.SelectedValue);
                ddl_BusAdd.Dispose();

                TextBox tb_RadioGateAdd = (TextBox)gv_Alocator.FooterRow.FindControl("tb_radioGateAdd");
                newOp.RadioGate = tb_RadioGateAdd.Text;
                tb_RadioGateAdd.Dispose();

                TextBox tb_RadioNeonAdd = (TextBox)gv_Alocator.FooterRow.FindControl("tb_radioNeonAdd");
                newOp.RadioNeon = tb_RadioNeonAdd.Text;
                tb_RadioNeonAdd.Dispose();

                newOp.BusNb = ddl_BusAdd.SelectedItem.ToString();

                try
                {
                    success = bl.AddNewOperation(newOp);
                }
                catch (Exception Gv_Alocator_RowCommand_ex)
                {
                    Response.Write("<script> alert('Błąd - Gv_Alocator_RowCommand()' ) </script>");
                }
            }
            else
            if (e.CommandName.Equals("Edit"))
            {
                btn_addNewOperation.Visible = true;
                gv_Alocator.ShowFooter = false;
                BindGrid();
            }
            if (success)
            {
                btn_addNewOperation.Visible = true;
                gv_Alocator.ShowFooter = false;
                AlocatorHomeTimer.Enabled = true;
                BindGrid();
            }
        }

        // Poprawienie danych w istniejącym rekordzie
        protected void Gv_Alocator_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            string id = ((Label)gv_Alocator.Rows[e.RowIndex].Cells[1].FindControl("lb_id")).Text;
            Session["Id"] = id;
            DateTime godzinaRozkładowa = CheckTimeFormat(((TextBox)gv_Alocator.Rows[e.RowIndex].Cells[1].FindControl("tb_godzinaRozkładowa")).Text);
            Session["GodzinaRozkladowa"] = godzinaRozkładowa;
            string operation = ((DropDownList)gv_Alocator.Rows[e.RowIndex].Cells[1].FindControl("ddl_operationEdit")).SelectedValue;
            Session["Operation"] = operation;
            string flightNb = ((TextBox)gv_Alocator.Rows[e.RowIndex].Cells[1].FindControl("tb_flightNbEdit")).Text;
            Session["FlightNb"] = flightNb;
            string airPort = ((DropDownList)gv_Alocator.Rows[e.RowIndex].Cells[1].FindControl("ddl_airPortEdit")).SelectedValue;
            Session["AirPort"] = airPort;
            string pax = ((TextBox)gv_Alocator.Rows[e.RowIndex].Cells[1].FindControl("tb_paxEdit")).Text;
            Session["Pax"] = pax;
            string gate = ((DropDownList)gv_Alocator.Rows[e.RowIndex].Cells[1].FindControl("ddl_gateEdit")).SelectedValue;
            Session["Gate"] = gate;
            string pps = ((DropDownList)gv_Alocator.Rows[e.RowIndex].Cells[1].FindControl("ddl_ppsEdit")).SelectedValue;
            Session["Pps"] = pps;
            string bus = ((DropDownList)gv_Alocator.Rows[e.RowIndex].Cells[1].FindControl("ddl_busEdit")).SelectedValue;
            Session["Bus"] = bus;
            string radioGate = ((TextBox)gv_Alocator.Rows[e.RowIndex].Cells[1].FindControl("tb_radioGateEdit")).Text;
            Session["RadioGate"] = radioGate;
            string radioNeon = ((TextBox)gv_Alocator.Rows[e.RowIndex].Cells[1].FindControl("tb_radioNeonEdit")).Text;
            Session["RadioNeon"] = radioNeon;

            bool success = false;

            success = bl.AlocatorRowUpdate(id, godzinaRozkładowa, operation, flightNb, airPort, pax, gate, pps, bus, radioGate, radioNeon);

            if (success)
            {
                gv_Alocator.EditIndex = -1;
                btn_addNewOperation.Enabled = true;
            }
            BindGrid();
        }

        // Rezygnacja z poprawienia operacjhi
        protected void Gv_Alocator_CancelEdit(object sender, GridViewCancelEditEventArgs e)
        {
            btn_addNewOperation.Enabled = true;
            gv_Alocator.EditIndex = -1;
            AlocatorHomeTimer.Enabled = true;
            BindGrid();
        }

        // Wyświetlenie "Footer" kontrolki GridView
        protected void Gv_Alocator_NewOperation(object sender, EventArgs e)
        {
            btn_addNewOperation.Visible = false;
            btn_cancelNewOperation.Visible = true;
            gv_Alocator.ShowFooter = true;
            AlocatorHomeTimer.Enabled = false;
            BindGrid();
        }

        // Anulowanie operacji dodawania nowej operacji
        protected void Gv_Alocator_CancelNewOperation(object sender, EventArgs e)
        {
            btn_addNewOperation.Visible = true;
            btn_cancelNewOperation.Visible = false;
            gv_Alocator.ShowFooter = false;
            AlocatorHomeTimer.Enabled = true;
            BindGrid();
        }

        // sprawdzenie statusu wszystkich pojazdów i naniesienie informacji graficznych na panel
        private void SetBusStatus()
        {
            //Bus status: 0 - Not Available, 1 - Empty, 2 - Free, 3 - In Work

            DataSet ds = new DataSet();
            bl.GetBus(4, (string)Session["Bus"], ref ds);
            string numer = "";
            string str = "";
            int status = -1;
            int work_Status = -1;
            if (ds.Tables[0] != null)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    // pobranie numeru autobusu
                    numer = ds.Tables[0].Rows[i].Field<string>("VehicleNb");
                    // pobranie statusu tego autobusu
                    status = ds.Tables[0].Rows[i].Field<int>("Bus_Status");
                    work_Status = ds.Tables[0].Rows[i].Field<int>("Work_Status");

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
            }
            ds.Dispose();
        }

        // utworzenie struktury GV, jeśli nie ma jeszcze żadnych operacji
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
            DateTime dateTime = new DateTime();
            if (!DateTime.TryParseExact(time, "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out dateTime))
            {
                // w przypadku błędu konwersji daty
                Response.Write("<script> alert('Błąd - format daty wydaje się być nieprawidłowy' ) </script>");
                Response.Redirect("Alocator.aspx");
            }
            return dateTime;
        }
    }
}