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
    public partial class TSewaProperty : Form
    {
        public TSewaProperty()
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

            string sqlQuery = "SELECT TOP (1) MAX(RIGHT (idTSewaProperty,2))+1 AS idTSewaProperty FROM TSewaProperty";
            SqlCommand cmd = new SqlCommand(sqlQuery, myConnection);
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                if (dr["idTSewaProperty"].ToString() == "")
                {
                    autoid = 1;
                }
                else
                {
                    autoid = Int32.Parse(dr["idTSewaProperty"].ToString());
                }
            }

            if (autoid < 10)
            {
                kode = "SP00" + autoid;
            }
            else if (autoid < 100)
            {
                kode = "SP0" + autoid;
            }

            myConnection.Close();

            return kode;
        }

        private void txtIDSewa_Click(object sender, EventArgs e)
        {
            txtIDSewa.Text = IDOtomatis();
        }

        public int ambildurasi()
        {
            string temp = CBDurasi.SelectedItem.ToString().Substring(0, 2);
            return Int32.Parse(temp);
        }

        private void btnBayar_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtIDSewa.Text == "" || tanggal.Text == "" || cbClient.Text == "" || txtHarga.Text == "")
                {
                    MessageBox.Show("Semua Data Harus diisi !!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    //SEWA
                    SqlConnection myConnection = connection.Getcon();
                    myConnection.Open();
                    SqlCommand insert = new SqlCommand("sp_InsertTSewaProperty", myConnection);
                    insert.CommandType = CommandType.StoredProcedure;

                    insert.Parameters.AddWithValue("@idTSewaProperty", txtIDSewa.Text);
                    insert.Parameters.AddWithValue("@tanggal", tanggal.Value.ToString("yyyy-MM-dd"));

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

                    insert.Parameters.AddWithValue("@harga", hargasparator);
                    insert.ExecuteNonQuery();
                    myConnection.Close();

                    //DETAIL
                    SqlConnection myConnection1 = connection.Getcon();
                    myConnection1.Open();
                    SqlCommand insert1 = new SqlCommand("sp_InsertTSewaPropertyDet", myConnection1);
                    insert1.CommandType = CommandType.StoredProcedure;

                    insert1.Parameters.AddWithValue("@idTSewaProperty", txtIDSewa.Text);
                    insert1.Parameters.AddWithValue("@idProperty", txtProperty.Text);
                    insert1.Parameters.AddWithValue("@mulai", tanggalMulai.Value.ToString("yyyy-MM-dd"));

                    DateTime tglsampai = DateTime.Parse(tanggalMulai.Value.ToString("yyyy-MM-dd"));

                    insert1.Parameters.AddWithValue("@sampai", tglsampai.AddMonths(ambildurasi()).ToString("yyyy-MM-dd"));
                    insert1.Parameters.AddWithValue("@harga", hargasparator);

                    insert1.ExecuteNonQuery();
                    myConnection1.Close();
                    clear();
                    MessageBox.Show("Save Sucessfully", "Add Transaksi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to save ", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void clear()
        {
            txtCariProperty.Clear();
            txtIDSewa.Clear();
            tanggal.ResetText();
            cbClient.SelectedIndex = -1;

            txtProperty.Clear();
            txtHarga.Clear();
            tanggalMulai.ResetText();
            CBDurasi.SelectedIndex = -1;

            txtUang.Clear();
            txtKembalian.Clear();

            loadClient();
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
                //SEWA
                SqlConnection con = connection.Getcon();
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM TSewaProperty", con);
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

                    row["ID Transaksi"] = sqlDataReader["idTSewaProperty"];
                    row["Tanggal"] = sqlDataReader["tanggal"];
                    row["ID Client"] = sqlDataReader["idClient"];
                    int num = Convert.ToInt32(sqlDataReader["harga"].ToString());
                    string res2 = String.Format("{0:#,##0}", num);
                    row["Harga"] = res2.ToString();
                    dataTable.Rows.Add(row);
                }
                //this.dataGridView1.DataSource = con.table;
                dgSewaProperty.DataSource = dataTable;
                con.Close();

                //DETAIL
                SqlConnection con1 = connection.Getcon();
                con1.Open();
                SqlCommand cmd1 = new SqlCommand("SELECT * FROM TSewaPropertyDet", con1);
                SqlDataReader dr1 = cmd1.ExecuteReader();
                SqlDataReader sqlDataReader1 = dr1;
                DataTable dataTable1 = new DataTable();
                dataTable1.Columns.Add("ID Transaksi");
                dataTable1.Columns.Add("ID Property");
                dataTable1.Columns.Add("Mulai Sewa");
                dataTable1.Columns.Add("Sampai");
                dataTable1.Columns.Add("Harga");
                while (dr1.Read())
                {
                    DataRow row1 = dataTable1.NewRow();
                    row1["ID Transaksi"] = sqlDataReader1["idTSewaProperty"];
                    row1["ID Property"] = sqlDataReader1["idProperty"];
                    row1["Mulai Sewa"] = sqlDataReader1["mulaiSewa"];
                    row1["Sampai"] = sqlDataReader1["sampai"];
                    int num = Convert.ToInt32(sqlDataReader1["harga"].ToString());
                    string res2 = String.Format("{0:#,##0}", num);
                    row1["Harga"] = res2.ToString();
                    dataTable1.Rows.Add(row1);
                }
                dgDetailSewaProperty.DataSource = dataTable1;
                con1.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Occured" + ex);
            }
        }

        private void TSewaProperty_Load(object sender, EventArgs e)
        {
            loadClient();
            LoadDataProperty();
            LoadData();
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
                double temp = temp1 - temp2;
                string res1 = String.Format("{0:#,##0}", temp);
                txtKembalian.Text = res1.ToString();
                txtKembalian.Select(txtKembalian.Text.Length, 0);
            }
            catch (Exception)
            {
            }
        }

        private void btnX_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnSewa_Click(object sender, EventArgs e)
        {
            TSewaProperty sewa = new TSewaProperty();
            sewa.Show();
            this.Hide();
        }

        private void btnJual_Click(object sender, EventArgs e)
        {
            TJualPropertyLunas lunas = new TJualPropertyLunas();
            lunas.Show();
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
