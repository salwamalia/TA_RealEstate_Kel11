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
        public TransaksiBeli()
        {
            InitializeComponent();
        }

        //koneksi
        koneksi connection = new koneksi();
        REALESTATEDataContext dc = new REALESTATEDataContext();

        private string IDOtomatis()
        {
            int autoid = 0;
            string kode = null;

            SqlConnection myConnection = connection.Getcon();
            myConnection.Open();

            string sqlQuery = "SELECT TOP (1) MAX(RIGHT (idTBeli,2))+1 AS idTBeli FROM tPembelian";
            SqlCommand cmd = new SqlCommand(sqlQuery, myConnection);
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                if (dr["idTBeli"].ToString() == "")
                {
                    autoid = 1;
                }
                else
                {
                    autoid = Int32.Parse(dr["idTBeli"].ToString());
                }
            }

            if (autoid < 10)
            {
                kode = "TB00" + autoid;
            }
            else if (autoid < 100)
            {
                kode = "TB" + autoid;
            }

            myConnection.Close();

            return kode;
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
            rbCicil.Checked = false;
            rbLunas.Checked = false;
            cbCicilan.SelectedIndex = -1;
            txtperBulan.Clear();
            txtDP.Clear();
            
            txtIDBeli.Text = IDOtomatis();
            //LoadData();
        }
        
        //combobox
        private void cbProperty_SelectedIndexChanged(object sender, EventArgs e)
        {
            SqlConnection myConnection = connection.Getcon();
            string sql = "SELECT * FROM property p INNER JOIN pemilik b ON p.idPemilik = b.idPemilik WHERE p.idProperty  = '" + cbProperty.SelectedValue + "' ";
            SqlCommand cmd = new SqlCommand(sql, myConnection);
            SqlDataReader myreader;
            try
            {
                myConnection.Open();
                myreader = cmd.ExecuteReader();
                while (myreader.Read())
                {
                    string pemilik = myreader.GetString(10);
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
        
        private void txtDP_TextChanged(object sender, EventArgs e)
        {
            try
            {
                //txtTotal.Text = ((int.Parse(txtHargaProperty.Text) - int.Parse(txtDP.Text)));
                int temp = Int32.Parse(txtDP.Text);
                int temp1 = Int32.Parse(txtHargaProperty.Text);
                int temp2 = temp1 - temp;
                txtTotal.Text = temp2.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Ocucured" + ex);
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
            // TODO: This line of code loads data into the 'rEALESTATEDataSet.property' table. You can move, or remove it, as needed.
            this.propertyTableAdapter.Fill(this.rEALESTATEDataSet.property);
            txtIDBeli.Text = IDOtomatis();
            loadCicilan();
            loadClient();
        }

        private void cbCicilan_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if(txtHargaProperty.Text != "")
                {
                    int temp = Int32.Parse(cbCicilan.SelectedItem.ToString());
                    int temp1 = Int32.Parse(txtHargaProperty.Text);
                    int temp2 = temp1 / temp;
                    txtperBulan.Text = temp2.ToString();
                }
            }
            catch(Exception ex)
            {

            }
        }

        public void loadCicilan()
        {
            SqlConnection con1 = connection.Getcon();
            con1.Open();
            SqlCommand cmd1 = new SqlCommand("SELECT cicilan FROM kategoriCicilan", con1);
            SqlDataReader dr1 = cmd1.ExecuteReader();
            SqlDataReader sqlDataReader1 = dr1;
            while (dr1.Read())
            {

                if((int.Parse(sqlDataReader1["cicilan"].ToString()) <= 12))
                {
                    cbCicilan.Items.Add(sqlDataReader1["cicilan"]);
                }
            }
            dr1.Close();
            con1.Close();
        }

        public void loadClient()
        {
            SqlConnection con1 = connection.Getcon();
            con1.Open();
            SqlCommand cmd1 = new SqlCommand("SELECT nama FROM client", con1);
            SqlDataReader dr1 = cmd1.ExecuteReader();
            SqlDataReader sqlDataReader1 = dr1;
            while (dr1.Read())
            {
                cbClient.Items.Add(sqlDataReader1["nama"]);
            }
            dr1.Close();
            con1.Close();
        }

        private void rbLunas_CheckedChanged(object sender, EventArgs e)
        {
            cbCicilan.Enabled = false;
            txtperBulan.Enabled = false;
        }

        private void rbCicil_CheckedChanged(object sender, EventArgs e)
        {
            cbCicilan.Enabled = true;
            txtperBulan.Enabled = true;
        }
    }
}
