using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TA_RealEstate_Kel11
{
    public partial class TransaksiBeli : Form
    {
        //string myConnectionString = @"Data Source=LAPTOP-L1AODT95;Initial Catalog=REALESTATE;Integrated Security=True";
        string myConnectionString = @"Data Source=WINDOWS-LD56BQV;Initial Catalog=REALESTATE;Integrated Security=True";

        public TransaksiBeli()
        {
            InitializeComponent();
        }

        REALESTATEDataContext dc = new REALESTATEDataContext();

        private string IDOtomatis()
        {
            string autoid = null;

            SqlConnection myConnection = new SqlConnection(myConnectionString);
            myConnection.Open();

            string sqlQuery = "SELECT TOP 1 idTBeli FROM tPembelian ORDER BY idTBeli DESC";
            SqlCommand cmd = new SqlCommand(sqlQuery, myConnection);
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                string input = dr["idTBeli"].ToString();
                string angka = input.Substring(input.Length - Math.Min(2, input.Length));
                int number = Convert.ToInt32(angka);
                number += 1;
                string str = number.ToString("D2");

                autoid = "TRB" + str;
            }

            if (autoid == null)
            {
                autoid = "TRB01";
            }

            myConnection.Close();

            return autoid;
        }

        private void btnSimpan_Click(object sender, EventArgs e)
        {
            SqlConnection myConnection = new SqlConnection(myConnectionString);

            SqlCommand insert = new SqlCommand("sp_InsertPembelian", myConnection);
            insert.CommandType = CommandType.StoredProcedure;
            
            insert.Parameters.AddWithValue("idTBeli", txtIDBeli.Text);
            //insert.Parameters.AddWithValue("tanggal",value: dateTimePicker1.ToString("yyyy-MM-dd"));
            insert.Parameters.AddWithValue("idProperty", cbProperty.SelectedValue.ToString());
            insert.Parameters.AddWithValue("idClient", cbClient.SelectedValue.ToString());
            insert.Parameters.AddWithValue("idPemilik", cbPemilik.SelectedValue.ToString());

            if (txtIDBeli.Text == "" || cbProperty.Text == "" || cbClient.Text == "" || cbPemilik.Text == "")
            {
                MessageBox.Show("Semua Data Harus diisi !!", "Add Pembelian", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                try
                {
                    myConnection.Open();
                    insert.ExecuteNonQuery();
                    MessageBox.Show("Pembelian Telah Ditambahkan", "Add Pembelian", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    clear();
                }
                catch (Exception)
                {
                    MessageBox.Show("Unable to save ", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {

        }

        private void btnHapus_Click(object sender, EventArgs e)
        {

        }

        private void btnCariTransaksi_Click(object sender, EventArgs e)
        {

        }

        private void btnBatal_Click(object sender, EventArgs e)
        {
            clear();
        }

        public void clear()
        {
            txtCariTransaksi.Clear();
            txtIDBeli.Clear();
            cbClient.SelectedIndex = -1;
            cbPemilik.SelectedIndex = -1;
            cbProperty.SelectedIndex = -1;
            txtIDBeli.Text = IDOtomatis();
            //LoadData();
        }
        private void btnX_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void bunifuTileButton1_Click(object sender, EventArgs e)
        {
            MenuKasir kasir = new MenuKasir();
            kasir.Show();
            this.Hide();
        }

        private void btnPembelian_Click(object sender, EventArgs e)
        {
            TransaksiBeli beli = new TransaksiBeli();
            beli.Show();
            this.Hide();
        }

        private void btnPenyewaan_Click(object sender, EventArgs e)
        {
            TransaksiSewa sewa = new TransaksiSewa();
            sewa.Show();
            this.Hide();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            Login log = new Login();
            log.Visible = true;
            this.Hide();
        }

        private void btnDetailBeli_Click(object sender, EventArgs e)
        {
            TransaksiDetailBeli detailBeli = new TransaksiDetailBeli();
            detailBeli.Show();
            this.Hide();
        }

        private void TransaksiBeli_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'rEALESTATEDataSet.pemilik' table. You can move, or remove it, as needed.
            this.pemilikTableAdapter.Fill(this.rEALESTATEDataSet.pemilik);
            // TODO: This line of code loads data into the 'rEALESTATEDataSet.client' table. You can move, or remove it, as needed.
            this.clientTableAdapter.Fill(this.rEALESTATEDataSet.client);
            // TODO: This line of code loads data into the 'rEALESTATEDataSet.property' table. You can move, or remove it, as needed.
            this.propertyTableAdapter.Fill(this.rEALESTATEDataSet.property);
        }
    }
}
