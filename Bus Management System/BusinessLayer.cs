﻿using System;
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


        public DataTable GetUserData(int iD)
        {
            SqlCommand cmd = new SqlCommand("SELECT Employee_CompanyId, Employee_Imie, Employee_Nazwisko, Employee_Priv FROM Employees_Basic WHERE Id = @userId");
            cmd.Parameters.AddWithValue("@userId", iD);

            try
            {
                DataTable dt = dal.GetDataTable(cmd);
                return dt;
            }
            catch
            {
                return null;
            }
        }

        // zapisanie w systemie informacji o zalogowaniu użytykownika
        public bool UserLogIn(int iD, DateTime loginDate)
        {
            try
            {
                DataAccessLayer dal = new DataAccessLayer();
                SqlCommand cmd = new SqlCommand("INSERT INTO Employees_Status (Employee_Id, Employee_Login) VALUES (@iD, @loginDate)");
                cmd.Parameters.AddWithValue("@iD", iD);
                cmd.Parameters.AddWithValue("@loginDate", loginDate);
                dal.QueryExecution(cmd);
                return true;
            }
            catch
            {
                return false;
            }

        }


        public bool UserLogOut(HttpCookie cookie)
        {
            SqlCommand cmd = new SqlCommand();
            try
            {
                cmd.CommandText = "INSERT INTO Employees_Status (Employee_Id, Employee_Logout) VALUES (@userId, @logOut)";
                cmd.Parameters.AddWithValue("@userId", Convert.ToInt32(cookie.Values["Id"]));
                cmd.Parameters.AddWithValue("@logOut", DateTime.Now);
                dal.QueryExecution(cmd);
                cmd.CommandText = "UPDATE VEHICLES SET Bus_Status = @busStatus WHERE VehicleNb = @busNb";
                cmd.Parameters.AddWithValue("@busStatus", 1);
                cmd.Parameters.AddWithValue("@busNb", cookie.Values["busNb"].ToString());
                dal.QueryExecution(cmd);
                return true;
            }
            catch
            {
                return false;
            }

        }
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
            string sqlQuery = "SELECT * FROM Vehicles";
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


        public DataSet GetOperations(string bus)
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM Operations WHERE Bus=(SELECT Id FROM Vehicles WHERE VehicleNb = @busNb) AND Finished = @finished");
            cmd.Parameters.AddWithValue("@busNb", bus);
            cmd.Parameters.AddWithValue("@finished", 0);
            DataSet ds = new DataSet();
            ds = dal.GetDataSet(cmd);

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
            catch
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
            catch
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


        public DataSet GetPPS(int pps)
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM Stations WHERE Id = @ppsNb");
            cmd.Parameters.AddWithValue("@ppsNb", pps);
            try
            {
                DataSet ds = dal.GetDataSet(cmd);
                return ds;
            }
            catch
            {
                return null;
            }
        }


        public string GetAirPort(int airPort, ref int shengen, ref string fullName, ref string country)
        {
            // rozbudowac do zwracania FullName i CountryName
            SqlCommand cmd = new SqlCommand("SELECT a.IATA_Name, a.Full_Name, b.Country_name, b.Shengen " +
                                            "FROM AirPorts AS a " +
                                            "INNER JOIN Countries AS b ON a.Country_Id=b.Id " +
                                            "WHERE a.Id = @airPortNb ");
            cmd.Parameters.AddWithValue("@airPortNb", airPort);
            try
            {
                DataSet ds = dal.GetDataSet(cmd);
                shengen = Convert.ToInt32(GetCountry(ds.Tables[0].Rows[0].Field<int>("Shengen")));
                return ds.Tables[0].Rows[0].Field<string>("IATA_Name");
            }
            catch
            {
                return "";
            }
        }


        public int GetCountry(int country)
        {
            SqlCommand cmd = new SqlCommand("SELECT Shengen FROM Countries WHERE Id = @countryId");
            cmd.Parameters.AddWithValue("@countryId", country);
            try
            {
                DataSet ds = dal.GetDataSet(cmd);
                return ds.Tables[0].Rows[0].Field<int>("Shengen");
            }
            catch
            {
                return 0;
            }
        }


        public DataSet GetGate(int gate)
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM Gates WHERE Id = @gateId");
            cmd.Parameters.AddWithValue("@gateId", gate);
            try
            {
                DataSet ds = dal.GetDataSet(cmd);
                return ds;
            }
            catch
            {
                return null;
            }
        }



        public int GetOperations(int operation)
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM OperationType WHERE Id = @operationId");
            cmd.Parameters.AddWithValue("@operationId", operation);
            try
            {
                DataSet ds = dal.GetDataSet(cmd);
                return ds.Tables[0].Rows[0].Field<int>("OpValue");
            }
            catch
            { 
                return 0;
            }
        }
    }
}