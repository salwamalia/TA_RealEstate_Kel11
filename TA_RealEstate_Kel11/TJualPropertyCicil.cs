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

        private string IDOtomatis2()
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

        private void txtIDSewa_Click(object sender, EventArgs e)
        {
            txtIDJual.Text = IDOtomatis();
        }

        private void btnBayar_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtIDJual.Text == "" || cbClient.Text == "" || txtProperty.Text == "" || cbCicilan.Text == "" || txtTotal.Text == "")
                {
                    MessageBox.Show("Semua Data Harus diisi !!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    //SEWA
                    SqlConnection myConnection = connection.Getcon();
                    myConnection.Open();
                    SqlCommand insert = new SqlCommand("sp_InsertTJualPropertyCicil", myConnection);
                    insert.CommandType = CommandType.StoredProcedure;

                    insert.Parameters.AddWithValue("@idTJualPropertyCicil", txtIDJual.Text);
                    insert.Parameters.AddWithValue("@tanggal", tanggal.Value.ToString("yyyy-MM-dd"));
                    insert.Parameters.AddWithValue("@idProperty", txtProperty.Text);

                    //convert nama ke id  cbCLient
                    SqlConnection convert = connection.Getcon();
                    convert.Open();
                    SqlCommand cmd = new SqlCommand("SELECT idClient FROM client WHERE nama = '" + cbClient.SelectedItem + "'", convert);
                    SqlDataReader dr = cmd.ExecuteReader();
                    SqlDataReader sqlDataReader = dr;
                    dr.Read();
                    insert.Parameters.AddWithValue("@idClient", sqlDataReader["idClient"]);
                    dr.Close();
                    convert.Close();
                    ///

                    insert.Parameters.AddWithValue("@hargaProperty", hargasparator);
                    insert.Parameters.AddWithValue("@perBulan", perbulansparator);
                    insert.Parameters.AddWithValue("@jumlahCicilan", txtJmhlCicilan.Text);
                    insert.Parameters.AddWithValue("@total", totalsparator);

                    insert.ExecuteNonQuery();
                    myConnection.Close();

                    //DETAIL
                    SqlConnection myConnection1 = connection.Getcon();
                    myConnection1.Open();
                    SqlCommand insert1 = new SqlCommand("sp_InsertTJualPropertyCicilDet", myConnection1);
                    insert1.CommandType = CommandType.StoredProcedure;

                    insert1.Parameters.AddWithValue("@idDetailCicil", IDOtomatis2());
                    insert1.Parameters.AddWithValue("@idTJualPropertyCicil", txtIDJual.Text);
                    insert1.Parameters.AddWithValue("@tanggalDetail", tanggal.Value.ToString("yyyy-MM-dd"));
                    insert1.Parameters.AddWithValue("@harga", uangsparator);
                    insert1.Parameters.AddWithValue("@jumlah", 0);

                    insert1.ExecuteNonQuery();
                    myConnection1.Close();
                    clear();
                    MessageBox.Show("Save Sucessfully", "Add Transaksi", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            txtCariProperty.Clear();

            txtIDJual.Clear();
            tanggal.ResetText();
            cbClient.SelectedIndex = -1;


            txtProperty.Clear();
            txtHarga.Clear();
            cbCicilan.SelectedIndex = -1;
            txtperBulan.Clear();
            txtJmhlCicilan.Clear();
            txtTotal.Clear();

            LoadDataProperty();
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
        private double perbulansparator;
        private double totalsparator;
        private double uangsparator;
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

        private void txtperBulan_TextChanged(object sender, EventArgs e)
        {
            if (txtperBulan.Text != "")
            {
                try
                {
                    perbulansparator = Convert.ToDouble(txtperBulan.Text);
                    string res2 = String.Format("{0:#,###}", perbulansparator);
                    txtperBulan.Text = res2.ToString();
                    txtperBulan.Select(txtperBulan.Text.Length, 0);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error Occured" + ex);
                }
            }
        }

        private void txtJmhlCicilan_TextChanged(object sender, EventArgs e)
        {
            try
            {
                double temp1 = Convert.ToInt32(txtJmhlCicilan.Text);
                double temp2 = Convert.ToDouble(txtperBulan.Text);
                double temp = temp2 * temp1;
                string res1 = String.Format("{0:#,##0}", temp);
                txtTotal.Text = res1.ToString();
                txtTotal.Select(txtTotal.Text.Length, 0);
            }
            catch (Exception)
            {
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

        private void txtUang_TextChanged(object sender, EventArgs e)
        {
            if (txtUang.Text != "")
            {
                try
                {
                    uangsparator = Convert.ToDouble(txtUang.Text);
                    string res2 = String.Format("{0:#,###}", uangsparator);
                    txtUang.Text = res2.ToString();
                    txtUang.Select(txtUang.Text.Length, 0);
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
                dgJualProperty.DataSource = dataTable;
                con.Close();

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
                //dgDetailJualProperty.DataSource = dataTable1;
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

        private void bunifuTileButton1_Click(object sender, EventArgs e)
        {
            MenuKasir kasir = new MenuKasir();
            kasir.Show();
            this.Hide();
        }
    }
}
