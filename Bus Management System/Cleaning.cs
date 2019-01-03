using System;

namespace Bus_Management_System
{
    public class Cleaning : Bus
    {
        public bool ClearSessionObject()
        {
            try
            {
                Session["Pps"] = 0;
                return true;
            }
            catch (Exception ClearSessionObject_ex)
            {
                return false;
            }
        }
    }
}