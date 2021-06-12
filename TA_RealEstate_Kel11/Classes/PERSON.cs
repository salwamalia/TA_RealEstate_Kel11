using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TA_RealEstate_Kel11.Classes
{
    class PERSON
    {
        FUNC func = new FUNC();

        //class ini digunakan untuk owner dan client
        private string id;
        private string name;
        private string phone;
        private string email;
        private string address;

        public PERSON() { }

        public PERSON(string ID, string NAME, string PHONE, string EMAIL, string ADDRESS)
        {
            this.id = ID;
            this.name = NAME;
            this.phone = PHONE;
            this.email = EMAIL;
            this.address = ADDRESS;
        }

        //insert a new type
        public Boolean insertPerson(string ownerORclient, PERSON person)
        {
            string tableName = ownerORclient;

            SqlCommand command = new SqlCommand("INSERT INTO '" + tableName + "' (nama, telepon, email, alamat) VALUES (@nm, @phn, @mail, @adrs)");

            command.Parameters.Add("@nm", SqlDbType.VarChar).Value = person.name;
            command.Parameters.Add("@phn", SqlDbType.VarChar).Value = person.phone;
            command.Parameters.Add("@mail", SqlDbType.VarChar).Value = person.email;
            command.Parameters.Add("@adrs", SqlDbType.VarChar).Value = person.address;

            return func.ExecQuery(command);
        }
    }
}
