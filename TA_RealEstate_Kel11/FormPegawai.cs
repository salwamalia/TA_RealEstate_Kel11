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

namespace TA_RealEstate_Kel11
{
    public partial class FormPegawai : Form
    {
        public FormPegawai()
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

            string sqlQuery = "SELECT TOP (1) MAX(RIGHT (idPegawai,2))+1 AS idPegawai FROM pegawai";
            SqlCommand cmd = new SqlCommand(sqlQuery, myConnection);
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                if (dr["idPegawai"].ToString() == "")
                {
                    autoid = 1;
                }
                else
                {
                    autoid = Int32.Parse(dr["idPegawai"].ToString());
                }
            }

            if (autoid < 10)
            {
                kode = "PG00" + autoid;
            }
            else if (autoid < 100)
            {
                kode = "PG" + autoid;
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

            SqlCommand insert = new SqlCommand("sp_InsertPegawai", myConnection);
            insert.CommandType = CommandType.StoredProcedure;

            insert.Parameters.AddWithValue("idPegawai", txtID.Text);
            insert.Parameters.AddWithValue("nama", txtNama.Text);
            insert.Parameters.AddWithValue("jeniskelamin", jeniskelamin);
            insert.Parameters.AddWithValue("username", txtUser.Text);
            insert.Parameters.AddWithValue("password", txtPass.Text);
            insert.Parameters.AddWithValue("idjabatan", cbJabatan.SelectedValue.ToString());

            if (txtID.Text == "" || txtNama.Text == "" || jeniskelamin == "" || txtUser.Text == "" || txtPass.Text == "" || cbJabatan.Text == "")
            {
                MessageBox.Show("Semua Data Harus diisi !!", "Add Pegawai", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                try
                {
                    myConnection.Open();
                    insert.ExecuteNonQuery();
                    MessageBox.Show("Pegawai Telah Ditambahkan", "Add Pegawai", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    clear();
                }
                catch (Exception)
                {
                    MessageBox.Show("Unable to save ", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            SqlCommand Update = new SqlCommand("sp_UpdatePegawai", myConnection);
            Update.CommandType = CommandType.StoredProcedure;

            Update.Parameters.AddWithValue("idPegawai", txtID.Text);
            Update.Parameters.AddWithValue("nama", txtNama.Text);
            Update.Parameters.AddWithValue("jeniskelamin", jeniskelamin);
            Update.Parameters.AddWithValue("username", txtUser.Text);
            Update.Parameters.AddWithValue("password", txtPass.Text);
            Update.Parameters.AddWithValue("idJabatan", cbJabatan.SelectedValue.ToString());

            if (txtID.Text == "" || txtNama.Text == "" || jeniskelamin == "" || txtUser.Text == "" || txtPass.Text == "" || cbJabatan.Text == "")
            {
                MessageBox.Show("Semua Data Harus diisi !!", "Update Pegawai", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                try
                {
                    myConnection.Open();
                    Update.ExecuteNonQuery();
                    MessageBox.Show("Update Pegawai Succesfully", "Update Pegawai", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    clear();
                }
                catch (Exception )
                {
                    MessageBox.Show("Unable to save ", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void btnHapus_Click_1(object sender, EventArgs e)
        {
            SqlConnection myConnection = connection.Getcon();

            if (txtCariPegawai.Text == "")
            {
                MessageBox.Show("Semua Data Harus diisi !!", "Delete Pegawai", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                if (MessageBox.Show("Lanjut ingin Menghapus?", "Delete Pegawai", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    //delete command
                    SqlCommand delete = new SqlCommand("sp_DeletePegawai", myConnection);
                    delete.CommandType = CommandType.StoredProcedure;

                    delete.Parameters.AddWithValue("idPegawai", txtID.Text);

                    try
                    {
                        myConnection.Open();
                        delete.ExecuteNonQuery();
                        MessageBox.Show("Delete Pegawai Succesfully", "Delete Pegawai", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void clear()
        {
            txtCariPegawai.Clear();
            txtID.Clear();
            txtNama.Clear();
            txtUser.Clear();
            txtPass.Clear();
            cbJabatan.SelectedValue = -1;
            rbLaki.Checked = false;
            rbPerempuan.Checked = false;
            txtID.Text = IDOtomatis();
            LoadData();
        }

        private void btnCari_Click(object sender, EventArgs e)
        {
            var st = from s in dc.pegawais where s.idPegawai == txtCariPegawai.Text select s;
            dgPegawai.DataSource = st;

            string jeniskelamin = null;

            if (txtCariPegawai.Text == "")
            {
                MessageBox.Show("Masukkan ID Untuk Cari !!", "Cari Pegawai", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                try
                {
                    SqlConnection myConnection = connection.Getcon();
                    myConnection.Open();

                    DataTable dt = new DataTable();
                    SqlCommand cmd = new SqlCommand("SELECT * FROM pegawai WHERE idPegawai ='" + txtCariPegawai.Text + "'", myConnection);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);

                    da.Fill(dt);

                    txtID.Text = dt.Rows[0]["idPegawai"].ToString();
                    txtNama.Text = dt.Rows[0]["nama"].ToString();
                    jeniskelamin = dt.Rows[0]["jeniskelamin"].ToString();
                    txtUser.Text = dt.Rows[0]["username"].ToString();
                    txtPass.Text = dt.Rows[0]["password"].ToString();
                    cbJabatan.SelectedValue = dt.Rows[0]["idJabatan"].ToString();

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
                    MessageBox.Show("Error Pegawai Tidak Ditemukan! ", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }     
                
        private void FormPegawai_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'rEALESTATEDataSet.jabatan' table. You can move, or remove it, as needed.
            this.jabatanTableAdapter.Fill(this.rEALESTATEDataSet.jabatan);
            // TODO: This line of code loads data into the 'rEALESTATEDataSet.jabatan' table. You can move, or remove it, as needed.
            this.jabatanTableAdapter.Fill(this.rEALESTATEDataSet.jabatan);
            // TODO: This line of code loads data into the 'rEALESTATEDataSet.jabatan' table. You can move, or remove it, as needed.
            this.jabatanTableAdapter.Fill(this.rEALESTATEDataSet.jabatan);
            txtID.Text = IDOtomatis();
            LoadData();
        }

        void LoadData()
        {
            try
            {
                SqlConnection con = connection.Getcon();
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM pegawai", con);
                SqlDataReader dr = cmd.ExecuteReader();
                SqlDataReader sqlDataReader = dr;
                DataTable dataTable = new DataTable();
                dataTable.Columns.Add("ID");
                dataTable.Columns.Add("Nama");
                dataTable.Columns.Add("Jenis Kelamin");
                dataTable.Columns.Add("Username");
                dataTable.Columns.Add("Password");
                dataTable.Columns.Add("Jabatan");
                while (dr.Read())
                {
                    DataRow row = dataTable.NewRow();
                    row["ID"] = sqlDataReader["idPegawai"];
                    row["Nama"] = sqlDataReader["nama"];
                    row["Jenis Kelamin"] = sqlDataReader["jenisKelamin"];
                    row["Username"] = sqlDataReader["username"];
                    row["Password"] = sqlDataReader["password"];
                    row["Jabatan"] = sqlDataReader["idJabatan"];
                    dataTable.Rows.Add(row);
                }
                //this.dataGridView1.DataSource = con.table;
                dgPegawai.DataSource = dataTable;
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Occured" + ex.Message);
            }
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
