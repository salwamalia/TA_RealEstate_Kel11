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
    public partial class FormProperty : Form
    {
        public FormProperty()
        {
            InitializeComponent();
        }

        private string IDOtomatis()
        {
            string autoid = null;

            string myConnectionString = @"Data Source=LAPTOP-L1AODT95;Initial Catalog=REALESTATE;Integrated Security=True";
            //string myConnectionString = @"Data Source=WINDOWS-LD56BQV;Initial Catalog=REALESTATE;Integrated Security=True";
            SqlConnection myConnection = new SqlConnection(myConnectionString);
            myConnection.Open();

            string sqlQuery = "SELECT TOP 1 idProperty FROM property ORDER BY idProperty DESC";
            SqlCommand cmd = new SqlCommand(sqlQuery, myConnection);
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                string input = dr["idProperty"].ToString();
                string angka = input.Substring(input.Length - Math.Min(2, input.Length));
                int number = Convert.ToInt32(angka);
                number += 1;
                string str = number.ToString("D2");

                autoid = "PTY" + str;
            }

            if (autoid == null)
            {
                autoid = "PTY01";
            }
            myConnection.Close();
            return autoid;
        }

        private void clear()
        {
            txtCariProperty.Clear();
            txtID.Clear();
            txtNama.Clear();
            cbTipe.SelectedIndex = -1;
            cbPemilik.SelectedIndex = -1;
            txtUkuran.Clear();
            txtFasilitas.Clear();
            txtHarga.Clear();
        }

        private void FormProperty_Load(object sender, EventArgs e)
        {
            txtID.Text = IDOtomatis();
        }

        private void btnSimpan_Click_1(object sender, EventArgs e)
        {
            string myConnectionString = @"Data Source=LAPTOP-L1AODT95;Initial Catalog=REALESTATE;Integrated Security=True";
            //string myConnectionString = @"Data Source=WINDOWS-LD56BQV;Initial Catalog=REALESTATE;Integrated Security=True";
            SqlConnection myConnection = new SqlConnection(myConnectionString);

            SqlCommand insert = new SqlCommand("sp_InsertProperty", myConnection);
            insert.CommandType = CommandType.StoredProcedure;

            insert.Parameters.AddWithValue("idProperty", txtID.Text);
            insert.Parameters.AddWithValue("namaProperty", txtNama.Text);
            insert.Parameters.AddWithValue("idTipe", cbTipe.SelectedItem.ToString());
            insert.Parameters.AddWithValue("idPemilik", cbPemilik.SelectedItem.ToString());
            insert.Parameters.AddWithValue("ukuran", txtUkuran.Text);
            insert.Parameters.AddWithValue("fasilitas", txtFasilitas.Text);
            insert.Parameters.AddWithValue("harga", txtHarga.Text);

            if (txtID.Text == "" || txtNama.Text == "" || cbTipe.Text == "" || cbPemilik.Text == "" || txtUkuran.Text == "" || txtFasilitas.Text == "" || txtHarga.Text == "")
            {
                MessageBox.Show("Data tersebut Harus diisi !!", "Add Property", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                try
                {
                    myConnection.Open();
                    insert.ExecuteNonQuery();
                    MessageBox.Show("Property Telah Ditambahkan", "Add Property", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    clear();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Unable to save " + ex.Message);
                }
            }

            txtID.Text = IDOtomatis();
        }
    }
}
