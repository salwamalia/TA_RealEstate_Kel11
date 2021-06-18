using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TA_RealEstate_Kel11
{
    public partial class FormCicilan : Form
    {
        public FormCicilan()
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

            string sqlQuery = "SELECT TOP 1 idCicilan FROM kategoriCicilan ORDER BY idCicilan DESC";
            SqlCommand cmd = new SqlCommand(sqlQuery, myConnection);
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                string input = dr["idCicilan"].ToString();
                string angka = input.Substring(input.Length - Math.Min(2, input.Length));
                int number = Convert.ToInt32(angka);
                number += 1;
                string str = number.ToString("D2");

                autoid = "CCL" + str;
            }

            if (autoid == null)
            {
                autoid = "CCL01";
            }

            myConnection.Close();

            return autoid;
        }

        private void btnSimpan_Click_1(object sender, EventArgs e)
        {
            //string myConnectionString = @"Data Source=LAPTOP-L1AODT95;Initial Catalog=REALESTATE;Integrated Security=True";
            string myConnectionString = @"Data Source=WINDOWS-LD56BQV;Initial Catalog=REALESTATE;Integrated Security=True";
            SqlConnection myConnection = new SqlConnection(myConnectionString);

            SqlCommand insert = new SqlCommand("sp_InsertCicilan", myConnection);
            insert.CommandType = CommandType.StoredProcedure;

            insert.Parameters.AddWithValue("idCicilan", txtID.Text);
            insert.Parameters.AddWithValue("jenisCicilan", txtNama.Text);
            insert.Parameters.AddWithValue("harga", txtHarga.Text);
            insert.Parameters.AddWithValue("keterangan", txtKet.Text);

            if (txtNama.Text == "" && txtHarga.Text == "")
            {
                MessageBox.Show("Harus diisi !!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                try
                {
                    myConnection.Open();
                    insert.ExecuteNonQuery();
                    MessageBox.Show("Cicilan Telah Ditambahkan", "Add Cicilan", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            SqlCommand Update = new SqlCommand("sp_UpdateCicilan", myConnection);
            Update.CommandType = CommandType.StoredProcedure;

            Update.Parameters.AddWithValue("idCicilan", txtID.Text);
            Update.Parameters.AddWithValue("jenisCicilan", txtNama.Text);
            Update.Parameters.AddWithValue("harga", txtHarga.Text);
            Update.Parameters.AddWithValue("keterangan", txtKet.Text);

            try
            {
                myConnection.Open();
                Update.ExecuteNonQuery();
                MessageBox.Show("Cicilan Update Succesfully", "Update Cicilan", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

                if (MessageBox.Show("Lanjut ingin Menghapus?", "Delete Cicilan", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (kategori.deleteCicilan(id))
                    {
                        MessageBox.Show("Cicilan Telah DiHapus", "Delete Cicilan", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        clear();
                        txtID.Text = IDOtomatis();
                        LoadData();
                    }
                    else
                    {
                        MessageBox.Show("Cicilan Tidak DiHapus", "Delete Cicilan", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch
            {
                MessageBox.Show("Tidak Ada Cicilan yang dipilih", "Delete Cicilan", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void clear()
        {
            txtID.Clear();
            txtNama.Clear();
            txtHarga.Clear();
            txtKet.Clear();
        }

        private void btnBatal_Click_1(object sender, EventArgs e)
        {
            clear();
            txtID.Text = IDOtomatis();
        }

        private void btnCari_Click_1(object sender, EventArgs e)
        {
            var st = from s in dc.kategoriCicilans where s.idCicilan == txtCariCicilan.Text select s;
            dgCicilan.DataSource = st;

            try
            {
                //string myConnectionString = @"Data Source=LAPTOP-L1AODT95;Initial Catalog=REALESTATE;Integrated Security=True";
                string myConnectionString = @"Data Source=WINDOWS-LD56BQV;Initial Catalog=REALESTATE;Integrated Security=True";
                SqlConnection myConnection = new SqlConnection(myConnectionString);
                myConnection.Open();

                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("SELECT * FROM kategoriCicilan WHERE idCicilan ='" + txtCariCicilan.Text + "'", myConnection);
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                da.Fill(dt);

                txtID.Text = dt.Rows[0]["idCicilan"].ToString();
                txtNama.Text = dt.Rows[0]["jenisCicilan"].ToString();
                txtHarga.Text = dt.Rows[0]["harga"].ToString();
                txtKet.Text = dt.Rows[0]["keterangan"].ToString();

                myConnection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Data tersebut Tidak ada!" + ex);
            }

            txtCariCicilan.Clear();
        }

        void LoadData()
        {
            var st = from tb in dc.kategoriCicilans select tb;
            dgCicilan.DataSource = st;
        }

        private void FormCicilan_Load(object sender, EventArgs e)
        {
            txtID.Text = IDOtomatis();
            LoadData();
        }

        private void btnX_Click(object sender, EventArgs e)
        {
            Application.Exit();
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
