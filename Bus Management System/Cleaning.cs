using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bus_Management_System
{
    public class Cleaning : Bus
    {
        // wyzerowanie wartości w zmiennej sesyjnej
        public void ClearSessionObject( User loggedUser)
        {
            loggedUser.Interval = 0;
            loggedUser.Operation = 0;
            loggedUser.FlightNb = "";
            loggedUser.AirPort = "";
            loggedUser.Pax = "";
            loggedUser.Pps = "";
            loggedUser.Gate = "";
            loggedUser.RadioGate = "";
            loggedUser.RadioNeon = "";
            loggedUser.Shengen = 0;
            loggedUser.PortName = "";
            loggedUser.Country = "";
            loggedUser.GodzinaRozkladowa = "";
            loggedUser.Created = "";
            loggedUser.Accepted = "";
            loggedUser.StartLoad = "";
            loggedUser.StartDrive = "";
            loggedUser.StartUnload = "";
            loggedUser.EndOp = "";
            loggedUser.PpsLat = 0.0d;
            loggedUser.PpsLon = 0.0d;
            loggedUser.GateLat = 0.0d;
            loggedUser.GateLon = 0.0d;
            loggedUser.Alert = 0;
            loggedUser.LoopTime = 0;
            loggedUser.Speed = 0.0d;
            loggedUser.Accuracy = 0.0d;
            loggedUser.OldDistanceT = 0.0d;
            loggedUser.DistanceT = 0.0d;
            loggedUser.OldDistanceS = 0.0d;
            loggedUser.DistanceS = 0.0d;
            loggedUser.OldDistanceN = 0.0d;
            loggedUser.DistanceN = 0.0d;
            loggedUser.PredictedDistance = 0.0d;
            loggedUser.CurrentLat = 0.0d;
            loggedUser.CurrentLon = 0.0d;
            loggedUser.StartLat = 0.0d;
            loggedUser.StartLon = 0.0d;
            loggedUser.StartLocLatDegree = "";
            loggedUser.StartLocLonDegree = "";
        }
    }
}