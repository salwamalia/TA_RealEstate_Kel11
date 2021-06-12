using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TA_RealEstate_Kel11.Classes
{
    class FUNC
    {
        DB_CONNECTION connection = new DB_CONNECTION();

        public Boolean ExecQuery(SqlCommand command)
        {
            command.Connection = connection.getConnection;
            connection.openConnection();

            if (command.ExecuteNonQuery() == 1)
            {
                connection.closeConnection();
                return true;
            }
            else
            {
                connection.closeConnection();
                return false;
            }
        }

        //get data
        public DataTable getData(SqlCommand command)
        {
            command.Connection = connection.getConnection;

            //SqlCommand command = new SqlCommand(query, connection.getConnection);

            SqlDataAdapter adapter = new SqlDataAdapter(command);

            DataTable table = new DataTable();

            adapter.Fill(table);

            return table;
        }
    }
}
