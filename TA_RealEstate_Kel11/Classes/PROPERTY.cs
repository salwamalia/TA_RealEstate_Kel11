using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TA_RealEstate_Kel11.Classes
{
    class PROPERTY
    {
        FUNC func = new FUNC();

        //get all type
        public DataTable getAllTypes()
        {
            SqlCommand command = new SqlCommand("SELECT * FROM propertyTipe");

            return func.getData(command);
        }

        public DataTable getAlljabatan()
        {
            SqlCommand command = new SqlCommand("SELECT * FROM jabatan");

            return func.getData(command);
        }

        public DataTable getAllPegawai()
        {
            SqlCommand command = new SqlCommand("SELECT * FROM pegawai");

            return func.getData(command);
        }

        public DataTable getAllProperty()
        {
            SqlCommand command = new SqlCommand("SELECT * FROM property");

            return func.getData(command);
        }

        //delete a new property
        public Boolean deleteProperty(string id)
        {
            SqlCommand command = new SqlCommand("DELETE FROM property WHERE idProperty=@id");

            command.Parameters.Add("@id", SqlDbType.VarChar).Value = id;

            return func.ExecQuery(command);
        }
    }
}
