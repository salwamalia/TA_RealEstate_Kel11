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

        public String seperate(String a)
        {
            string[] test = a.Split(',');
            string x = "";
            foreach (string tst in test)
            {

                if (tst.Trim() != "")
                {
                    x = x + tst;
                    Console.Write(tst);
                }
            }
            return x;
        }

        public int split(String x)
        {
            String[] a = x.Split(' ');
            String[] b = a[1].Split('.');
            String[] c = b[1].Split(',');
            String Total = b[0] + c[0];
            int total = int.Parse(seperate(Total));

            return total;
        }

        private void btnSimpan_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtIDBeli.Text == "" || dateTimePicker1.Text == "" || cbProperty.Text == "" || cbClient.Text == "" || txtTotal.Text == "")
                {
                    MessageBox.Show("Semua Data Harus diisi !!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    //INSERT PEMBELIAN
                    SqlConnection myConnection = connection.Getcon();
                    myConnection.Open();
                    SqlCommand insert = new SqlCommand("sp_InsertPembelian", myConnection);
                    insert.CommandType = CommandType.StoredProcedure;

                    insert.Parameters.AddWithValue("@idTBeli", txtIDBeli.Text);
                    insert.Parameters.AddWithValue("@tanggal", dateTimePicker1.Value.ToString("yyyy-MM-dd"));
                    insert.Parameters.AddWithValue("@idProperty", cbProperty.SelectedValue.ToString());

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

                    insert.Parameters.AddWithValue("@total", txtTotal.Text);
                    insert.ExecuteNonQuery();
                    myConnection.Close();

                    //INSERT DETAIL PEMBELIAN
                    string pembayaran = null;
                    if (rbCicil.Checked)
                    {
                        pembayaran = rbCicil.Text;
                    }
                    if (rbLunas.Checked)
                    {
                        pembayaran = rbLunas.Text;
                    }

                    SqlConnection myConnection1 = connection.Getcon();
                    myConnection1.Open();
                    SqlCommand insert1 = new SqlCommand("sp_InsertDetailPembelian", myConnection1);
                    insert1.CommandType = CommandType.StoredProcedure;

                    insert1.Parameters.AddWithValue("idTBeli", txtIDBeli.Text);
                    insert1.Parameters.AddWithValue("idProperty", cbProperty.SelectedValue.ToString());
                    insert1.Parameters.AddWithValue("pembayaran", pembayaran);
                    //insert1.Parameters.AddWithValue("idCicilan", cbCicilan.SelectedValue.ToString());

                    //convert nama ke id  cbCLient
                    SqlConnection convert1 = connection.Getcon();
                    convert1.Open();
                    SqlCommand cmd1 = new SqlCommand("SELECT idCicilan FROM kategoriCicilan WHERE cicilan = '" + cbCicilan.SelectedItem + "'", convert1);
                    SqlDataReader dr1 = cmd1.ExecuteReader();
                    SqlDataReader sqlDataReader1 = dr1;
                    dr1.Read();
                    insert1.Parameters.AddWithValue("@idCicilan", sqlDataReader1["idCicilan"]);
                    dr1.Close();
                    convert1.Close();
                    ///

                    insert1.Parameters.AddWithValue("perBulan", txtperBulan.Text);
                    insert1.Parameters.AddWithValue("dp", txtDP.Text);
                    insert1.Parameters.AddWithValue("total", txtTotal.Text);
                    insert1.ExecuteNonQuery();
                    myConnection1.Close();
                    clear();
                }
                MessageBox.Show("Save Sucessfully", "Add Pembelian", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Unable to save ", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                MessageBox.Show("Unable to save " + ex);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {

        }

        private void btnHapus_Click(object sender, EventArgs e)
        {

        }

        private SqlCommand cmd;
        private void btnCariTransaksi_Click(object sender, EventArgs e)
        {
            if (txtCariTransaksi.Text == "")
            {
                MessageBox.Show("Masukkan ID Untuk Cari !!", "Cari Transaksi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                try
                {
                    SqlConnection con = connection.Getcon();
                    con.Open();
                    DataTable table = new DataTable();
                    table.Rows.Clear();
                    table.Columns.Clear();
                    dgTransaksi.DataSource = table;
                    SqlCommand commanbahan = new SqlCommand("sp_SearchNamaBahan", conn);
                    commanbahan.CommandType = CommandType.StoredProcedure;
                    commanbahan.Parameters.AddWithValue("@NamaBahan", Convert.ToString(txtcarinama.Text));
                    SqlDataReader readbahan;
                    readbahan = commanbahan.ExecuteReader();
                    int nobahan = 0;
                    int ibahan = 0;

                    String[] idBahan = new String[1000];
                    String[] namBahan = new String[1000];
                    String[] jumlahBahan = new String[1000];
                    String[] jenisBahan = new String[1000];
                    String[] kadaluarsa = new String[1000];
                    String[] idSupplier = new String[1000];
                    String[] Status = new String[1000];
                    String[] Hargabahan = new String[1000];
                    while (readbahan.Read())
                    {
                        ibahan++;
                        idBahan[ibahan] = readbahan["IdBahan"].ToString();
                        namBahan[ibahan] = readbahan["NamaBahan"].ToString();
                        jumlahBahan[ibahan] = readbahan["JumlahBahan"].ToString();
                        jenisBahan[ibahan] = readbahan["JenisBahan"].ToString();
                        kadaluarsa[ibahan] = readbahan["Kadaluarsa"].ToString();
                        idSupplier[ibahan] = readbahan["Id_Supplier"].ToString();
                    }
                    con.Close();

                    for (int a = 1; a <= ibahan; a++)
                    {
                        con.Open();

                        SqlCommand cmdsupplier = new SqlCommand("sp_SearchSupplier", con);
                        cmdsupplier.CommandType = CommandType.StoredProcedure;
                        cmdsupplier.Parameters.AddWithValue("@IdSupplier", idSupplier[a]);
                        SqlDataReader drsupplier;
                        drsupplier = cmdsupplier.ExecuteReader();
                        String supplier = "";
                        if (drsupplier.Read())
                        {
                            supplier = drsupplier["NamaSupplier"].ToString();
                        }
                        if (Status[a].Equals("aktif"))
                        {
                            nobahan++;
                            table.Rows.Add(nobahan, idBahan[a], namBahan[a], jumlahBahan[a], Hargabahan[a], jenisBahan[a], kadaluarsa[a], supplier, Status[a]);
                            dgTransaksi.DataSource = table;
                        }
                        con.Close();
                    }
                    con.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error Occured" + ex);
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
            LoadData();
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

        private void cbCicilan_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtHargaProperty.Text != "")
                {
                    int temp = Int32.Parse(cbCicilan.SelectedItem.ToString());
                    int temp1 = Int32.Parse(txtHargaProperty.Text);
                    int temp2 = temp1 / temp;
                    txtperBulan.Text = temp2.ToString();
                }
            }
            catch (Exception)
            {
            }
        }

        //TEXTBOX
        private void txtDP_TextChanged(object sender, EventArgs e)
        {
            try
            {
                int temp = Int32.Parse(txtDP.Text);
                int temp1 = Int32.Parse(txtHargaProperty.Text);
                int temp2 = temp1 - temp;
                txtTotal.Text = temp2.ToString();
            }
            catch (Exception ex)
            {
            }
        }

        //SEPARATOR
        private int hargasparator;
        private void txtHargaProperty_Leave(object sender, EventArgs e)
        {
            if (txtHargaProperty.Text != "")
            {
                hargasparator = Convert.ToInt32(txtHargaProperty.Text);
                string res2 = String.Format("{0:#,##0.00}", hargasparator);
                txtHargaProperty.Text = res2.ToString();
            }
        }

        //LOAD DATA
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
                //TABEL PEMBELIAN
                SqlConnection con = connection.Getcon();
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM tPembelian", con);
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
                    row["ID Transaksi"] = sqlDataReader["idTBeli"];
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

                //TABLE DETAIL PEMBELIAN
                SqlConnection con1 = connection.Getcon();
                con1.Open();
                SqlCommand cmd1 = new SqlCommand("SELECT * FROM tDetailPembelian", con1);
                SqlDataReader dr1 = cmd1.ExecuteReader();
                SqlDataReader sqlDataReader1 = dr1;
                DataTable dataTable1 = new DataTable();
                dataTable1.Columns.Add("ID Transaksi");
                dataTable1.Columns.Add("ID Property");
                dataTable1.Columns.Add("Pembayaran");
                dataTable1.Columns.Add("ID Cicilan");
                dataTable1.Columns.Add("Perbulan");
                dataTable1.Columns.Add("DP");
                dataTable1.Columns.Add("Total");
                while (dr1.Read())
                {
                    DataRow row1 = dataTable1.NewRow();
                    row1["ID Transaksi"] = sqlDataReader1["idTBeli"];
                    row1["ID Property"] = sqlDataReader1["idProperty"];
                    row1["Pembayaran"] = sqlDataReader1["pembayaran"];
                    row1["ID Cicilan"] = sqlDataReader1["idCicilan"];
                    row1["Perbulan"] = sqlDataReader1["perBulan"];
                    row1["DP"] = sqlDataReader1["dp"];
                    row1["Total"] = sqlDataReader1["total"];
                    dataTable1.Rows.Add(row1);
                }
                dgDetailTransaksi.DataSource = dataTable1;
                con1.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Occured" + ex);
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
            LoadData();
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

        private void dgTransaksi_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtIDBeli.Text = dgTransaksi.Rows[e.RowIndex].Cells[0].Value.ToString();
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
    }
}
