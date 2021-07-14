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
    public partial class TPelunasanCicilanProperty : Form
    {
        public TPelunasanCicilanProperty()
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

            string sqlQuery = "SELECT TOP (1) MAX(RIGHT (idDetailCIcil,2))+1 AS idDetailCIcil FROM TJualPropertyCicilDet";
            SqlCommand cmd = new SqlCommand(sqlQuery, myConnection);
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                if (dr["idDetailCIcil"].ToString() == "")
                {
                    autoid = 1;
                }
                else
                {
                    autoid = Int32.Parse(dr["idDetailCIcil"].ToString());
                }
            }

            if (autoid < 10)
            {
                kode = "BY00" + autoid;
            }
            else if (autoid < 100)
            {
                kode = "BY0" + autoid;
            }

            myConnection.Close();

            return kode;
        }

        private void txtIDBayar_Click(object sender, EventArgs e)
        {
            txtIDBayar.Text = IDOtomatis();
        }

        private void btnBayar_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtIDBayar.Text == "" || txtIDTransaksi.Text == "" )
                {
                    MessageBox.Show("Semua Data Harus diisi !!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    //DETAIL
                    SqlConnection myConnection1 = connection.Getcon();
                    myConnection1.Open();
                    SqlCommand insert1 = new SqlCommand("sp_InsertTJualPropertyCicilDet", myConnection1);
                    insert1.CommandType = CommandType.StoredProcedure;

                    insert1.Parameters.AddWithValue("@idDetailCicil", txtIDBayar.Text);
                    insert1.Parameters.AddWithValue("@idTJualPropertyCicil", txtIDTransaksi.Text);
                    insert1.Parameters.AddWithValue("@tanggalDetail", tanggalDetail.Value.ToString("yyyy-MM-dd"));
                    insert1.Parameters.AddWithValue("@harga", totalsparator);
                    insert1.Parameters.AddWithValue("@jumlah", 0);

                    insert1.ExecuteNonQuery();
                    myConnection1.Close();
                    MessageBox.Show("Save Sucessfully", "Add Pelunasan", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    clear();
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Unable to save ", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                MessageBox.Show("Unable to save "+ex);
            }
        }

        public void clear()
        {
            txtCariCicilan.Clear();

            txtIDBayar.Clear();
            txtIDTransaksi.Clear();
            tanggalDetail.ResetText();
            txtHarga.Clear();

            txtUang.Clear();
            txtTotal.Clear();

            LoadDataTransaksi();
            LoadData();
        }

        private void btnBatal_Click(object sender, EventArgs e)
        {
            clear();
        }
        
        private void btnCariProperty_Click(object sender, EventArgs e)
        {
            if (txtCariCicilan.Text == "")
            {
                MessageBox.Show("Masukkan ID Untuk Cari !!", "Cari Property", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                try
                {
                    SqlConnection myConnection = connection.Getcon();
                    myConnection.Open();

                    SqlCommand cmd = new SqlCommand("SELECT * FROM TJualPropertyCicil WHERE idTJualPropertyCicil ='" + txtCariCicilan.Text + "' AND statusBayar = 0", myConnection);
                    SqlDataReader dr = cmd.ExecuteReader();
                    SqlDataReader sqlDataReader = dr;

                    dr.Read();

                    txtIDTransaksi.Text = sqlDataReader["idTJualPropertyCicil"].ToString();
                    txtHarga.Text = sqlDataReader["total"].ToString();

                    dr.Close();
                    sqlDataReader.Close();
                    myConnection.Close();

                }
                catch (Exception ex)
                {
                    MessageBox.Show("ID Tidak Ada !!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        //LOAD DATA
        void LoadDataTransaksi()
        {
            try
            {
                SqlConnection con = connection.Getcon();
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM TJualPropertyCicil WHERE statusBayar = 0", con);
                SqlDataReader dr = cmd.ExecuteReader();
                SqlDataReader sqlDataReader = dr;
                DataTable dataTable = new DataTable();
                dataTable.Columns.Add("ID Transaksi");
                dataTable.Columns.Add("Tanggal");
                dataTable.Columns.Add("ID Property");
                dataTable.Columns.Add("ID Client");
                dataTable.Columns.Add("Harga Property");
                dataTable.Columns.Add("Perbulan");
                dataTable.Columns.Add("Jumlah Cicilan");
                dataTable.Columns.Add("Total");
                dataTable.Columns.Add("Status Bayar");

                while (dr.Read())
                {
                    DataRow row = dataTable.NewRow();

                    row["ID Transaksi"] = sqlDataReader["idTJualPropertyCicil"];
                    row["Tanggal"] = sqlDataReader["tanggal"];
                    row["ID Property"] = sqlDataReader["idProperty"];
                    row["ID Client"] = sqlDataReader["idClient"];

                    int num = Convert.ToInt32(sqlDataReader["hargaProperty"].ToString());
                    string res = String.Format("{0:#,##0}", num);
                    row["Harga Property"] = res.ToString();

                    int num1 = Convert.ToInt32(sqlDataReader["perbulan"].ToString());
                    string res1 = String.Format("{0:#,##0}", num1);
                    row["Perbulan"] = res1.ToString();

                    row["Jumlah Cicilan"] = sqlDataReader["jumlahCicilan"];

                    int num2 = Convert.ToInt32(sqlDataReader["total"].ToString());
                    string res2 = String.Format("{0:#,##0}", num2);
                    row["Total"] = res2.ToString();

                    if (sqlDataReader["statusBayar"].ToString().Equals("0"))
                    {
                        row["Status Bayar"] = "Belum Lunas";
                    }
                    else
                    {
                        row["Status Bayar"] = "Lunas";
                    }
                    dataTable.Rows.Add(row);
                }
                //this.dataGridView1.DataSource = con.table;
                dgCicilProperty.DataSource = dataTable;
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Occured" + ex);
            }
        }

        void LoadData()
        {
            try
            {
                //DETAIL
                SqlConnection con1 = connection.Getcon();
                con1.Open();
                SqlCommand cmd1 = new SqlCommand("SELECT * FROM TJualPropertyCicilDet", con1);
                SqlDataReader dr1 = cmd1.ExecuteReader();
                SqlDataReader sqlDataReader1 = dr1;
                DataTable dataTable1 = new DataTable();
                dataTable1.Columns.Add("ID Bayar");
                dataTable1.Columns.Add("ID Transaksi");
                dataTable1.Columns.Add("Tanggal Detail");
                dataTable1.Columns.Add("Harga");
                while (dr1.Read())
                {
                    DataRow row1 = dataTable1.NewRow();
                    row1["ID Bayar"] = sqlDataReader1["idDetailCicil"];
                    row1["ID Transaksi"] = sqlDataReader1["idTJualPropertyCicil"];
                    row1["Tanggal Detail"] = sqlDataReader1["tanggalDetail"];

                    int num = Convert.ToInt32(sqlDataReader1["harga"].ToString());
                    string res = String.Format("{0:#,##0}", num);
                    row1["Harga"] = res.ToString();
                    dataTable1.Rows.Add(row1);
                }
                dgDetailCicil.DataSource = dataTable1;
                con1.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Occured" + ex);
            }
        }

        private void TPelunasanCicilanProperty_Load(object sender, EventArgs e)
        {
            LoadDataTransaksi();
            LoadData();
        }

        //separator
        private double hargasparator;
        private double totalsparator;
        private void txtHarga_Leave(object sender, EventArgs e)
        {
            if (txtHarga.Text != "")
            {
                try
                {
                    hargasparator = Convert.ToDouble(txtHarga.Text);
                    string res2 = String.Format("{0:#,###}", hargasparator);
                    txtHarga.Text = res2.ToString();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error Occured" + ex);
                }
            }
        }

        private void txtHarga_TextChanged(object sender, EventArgs e)
        {
            if (txtHarga.Text != "")
            {
                try
                {
                    hargasparator = Convert.ToDouble(txtHarga.Text);
                    string res2 = String.Format("{0:#,###}", hargasparator);
                    txtHarga.Text = res2.ToString();
                    txtHarga.Select(txtHarga.Text.Length, 0);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error Occured" + ex);
                }
            }
        }

        private void txtUang_TextChanged(object sender, EventArgs e)
        {
            try
            {
                hargasparator = Convert.ToDouble(txtUang.Text);
                string res2 = String.Format("{0:#,###}", hargasparator);
                txtUang.Text = res2.ToString();
                txtUang.Select(txtUang.Text.Length, 0);


                double temp1 = Convert.ToDouble(txtUang.Text);
                double temp2 = Convert.ToDouble(txtHarga.Text);
                double temp = temp2 - temp1;
                string res1 = String.Format("{0:#,##0}", temp);
                txtTotal.Text = res1.ToString();
                txtTotal.Select(txtTotal.Text.Length, 0);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Occured" + ex);
            }
        }

        private void txtTotal_TextChanged(object sender, EventArgs e)
        {
            if (txtTotal.Text != "")
            {
                try
                {
                    totalsparator = Convert.ToDouble(txtTotal.Text);
                    string res2 = String.Format("{0:#,###}", totalsparator);
                    txtTotal.Text = res2.ToString();
                    txtTotal.Select(txtTotal.Text.Length, 0);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error Occured" + ex);
                }
            }
        }

        private void btnX_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnKembali_Click(object sender, EventArgs e)
        {
            MenuKasir kasir = new MenuKasir();
            kasir.Show();
            this.Hide();
        }

        private void bunifuTileButton1_Click(object sender, EventArgs e)
        {
            MenuKasir kasir = new MenuKasir();
            kasir.Show();
            this.Hide();
        }
    }
}
