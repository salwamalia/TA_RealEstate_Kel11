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

                    insert1.Parameters.AddWithValue("@idTBeli", txtIDBeli.Text);
                    insert1.Parameters.AddWithValue("@idProperty", cbProperty.SelectedValue.ToString());
                    insert1.Parameters.AddWithValue("@pembayaran", pembayaran);

                    //convert nama ke id  cbCicilan
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

                    insert1.Parameters.AddWithValue("@perBulan", txtperBulan.Text);
                    insert1.Parameters.AddWithValue("@dp", txtDP.Text);
                    insert1.Parameters.AddWithValue("@total", txtTotal.Text);
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
            try
            {
                //UPDATE PEMBELIAN
                SqlConnection myConnection = connection.Getcon();
                myConnection.Open();
                SqlCommand cmd = new SqlCommand("sp_UpdatePembelian", myConnection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idTBeli", txtIDBeli.Text);
                cmd.Parameters.AddWithValue("@tanggal", dateTimePicker1.Value.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@idProperty", cbProperty.SelectedValue.ToString());
                
                //convert nama ke id  cbCLient
                SqlConnection convert = connection.Getcon();
                convert.Open();
                SqlCommand cmd1 = new SqlCommand("SELECT idClient FROM client WHERE nama = '" + cbClient.SelectedItem + "'", convert);
                SqlDataReader dr = cmd1.ExecuteReader();
                SqlDataReader sqlDataReader = dr;
                dr.Read();
                cmd.Parameters.AddWithValue("@idClient", sqlDataReader["idClient"]);
                dr.Close();
                convert.Close();
                ///

                cmd.Parameters.AddWithValue("@total", txtTotal.Text);
                cmd.ExecuteNonQuery();
                myConnection.Close();

                //UPDATE DETAIL PEMBELIAN
                string pembayaran = null;
                if (rbCicil.Checked)
                {
                    pembayaran = rbCicil.Text;
                }
                if (rbLunas.Checked)
                {
                    pembayaran = rbLunas.Text;
                }

                //UPDATE PEMBELIAN
                SqlConnection myConnection1 = connection.Getcon();
                myConnection1.Open();
                SqlCommand cmd2 = new SqlCommand("sp_UpdateDetailPembelian", myConnection1);
                cmd2.CommandType = CommandType.StoredProcedure;
                cmd2.Parameters.AddWithValue("@idTBeli", txtIDBeli.Text);
                cmd2.Parameters.AddWithValue("@idProperty", cbProperty.SelectedValue.ToString());
                cmd2.Parameters.AddWithValue("@pembayaran", pembayaran);

                //convert nama ke id  cbCicilan
                SqlConnection convert1 = connection.Getcon();
                convert1.Open();
                SqlCommand cmd3 = new SqlCommand("SELECT idCicilan FROM kategoriCicilan WHERE cicilan = '" + cbCicilan.SelectedItem + "'", convert1);
                SqlDataReader dr1 = cmd3.ExecuteReader();
                SqlDataReader sqlDataReader1 = dr1;
                dr1.Read();
                cmd2.Parameters.AddWithValue("@idCicilan", sqlDataReader1["idCicilan"]);
                dr1.Close();
                convert1.Close();
                ///

                cmd2.Parameters.AddWithValue("@perBulan", txtperBulan.Text);
                cmd2.Parameters.AddWithValue("@dp", txtDP.Text);
                cmd2.Parameters.AddWithValue("@total", txtTotal.Text);
                cmd2.ExecuteNonQuery();
                myConnection1.Close();

                MessageBox.Show("Update Succesfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                clear();
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error"+ex);
            }
        }

        private void btnHapus_Click(object sender, EventArgs e)
        {
            if (txtIDBeli.Text == "")
            {
                MessageBox.Show("Semua Data Harus diisi !!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                if (MessageBox.Show("Lanjut ingin Menghapus?", "Delete Transaksi", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    //DELETE PEMBELIAN
                    SqlConnection myConnection = connection.Getcon();
                    SqlCommand delete = new SqlCommand("sp_DeletePembelian", myConnection);
                    delete.CommandType = CommandType.StoredProcedure;

                    delete.Parameters.AddWithValue("idTBeli", txtIDBeli.Text);

                    try
                    {
                        myConnection.Open();
                        delete.ExecuteNonQuery();
                        MessageBox.Show("Transaksi Delete Succesfully", "Delete Transaksi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        clear();
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Unable to save ", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
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
                    //PEMBELIAN
                    SqlConnection con = connection.Getcon();
                    con.Open();
                    SqlCommand cmd = new SqlCommand("SELECT * FROM tPembelian WHERE idTbeli ='" + txtCariTransaksi.Text + "'", con);
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

                    //DETAIL PEMBELIAN
                    SqlConnection con1 = connection.Getcon();
                    con1.Open();
                    SqlCommand cmd3 = new SqlCommand("SELECT * FROM tDetailPembelian WHERE idTbeli ='" + txtCariTransaksi.Text + "'", con1);
                    SqlDataReader dr3 = cmd3.ExecuteReader();
                    SqlDataReader sqlDataReader3 = dr3;
                    DataTable dataTable1 = new DataTable();
                    dataTable1.Columns.Add("ID Transaksi");
                    dataTable1.Columns.Add("ID Property");
                    dataTable1.Columns.Add("Pembayaran");
                    dataTable1.Columns.Add("ID Cicilan");
                    dataTable1.Columns.Add("Perbulan");
                    dataTable1.Columns.Add("DP");
                    dataTable1.Columns.Add("Total");
                    while (dr3.Read())
                    {
                        DataRow row1 = dataTable1.NewRow();
                        row1["ID Transaksi"] = sqlDataReader3["idTBeli"];

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
                    string pemilik = myreader.GetString(11);
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

        private void cbCicilan_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (txtHargaProperty.Text != "")
            {
                int temp = Int32.Parse(cbCicilan.SelectedItem.ToString());
                int temp1 = Int32.Parse(txtHargaProperty.Text);
                int temp2 = temp1 / temp;
                txtperBulan.Text = temp2.ToString();
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
            catch (Exception)
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
        public void loadProperty()
        {
            SqlConnection con1 = connection.Getcon();
            con1.Open();
            SqlCommand cmd1 = new SqlCommand("SELECT namaProperty FROM property", con1);
            SqlDataReader dr1 = cmd1.ExecuteReader();
            SqlDataReader sqlDataReader1 = dr1;
            while (dr1.Read())
            {
                cbProperty.Items.Add(sqlDataReader1["namaProperty"]);
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

                    if (String.IsNullOrEmpty(sqlDataReader["idProperty"].ToString()))
                    {
                        row["ID Property"] = null;
                    }
                    else
                    {
                        SqlConnection con2 = connection.Getcon();
                        con2.Open();
                        SqlCommand cmd2 = new SqlCommand("SELECT namaProperty FROM property WHERE idProperty = '" + sqlDataReader["idProperty"] + "'", con2);
                        SqlDataReader dr2 = cmd2.ExecuteReader();
                        SqlDataReader sqlDataReader2 = dr2;
                        dr2.Read();
                        row["ID Property"] = sqlDataReader2["namaProperty"];
                        dr2.Close();
                        con2.Close();
                    }

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

        private void TransaksiBeli_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'rEALESTATEDataSet.property' table. You can move, or remove it, as needed.
            this.propertyTableAdapter.Fill(this.rEALESTATEDataSet.property);
            txtIDBeli.Text = IDOtomatis();

            //loadProperty();
            loadCicilan();
            loadClient();
            LoadData();
        }

        private void dgTransaksi_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                //PEMBELIAN
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
                    for (int i = 0; i < cbClient.Items.Count; i++)
                    {
                        cbClient.SelectedIndex = i;
                        if (dgTransaksi.Rows[e.RowIndex].Cells[3].Value.ToString() == cbClient.SelectedItem)
                        {
                            break;
                        }
                    }
                }
                txtTotal.Text = dgTransaksi.Rows[e.RowIndex].Cells[4].Value.ToString();

                //DETAIL PEMBELIAN
                string pembayaran = null;

                txtIDBeli.Text = dgDetailTransaksi.Rows[e.RowIndex].Cells[0].Value.ToString();

                //property
                if (!(String.IsNullOrEmpty(dgDetailTransaksi.Rows[e.RowIndex].Cells[1].Value.ToString())))
                {
                    for (int i = 0; i < cbProperty.Items.Count; i++)
                    {
                        cbProperty.SelectedIndex = i;
                        if (dgDetailTransaksi.Rows[e.RowIndex].Cells[1].Value.ToString() == cbProperty.SelectedItem)
                        {
                            break;
                        }
                    }
                }

                pembayaran = dgDetailTransaksi.Rows[e.RowIndex].Cells[2].Value.ToString();

                //cicilan
                if (!(String.IsNullOrEmpty(dgDetailTransaksi.Rows[e.RowIndex].Cells[3].Value.ToString())))
                {
                    for (int i = 0; i < cbCicilan.Items.Count; i++)
                    {
                        cbCicilan.SelectedIndex = i;
                        if (dgDetailTransaksi.Rows[e.RowIndex].Cells[3].Value.ToString() == cbCicilan.SelectedItem)
                        {
                            break;
                        }
                    }
                }

                txtperBulan.Text = dgDetailTransaksi.Rows[e.RowIndex].Cells[4].Value.ToString();
                txtDP.Text = dgDetailTransaksi.Rows[e.RowIndex].Cells[5].Value.ToString();
                txtTotal.Text = dgDetailTransaksi.Rows[e.RowIndex].Cells[6].Value.ToString();

                if (pembayaran == rbCicil.Text)
                {
                    rbCicil.Checked = true;
                }
                else if (pembayaran == rbLunas.Text)
                {
                    rbLunas.Checked = true;
                }

                rbCicil.Enabled = true;
                rbLunas.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Occured" + ex.Message);
            }
        }

        private void dgDetailTransaksi_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                //PEMBELIAN
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
                    for (int i = 0; i < cbClient.Items.Count; i++)
                    {
                        cbClient.SelectedIndex = i;
                        if (dgTransaksi.Rows[e.RowIndex].Cells[3].Value.ToString() == cbClient.SelectedItem)
                        {
                            break;
                        }
                    }
                }
                txtTotal.Text = dgTransaksi.Rows[e.RowIndex].Cells[4].Value.ToString();

                //DETAIL PEMBELIAN
                string pembayaran = null;

                txtIDBeli.Text = dgDetailTransaksi.Rows[e.RowIndex].Cells[0].Value.ToString();

                //property
                if (!(String.IsNullOrEmpty(dgDetailTransaksi.Rows[e.RowIndex].Cells[1].Value.ToString())))
                {
                    for (int i = 0; i < cbProperty.Items.Count; i++)
                    {
                        cbProperty.SelectedIndex = i;
                        if (dgDetailTransaksi.Rows[e.RowIndex].Cells[1].Value.ToString() == cbProperty.SelectedItem)
                        {
                            break;
                        }
                    }
                }

                pembayaran = dgDetailTransaksi.Rows[e.RowIndex].Cells[2].Value.ToString();

                //cicilan
                if (!(String.IsNullOrEmpty(dgDetailTransaksi.Rows[e.RowIndex].Cells[3].Value.ToString())))
                {
                    for (int i = 0; i < cbCicilan.Items.Count; i++)
                    {
                        cbCicilan.SelectedIndex = i;
                        if (dgDetailTransaksi.Rows[e.RowIndex].Cells[3].Value.ToString() == cbCicilan.SelectedItem)
                        {
                            break;
                        }
                    }
                }

                txtperBulan.Text = dgDetailTransaksi.Rows[e.RowIndex].Cells[4].Value.ToString();
                txtDP.Text = dgDetailTransaksi.Rows[e.RowIndex].Cells[5].Value.ToString();
                txtTotal.Text = dgDetailTransaksi.Rows[e.RowIndex].Cells[6].Value.ToString();

                if (pembayaran == rbCicil.Text)
                {
                    rbCicil.Checked = true;
                }
                else if (pembayaran == rbLunas.Text)
                {
                    rbLunas.Checked = true;
                }

                rbCicil.Enabled = true;
                rbLunas.Enabled = true;
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error Occured" + ex.Message);
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

        private void btnPembayaran_Click(object sender, EventArgs e)
        {
            PembayaranPembelian bayar = new PembayaranPembelian();
            bayar.Show();
            this.Hide();
        }

        private void btnbayarsewa_Click(object sender, EventArgs e)
        {
            PembayaranPenyewaan bayar = new PembayaranPenyewaan();
            bayar.Show();
            this.Hide();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            Login log = new Login();
            log.Visible = true;
            this.Hide();
        }
    }
}
