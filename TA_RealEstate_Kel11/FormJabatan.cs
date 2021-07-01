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
    public partial class FormJabatan : Form
    {
        public FormJabatan()
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

            string sqlQuery = "SELECT TOP (1) MAX(RIGHT (idJabatan,2))+1 AS idJabatan FROM jabatan";
            SqlCommand cmd = new SqlCommand(sqlQuery, myConnection);
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                if (dr["idJabatan"].ToString() == "")
                {
                    autoid = 1;
                }
                else
                {
                    autoid = Int32.Parse(dr["idJabatan"].ToString());
                }
            }

            if (autoid < 10)
            {
                kode = "JB00" + autoid;
            }
            else if (autoid < 100)
            {
                kode = "JB" + autoid;
            }

            myConnection.Close();

            return kode;
        }

        private void btnSimpan_Click_1(object sender, EventArgs e)
        {
            SqlConnection myConnection = connection.Getcon();

            SqlCommand insert = new SqlCommand("sp_InputJabatan", myConnection);
            insert.CommandType = CommandType.StoredProcedure;

            insert.Parameters.AddWithValue("idJabatan", txtID.Text);
            insert.Parameters.AddWithValue("jabatan", txtNama.Text);

            if (txtID.Text == "" || txtNama.Text == "")
            {
                MessageBox.Show("Semua Data Harus diisi !!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                try
                {
                    myConnection.Open();
                    insert.ExecuteNonQuery();
                    MessageBox.Show("Jabatan Telah Ditambahkan", "Add Jabatan", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            SqlConnection myConnection = connection.Getcon();

            //update command
            SqlCommand Update = new SqlCommand("sp_UpdateJabatan", myConnection);
            Update.CommandType = CommandType.StoredProcedure;

            Update.Parameters.AddWithValue("idJabatan", txtID.Text);
            Update.Parameters.AddWithValue("jabatan", txtNama.Text);

            if (txtID.Text == "" || txtNama.Text == "")
            {
                MessageBox.Show("Semua Data Harus diisi !!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                try
                {
                    myConnection.Open();
                    Update.ExecuteNonQuery();
                    MessageBox.Show("Jabatan Update Succesfully", "Update Jabatan", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    clear();
                }
                catch (Exception )
                {
                    MessageBox.Show("Unable to save ", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void btnHapus_Click_1(object sender, EventArgs e)
        {
            SqlConnection myConnection = connection.Getcon();

            if (txtNama.Text == "")
            {
                MessageBox.Show("Semua Data Harus diisi !!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                if (MessageBox.Show("Lanjut ingin Menghapus?", "Delete Jabatan", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    //delete command
                    SqlCommand delete = new SqlCommand("sp_DeleteJabatan", myConnection);
                    delete.CommandType = CommandType.StoredProcedure;

                    delete.Parameters.AddWithValue("idJabatan", txtID.Text);

                    try
                    {
                        myConnection.Open();
                        delete.ExecuteNonQuery();
                        MessageBox.Show("Jabatan Delete Succesfully", "Delete Jabatan", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        clear();
                    }
                    catch (Exception )
                    {
                        MessageBox.Show("Unable to save ", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
        }

        private void FormJabatan_Load(object sender, EventArgs e)
        {
            txtID.Text = IDOtomatis();
            LoadData();
        }

        void LoadData()
        {
            try
            {
                SqlConnection con = connection.Getcon();
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM jabatan", con);
                SqlDataReader dr = cmd.ExecuteReader();
                SqlDataReader sqlDataReader = dr;
                DataTable dataTable = new DataTable();
                dataTable.Columns.Add("ID");
                dataTable.Columns.Add("Jabatan");
                while (dr.Read())
                {
                    DataRow row = dataTable.NewRow();
                    row["ID"] = sqlDataReader["idJabatan"];
                    row["Jabatan"] = sqlDataReader["jabatan"];
                    dataTable.Rows.Add(row);
                }
                //this.dataGridView1.DataSource = con.table;
                dgJabatan.DataSource = dataTable;
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Occured" + ex.Message);
            }
        }

        private void btnBatal_Click_1(object sender, EventArgs e)
        {
            clear();
        }

        public void clear()
        {
            txtNama.Clear();
            txtCariJabatan.Clear();
            txtID.Text = IDOtomatis();
            LoadData();
        }

        private void btnCari_Click(object sender, EventArgs e)
        {
            var st = from s in dc.jabatans where s.idJabatan == txtID.Text select s;
            dgJabatan.DataSource = st;

            if (txtCariJabatan.Text == "")
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
                    SqlCommand cmd = new SqlCommand("SELECT * FROM jabatan WHERE idJabatan='" + txtCariJabatan.Text + "'", myConnection);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);

                    da.Fill(dt);

                    txtID.Text = dt.Rows[0]["idJabatan"].ToString();
                    txtNama.Text = dt.Rows[0]["jabatan"].ToString();

                    myConnection.Close();
                }
                catch (Exception ex )
                {
                    MessageBox.Show("Error Data tersebut Tidak ada!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
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
