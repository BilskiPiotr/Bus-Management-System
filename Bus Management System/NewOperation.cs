using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bus_Management_System
{
    public class NewOperation
    {
        public int Operation { get; set; }
        public string FlightNb { get; set; }
        public DateTime GodzinaRozkładowa { get; set; }
        public int AirPort { get; set; }
        public int Pax { get; set; }
        public int Gate { get; set; }
        public int PPS { get; set; }
        public int Bus { get; set; }
        public string BusNb { get; set; }
        public string RadioGate { get; set; }
        public string RadioNeon { get; set; }
    }
}