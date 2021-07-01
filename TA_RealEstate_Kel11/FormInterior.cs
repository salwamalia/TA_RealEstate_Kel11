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
    public partial class FormInterior : Form
    {
        public FormInterior()
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

            string sqlQuery = "SELECT TOP (1) MAX(RIGHT (idInterior,2))+1 AS idInterior FROM desainInterior";
            SqlCommand cmd = new SqlCommand(sqlQuery, myConnection);
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                if (dr["idInterior"].ToString() == "")
                {
                    autoid = 1;
                }
                else
                {
                    autoid = Int32.Parse(dr["idInterior"].ToString());
                }
            }

            if (autoid < 10)
            {
                kode = "IR00" + autoid;
            }
            else if (autoid < 100)
            {
                kode = "IR" + autoid;
            }

            myConnection.Close();

            return kode;
        }

        private void btnSimpan_Click_1(object sender, EventArgs e)
        {
            SqlConnection myConnection = connection.Getcon();

            SqlCommand insert = new SqlCommand("sp_InsertInterior", myConnection);
            insert.CommandType = CommandType.StoredProcedure;

            insert.Parameters.AddWithValue("idInterior", txtID.Text);
            insert.Parameters.AddWithValue("interior", txtNama.Text);

            if (txtNama.Text == "")
            {
                MessageBox.Show("Semua Data Harus diisi !!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                try
                {
                    myConnection.Open();
                    insert.ExecuteNonQuery();
                    MessageBox.Show("Interior Telah Ditambahkan", "Add Interior", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    clear();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Unable to save ", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

            txtID.Text = IDOtomatis();
            LoadData();
        }

        private void btnUpdate_Click_1(object sender, EventArgs e)
        {
            SqlConnection myConnection = connection.Getcon();

            //update command
            SqlCommand Update = new SqlCommand("sp_UpdateInterior", myConnection);
            Update.CommandType = CommandType.StoredProcedure;

            Update.Parameters.AddWithValue("idInterior", txtID.Text);
            Update.Parameters.AddWithValue("interior", txtNama.Text);

            if (txtNama.Text == "")
            {
                MessageBox.Show("Semua Data Harus diisi !!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                try
                {
                    myConnection.Open();
                    Update.ExecuteNonQuery();
                    MessageBox.Show("Interior Update Succesfully", "Update Interior", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

            if (txtNama.Text == "")
            {
                MessageBox.Show("Semua Data Harus diisi !!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                if (MessageBox.Show("Lanjut ingin Menghapus?", "Delete Interior", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    //delete command
                    SqlCommand delete = new SqlCommand("sp_DeleteInterior", myConnection);
                    delete.CommandType = CommandType.StoredProcedure;

                    delete.Parameters.AddWithValue("idInterior", txtID.Text);

                    try
                    {
                        myConnection.Open();
                        delete.ExecuteNonQuery();
                        MessageBox.Show("Interior Delete Succesfully", "Delete Interior", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        clear();
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Unable to save ", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
        }

        private void btnBatal_Click(object sender, EventArgs e)
        {
            clear();
        }

        private void clear()
        {
            txtCariInterior.Clear();
            txtID.Clear();
            txtNama.Clear();
            txtID.Text = IDOtomatis();
            LoadData();
        }

        private void btnCari_Click_1(object sender, EventArgs e)
        {
            var st = from s in dc.desainInteriors where s.idInterior == txtCariInterior.Text select s;
            dgInterior.DataSource = st;

            if (txtCariInterior.Text == "")
            {
                MessageBox.Show("Semua Data Harus diisi !!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                try
                {
                    SqlConnection myConnection = connection.Getcon();
                    myConnection.Open();

                    DataTable dt = new DataTable();
                    SqlCommand cmd = new SqlCommand("SELECT * FROM desainInterior WHERE idInterior ='" + txtCariInterior.Text + "'", myConnection);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);

                    da.Fill(dt);

                    txtID.Text = dt.Rows[0]["idInterior"].ToString();
                    txtNama.Text = dt.Rows[0]["interior"].ToString();

                    myConnection.Close();
                }
                catch (Exception )
                {
                    MessageBox.Show("Error Data tersebut Tidak ada!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        void LoadData()
        {
            try
            {
                SqlConnection con = connection.Getcon();
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM desainInterior", con);
                SqlDataReader dr = cmd.ExecuteReader();
                SqlDataReader sqlDataReader = dr;
                DataTable dataTable = new DataTable();
                dataTable.Columns.Add("ID");
                dataTable.Columns.Add("Interior");
                while (dr.Read())
                {
                    DataRow row = dataTable.NewRow();
                    row["ID"] = sqlDataReader["idInterior"];
                    row["Interior"] = sqlDataReader["interior"];
                    dataTable.Rows.Add(row);
                }
                //this.dataGridView1.DataSource = con.table;
                dgInterior.DataSource = dataTable;
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

        private void FormKategoriBayar_Load(object sender, EventArgs e)
        {
            txtID.Text = IDOtomatis();
            LoadData();
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
