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
    public partial class FormPropertyTipe : Form
    {
        public FormPropertyTipe()
        {
            InitializeComponent();
        }

        REALESTATEDataContext dc = new REALESTATEDataContext();
        Classes.PROPERTY_TYPE pType = new Classes.PROPERTY_TYPE();

        private string IDOtomatis()
        {
            string autoid = null;

            string myConnectionString = @"Data Source=LAPTOP-L1AODT95;Initial Catalog=REALESTATE;Integrated Security=True";
            //string myConnectionString = @"Data Source=WINDOWS-LD56BQV;Initial Catalog=REALESTATE;Integrated Security=True";
            SqlConnection myConnection = new SqlConnection(myConnectionString);
            myConnection.Open();

            string sqlQuery = "SELECT TOP 1 idTipe FROM propertyTipe ORDER BY idTipe DESC";
            SqlCommand cmd = new SqlCommand(sqlQuery, myConnection);
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                string input = dr["idTipe"].ToString();
                string angka = input.Substring(input.Length - Math.Min(2, input.Length));
                int number = Convert.ToInt32(angka);
                number += 1;
                string str = number.ToString("D2");

                autoid = "Tp" + str;
            }

            if (autoid == null)
            {
                autoid = "Tp01";
            }

            myConnection.Close();

            return autoid;
        }

        private void btnSimpan_Click_1(object sender, EventArgs e)
        {
            string myConnectionString = @"Data Source=LAPTOP-L1AODT95;Initial Catalog=REALESTATE;Integrated Security=True";
            //string myConnectionString = @"Data Source=WINDOWS-LD56BQV;Initial Catalog=REALESTATE;Integrated Security=True";
            SqlConnection myConnection = new SqlConnection(myConnectionString);

            SqlCommand insert = new SqlCommand("sp_InsertType", myConnection);
            insert.CommandType = CommandType.StoredProcedure;

            insert.Parameters.AddWithValue("idTipe", txtID.Text);
            insert.Parameters.AddWithValue("nama", txtNama.Text);

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

        private void btnUpdate_Click_1(object sender, EventArgs e)
        {
            try
            {
                string id = txtID.Text;
                string name = txtNama.Text;

                if (!name.Trim().Equals(""))
                {
                    if (pType.updateType(id, name))
                    {
                        MessageBox.Show("Type Telah DiUpdate", "Update Type", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtID.Text = IDOtomatis();
                        LoadData();
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

            LoadData();
        }

        private void btnHapus_Click_1(object sender, EventArgs e)
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
                        LoadData();
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

        private void btnBatal_Click_1(object sender, EventArgs e)
        {
            txtID.Clear();
            txtNama.Clear();
        }

        private void btnCari_Click(object sender, EventArgs e)
        {
            var st = from s in dc.propertyTipes where s.idTipe == txtID.Text select s;
            DataGridViewTipe.DataSource = st;

            try
            {
                string myConnectionString = @"Data Source=LAPTOP-L1AODT95;Initial Catalog=REALESTATE;Integrated Security=True";
                //string myConnectionString = @"Data Source=WINDOWS-LD56BQV;Initial Catalog=REALESTATE;Integrated Security=True";
                SqlConnection myConnection = new SqlConnection(myConnectionString);
                myConnection.Open();

                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("SELECT * FROM propertyTipe WHERE idTipe ='" + txtCariTipe.Text + "'", myConnection);
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                da.Fill(dt);

                txtID.Text = dt.Rows[0]["idTipe"].ToString();
                txtNama.Text = dt.Rows[0]["nama"].ToString();

                myConnection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Data tersebut Tidak ada!" + ex);
            }
        }

        private void FormPropertyTipe_Load(object sender, EventArgs e)
        {
            txtID.Text = IDOtomatis();
            LoadData();
        }
        
        void LoadData()
        {
            var st = from tb in dc.propertyTipes select tb;
            DataGridViewTipe.DataSource = st;
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
