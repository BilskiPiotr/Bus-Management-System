using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Bus_Management_System
{
    public class BusinessLayer
    {
        DataAccessLayer dal = new DataAccessLayer();
        NewOperation newOp = new NewOperation();




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


        public DataTable GetCurrentOp(DataSet rawData, ref string errorMsg)
        {
            DataTable op = new DataTable();

            for (int i = 0; i < rawData.Tables[0].Rows.Count; i++)
            {
                string employeeId = GetOpData(Convert.ToInt32(rawData.Tables[0].Rows[0][1]), 1);
                string operationId = Convert.ToString(rawData.Tables[0].Rows[0][2]);
                string flightNb = Convert.ToString(rawData.Tables[0].Rows[0][3]);
                string paxCount = Convert.ToString(rawData.Tables[0].Rows[0][4]);
                string airPort = GetOpData(Convert.ToInt32(rawData.Tables[0].Rows[0][5]), 2);
                string pps = GetOpData(Convert.ToInt32(rawData.Tables[0].Rows[0][6]), 3);
                string gate = GetOpData(Convert.ToInt32(rawData.Tables[0].Rows[0][7]), 4);
                string bus = GetOpData(Convert.ToInt32(rawData.Tables[0].Rows[0][8]), 5);
                if (rawData.Tables[0].Rows[0][9] != null)
                {
                    string created = Convert.ToString(rawData.Tables[0].Rows[0][9]).Substring(11, 5);
                }
                else
                {
                    string created = "";
                }
                if (rawData.Tables[0].Rows[0][10] != null)
                {
                    string accepted = Convert.ToString(rawData.Tables[0].Rows[0][10]).Substring(11, 5);
                }
                {
                    string accepted = "";
                }
                if (rawData.Tables[0].Rows[0][11] != null)
                {
                    string startLoad = Convert.ToString(rawData.Tables[0].Rows[0][11]).Substring(11, 5);
                }
                {
                    string startLoad = "";
                }
                if (rawData.Tables[0].Rows[0][12] != null)
                {
                    string startDrive = Convert.ToString(rawData.Tables[0].Rows[0][12]).Substring(11, 5);
                }
                {
                    string startDrive = "";
                }
                if (rawData.Tables[0].Rows[0][13] != null)
                {
                    string startUnload = Convert.ToString(rawData.Tables[0].Rows[0][13]).Substring(11, 5);
                }
                {
                    string startUnload = "";
                }
                if (rawData.Tables[0].Rows[0][14] != null)
                {
                    string ednOp = Convert.ToString(rawData.Tables[0].Rows[0][14]).Substring(11, 5);
                }
                {
                    string endOp = "";
                }
                string radioGate = (string)rawData.Tables[0].Rows[0][15];
                string radioNeon = (string)rawData.Tables[0].Rows[0][16];
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
    }
}