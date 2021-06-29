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
                string angka = input.Substring(input.Length - Math.Min(3, input.Length));
                int number = Convert.ToInt32(angka);
                number += 1;
                string str = number.ToString("D3");

                autoid = "TRB" + str;
            }

            if (autoid == null)
            {
                autoid = "TRB001";
            }

            myConnection.Close();

            return autoid;
        }

        private void btnSimpan_Click(object sender, EventArgs e)
        {

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
            dateTimePicker1.ResetText();
            cbProperty.SelectedIndex = -1;
            txtPemilik.Clear();
            cbClient.SelectedIndex = -1;
            txtTotal.Clear();

            txtHargaProperty.Clear();
            cbPembayaran.SelectedIndex = -1;
            cbCicilan.SelectedIndex = -1;
            txtLamaCicilan.Clear();
            txtperBulan.Clear();
            txtDP.Clear();
            txtTotalBayar.Clear();
            
            txtIDBeli.Text = IDOtomatis();
            //LoadData();
        }
        
        //combobox
        private void cbProperty_SelectedIndexChanged(object sender, EventArgs e)
        {
            SqlConnection myConnection = new SqlConnection(myConnectionString);
            string sql = "SELECT * FROM property p INNER JOIN pemilik b ON p.idPemilik = b.idPemilik WHERE p.idProperty  = '" + cbProperty.SelectedValue + "' ";
            SqlCommand cmd = new SqlCommand(sql, myConnection);
            SqlDataReader myreader;
            try
            {
                myConnection.Open();
                myreader = cmd.ExecuteReader();
                while (myreader.Read())
                {
                    string pemilik = myreader.GetString(9);
                    txtPemilik.Text = pemilik;

                    string harga = myreader.GetInt32(6).ToString();
                    txtHargaProperty.Text = harga;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Occured" + ex.Message);
            }
        }

        //textbox
        private void txtLamaCicilan_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtLamaCicilan.Text == "1")
                {
                    txtperBulan.Text = ((int.Parse(txtHargaProperty.Text) * int.Parse(txtLamaCicilan.Text) / 12).ToString());
                }
                else if (txtLamaCicilan.Text == "5")
                {
                    txtperBulan.Text = ((int.Parse(txtHargaProperty.Text) * int.Parse(txtLamaCicilan.Text) / 60).ToString());
                }
                else if (txtLamaCicilan.Text == "10")
                {
                    txtperBulan.Text = ((int.Parse(txtHargaProperty.Text) * int.Parse(txtLamaCicilan.Text) / 120).ToString());
                }
                else if (txtLamaCicilan.Text == "15")
                {
                    txtperBulan.Text = ((int.Parse(txtHargaProperty.Text) * int.Parse(txtLamaCicilan.Text) / 180).ToString());
                }
                else if (txtLamaCicilan.Text != "1" || txtLamaCicilan.Text != "5" || txtLamaCicilan.Text != "10" || txtLamaCicilan.Text != "15")
                {
                    MessageBox.Show("Inputkan Lama Cicilan dengan Benar !");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Ocucured" + ex);
            }
        }
        //
        private void txtDP_TextChanged(object sender, EventArgs e)
        {
            try
            {
                txtTotalBayar.Text = ((int.Parse(txtHargaProperty.Text) - int.Parse(txtDP.Text)).ToString());
            }
            catch (Exception ex)
            {

            }
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

        private void TransaksiBeli_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'rEALESTATEDataSet.kategoriCicilan' table. You can move, or remove it, as needed.
            this.kategoriCicilanTableAdapter.Fill(this.rEALESTATEDataSet.kategoriCicilan);
            // TODO: This line of code loads data into the 'rEALESTATEDataSet.kategoriBayar' table. You can move, or remove it, as needed.
            this.kategoriBayarTableAdapter.Fill(this.rEALESTATEDataSet.kategoriBayar);
            // TODO: This line of code loads data into the 'rEALESTATEDataSet.client' table. You can move, or remove it, as needed.
            this.clientTableAdapter.Fill(this.rEALESTATEDataSet.client);
            // TODO: This line of code loads data into the 'rEALESTATEDataSet.property' table. You can move, or remove it, as needed.
            this.propertyTableAdapter.Fill(this.rEALESTATEDataSet.property);

            txtIDBeli.Text = IDOtomatis();
        }
    }
}
