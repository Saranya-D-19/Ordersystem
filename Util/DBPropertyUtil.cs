using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order_Management_System.Util
{
  public static class DBPropertyUtil
        {
            public static string GetConnectionString()
            {
                return "Data Source=DESKTOP-N03VLVF;Initial Catalog=OrderManagementDB;Integrated Security=True;";
            }
        }
}
