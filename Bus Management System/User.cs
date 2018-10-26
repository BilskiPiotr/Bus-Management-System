namespace Bus_Management_System
{
    public class User
    {
        public string StrSessionID { get; }


        public int EmployeeId { get; set; }
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



        public string PpsLat { get; set; }
        public string PpsLon { get; set; }
        public string GateLat { get; set; }
        public string GateLon { get; set; }



        public string StartLat { get; set; }
        public string StartLon { get; set; }
        public string StartLocLatDegree { get; set; }
        public string StartLocLonDegree { get; set; }



        public int Alert { get; set; }



        public double Speed { get; set; }
        public double Accuracy { get; set; }
        public string Distance { get; set; }

    }
}