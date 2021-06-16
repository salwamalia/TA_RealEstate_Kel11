using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace TA_RealEstate_Kel11
{
    public partial class FormJabatan : Form
    {
        public FormJabatan()
        {
            InitializeComponent();
        }

        REALESTATEDataContext dc = new REALESTATEDataContext();

        SqlConnection con = new SqlConnection(@"Data Source=LAPTOP-L1AODT95;Initial Catalog=REALESTATE;Integrated Security=True");

        private string IDOtomatis()
        {
            string autoid = null;

            string myConnectionString = @"Data Source=LAPTOP-L1AODT95;Initial Catalog=REALESTATE;Integrated Security=True";
            SqlConnection myConnection = new SqlConnection(myConnectionString);
            myConnection.Open();

            string sqlQuery = "SELECT TOP 1 idJabatan FROM jabatan ORDER BY idJabatan DESC";
            SqlCommand cmd = new SqlCommand(sqlQuery, myConnection);
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                string input = dr["idJabatan"].ToString();
                string angka = input.Substring(input.Length - Math.Min(2, input.Length));
                int number = Convert.ToInt32(angka);
                number += 1;
                string str = number.ToString("D2");

                autoid = "JBT" + str;
            }

            if (autoid == null)
            {
                autoid = "JBT01";
            }

            myConnection.Close();

            return autoid;
        }

        private void btnSimpan_Click(object sender, EventArgs e)
        {
            /*string id = txtID.Text;
            string nama = txtNama.Text;

            var data = new jabatan
            {
                idJabatan = id,
                jabatan1 = nama
            };

            dc.jabatans.InsertOnSubmit(data);
            dc.SubmitChanges(); 
            MessageBox.Show("Save Succesfully");
            txtID.Clear();
            txtNama.Clear(); */
            txtID.Text = IDOtomatis();
        }

        private void FormJabatan_Load(object sender, EventArgs e)
        {
            txtID.Text = IDOtomatis();
        }
    }
}
