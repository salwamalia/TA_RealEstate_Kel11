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
        Classes.PROPERTY_TYPE pType = new Classes.PROPERTY_TYPE();

        public FormJabatan()
        {
            InitializeComponent();
        }

        private string IDOtomatis()
        {
            string autoid = null;

            string myConnectionString = @"Data Source=LAPTOP-L1AODT95;Initial Catalog=TARealEstateKel11;Integrated Security=True";
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

        void LoadData()
        {
            dataGridView1.DataSource = pType.getAlljabatan();
        }

        private void btnSimpan_Click(object sender, EventArgs e)
        {
            string myConnectionString = @"Data Source=LAPTOP-L1AODT95;Initial Catalog=TARealEstateKel11;Integrated Security=True";
            SqlConnection myConnection = new SqlConnection(myConnectionString);

            SqlCommand insert = new SqlCommand("sp_InsertJabatan", myConnection);
            insert.CommandType = CommandType.StoredProcedure;

            insert.Parameters.AddWithValue("idJabatan", txtID.Text);
            //insert.Parameters.AddWithValue("Jabatan", txtNama.Text);

            if (txtID.Text == "" /* || txtNama.Text == ""*/)
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
                SqlCommand cmd = new SqlCommand("SELECT * FROM jabatan WHERE idJabatan ='" + txtID.Text + "'", myConnection);
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                da.Fill(dt);

                //txtNama.Text = dt.Rows[0]["jabatan"].ToString();

                myConnection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error ! " + ex.Message);
            }

            LoadData();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            string myConnectionString = @"Data Source=LAPTOP-L1AODT95;Initial Catalog=TARealEstateKel11;Integrated Security=True";
            SqlConnection myConnection = new SqlConnection(myConnectionString);

            //insert command
            SqlCommand Update = new SqlCommand("sp_UpdateJabatan", myConnection);
            Update.CommandType = CommandType.StoredProcedure;

            Update.Parameters.AddWithValue("idJabatan", txtID.Text);
            //Update.Parameters.AddWithValue("jabatan", txtNama.Text);

            try
            {
                myConnection.Open();
                Update.ExecuteNonQuery();
                MessageBox.Show("Jabatan Update Succesfully", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

            //insert command
            SqlCommand delete = new SqlCommand("sp_DeleteJabatan", myConnection);
            delete.CommandType = CommandType.StoredProcedure;

            delete.Parameters.AddWithValue("idJabatan", txtID.Text);

            try
            {
                myConnection.Open();
                delete.ExecuteNonQuery();
                MessageBox.Show("Jabatan Delete Succesfully", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to save " + ex.Message);
            }

            txtID.Text = IDOtomatis();
            LoadData();
        }

        private void btnBatal_Click(object sender, EventArgs e)
        {
            clear();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            MenuAdmin adm = new MenuAdmin();
            adm.Visible = true;
            this.Dispose();
        }

        private void clear()
        {
            txtID.Clear();
            //txtNama.Clear();
        }

        private void FormJabatan_Load(object sender, EventArgs e)
        {
            txtID.Text = IDOtomatis();
        }
    }
}
