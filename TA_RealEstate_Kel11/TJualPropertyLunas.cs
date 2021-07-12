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
    public partial class TJualPropertyLunas : Form
    {
        public TJualPropertyLunas()
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

            string sqlQuery = "SELECT TOP (1) MAX(RIGHT (idJPLunas,2))+1 AS idJPLunas FROM TJualPropertyLunas";
            SqlCommand cmd = new SqlCommand(sqlQuery, myConnection);
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                if (dr["idJPLunas"].ToString() == "")
                {
                    autoid = 1;
                }
                else
                {
                    autoid = Int32.Parse(dr["idJPLunas"].ToString());
                }
            }

            if (autoid < 10)
            {
                kode = "PL00" + autoid;
            }
            else if (autoid < 100)
            {
                kode = "PL0" + autoid;
            }

            myConnection.Close();

            return kode;
        }

        private void txtIDJual_Click(object sender, EventArgs e)
        {
            txtIDJual.Text = IDOtomatis();
        }

        private void btnBayar_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtIDJual.Text == "" || txtProperty.Text == "")
                {
                    MessageBox.Show("Semua Data Harus diisi !!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    //INSERT
                    SqlConnection myConnection = connection.Getcon();
                    myConnection.Open();
                    SqlCommand insert = new SqlCommand("sp_InsertTJualPropertyLunas", myConnection);
                    insert.CommandType = CommandType.StoredProcedure;

                    insert.Parameters.AddWithValue("@idJPLunas", txtIDJual.Text);
                    insert.Parameters.AddWithValue("@tanggal", tanggal.Value.ToString("yyyy-MM-dd"));
                    insert.Parameters.AddWithValue("@idProperty", txtProperty.Text);
                    insert.Parameters.AddWithValue("@harga", hargasparator);

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

                    insert.ExecuteNonQuery();
                    MessageBox.Show("Transaksi Berhasil", "Jual Property", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    myConnection.Close();
                    clear();
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
            txtIDJual.Clear();
            txtProperty.Clear();
            txtHarga.Clear();
            cbClient.SelectedIndex = -1;
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
                    //MessageBox.Show("ID Tidak Ada !!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    MessageBox.Show("ID Tidak Ada !!" + ex);
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
                SqlConnection con = connection.Getcon();
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM TJualPropertyLunas", con);
                SqlDataReader dr = cmd.ExecuteReader();
                SqlDataReader sqlDataReader = dr;
                DataTable dataTable = new DataTable();
                dataTable.Columns.Add("ID Transaksi");
                dataTable.Columns.Add("Tanggal");
                dataTable.Columns.Add("ID Property");
                dataTable.Columns.Add("Harga");
                dataTable.Columns.Add("ID Client");
                while (dr.Read())
                {
                    DataRow row = dataTable.NewRow();

                    row["ID Transaksi"] = sqlDataReader["idJPLunas"];
                    row["Tanggal"] = sqlDataReader["tanggal"];
                    row["ID Property"] = sqlDataReader["idProperty"];
                    int num = Convert.ToInt32(sqlDataReader["harga"].ToString());
                    string res2 = String.Format("{0:#,##0}", num);
                    row["Harga"] = res2.ToString();
                    row["ID Client"] = sqlDataReader["idClient"];
                    dataTable.Rows.Add(row);
                }
                //this.dataGridView1.DataSource = con.table;
                dgJualProperty.DataSource = dataTable;
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Occured" + ex);
            }
        }

        private void TJualPropertyLunas_Load(object sender, EventArgs e)
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
