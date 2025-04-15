using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace Order_Management_System.Util
{
    public static class DBConnUtil
    {
        public static SqlConnection GetDBConnection()
        {
            string connectionString = DBPropertyUtil.GetConnectionString();
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            return  conn;
        }
    }
}
