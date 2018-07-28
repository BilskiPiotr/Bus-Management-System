using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace Bus_Management_System
{
    public class User
    {
        public int EmployeeId { get; set; }
        public int CompanyId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int AdminPrivileges { get; set; }
        public string strSessionID { get; }
    }
}