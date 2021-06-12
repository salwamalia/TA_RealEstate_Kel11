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
        Classes.PROPERTY_TYPE pType = new Classes.PROPERTY_TYPE();

        public FormPegawai()
        {
            InitializeComponent();
        }

        private string IDOtomatis()
        {
            string autoid = null;

            string myConnectionString = @"Data Source=LAPTOP-L1AODT95;Initial Catalog=TARealEstateKel11;Integrated Security=True";
            SqlConnection myConnection = new SqlConnection(myConnectionString);
            myConnection.Open();

            string sqlQuery = "SELECT TOP 1 idPegawai FROM pegawai ORDER BY idPegawai DESC";
            SqlCommand cmd = new SqlCommand(sqlQuery, myConnection);
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                string input = dr["idPegawai"].ToString();
                string angka = input.Substring(input.Length - Math.Min(2, input.Length));
                int number = Convert.ToInt32(angka);
                number += 1;
                string str = number.ToString("D2");

                autoid = "PGW" + str;
            }

            if (autoid == null)
            {
                autoid = "PGW01";
            }

            myConnection.Close();

            return autoid;
        }

        void LoadData()
        {
            dataGridView1.DataSource = pType.getAllPegawai();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MenuAdmin adm = new MenuAdmin();
            adm.Visible = true;
            this.Dispose();
        }

        private void btnSimpan_Click(object sender, EventArgs e)
        {
            string myConnectionString = @"Data Source=LAPTOP-L1AODT95;Initial Catalog=TARealEstateKel11;Integrated Security=True";
            SqlConnection myConnection = new SqlConnection(myConnectionString);

            SqlCommand insert = new SqlCommand("sp_InsertPegawai", myConnection);
            insert.CommandType = CommandType.StoredProcedure;

            insert.Parameters.AddWithValue("idPegawai", txtID.Text);
            insert.Parameters.AddWithValue("nama", txtNama.Text);
            insert.Parameters.AddWithValue("username", txtUsername.Text);
            insert.Parameters.AddWithValue("password", txtPass.Text);
            insert.Parameters.AddWithValue("jabatan", cmbJabatan.SelectedValue.ToString());

            if (txtID.Text == "" || txtNama.Text == "" || txtUsername.Text == "" || txtPass.Text == "" || cmbJabatan.Text == "")
            {
                MessageBox.Show("Data tersebut Harus diisi !!", "Add Pegawai", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                catch (Exception ex)
                {
                    MessageBox.Show("Unable to save " + ex.Message);
                }
            }

            txtID.Text = IDOtomatis();
            LoadData();
        }

        private void btnCari_Click(object sender, EventArgs e)
        {
            try
            {
                string myConnectionString = @"Data Source=LAPTOP-L1AODT95;Initial Catalog=TARealEstateKel11;Integrated Security=True";
                SqlConnection myConnection = new SqlConnection(myConnectionString);
                myConnection.Open();

                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("SELECT * FROM pegawai WHERE idPegawai ='" + txtID.Text + "'", myConnection);
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                da.Fill(dt);

                txtNama.Text = dt.Rows[0]["nama"].ToString();
                txtUsername.Text = dt.Rows[0]["username"].ToString();
                txtPass.Text = dt.Rows[0]["password"].ToString();
                cmbJabatan.SelectedValue = dt.Rows[0]["jabatan"].ToString();

                myConnection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error ! " + ex.Message);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            string myConnectionString = @"Data Source=LAPTOP-L1AODT95;Initial Catalog=TARealEstateKel11;Integrated Security=True";
            SqlConnection myConnection = new SqlConnection(myConnectionString);

            //insert command
            SqlCommand Update = new SqlCommand("sp_UpdatePegawai", myConnection);
            Update.CommandType = CommandType.StoredProcedure;

            Update.Parameters.AddWithValue("idPegawai", txtID.Text);
            Update.Parameters.AddWithValue("nama", txtNama.Text);
            Update.Parameters.AddWithValue("username", txtUsername.Text);
            Update.Parameters.AddWithValue("password", txtPass.Text);
            Update.Parameters.AddWithValue("jabatan", cmbJabatan.SelectedValue.ToString());

            try
            {
                myConnection.Open();
                Update.ExecuteNonQuery();
                MessageBox.Show("Data Save Succesfully", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                clear();
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
            string myConnectionString = @"Data Source=LAPTOP-L1AODT95;Initial Catalog=TARealEstateKel11;Integrated Security=True";
            SqlConnection myConnection = new SqlConnection(myConnectionString);

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
                    MessageBox.Show("Karyawan Delete Succesfully", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    clear();
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
            clear();
        }

        private void clear()
        {
            txtID.Clear();
            txtNama.Clear();
            txtUsername.Clear();
            txtPass.Clear();
            cmbJabatan.SelectedIndex = -1;
        }

        private void FormPegawai_Load(object sender, EventArgs e)
        {
            txtID.Text = IDOtomatis();
        }
    }
}
