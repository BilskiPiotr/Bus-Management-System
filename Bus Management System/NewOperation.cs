using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bus_Management_System
{
    public class NewOperation
    {
        public int OperationType { get; set; }
        public string FlightNb { get; set; }
        public int PaxCount { get; set; }
        public string RadioGate { get; set; }
        public string RadioNeon { get; set; }
        public int PPSNb { get; set; }
        public int Port { get; set; }
        public int GateNb { get; set; }
        public int BusSelected { get; set; }
    }
}