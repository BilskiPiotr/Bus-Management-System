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


            sqlCmd.CommandText = " INSERT INTO Operations (Employee_Id, Operation, FlightNb, Pax, AirPort, PPS, Gate, Bus, Created) VALUES (@basicUserId, @operation, @flightNb, @pax, @airPort, @pps, @gate, @bus, @created)";

            //+ " WHERE (airPort = (SELECT Id FROM AirPorts WHERE IATA_Name = '" + iataName + "'), PPS = (SELECT Id FROM Stations WHERE StationNb = '" + pps + "'), Gate = (SELECT Id FROM Gates WHERE GateNb = '" + gate + "'), Bus = (SELECT Id FROM Vehicles WHERE VehicleNb = '" + bus + "')";

            sqlCmd.Parameters.AddWithValue("@basicUserId", id);
            sqlCmd.Parameters.AddWithValue("@operation", operation);
            sqlCmd.Parameters.AddWithValue("@flightNb", flightNumber);
            sqlCmd.Parameters.AddWithValue("@pax", paxCount);
            sqlCmd.Parameters.AddWithValue("@airPort", iataName);
            sqlCmd.Parameters.AddWithValue("@pps", planeStation);
            sqlCmd.Parameters.AddWithValue("@gate", gateNb);
            sqlCmd.Parameters.AddWithValue("@bus", busNb);
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