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
    public partial class PembayaranPenyewaan : Form
    {
        public PembayaranPenyewaan()
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

            string sqlQuery = "SELECT TOP (1) MAX(RIGHT (idBSewa,2))+1 AS idBSewa FROM tPembayaranPenyewaan";
            SqlCommand cmd = new SqlCommand(sqlQuery, myConnection);
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                if (dr["idBSewa"].ToString() == "")
                {
                    autoid = 1;
                }
                else
                {
                    autoid = Int32.Parse(dr["idBSewa"].ToString());
                }
            }

            if (autoid < 10)
            {
                kode = "BS00" + autoid;
            }
            else if (autoid < 100)
            {
                kode = "BS" + autoid;
            }

            myConnection.Close();

            return kode;
        }

        private void PembayaranPenyewaan_Load(object sender, EventArgs e)
        {
            txtIDBayar.Text = IDOtomatis();
            
            loadClient();
            LoadData();
        }

        private void btnCariTransaksi_Click(object sender, EventArgs e)
        {
            if (txtCariTransaksi.Text == "")
            {
                MessageBox.Show("Masukkan ID untuk Cari !!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                try
                {
                    //PENYEWAAN
                    SqlConnection con = connection.Getcon();
                    con.Open();
                    SqlCommand cmd = new SqlCommand("SELECT * FROM tPenyewaan WHERE idTSewa ='" + txtCariTransaksi.Text + "'", con);
                    SqlDataReader dr = cmd.ExecuteReader();
                    SqlDataReader sqlDataReader = dr;
                    DataTable dataTable = new DataTable();
                    dataTable.Columns.Add("ID Transaksi");
                    dataTable.Columns.Add("Tanggal");
                    dataTable.Columns.Add("ID Property");
                    dataTable.Columns.Add("ID Client");
                    dataTable.Columns.Add("Total");
                    dataTable.Columns.Add("Status");
                    while (dr.Read())
                    {
                        DataRow row = dataTable.NewRow();
                        row["ID Transaksi"] = sqlDataReader["idTSewa"];
                        row["Tanggal"] = sqlDataReader["tanggal"];

                        //cbProperty cmd1 dr1 sql1 convert
                        SqlConnection convert = connection.Getcon();
                        convert.Open();
                        SqlCommand cmd1 = new SqlCommand("SELECT namaProperty FROM property WHERE idProperty = '" + sqlDataReader["idProperty"] + "'", convert);
                        SqlDataReader dr1 = cmd1.ExecuteReader();
                        SqlDataReader sqlDataReader1 = dr1;
                        dr1.Read();
                        row["ID Property"] = sqlDataReader1["namaProperty"];
                        dr1.Close();
                        convert.Close();

                        //cbClient cmd2 dr2 sql2 convert1
                        SqlConnection convert1 = connection.Getcon();
                        convert1.Open();
                        SqlCommand cmd2 = new SqlCommand("SELECT nama FROM client WHERE idCLient = '" + sqlDataReader["idClient"] + "'", convert1);
                        SqlDataReader dr2 = cmd2.ExecuteReader();
                        SqlDataReader sqlDataReader2 = dr2;
                        dr2.Read();
                        row["ID Client"] = sqlDataReader2["nama"];
                        dr2.Close();
                        convert1.Close();

                        row["Total"] = sqlDataReader["total"];
                        if (sqlDataReader["Status"].ToString().Equals("0"))
                        {
                            row["status"] = "Unpaid";
                        }
                        else
                        {
                            row["status"] = "Paid";
                        }
                        dataTable.Rows.Add(row);
                    }
                    //this.dataGridView1.DataSource = con.table;
                    dgTransaksi.DataSource = dataTable;
                    con.Close();

                    //////////////////////

                    //DETAIL PENYEWAAN
                    SqlConnection con1 = connection.Getcon();
                    con1.Open();
                    SqlCommand cmd3 = new SqlCommand("SELECT * FROM tDetailPenyewaan WHERE idTSewa ='" + txtCariTransaksi.Text + "'", con1);
                    SqlDataReader dr3 = cmd3.ExecuteReader();
                    SqlDataReader sqlDataReader3 = dr3;
                    DataTable dataTable1 = new DataTable();
                    dataTable1.Columns.Add("ID Transaksi");
                    dataTable1.Columns.Add("ID Property");
                    dataTable1.Columns.Add("Mulai Sewa");
                    dataTable1.Columns.Add("Sampai");
                    dataTable1.Columns.Add("Pembayaran");
                    dataTable1.Columns.Add("ID Cicilan");
                    dataTable1.Columns.Add("Perbulan");
                    dataTable1.Columns.Add("DP");
                    dataTable1.Columns.Add("Total");
                    while (dr3.Read())
                    {
                        DataRow row1 = dataTable1.NewRow();
                        row1["ID Transaksi"] = sqlDataReader3["idTSewa"];

                        //cbProperty cmd4 dr4 sql4 convert2
                        SqlConnection convert2 = connection.Getcon();
                        convert2.Open();
                        SqlCommand cmd4 = new SqlCommand("SELECT namaProperty FROM property WHERE idProperty = '" + sqlDataReader3["idProperty"] + "'", convert2);
                        SqlDataReader dr4 = cmd4.ExecuteReader();
                        SqlDataReader sqlDataReader4 = dr4;
                        dr4.Read();
                        row1["ID Property"] = sqlDataReader4["namaProperty"];
                        dr4.Close();
                        convert2.Close();

                        row1["Mulai Sewa"] = sqlDataReader3["mulaiSewa"];
                        row1["Sampai"] = sqlDataReader3["sampai"];
                        row1["Pembayaran"] = sqlDataReader3["pembayaran"];

                        //cbCicilan cmd5 dr5 sql5 convert3
                        SqlConnection convert3 = connection.Getcon();
                        convert3.Open();
                        SqlCommand cmd5 = new SqlCommand("SELECT cicilan FROM kategoriCicilan WHERE idCicilan = '" + sqlDataReader3["idCicilan"] + "'", convert3);
                        SqlDataReader dr5 = cmd5.ExecuteReader();
                        SqlDataReader sqlDataReader5 = dr5;
                        dr5.Read();
                        row1["ID Cicilan"] = sqlDataReader5["cicilan"];
                        dr5.Close();
                        convert3.Close();

                        row1["Perbulan"] = sqlDataReader3["perBulan"];
                        row1["DP"] = sqlDataReader3["dp"];
                        row1["Total"] = sqlDataReader3["total"];
                        dataTable1.Rows.Add(row1);
                    }
                    dgDetailTransaksi.DataSource = dataTable1;
                    con1.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error Transaksi Tidak Ditemukan! ", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    MessageBox.Show("Error Transaksi Tidak Ditemukan! " + ex);
                }
            }
        }

        private void btnBatal_Click(object sender, EventArgs e)
        {
            clear();
        }

        public void clear()
        {
            txtCariTransaksi.Clear();
            txtIDSewa.Clear();
            dateTimePicker1.ResetText();
            cbProperty.SelectedIndex = -1;
            txtPemilik.Clear();
            cbClient.SelectedIndex = -1;
            txtTotal.Clear();

            txtIDBayar.Text = IDOtomatis();
            LoadData();
            loadClient();
        }

        private void btnBayar_Click(object sender, EventArgs e)
        {

        }

        //combobox
        private void cbProperty_SelectedIndexChanged_1(object sender, EventArgs e)
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
                    string pemilik = myreader.GetString(11);
                    txtPemilik.Text = pemilik;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Occured" + ex.Message);
            }
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

        void LoadData()
        {
            try
            {
                //TABEL PENYEWAAN
                SqlConnection con = connection.Getcon();
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM tPenyewaan", con);
                SqlDataReader dr = cmd.ExecuteReader();
                SqlDataReader sqlDataReader = dr;
                DataTable dataTable = new DataTable();
                dataTable.Columns.Add("ID Transaksi");
                dataTable.Columns.Add("Tanggal");
                dataTable.Columns.Add("ID Property");
                dataTable.Columns.Add("ID Client");
                dataTable.Columns.Add("Total");
                dataTable.Columns.Add("Status");
                while (dr.Read())
                {
                    DataRow row = dataTable.NewRow();
                    row["ID Transaksi"] = sqlDataReader["idTSewa"];
                    row["Tanggal"] = sqlDataReader["tanggal"];
                    row["ID Property"] = sqlDataReader["idProperty"];
                    row["ID Client"] = sqlDataReader["idClient"];
                    row["Total"] = sqlDataReader["total"];
                    if (sqlDataReader["status"].ToString().Equals("0"))
                    {
                        row["Status"] = "Unpaid";
                    }
                    else
                    {
                        row["Status"] = "Paid";
                    }
                    dataTable.Rows.Add(row);
                }
                dgTransaksi.DataSource = dataTable;
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Occured" + ex);
            }
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

        private void btnPembayaran_Click(object sender, EventArgs e)
        {
            PembayaranPembelian bayar = new PembayaranPembelian();
            bayar.Show();
            this.Hide();
        }

        private void btnBayarSewa_Click(object sender, EventArgs e)
        {
            PembayaranPenyewaan sewa = new PembayaranPenyewaan();
            sewa.Show();
            this.Hide();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            Login log = new Login();
            log.Visible = true;
            this.Hide();
        }

        private void btnX_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void dgTransaksi_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                //PENYEWAAN
                txtIDSewa.Text = dgTransaksi.Rows[e.RowIndex].Cells[0].Value.ToString();
                dateTimePicker1.Text = dgTransaksi.Rows[e.RowIndex].Cells[1].Value.ToString();

                //property
                if (!(String.IsNullOrEmpty(dgTransaksi.Rows[e.RowIndex].Cells[2].Value.ToString())))
                {
                    for (int i = 0; i < cbProperty.Items.Count; i++)
                    {
                        cbProperty.SelectedIndex = i;
                        if (dgTransaksi.Rows[e.RowIndex].Cells[2].Value.ToString() == cbProperty.SelectedItem)
                        {
                            break;
                        }
                    }
                }

                //client
                if (!(String.IsNullOrEmpty(dgTransaksi.Rows[e.RowIndex].Cells[3].Value.ToString())))
                {
                    for (int i = 0; i < cbProperty.Items.Count; i++)
                    {
                        cbClient.SelectedIndex = i;
                        if (dgTransaksi.Rows[e.RowIndex].Cells[3].Value.ToString() == cbClient.SelectedItem)
                        {
                            break;
                        }
                    }
                }
                txtTotal.Text = dgTransaksi.Rows[e.RowIndex].Cells[4].Value.ToString();
            }
            catch (Exception)
            {
            }
        }
    }
}
