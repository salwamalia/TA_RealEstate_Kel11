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
            //int StatusBeli = 0;

            if (txtIDBeli.Text == "" || txtDP.Text == "" || cbProperty.Text == "" || cbCicilan.Text == "" || cbClient.Text == "")
            {
                MessageBox.Show("Semua Data Harus diisi !!", "Add Property", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                try
                {
                    SqlConnection myConnection = connection.Getcon();
                    SqlCommand cmd1 = new SqlCommand("sp_InsertPembelian", myConnection);
                    cmd1.CommandType = CommandType.StoredProcedure;
                    cmd1.Parameters.AddWithValue("idTBeli", txtIDBeli.Text);
                    cmd1.Parameters.AddWithValue("tanggal", TglTrans.Value.ToString("yyyy-MM-dd"));
                    cmd1.Parameters.AddWithValue("idProperty", cbProperty.SelectedValue.ToString());
                    cmd1.Parameters.AddWithValue("idClient", cbClient.SelectedItem.ToString());
                    cmd1.Parameters.AddWithValue("total", txtTotal.Text);
                    myConnection.Open();
                    cmd1.ExecuteNonQuery();
                    myConnection.Close();

                    for (int i = 0; i < dbTransaksi.Rows.Count - 1; i++)
                    {
                        string pembayaran = null;
                        if (rbCicil.Checked)
                        {
                            pembayaran = rbCicil.Text;
                        }
                        if (rbLunas.Checked)
                        {
                            pembayaran = rbLunas.Text;
                        }

                        //menambah data pada detail transaksi
                        SqlConnection myConnection1 = connection.Getcon();
                        myConnection1.Open();
                        SqlCommand cmd2 = new SqlCommand("sp_InsertDetailPembelian", myConnection1);
                        cmd2.CommandType = CommandType.StoredProcedure;
                        cmd2.Parameters.AddWithValue("idTBeli", txtIDBeli.Text);
                        cmd2.Parameters.AddWithValue("idProperty", cbProperty.SelectedValue.ToString());
                        cmd2.Parameters.AddWithValue("harga", txtHargaProperty.Text);
                        cmd2.Parameters.AddWithValue("pembayaran", pembayaran);
                        cmd2.Parameters.AddWithValue("idCicilan", cbCicilan.SelectedValue.ToString());
                        cmd2.Parameters.AddWithValue("perBulan", txtperBulan.Text);
                        cmd2.Parameters.AddWithValue("dp", txtDP.Text);
                        cmd2.Parameters.AddWithValue("total", txtTotal.Text);
                        cmd2.ExecuteNonQuery();
                        myConnection1.Close();

                        //mencari jumlah dari bahan yang ditambahkan
                        SqlConnection conn = connection.Getcon();
                        conn.Open();
                        SqlCommand cmd = new SqlCommand("sp_SearchProperty", conn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@idProperty", dbTransaksi.Rows[i].Cells[0].Value);
                        SqlDataReader drprop;
                        drprop = cmd.ExecuteReader();
                        String Prop = "";
                        int found = 0;
                        
                        while (drprop.Read())
                        {
                            Prop = drprop["statusProperty"].ToString();
                        }

                        /*for (int i = 0; i < dbTransaksi.Rows.Count - 1; i++)
                        {
                            if (Prop.Equals("aktif"))
                            {
                                found++;
                            }
                        }*/
                        conn.Close();

                        //Menyimpan data jumlah yang sudah diupdate
                        SqlConnection conn1 = connection.Getcon();
                        conn1.Open();
                        SqlCommand cmd7 = new SqlCommand("sp_UpdateStatusProp", conn1);
                        cmd7.CommandType = CommandType.StoredProcedure;
                        cmd7.Parameters.AddWithValue("@idProperty", SqlDbType.VarChar).Value = dbTransaksi.Rows[i].Cells[0].Value;
                        cmd7.Parameters.AddWithValue("@statusProperty", SqlDbType.NVarChar).Value = "Not Available";
                        cmd7.ExecuteNonQuery();
                        conn1.Close();
                    }

                    myConnection.Close();
                    MessageBox.Show("Pembelian Telah Ditambahkan", "Add Pembelian", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    clear();
                }
                catch (Exception ex)
                {
                    //MessageBox.Show("Unable to save " + ex.Message , "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning );
                    MessageBox.Show("Unable to save " + ex);
                }
            }
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
            TglTrans.ResetText();
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
            // TODO: This line of code loads data into the 'rEALESTATEDataSet.property' table. You can move, or remove it, as needed.
            this.propertyTableAdapter.Fill(this.rEALESTATEDataSet.property);
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
