using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TA_RealEstate_Kel11
{
    class koneksi
    {
        public SqlConnection Getcon()
        {
            SqlConnection connection = new SqlConnection();
            //connection.ConnectionString = @"Data Source=LAPTOP-L1AODT95;Initial Catalog=REALESTATE;Integrated Security=True";
            connection.ConnectionString = @"Data Source=WINDOWS-LD56BQV;Initial Catalog=REALESTATE;Integrated Security=True";
            return connection;
        }
    }
}
