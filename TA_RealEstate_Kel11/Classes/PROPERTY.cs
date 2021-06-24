using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TA_RealEstate_Kel11.Classes
{
    class PROPERTY
    {
        FUNC func = new FUNC();

        //get all property
        public DataTable getAllBayar()
        {
            SqlCommand command = new SqlCommand("SELECT * FROM property");

            return func.getData(command);
        }

        //Update a new property
        public Boolean updateProperty(string idProperty,string namaProperty,string idTipe,string idPemilik,string ukuran, string fasilitas,int harga,Image gambar)
        {
            SqlCommand command = new SqlCommand("UPDATE property SET namaProperty=@namaProperty,idTipe=@idTipe,idPemilik=@idPemilik,ukuran=@ukuran," +
                "fasilitas=@fasilitas,harga=@harga,gambar=@gambar WHERE idProperty=@idProperty");

            command.Parameters.Add("@idProperty", SqlDbType.VarChar).Value = idProperty;
            command.Parameters.Add("@namaProperty", SqlDbType.VarChar).Value = namaProperty;
            command.Parameters.Add("@idTipe", SqlDbType.VarChar).Value = idTipe;
            command.Parameters.Add("@idPemilik", SqlDbType.VarChar).Value = idPemilik;
            command.Parameters.Add("@ukuran", SqlDbType.VarChar).Value = ukuran;
            command.Parameters.Add("@fasilitas", SqlDbType.VarChar).Value = fasilitas;
            command.Parameters.Add("@harga", SqlDbType.VarChar).Value = harga;
            command.Parameters.Add("@gambar", SqlDbType.VarChar).Value = gambar;

            return func.ExecQuery(command);
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
