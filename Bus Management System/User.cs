﻿using System;

namespace Bus_Management_System
{
    public class User
    {
        public string StrSessionID { get; }


        public int Id { get; set; }
        public int CompanyId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int AdminPrivileges { get; set; }
        public string Bus { get; set; }



        public int Interval { get; set; }
        public int OperationStatus { get; set; }



        public int Operation { get; set; }
        public string FlightNb { get; set; }
        public string AirPort { get; set; }
        public string Pax { get; set; }
        public string Pps { get; set; }
        public string Gate { get; set; }
        public string RadioGate { get; set; }
        public string RadioNeon { get; set; }
        public int Shengen { get; set; }
        public string PortName { get; set; }
        public string Country { get; set; }



        public string GodzinaRozkladowa { get; set; }
        public string Created { get; set; }
        public string Accepted { get; set; }
        public string StartLoad { get; set; }
        public string StartDrive { get; set; }
        public string StartUnload { get; set; }
        public string EndOp { get; set; }



        public double PpsLat { get; set; }
        public double PpsLon { get; set; }
        public double GateLat { get; set; }
        public double GateLon { get; set; }



        public int Alert { get; set; }
        public int LoopTime { get; set; }



        public double Speed { get; set; }
        public double Accuracy { get; set; }
        public double OldDistanceT { get; set; }
        public double DistanceT { get; set; }
        public double OldDistanceS { get; set; }
        public double DistanceS { get; set; }
        public double OldDistanceN { get; set; }
        public double DistanceN { get; set; }

        public double PredictedDistance { get; set; }

        public double CurrentLat { get; set; }
        public double CurrentLon { get; set; }

        public double StartLat { get; set; }
        public double StartLon { get; set; }

        public string StartLocLatDegree { get; set; }
        public string StartLocLonDegree { get; set; }


        public string LogFilePath { get; set; }

        public string Al_Id { get; set; }
        public string Al_Op { get; set; }
        public DateTime Al_Gr { get; set; }
        public string Al_Fl { get; set; }
        public string Al_Ai { get; set; }
        public string Al_Pa { get; set; }
        public string Al_Ga { get; set; }
        public string Al_Pp { get; set; }
        public string Al_Bu { get; set; }
        public string Al_Rg { get; set; }
        public string Al_Rn { get; set; }
    }
}