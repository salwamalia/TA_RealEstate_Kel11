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
    public partial class FormKategoriBayar : Form
    {
        public FormKategoriBayar()
        {
            InitializeComponent();
        }

        REALESTATEDataContext dc = new REALESTATEDataContext();
        Classes.KATEGORI kategori = new Classes.KATEGORI();

        private string IDOtomatis()
        {
            string autoid = null;

            //string myConnectionString = @"Data Source=LAPTOP-L1AODT95;Initial Catalog=REALESTATE;Integrated Security=True";
            string myConnectionString = @"Data Source=WINDOWS-LD56BQV;Initial Catalog=REALESTATE;Integrated Security=True";
            SqlConnection myConnection = new SqlConnection(myConnectionString);
            myConnection.Open();

            string sqlQuery = "SELECT TOP 1 idKategoriBayar FROM kategoriBayar ORDER BY idKategoriBayar DESC";
            SqlCommand cmd = new SqlCommand(sqlQuery, myConnection);
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                string input = dr["idKategoriBayar"].ToString();
                string angka = input.Substring(input.Length - Math.Min(2, input.Length));
                int number = Convert.ToInt32(angka);
                number += 1;
                string str = number.ToString("D2");

                autoid = "KTB" + str;
            }

            if (autoid == null)
            {
                autoid = "KTB01";
            }

            myConnection.Close();

            return autoid;
        }

        private void btnSimpan_Click_1(object sender, EventArgs e)
        {
            //string myConnectionString = @"Data Source=LAPTOP-L1AODT95;Initial Catalog=REALESTATE;Integrated Security=True";
            string myConnectionString = @"Data Source=WINDOWS-LD56BQV;Initial Catalog=REALESTATE;Integrated Security=True";
            SqlConnection myConnection = new SqlConnection(myConnectionString);

            SqlCommand insert = new SqlCommand("sp_InsertKategori", myConnection);
            insert.CommandType = CommandType.StoredProcedure;

            insert.Parameters.AddWithValue("idKategoriBayar", txtID.Text);
            insert.Parameters.AddWithValue("kategoriBayar", txtNama.Text);
            insert.Parameters.AddWithValue("keterangan", txtKeterangan.Text);

            if (txtNama.Text == "")
            {
                MessageBox.Show("Harus diisi !!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                try
                {
                    myConnection.Open();
                    insert.ExecuteNonQuery();
                    MessageBox.Show("Kategori Telah Ditambahkan", "Add Kategori", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    clear();
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
            //string myConnectionString = @"Data Source=LAPTOP-L1AODT95;Initial Catalog=REALESTATE;Integrated Security=True";
            string myConnectionString = @"Data Source=WINDOWS-LD56BQV;Initial Catalog=REALESTATE;Integrated Security=True";
            SqlConnection myConnection = new SqlConnection(myConnectionString);

            //update command
            SqlCommand Update = new SqlCommand("sp_UpdateKategori", myConnection);
            Update.CommandType = CommandType.StoredProcedure;

            Update.Parameters.AddWithValue("idKategoriBayar", txtID.Text);
            Update.Parameters.AddWithValue("kategoriBayar", txtNama.Text);
            Update.Parameters.AddWithValue("keterangan", txtKeterangan.Text);

            try
            {
                myConnection.Open();
                Update.ExecuteNonQuery();
                MessageBox.Show("Kategori Update Succesfully", "Update Kategori", MessageBoxButtons.OK, MessageBoxIcon.Information);
                clear();
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to save " + ex);
            }

            txtID.Text = IDOtomatis();
            LoadData();
        }

        private void btnHapus_Click_1(object sender, EventArgs e)
        {
            try
            {
                string id = txtID.Text;

                if (MessageBox.Show("Lanjut ingin Menghapus?", "Delete Kategori", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (kategori.deleteKategori(id))
                    {
                        MessageBox.Show("Kategori Telah DiHapus", "Delete Kategori", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        clear();
                        txtID.Text = IDOtomatis();
                        LoadData();
                    }
                    else
                    {
                        MessageBox.Show("Kategori Tidak DiHapus", "Delete Kategori", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch
            {
                MessageBox.Show("Tidak Ada Kategori yang dipilih", "Delete Kategori", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnBatal_Click_1(object sender, EventArgs e)
        {
            clear();
            txtID.Text = IDOtomatis();
            LoadData();
        }

        private void clear()
        {
            txtCariKategori.Clear();
            txtID.Clear();
            txtNama.Clear();
            txtKeterangan.Clear();
        }

        private void btnCari_Click_1(object sender, EventArgs e)
        {
            var st = from s in dc.kategoriBayars where s.idKategoriBayar == txtCariKategori.Text select s;
            dgKategoriBayar.DataSource = st;

            try
            {
                //string myConnectionString = @"Data Source=LAPTOP-L1AODT95;Initial Catalog=REALESTATE;Integrated Security=True";
                string myConnectionString = @"Data Source=WINDOWS-LD56BQV;Initial Catalog=REALESTATE;Integrated Security=True";
                SqlConnection myConnection = new SqlConnection(myConnectionString);
                myConnection.Open();

                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("SELECT * FROM kategoriBayar WHERE idKategoriBayar ='" + txtCariKategori.Text + "'", myConnection);
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                da.Fill(dt);

                txtID.Text = dt.Rows[0]["idKategoriBayar"].ToString();
                txtNama.Text = dt.Rows[0]["kategoriBayar"].ToString();
                txtKeterangan.Text = dt.Rows[0]["keterangan"].ToString();

                myConnection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Data tersebut Tidak ada!" + ex);
            }

            txtCariKategori.Clear();
        }

        void LoadData()
        {
            var st = from tb in dc.kategoriBayars select tb;
            dgKategoriBayar.DataSource = st;
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
