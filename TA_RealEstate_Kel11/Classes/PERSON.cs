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
        private string JenisKelamin;
        private string phone;
        private string email;
        private string address;

        public PERSON() { }

        public PERSON(string ID, string NAME, string JENISKELAMIN, string PHONE, string EMAIL, string ADDRESS)
        {
            this.id = ID;
            this.name = NAME;
            this.JenisKelamin = JENISKELAMIN;
            this.phone = PHONE;
            this.email = EMAIL;
            this.address = ADDRESS;
        }

        public DataTable getAllPemilik()
        {
            SqlCommand command = new SqlCommand("SELECT * FROM pemilik");

            return func.getData(command);
        }

        public Boolean updatePemilik(string id, string name, string JenisKelamin, string phone, string email, string address)
        {
            SqlCommand command = new SqlCommand("UPDATE pemilik SET nama=@nm, jeniskelamin=@jk, telepon=@tlp, email=@mail, alamat=@adrs WHERE idPemilik=@id");

            command.Parameters.Add("@id", SqlDbType.VarChar).Value = id;
            command.Parameters.Add("@nm", SqlDbType.VarChar).Value = name;
            command.Parameters.Add("@jk", SqlDbType.VarChar).Value = JenisKelamin;
            command.Parameters.Add("@tlp", SqlDbType.VarChar).Value = phone;
            command.Parameters.Add("@mail", SqlDbType.VarChar).Value = email;
            command.Parameters.Add("@adrs", SqlDbType.VarChar).Value = address;

            return func.ExecQuery(command);
        }

        //delete a new type
        public Boolean deletePemilik(string id)
        {
            SqlCommand command = new SqlCommand("DELETE FROM pemilik WHERE idPemilik=@id");

            command.Parameters.Add("@id", SqlDbType.VarChar).Value = id;

            return func.ExecQuery(command);
        }

        public Boolean updateClient(string id, string name, string JenisKelamin, string phone, string email, string address)
        {
            SqlCommand command = new SqlCommand("UPDATE client SET nama=@nm, jeniskelamin=@jk, telepon=@tlp, email=@mail, alamat=@adrs WHERE idClient=@id");

            command.Parameters.Add("@id", SqlDbType.VarChar).Value = id;
            command.Parameters.Add("@nm", SqlDbType.VarChar).Value = name;
            command.Parameters.Add("@jk", SqlDbType.VarChar).Value = JenisKelamin;
            command.Parameters.Add("@tlp", SqlDbType.VarChar).Value = phone;
            command.Parameters.Add("@mail", SqlDbType.VarChar).Value = email;
            command.Parameters.Add("@adrs", SqlDbType.VarChar).Value = address;

            return func.ExecQuery(command);
        }

        //delete a new type
        public Boolean deleteClient(string id)
        {
            SqlCommand command = new SqlCommand("DELETE FROM client WHERE idClient=@id");

            command.Parameters.Add("@id", SqlDbType.VarChar).Value = id;

            return func.ExecQuery(command);
        }
    }
}
