﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TA_RealEstate_Kel11.Classes
{
    class PROPERTY_TYPE
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

        //Update a new type
        public Boolean updateType(string id, string name)
        {
            SqlCommand command = new SqlCommand("UPDATE propertyTipe SET nama=@nm WHERE idTipe=@id");

            command.Parameters.Add("@id", SqlDbType.VarChar).Value = id;
            command.Parameters.Add("@nm", SqlDbType.VarChar).Value = name;

            return func.ExecQuery(command);
        }

        //delete a new type
        public Boolean deleteType(string id)
        {
            SqlCommand command = new SqlCommand("DELETE FROM propertyTipe WHERE idTipe=@id");

            command.Parameters.Add("@id", SqlDbType.VarChar).Value = id;

            return func.ExecQuery(command);
        }
    }
}
