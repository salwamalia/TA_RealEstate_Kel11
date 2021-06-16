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

        REALESTATEDataContext dc = new REALESTATEDataContext();

        private string IDOtomatis()
        {
            string autoid = null;

            string myConnectionString = @"Data Source=LAPTOP-L1AODT95;Initial Catalog=REALESTATE;Integrated Security=True";
            SqlConnection myConnection = new SqlConnection(myConnectionString);
            myConnection.Open();

            string sqlQuery = "SELECT TOP 1 idJabatan FROM jabatan ORDER BY idJabatan DESC";
            SqlCommand cmd = new SqlCommand(sqlQuery, myConnection);
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                string input = dr["idJabatan"].ToString();
                string angka = input.Substring(input.Length - Math.Min(2, input.Length));
                int number = Convert.ToInt32(angka);
                number += 1;
                string str = number.ToString("D2");

                autoid = "JBT" + str;
            }

            if (autoid == null)
            {
                autoid = "JBT01";
            }

            myConnection.Close();

            return autoid;
        }

        private void btnSimpan_Click(object sender, EventArgs e)
        {
            string myConnectionString = @"Data Source=LAPTOP-L1AODT95;Initial Catalog=REALESTATE;Integrated Security=True";
            SqlConnection myConnection = new SqlConnection(myConnectionString);

            SqlCommand insert = new SqlCommand("sp_InputJabatan", myConnection);
            insert.CommandType = CommandType.StoredProcedure;

            insert.Parameters.AddWithValue("idJabatan", txtID.Text);
            insert.Parameters.AddWithValue("jabatan", txtNama.Text);

            if (txtID.Text == "" || txtNama.Text == "")
            {
                MessageBox.Show("Data tersebut Harus diisi !!");
            }
            else
            {
                try
                {
                    myConnection.Open();
                    insert.ExecuteNonQuery();
                    MessageBox.Show("Jabatan Telah Ditambahkan", "Add Jabatan", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtID.Clear();
                    txtNama.Clear();
                    LoadData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Unable to save " + ex.Message);
                }
            }
            txtID.Text = IDOtomatis();
            //LoadData();
        }

        private void FormJabatan_Load(object sender, EventArgs e)
        {
            txtID.Text = IDOtomatis();
            LoadData();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            string myConnectionString = @"Data Source=LAPTOP-L1AODT95;Initial Catalog=REALESTATE;Integrated Security=True";
            SqlConnection myConnection = new SqlConnection(myConnectionString);

            //update command
            SqlCommand Update = new SqlCommand("sp_UpdateJabatan", myConnection);
            Update.CommandType = CommandType.StoredProcedure;

            Update.Parameters.AddWithValue("idJabatan", txtID.Text);
            Update.Parameters.AddWithValue("jabatan", txtNama.Text);

            try
            {
                myConnection.Open();
                Update.ExecuteNonQuery();
                MessageBox.Show("Jabatan Update Succesfully", "Update Jabatan", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtID.Clear();
                txtNama.Clear();
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to save " + ex.Message);
            }

            txtID.Text = IDOtomatis();
            LoadData();
        }

        private void btnHapus_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Lanjut ingin Menghapus?", "Delete Pegawai", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                string myConnectionString = @"Data Source=LAPTOP-L1AODT95;Initial Catalog=REALESTATE;Integrated Security=True";
                SqlConnection myConnection = new SqlConnection(myConnectionString);

                //delete command
                SqlCommand delete = new SqlCommand("sp_DeleteJabatan", myConnection);
                delete.CommandType = CommandType.StoredProcedure;

                delete.Parameters.AddWithValue("idJabatan", txtID.Text);

                try
                {
                    myConnection.Open();
                    delete.ExecuteNonQuery();
                    MessageBox.Show("Jabatan Delete Succesfully", "Delete Jabatan", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtID.Clear();
                    txtNama.Clear();
                    LoadData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Unable to save " + ex.Message);
                }
            }

            txtID.Text = IDOtomatis();
            LoadData();
        }

        private void btnBatal_Click(object sender, EventArgs e)
        {
            txtID.Clear();
            txtNama.Clear();
        }

        private void btnCari_Click(object sender, EventArgs e)
        {
            var st = from s in dc.jabatans where s.idJabatan == txtID.Text select s;
            DataGridViewJabatan.DataSource = st;

            try
            {
                string myConnectionString = @"Data Source=LAPTOP-L1AODT95;Initial Catalog=REALESTATE;Integrated Security=True";
                SqlConnection myConnection = new SqlConnection(myConnectionString);
                myConnection.Open();

                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("SELECT * FROM jabatan WHERE idJabatan='" + txtID.Text + "'", myConnection);
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                da.Fill(dt);

                txtNama.Text = dt.Rows[0]["jabatan"].ToString();

                myConnection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Data tersebut Tidak ada!");
            }
        }

        void LoadData()
        {
            var st = from tb in dc.jabatans select tb;
            DataGridViewJabatan.DataSource = st;
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
            FormKategoriBayar byr = new FormKategoriBayar();
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
    }
}
