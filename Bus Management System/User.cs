namespace Bus_Management_System
{
    public class User
    {
        public string strSessionID { get; }


        public int EmployeeId { get; set; }
        public int CompanyId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int AdminPrivileges { get; set; }
        public string bus { get; set; }



        public int interval { get; set; }
        public int operationStatus { get; set; }



        public int operation { get; set; }
        public string flightNb { get; set; }
        public string airPort { get; set; }
        public string pax { get; set; }
        public string pps { get; set; }
        public string gate { get; set; }
        public string radioGate { get; set; }
        public string radioNeon { get; set; }
        public int shengen { get; set; }
        public string portName { get; set; }
        public string country { get; set; }



        public string godzinaRozkladowa { get; set; }
        public string created { get; set; }
        public string accepted { get; set; }
        public string startLoad { get; set; }
        public string startDrive { get; set; }
        public string startUnload { get; set; }
        public string endOp { get; set; }



        public string ppsLat { get; set; }
        public string ppsLon { get; set; }
        public string gateLat { get; set; }
        public string gateLon { get; set; }



        public string startLat { get; set; }
        public string startLon { get; set; }
        public string startLocLatDegree { get; set; }
        public string startLocLonDegree { get; set; }



        public string audioFile { get; set; }
    }
}