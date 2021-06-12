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
    public partial class FormOwner : Form
    {
        public FormOwner()
        {
            InitializeComponent();
        }

        Classes.PERSON person;

        private string IDOtomatis()
        {
            string autoid = null;

            string myConnectionString = @"Data Source=LAPTOP-L1AODT95;Initial Catalog=TARealEstateKel11;Integrated Security=True";
            SqlConnection myConnection = new SqlConnection(myConnectionString);
            myConnection.Open();

            string sqlQuery = "SELECT TOP 1 idPemilik FROM pemilik ORDER BY idPemilik DESC";
            SqlCommand cmd = new SqlCommand(sqlQuery, myConnection);
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                string input = dr["idPemilik"].ToString();
                string angka = input.Substring(input.Length - Math.Min(2, input.Length));
                int number = Convert.ToInt32(angka);
                number += 1;
                string str = number.ToString("D2");

                autoid = "PML" + str;
            }

            if (autoid == null)
            {
                autoid = "PML01";
            }

            myConnection.Close();

            return autoid;
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

            SqlCommand insert = new SqlCommand("sp_InsertPemilik", myConnection);
            insert.CommandType = CommandType.StoredProcedure;


            insert.Parameters.AddWithValue("idPemilik", txtID.Text);
            insert.Parameters.AddWithValue("nama", txtNama.Text);
            insert.Parameters.AddWithValue("telepon", txtPhone.Text);
            insert.Parameters.AddWithValue("email", txtEmail.Text);
            insert.Parameters.AddWithValue("alamat", txtAlamat.Text);


            if (txtNama.Text == "" || txtPhone.Text == "")
            {
                MessageBox.Show("Harus diisi !!");
            }
            else
            {
                try
                {
                    myConnection.Open();
                    insert.ExecuteNonQuery();
                    MessageBox.Show("Type Telah Ditambahkan", "Add Type", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    clear();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Unable to save " + ex.Message);
                }
            }

            txtID.Text = IDOtomatis();

            /*string id = txtID.Text = IDOtomatis();
            string name = txtNama.Text;
            string phone = txtPhone.Text;
            string email = txtEmail.Text;
            string address = txtAddress.Text;

            person = new Classes.PERSON(id, name, phone, email, address);

            if (verifTextBoxes())
            {
                if (person.insertPerson("Owner", person))
                {
                    MessageBox.Show("Owner Telah DiTambahkan", "Add Owner", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Owner Tidak DiTambahkan", "Add Owner", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("Silahkan Masukkan Data Owner", "Add Owner", MessageBoxButtons.OK, MessageBoxIcon.Error);
            } */
        }

        public bool verifTextBoxes()
        {
            string name = txtNama.Text.Trim();
            string phone = txtPhone.Text.Trim();

            if (name.Equals("") || phone.Equals(""))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private void btnCari_Click(object sender, EventArgs e)
        {
            try
            {
                string myConnectionString = @"Data Source=LAPTOP-L1AODT95;Initial Catalog=TARealEstateKel11;Integrated Security=True";
                SqlConnection myConnection = new SqlConnection(myConnectionString);
                myConnection.Open();

                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Pemilik WHERE idPemilik ='" + txtID.Text + "'", myConnection);
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                da.Fill(dt);

                txtNama.Text = dt.Rows[0]["nama"].ToString();
                txtPhone.Text = dt.Rows[0]["telepon"].ToString();
                txtEmail.Text = dt.Rows[0]["email"].ToString();
                txtAlamat.Text = dt.Rows[0]["alamat"].ToString();

                myConnection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error ! " + ex.Message);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {

        }

        private void btnHapus_Click(object sender, EventArgs e)
        {

        }

        private void btnBatal_Click(object sender, EventArgs e)
        {
            clear();
        }

        private void clear()
        {
            txtID.Clear();
            txtNama.Clear();
            txtPhone.Clear();
            txtEmail.Clear();
            txtAlamat.Clear();
        }

        private void FormOwner_Load(object sender, EventArgs e)
        {
            txtID.Text = IDOtomatis();
        }
    }
}
