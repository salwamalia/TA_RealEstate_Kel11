using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace TA_RealEstate_Kel11
{
    public partial class FormClient : Form
    {
        public FormClient()
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

            string sqlQuery = "SELECT TOP (1) MAX(RIGHT (idClient,2))+1 AS idClient FROM client";
            SqlCommand cmd = new SqlCommand(sqlQuery, myConnection);
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                if (dr["idClient"].ToString() == "")
                {
                    autoid = 1;
                }
                else
                {
                    autoid = Int32.Parse(dr["idClient"].ToString());
                }
            }

            if (autoid < 10)
            {
                kode = "CN00" + autoid;
            }
            else if (autoid < 100)
            {
                kode = "CN" + autoid;
            }

            myConnection.Close();

            return kode;
        }

        private void btnSimpan_Click_1(object sender, EventArgs e)
        {
            string jeniskelamin = null;
            if (rbLaki.Checked)
            {
                jeniskelamin = rbLaki.Text;
            }
            if (rbPerempuan.Checked)
            {
                jeniskelamin = rbPerempuan.Text;
            }

            SqlConnection myConnection = connection.Getcon();

            SqlCommand insert = new SqlCommand("sp_InsertClient", myConnection);
            insert.CommandType = CommandType.StoredProcedure;

            insert.Parameters.AddWithValue("idClient", txtID.Text);
            insert.Parameters.AddWithValue("nama", txtNama.Text);
            insert.Parameters.AddWithValue("jeniskelamin", jeniskelamin);
            insert.Parameters.AddWithValue("telepon", txtTelepon.Text);
            insert.Parameters.AddWithValue("email", txtEmail.Text);
            insert.Parameters.AddWithValue("alamat", txtAlamat.Text);

            if (txtID.Text == "" || txtNama.Text == "" || txtEmail.Text == "" || txtTelepon.Text == "" || txtAlamat.Text == "" || jeniskelamin == "")
            {
                MessageBox.Show("Semua Data Harus diisi !!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (!(Regex.IsMatch(txtEmail.Text, @"^^[^@\s]+@[^@\s]+(\.[^@\s]+)+$")))
            {
                MessageBox.Show("Format Email Salah !!" +
                    "\nGunakan Format a@b.c", "Information Email", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                try
                {
                    myConnection.Open();
                    insert.ExecuteNonQuery();
                    MessageBox.Show("Client Telah Ditambahkan", "Add Client", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    clear();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Unable to save " + ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void btnUpdate_Click_1(object sender, EventArgs e)
        {
            string jeniskelamin = null;
            if (rbLaki.Checked)
            {
                jeniskelamin = rbLaki.Text;
            }
            if (rbPerempuan.Checked)
            {
                jeniskelamin = rbPerempuan.Text;
            }
            SqlConnection myConnection = connection.Getcon();

            //Update command
            SqlCommand Update = new SqlCommand("sp_UpdateClient", myConnection);
            Update.CommandType = CommandType.StoredProcedure;

            Update.Parameters.AddWithValue("idClient", txtID.Text);
            Update.Parameters.AddWithValue("nama", txtNama.Text);
            Update.Parameters.AddWithValue("jeniskelamin", jeniskelamin);
            Update.Parameters.AddWithValue("telepon", txtTelepon.Text);
            Update.Parameters.AddWithValue("email", txtEmail.Text);
            Update.Parameters.AddWithValue("alamat", txtAlamat.Text);

            if (txtID.Text == "" || txtNama.Text == "" || txtEmail.Text == "" || txtTelepon.Text == "" || txtAlamat.Text == "" || jeniskelamin == "")
            {
                MessageBox.Show("Semua Data Harus diisi !!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (!(Regex.IsMatch(txtEmail.Text, @"^^[^@\s]+@[^@\s]+(\.[^@\s]+)+$")))
            {
                MessageBox.Show("Format Email Salah !!" +
                    "\nGunakan Format a@b.c", "Information Email", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                try
                {
                    myConnection.Open();
                    Update.ExecuteNonQuery();
                    MessageBox.Show("Update Client Succesfully", "Update Client", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    clear();
                }
                catch (Exception)
                {
                    MessageBox.Show("Unable to save ", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void btnHapus_Click_1(object sender, EventArgs e)
        {
            SqlConnection myConnection = connection.Getcon();

            if (txtCariClient.Text == "")
            {
                MessageBox.Show("Semua Data Harus diisi !!", "Delete Client", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                if (MessageBox.Show("Lanjut ingin Menghapus?", "Delete Client", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    //delete command
                    SqlCommand delete = new SqlCommand("sp_DeleteClient", myConnection);
                    delete.CommandType = CommandType.StoredProcedure;

                    delete.Parameters.AddWithValue("idClient", txtID.Text);

                    try
                    {
                        myConnection.Open();
                        delete.ExecuteNonQuery();
                        MessageBox.Show("Delete Client Succesfully", "Delete Client", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        clear();
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Unable to save ", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }

        private void btnBatal_Click_1(object sender, EventArgs e)
        {
            clear();
        }

        private void btnCariClient_Click_1(object sender, EventArgs e)
        {
            var st = from s in dc.clients where s.idClient == txtCariClient.Text select s;
            dgClient.DataSource = st;

            string jeniskelamin = null;

            if (txtCariClient.Text == "")
            {
                MessageBox.Show("Masukkan ID untuk Cari !!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                try
                {
                    SqlConnection myConnection = connection.Getcon();
                    myConnection.Open();

                    DataTable dt = new DataTable();
                    SqlCommand cmd = new SqlCommand("SELECT * FROM client WHERE idClient ='" + txtCariClient.Text + "'", myConnection);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);

                    da.Fill(dt);

                    txtID.Text = dt.Rows[0]["idClient"].ToString();
                    txtNama.Text = dt.Rows[0]["nama"].ToString();
                    jeniskelamin = dt.Rows[0]["jeniskelamin"].ToString();
                    txtTelepon.Text = dt.Rows[0]["telepon"].ToString();
                    txtEmail.Text = dt.Rows[0]["email"].ToString();
                    txtAlamat.Text = dt.Rows[0]["alamat"].ToString();

                    if (jeniskelamin == rbLaki.Text)
                    {
                        rbLaki.Checked = true;
                    }
                    else if (jeniskelamin == rbPerempuan.Text)
                    {
                        rbPerempuan.Checked = true;
                    }

                    rbLaki.Enabled = true;
                    rbPerempuan.Enabled = true;
                    
                    myConnection.Close();
                }
                catch (Exception)
                {
                    MessageBox.Show("Error Client Tidak Ditemukan! ", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        void LoadData()
        {
            try
            {
                SqlConnection con = connection.Getcon();
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM client", con);
                SqlDataReader dr = cmd.ExecuteReader();
                SqlDataReader sqlDataReader = dr;
                DataTable dataTable = new DataTable();
                dataTable.Columns.Add("ID");
                dataTable.Columns.Add("Nama");
                dataTable.Columns.Add("Jenis Kelamin");
                dataTable.Columns.Add("Telepon");
                dataTable.Columns.Add("Email");
                dataTable.Columns.Add("Alamat");
                while (dr.Read())
                {
                    DataRow row = dataTable.NewRow();
                    row["ID"] = sqlDataReader["idClient"];
                    row["Nama"] = sqlDataReader["nama"];
                    row["Jenis Kelamin"] = sqlDataReader["jenisKelamin"];
                    row["Telepon"] = sqlDataReader["telepon"];
                    row["Email"] = sqlDataReader["email"];
                    row["Alamat"] = sqlDataReader["alamat"];
                    dataTable.Rows.Add(row);
                }
                //this.dataGridView1.DataSource = con.table;
                dgClient.DataSource = dataTable;
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Occured" + ex.Message);
            }
        }

        private void clear()
        {
            txtCariClient.Clear();
            txtID.Clear();
            txtNama.Clear();
            txtEmail.Clear();
            txtTelepon.Clear();
            txtAlamat.Clear();
            rbLaki.Checked = false;
            rbPerempuan.Checked = false;
            txtID.Text = IDOtomatis();
            LoadData();
        }

        private void FormClient_Load(object sender, EventArgs e)
        {
            txtID.Text = IDOtomatis();
            LoadData();
        }

        private void btnJabatan_Click(object sender, EventArgs e)
        {
            FormJabatan jabat = new FormJabatan();
            jabat.Show();
            this.Hide();
        }

        private void btnPegawai_Click(object sender, EventArgs e)
        {
            FormPegawai kary = new FormPegawai();
            kary.Show();
            this.Hide();
        }

        private void btnPemilik_Click(object sender, EventArgs e)
        {
            FormPemilik Own = new FormPemilik();
            Own.Show();
            this.Hide();
        }

        private void btnClient_Click(object sender, EventArgs e)
        {
            FormClient klien = new FormClient();
            klien.Show();
            this.Hide();
        }

        private void btnProperty_Click(object sender, EventArgs e)
        {
            FormProperty Prop = new FormProperty();
            Prop.Show();
            this.Hide();
        }

        private void btnTipeProperty_Click(object sender, EventArgs e)
        {
            FormPropertyTipe PropType = new FormPropertyTipe();
            PropType.Show();
            this.Hide();
        }

        private void btnKategoriBayar_Click(object sender, EventArgs e)
        {
            FormInterior byr = new FormInterior();
            byr.Show();
            this.Hide();
        }

        private void btnCicilan_Click(object sender, EventArgs e)
        {
            FormCicilan cicil = new FormCicilan();
            cicil.Show();
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

        private void bunifuTileButton1_Click(object sender, EventArgs e)
        {
            MenuAdmin admin = new MenuAdmin();
            admin.Show();
            this.Hide();
        }
    }
}
