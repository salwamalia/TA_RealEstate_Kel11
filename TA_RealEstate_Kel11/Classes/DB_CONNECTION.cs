using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TA_RealEstate_Kel11.Classes
{
    class DB_CONNECTION
    {
        SqlConnection connection = new SqlConnection(@"Data Source=LAPTOP-L1AODT95;Initial Catalog=REALESTATE;Integrated Security=True");
        //SqlConnection connection = new SqlConnection(@"Data Source=WINDOWS-LD56BQV;Initial Catalog=REALESTATE;Integrated Security=True);

        //get the connection
        public SqlConnection getConnection
        {
            get
            {
                return connection;
            }
        }

        //open the connection
        public void openConnection()
        {
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }
        }

        //close the connection
        public void closeConnection()
        {
            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
        }
    }
}
