﻿using System;
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
    public partial class TJualPropertyCicil : Form
    {
        public TJualPropertyCicil()
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

            string sqlQuery = "SELECT TOP (1) MAX(RIGHT (idTJualPropertyCicil,2))+1 AS idTJualPropertyCicil FROM TJualPropertyCicil";
            SqlCommand cmd = new SqlCommand(sqlQuery, myConnection);
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                if (dr["idTJualPropertyCicil"].ToString() == "")
                {
                    autoid = 1;
                }
                else
                {
                    autoid = Int32.Parse(dr["idTJualPropertyCicil"].ToString());
                }
            }

            if (autoid < 10)
            {
                kode = "PC00" + autoid;
            }
            else if (autoid < 100)
            {
                kode = "PC0" + autoid;
            }

            myConnection.Close();

            return kode;
        }

        private void txtIDSewa_Click(object sender, EventArgs e)
        {
            txtIDJual.Text = IDOtomatis();
        }

        private void btnBayar_Click(object sender, EventArgs e)
        {

        }

        public void clear()
        {
            txtCariProperty.Clear();

            txtIDJual.Clear();
            tanggal.ResetText();
            cbClient.SelectedIndex = -1;


            txtProperty.Clear();
            txtHarga.Clear();
            cbCicilan.SelectedIndex = -1;
            txtperBulan.Clear();
            
            LoadData();
        }

        private void btnBatal_Click(object sender, EventArgs e)
        {
            clear();
        }

        private void btnCariProperty_Click(object sender, EventArgs e)
        {
            if (txtCariProperty.Text == "")
            {
                MessageBox.Show("Masukkan ID Untuk Cari !!", "Cari Property", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                try
                {
                    SqlConnection myConnection = connection.Getcon();
                    myConnection.Open();

                    SqlCommand cmd = new SqlCommand("SELECT * FROM property WHERE idProperty ='" + txtCariProperty.Text + "' AND statusProperty = 0", myConnection);
                    SqlDataReader dr = cmd.ExecuteReader();
                    SqlDataReader sqlDataReader = dr;

                    dr.Read();

                    txtProperty.Text = sqlDataReader["idProperty"].ToString();
                    txtHarga.Text = sqlDataReader["harga"].ToString();

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

        private void cbCicilan_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (txtHarga.Text != "")
            {
                try
                {
                    int temp1 = Convert.ToInt32(cbCicilan.SelectedItem.ToString());
                    double temp2 = Convert.ToDouble(txtHarga.Text);
                    double temp = temp2 / temp1;
                    string res1 = String.Format("{0:#,##0}", temp);
                    txtperBulan.Text = res1.ToString();
                    txtperBulan.Select(txtperBulan.Text.Length, 0);
                }
                catch (Exception)
                {
                }
            }
        }

        //separator
        private double hargasparator;
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

        //LOAD DATA
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

        public void loadCicilan()
        {
            SqlConnection con1 = connection.Getcon();
            con1.Open();
            SqlCommand cmd2 = new SqlCommand("SELECT cicilan FROM kategoriCicilan", con1);
            SqlDataReader dr2 = cmd2.ExecuteReader();
            SqlDataReader sqlDataReader1 = dr2;
            while (dr2.Read())
            {

                if ((int.Parse(sqlDataReader1["cicilan"].ToString()) <= 24))
                {
                    cbCicilan.Items.Add(sqlDataReader1["cicilan"]);
                }
            }
            dr2.Close();
            con1.Close();
        }

        void LoadDataProperty()
        {
            try
            {
                SqlConnection con = connection.Getcon();
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM property WHERE statusProperty = 0 ", con);
                SqlDataReader dr = cmd.ExecuteReader();
                SqlDataReader sqlDataReader = dr;
                DataTable dataTable = new DataTable();
                dataTable.Columns.Add("ID");
                dataTable.Columns.Add("Nama Property");
                dataTable.Columns.Add("ID Tipe");
                dataTable.Columns.Add("ID Pemilik");
                dataTable.Columns.Add("Ukuran");
                dataTable.Columns.Add("Fasilitas");
                dataTable.Columns.Add("Harga");
                dataTable.Columns.Add("ID Interior");
                dataTable.Columns.Add("Status");
                while (dr.Read())
                {
                    DataRow row = dataTable.NewRow();
                    row["ID"] = sqlDataReader["idProperty"];
                    row["Nama Property"] = sqlDataReader["namaProperty"];
                    row["ID Tipe"] = sqlDataReader["idTipe"];
                    row["ID Pemilik"] = sqlDataReader["idPemilik"];
                    row["Ukuran"] = sqlDataReader["ukuran"];
                    row["Fasilitas"] = sqlDataReader["fasilitas"];
                    int num = Convert.ToInt32(sqlDataReader["harga"].ToString());
                    string res2 = String.Format("{0:#,##0}", num);
                    row["Harga"] = res2.ToString();
                    row["ID Interior"] = sqlDataReader["idInterior"];
                    if (sqlDataReader["statusProperty"].ToString().Equals("0"))
                    {
                        row["Status"] = "Available";
                    }
                    else
                    {
                        row["Status"] = "Not Available";
                    }
                    dataTable.Rows.Add(row);
                }
                //this.dataGridView1.DataSource = con.table;
                dgProperty.DataSource = dataTable;
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
                //JUAL
                SqlConnection con = connection.Getcon();
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM TJualPropertyCicil", con);
                SqlDataReader dr = cmd.ExecuteReader();
                SqlDataReader sqlDataReader = dr;
                DataTable dataTable = new DataTable();
                dataTable.Columns.Add("ID Transaksi");
                dataTable.Columns.Add("Tanggal");
                dataTable.Columns.Add("ID Client");
                dataTable.Columns.Add("Harga");
                while (dr.Read())
                {
                    DataRow row = dataTable.NewRow();

                    row["ID Transaksi"] = sqlDataReader["idTJualPropertyCicil"];
                    row["Tanggal"] = sqlDataReader["tanggal"];
                    row["ID Client"] = sqlDataReader["idClient"];
                    int num = Convert.ToInt32(sqlDataReader["harga"].ToString());
                    string res2 = String.Format("{0:#,##0}", num);
                    row["Harga"] = res2.ToString();
                    dataTable.Rows.Add(row);
                }
                //this.dataGridView1.DataSource = con.table;
                dgJualProperty.DataSource = dataTable;
                con.Close();

                //DETAIL
                SqlConnection con1 = connection.Getcon();
                con1.Open();
                SqlCommand cmd1 = new SqlCommand("SELECT * FROM TJualPropertyCicilDet", con1);
                SqlDataReader dr1 = cmd1.ExecuteReader();
                SqlDataReader sqlDataReader1 = dr1;
                DataTable dataTable1 = new DataTable();
                dataTable1.Columns.Add("ID Transaksi");
                dataTable1.Columns.Add("ID Property");
                dataTable1.Columns.Add("Harga");
                dataTable1.Columns.Add("ID Cicilan");
                dataTable1.Columns.Add("Perbulan");
                while (dr1.Read())
                {
                    DataRow row1 = dataTable1.NewRow();
                    row1["ID Transaksi"] = sqlDataReader1["idTSewaProperty"];
                    row1["ID Property"] = sqlDataReader1["idProperty"];
                    int num = Convert.ToInt32(sqlDataReader1["harga"].ToString());
                    string res = String.Format("{0:#,##0}", num);
                    row1["Harga"] = res.ToString();
                    row1["ID Client"] = sqlDataReader1["idClients"];
                    int num1 = Convert.ToInt32(sqlDataReader1["perBulan"].ToString());
                    string res1 = String.Format("{0:#,##0}", num1);
                    row1["Perbulan"] = res1.ToString();
                    dataTable1.Rows.Add(row1);
                }
                dgDetailJualProperty.DataSource = dataTable1;
                con1.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Occured" + ex);
            }
        }

        private void TJualPropertyCicil_Load(object sender, EventArgs e)
        {
            loadClient();
            loadCicilan();
            LoadDataProperty();
            LoadData();
        }

        private void btnX_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnLunas_Click(object sender, EventArgs e)
        {
            TJualPropertyLunas lunas = new TJualPropertyLunas();
            lunas.Show();
            this.Hide();
        }

        private void btnCicil_Click(object sender, EventArgs e)
        {
            TJualPropertyCicil cicil = new TJualPropertyCicil();
            cicil.Show();
            this.Hide();
        }

        private void btnKembali_Click(object sender, EventArgs e)
        {
            MenuKasir kasir = new MenuKasir();
            kasir.Show();
            this.Hide();
        }
    }
}
