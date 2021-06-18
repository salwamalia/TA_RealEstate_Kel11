using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TA_RealEstate_Kel11.Classes
{
    class KATEGORI
    {
        FUNC func = new FUNC();
        
        //get all kategoriBayar
        public DataTable getAllBayar()
        {
            SqlCommand command = new SqlCommand("SELECT * FROM kategoriBayar");

            return func.getData(command);
        }

        //Update a new kategoriBayar
        public Boolean updateKategori(string id, string namakategori, string keterangan)
        {
            SqlCommand command = new SqlCommand("UPDATE kategoriBayar SET namakategori=@nm, keterangan=@ket WHERE idKategoriBayar=@id");

            command.Parameters.Add("@id", SqlDbType.VarChar).Value = id;
            command.Parameters.Add("@nm", SqlDbType.VarChar).Value = namakategori;
            command.Parameters.Add("@ket", SqlDbType.VarChar).Value = keterangan;

            return func.ExecQuery(command);
        }

        //delete a new kategoriBayar
        public Boolean deleteKategori(string id)
        {
            SqlCommand command = new SqlCommand("DELETE FROM kategoriBayar WHERE idKategoriBayar=@id");

            command.Parameters.Add("@id", SqlDbType.VarChar).Value = id;

            return func.ExecQuery(command);
        }

        //delete a new kategoriBayar
        public Boolean deleteCicilan(string id)
        {
            SqlCommand command = new SqlCommand("DELETE FROM kategoriCicilan WHERE idCicilan=@id");

            command.Parameters.Add("@id", SqlDbType.VarChar).Value = id;

            return func.ExecQuery(command);
        }
    }
}
