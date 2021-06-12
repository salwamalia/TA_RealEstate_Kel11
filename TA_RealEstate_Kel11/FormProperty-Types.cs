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
    public partial class FormPropertyTypes : Form
    {
        public FormPropertyTypes()
        {
            InitializeComponent();
        }

        Classes.PROPERTY_TYPE pType = new Classes.PROPERTY_TYPE();

        private void button1_Click(object sender, EventArgs e)
        {
            MenuAdmin adm = new MenuAdmin();
            adm.Visible = true;
            this.Dispose();
        }

        private string IDOtomatis()
        {
            string autoid = null;

            string myConnectionString = @"Data Source=LAPTOP-L1AODT95;Initial Catalog=TARealEstateKel11;Integrated Security=True";
            SqlConnection myConnection = new SqlConnection(myConnectionString);
            myConnection.Open();

            string sqlQuery = "SELECT TOP 1 idTipe FROM propertyType ORDER BY idTipe DESC";
            SqlCommand cmd = new SqlCommand(sqlQuery, myConnection);
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                string input = dr["idTipe"].ToString();
                string angka = input.Substring(input.Length - Math.Min(2, input.Length));
                int number = Convert.ToInt32(angka);
                number += 1;
                string str = number.ToString("D2");

                autoid = "Tipe" + str;
            }

            if (autoid == null)
            {
                autoid = "Tipe01";
            }

            myConnection.Close();

            return autoid;
        }

        private void Simpan_Click(object sender, EventArgs e)
        {
            string myConnectionString = @"Data Source=LAPTOP-L1AODT95;Initial Catalog=TARealEstateKel11;Integrated Security=True";
            SqlConnection myConnection = new SqlConnection(myConnectionString);

            SqlCommand insert = new SqlCommand("sp_InsertType", myConnection);
            insert.CommandType = CommandType.StoredProcedure;


            insert.Parameters.AddWithValue("idTipe", txtID.Text);
            insert.Parameters.AddWithValue("nama", txtNama.Text);
            insert.Parameters.AddWithValue("deskripsi", txtDeskripsi.Text);

            if (txtNama.Text == "")
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
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                string id = txtID.Text;
                string name = txtNama.Text;
                string description = txtDeskripsi.Text;

                if (!name.Trim().Equals(""))
                {
                    if (pType.updateType(id, name, description))
                    {
                        MessageBox.Show("Type Telah DiUpdate", "Update Type", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Type Tidak Ter-Update", "Update Type", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    MessageBox.Show("Silahkan Ubah Type untuk DiUpdate", "Update Type", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch
            {
                MessageBox.Show("Tidak Ada Type yang dipilih", "Update Type", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnHapus_Click(object sender, EventArgs e)
        {
            try
            {
                string id = txtID.Text;

                if (MessageBox.Show("Lanjut ingin Menghapus?", "Delete Type", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (pType.deleteType(id))
                    {
                        MessageBox.Show("Type Telah DiHapus", "Delete Type", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtID.Text = IDOtomatis();
                    }
                    else
                    {
                        MessageBox.Show("Type Tidak DiHapus", "Delete Type", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch
            {
                MessageBox.Show("Tidak Ada Type yang dipilih", "Delete Type", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnBatal_Click(object sender, EventArgs e)
        {
            clear();
        }

        private void clear()
        {
            txtID.Clear();
            txtNama.Clear();
            txtDeskripsi.Clear();
        }

        public void displayTypesCount()
        {
            // labelCount.Text = ListBoxTypes.Items.Count + "Type(s)";
        }

        private void FormPropertyTypes_Load(object sender, EventArgs e)
        {
            txtID.Text = IDOtomatis();
        }
    }
}
