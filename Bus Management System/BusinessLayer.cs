using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace Bus_Management_System
{
    public class BusinessLayer
    {
        DataAccessLayer dal = new DataAccessLayer();
        NewOperation newOp = new NewOperation();
        public static SqlDataAdapter adap;
        public static DataTable dt;
        // Stored image path before updating the record
        public static string imgEditPath;


        public void ClearFields(BusinessLayer bl)
        {

        }

        public DataSet GetGates()
        {
            string sqlQuery = "SELECT Id, GateNb FROM Gates";
            DataSet ds = new DataSet();
            SqlCommand sqlCmd = new SqlCommand(sqlQuery);
            ds = dal.GetDataSet(sqlCmd);

            return ds;
        }


        public DataSet GetStations()
        {
            string sqlQuery = "SELECT Id, StationNb FROM Stations";
            DataSet ds = new DataSet();
            SqlCommand sqlCmd = new SqlCommand(sqlQuery);
            ds = dal.GetDataSet(sqlCmd);

            return ds;
        }


        public DataSet GetAirPort()
        {
            string sqlQuery = "SELECT Id, IATA_Name FROM AirPorts";
            DataSet ds = new DataSet();
            SqlCommand sqlCmd = new SqlCommand(sqlQuery);
            ds = dal.GetDataSet(sqlCmd);

            return ds;
        }


        public DataSet GetBus()
        {
            string sqlQuery = "SELECT Id, Status, VehicleNb FROM Vehicles";
            DataSet ds = new DataSet();
            SqlCommand sqlCmd = new SqlCommand(sqlQuery);
            ds = dal.GetDataSet(sqlCmd);

            return ds;
        }


        public string GetOpData(int id, int dataSource)
        {
            string sqlQuery = "";

            switch (dataSource)
            {
                case 1:
                    sqlQuery = "SELECT Employee_CompanyId FROM Employees_Basic WHERE Id = '" + id + "'";
                    break;
                case 2:
                    sqlQuery = "SELECT IATA_Name FROM AirPorts WHERE Id = '" + id + "'";
                    break;
                case 3:
                    sqlQuery = "SELECT StationNb FROM  Stations WHERE Id = '" + id + "'";
                    break;
                case 4:
                    sqlQuery = "SELECT GateNb FROM  Gates WHERE Id = '" + id + "'";
                    break;
                case 5:
                    sqlQuery = "SELECT VehicleNb FROM  Vehicles WHERE Id = '" + id + "'";
                    break;
            }
            DataTable dt = new DataTable();
            SqlCommand sqlCmd = new SqlCommand(sqlQuery);
            dt= dal.GetDataTable(sqlCmd);

            return Convert.ToString(dt.Rows[0][0]);
        }


        //public static DataTable FillAlocatorGrid()
        //{
            //DataSet ds = GetOperations();
            //dt = GetCurrentOp(ds);

        //}


        public DataTable GetCurrentOp(DataSet rawData)
        {
            string created = "";
            string accepted = "";
            string startLoad = "";
            string startDrive = "";
            string startUnload = "";
            string endOp = "";

            DataTable op = new DataTable();

            op.Columns.Add("employeeId", typeof(string));
            op.Columns.Add("operationId", typeof(string));
            op.Columns.Add("flightNb", typeof(string));
            op.Columns.Add("paxCount", typeof(string));
            op.Columns.Add("airPort", typeof(string));
            op.Columns.Add("pps", typeof(string));
            op.Columns.Add("gate", typeof(string));
            op.Columns.Add("bus", typeof(string));
            op.Columns.Add("created", typeof(string));
            op.Columns.Add("accepted", typeof(string));
            op.Columns.Add("startLoad", typeof(string));
            op.Columns.Add("startDrive", typeof(string));
            op.Columns.Add("startUnload", typeof(string));
            op.Columns.Add("endOp", typeof(string));
            op.Columns.Add("radioGate", typeof(int));
            op.Columns.Add("radioNeon", typeof(int));


            for (int i = 0; i < rawData.Tables[0].Rows.Count; i++)
            {
                string employeeId = GetOpData(Convert.ToInt32(rawData.Tables[0].Rows[i][1]), 1);
                string operationId = "";
                if (Convert.ToString(rawData.Tables[0].Rows[i][2]) == "0")
                    operationId = "Odlot";
                else
                    operationId = "Przylot";
                string flightNb = Convert.ToString(rawData.Tables[0].Rows[i][3]);
                string paxCount = Convert.ToString(rawData.Tables[0].Rows[i][4]);
                string airPort = GetOpData(Convert.ToInt32(rawData.Tables[0].Rows[i][5]), 2);
                string pps = GetOpData(Convert.ToInt32(rawData.Tables[0].Rows[i][6]), 3);
                string gate = GetOpData(Convert.ToInt32(rawData.Tables[0].Rows[i][7]), 4);
                string bus = GetOpData(Convert.ToInt32(rawData.Tables[0].Rows[i][8]), 5);
                if (rawData.Tables[0].Rows[i][9] != null && Convert.ToString(rawData.Tables[0].Rows[i][9]) != "")
                {
                    created = Convert.ToString(rawData.Tables[0].Rows[i][9]).Substring(11, 5);
                }
                else
                {
                    created = "00:00";
                }
                if (rawData.Tables[0].Rows[i][10] != null && Convert.ToString(rawData.Tables[0].Rows[i][10]) != "")
                {
                    accepted = Convert.ToString(rawData.Tables[0].Rows[i][10]).Substring(11, 5);
                }
                {
                    accepted = "00:00";
                }
                if (rawData.Tables[0].Rows[i][11] != null && Convert.ToString(rawData.Tables[0].Rows[i][11]) != "")
                {
                    startLoad = Convert.ToString(rawData.Tables[0].Rows[i][11]).Substring(11, 5);
                }
                {
                    startLoad = "00:00";
                }
                if (rawData.Tables[0].Rows[i][12] != null && Convert.ToString(rawData.Tables[0].Rows[i][12]) != "")
                {
                    startDrive = Convert.ToString(rawData.Tables[0].Rows[i][12]).Substring(11, 5);
                }
                {
                    startDrive = "00:00";
                }
                if (rawData.Tables[0].Rows[i][13] != null && Convert.ToString(rawData.Tables[0].Rows[i][13]) != "")
                {
                    startUnload = Convert.ToString(rawData.Tables[0].Rows[i][13]).Substring(11, 5);
                }
                {
                    startUnload = "00:00";
                }
                if (rawData.Tables[0].Rows[i][14] != null && Convert.ToString(rawData.Tables[0].Rows[i][14]) != "")
                {
                    endOp = Convert.ToString(rawData.Tables[0].Rows[i][14]).Substring(11, 5);
                }
                {
                    endOp = "00:00";
                }
                string radioGate = (string)rawData.Tables[0].Rows[i][15];
                string radioNeon = (string)rawData.Tables[0].Rows[i][16];

                op.Rows.Add(employeeId, operationId, flightNb, paxCount, airPort, pps, gate, bus, created, accepted, startLoad, startDrive, startUnload, endOp, radioGate, radioNeon);
            }

            return op;
        }


        public DataSet GetOperations()
        {
            string sqlQuery = "SELECT * FROM Operations";
            DataSet ds = new DataSet();
            SqlCommand sqlCmd = new SqlCommand(sqlQuery);
            ds = dal.GetDataSet(sqlCmd);

            return ds;
        }


        public Boolean AddNewOperation(NewOperation newOp, int id)
        {
            SqlCommand sqlCmd = new SqlCommand();

            DateTime dateTime = DateTime.Now;

            int operation = newOp.OperationType;
            int paxCount = newOp.PaxCount;
            string flightNumber = newOp.FlightNb;
            int busNb = newOp.BusSelected;
            int iataName = newOp.Port;
            int planeStation = newOp.PPSNb;
            int gateNb = newOp.GateNb;
            string radioNeon = newOp.RadioNeon;
            string radioGate = newOp.RadioGate;

            sqlCmd.CommandText = "Update Vehicles SET Status = @status WHERE VehicleNb = @busSelected";
            sqlCmd.Parameters.Add(new SqlParameter("@status", 3));
            sqlCmd.Parameters.Add(new SqlParameter("@busSelected", busNb));

            try
            {
                dal.QueryExecution(sqlCmd);
            }
            catch (Exception ex)
            {
                // dodać obsługę błędu
            }


            sqlCmd.CommandText = " INSERT INTO Operations (Employee_Id, Operation, FlightNb, Pax, AirPort, PPS, Gate, Bus, RadioGate, RadioNeon, Created) VALUES (@basicUserId, @operation, @flightNb, @pax, @airPort, @pps, @gate, @bus, @radioGate, @radioNeon, @created)";

            //+ " WHERE (airPort = (SELECT Id FROM AirPorts WHERE IATA_Name = '" + iataName + "'), PPS = (SELECT Id FROM Stations WHERE StationNb = '" + pps + "'), Gate = (SELECT Id FROM Gates WHERE GateNb = '" + gate + "'), Bus = (SELECT Id FROM Vehicles WHERE VehicleNb = '" + bus + "')";

            sqlCmd.Parameters.AddWithValue("@basicUserId", id);
            sqlCmd.Parameters.AddWithValue("@operation", operation);
            sqlCmd.Parameters.AddWithValue("@flightNb", flightNumber);
            sqlCmd.Parameters.AddWithValue("@pax", paxCount);
            sqlCmd.Parameters.AddWithValue("@airPort", iataName);
            sqlCmd.Parameters.AddWithValue("@pps", planeStation);
            sqlCmd.Parameters.AddWithValue("@gate", gateNb);
            sqlCmd.Parameters.AddWithValue("@bus", busNb);
            sqlCmd.Parameters.AddWithValue("@radioGate", radioGate);
            sqlCmd.Parameters.AddWithValue("@radioNeon", radioNeon);
            sqlCmd.Parameters.AddWithValue("@created", dateTime);

            try
            {
                dal.QueryExecution(sqlCmd);
            }
            catch (Exception ex)
            {
                // dodać obsługę błędu
            }

            return true;
        }


        public static void CreateOperationDDL(ref DropDownList ddl, DataTable emptyDT)
        {

            ddl.DataSource = emptyDT;
            ddl.DataTextField = "opType";
            ddl.DataValueField = "opValue";
            ddl.DataBind();
        }
    }
}