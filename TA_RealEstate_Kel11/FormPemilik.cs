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
using System.Text.RegularExpressions;

namespace TA_RealEstate_Kel11
{
    public partial class FormPemilik : Form
    {
        public FormPemilik()
        {
            InitializeComponent();
        }

        REALESTATEDataContext dc = new REALESTATEDataContext();
        Classes.PERSON person = new Classes.PERSON();

        private string IDOtomatis()
        {
            string autoid = null;

            //string myConnectionString = @"Data Source=LAPTOP-L1AODT95;Initial Catalog=REALESTATE;Integrated Security=True";
            string myConnectionString = @"Data Source=WINDOWS-LD56BQV;Initial Catalog=REALESTATE;Integrated Security=True";
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

        private void btnSimpan_Click_1(object sender, EventArgs e)
        {
            string jeniskelamin = null;
            if (rbLaki.Checked)
            {
                jeniskelamin = rbLaki.Text;
            }
            if (rbPerempuan.Checked)
            {
                jeniskelamin = rbPerempuan.Text;
            }

            //string myConnectionString = @"Data Source=LAPTOP-L1AODT95;Initial Catalog=REALESTATE;Integrated Security=True";
            string myConnectionString = @"Data Source=WINDOWS-LD56BQV;Initial Catalog=REALESTATE;Integrated Security=True";
            SqlConnection myConnection = new SqlConnection(myConnectionString);

            SqlCommand insert = new SqlCommand("sp_InsertPemilik", myConnection);
            insert.CommandType = CommandType.StoredProcedure;

            insert.Parameters.AddWithValue("idPemilik", txtID.Text);
            insert.Parameters.AddWithValue("nama", txtNama.Text);
            insert.Parameters.AddWithValue("jeniskelamin", jeniskelamin);
            insert.Parameters.AddWithValue("telepon", txtTelepon.Text);
            insert.Parameters.AddWithValue("email", txtEmail.Text);
            insert.Parameters.AddWithValue("alamat", txtAlamat.Text);

            if (txtID.Text == "" || txtNama.Text == "" || txtEmail.Text == "" || txtTelepon.Text == "" || txtAlamat.Text == "" || jeniskelamin == "")
            {
                MessageBox.Show("Semua Data tersebut Harus diisi !!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (!(Regex.IsMatch(txtEmail.Text, @"^^[^@\s]+@[^@\s]+(\.[^@\s]+)+$")))
            {
                MessageBox.Show("Format Email Salah !!" +
                    "\nGunakan Format a@b.c", "Information Email", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                try
                {
                    myConnection.Open();
                    insert.ExecuteNonQuery();
                    MessageBox.Show("Pemilik Telah Ditambahkan", "Add Pemilik", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void btnUpdate_Click_1(object sender, EventArgs e)
        {
            string jeniskelamin = null;
            if (rbLaki.Checked)
            {
                jeniskelamin = rbLaki.Text;
            }
            if (rbPerempuan.Checked)
            {
                jeniskelamin = rbPerempuan.Text;
            }

            try
            {
                string id = txtID.Text;
                string name = txtNama.Text;
                string JenisKelamin = jeniskelamin;
                string phone = txtTelepon.Text;
                string email = txtEmail.Text;
                string address = txtAlamat.Text;

                if (!name.Trim().Equals("") && !phone.Trim().Equals(""))
                {
                    if (person.updatePemilik(id, name, JenisKelamin, phone, email, address))
                    {
                        MessageBox.Show("pemilik Telah DiUpdate", "Update pemilik", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        clear();
                        txtID.Text = IDOtomatis();
                        LoadData();
                    }
                    else
                    {
                        MessageBox.Show("pemilik Tidak Ter-Update", "Update pemilik", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    MessageBox.Show("Silahkan Isi Nama dan Telepon pemilik untuk DiUpdate", "Update pemilik", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch
            {
                MessageBox.Show("Tidak Ada pemilik yang dipilih", "Update pemilik", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            LoadData();
        }

        private void btnHapus_Click_1(object sender, EventArgs e)
        {
            try
            {
                string id = txtID.Text;

                if (MessageBox.Show("Lanjut ingin Menghapus?", "Delete pemilik", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (person.deletePemilik(id))
                    {
                        MessageBox.Show("pemilik Telah DiHapus", "Delete pemilik", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtID.Text = IDOtomatis();
                        LoadData();
                    }
                    else
                    {
                        MessageBox.Show("pemilik Tidak DiHapus", "Delete pemilik", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch
            {
                MessageBox.Show("Tidak Ada pemilik yang dipilih untuk dihapus", "Delete pemilik", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnBatal_Click_1(object sender, EventArgs e)
        {
            clear();
            txtID.Text = IDOtomatis();
        }

        private void clear()
        {
            txtID.Clear();
            txtCariPemilik.Clear();
            txtNama.Clear();
            txtEmail.Clear();
            txtTelepon.Clear();
            txtAlamat.Clear();
            rbLaki.Checked = false;
            rbPerempuan.Checked = false;
        }

        private void btnCari_Click_1(object sender, EventArgs e)
        {
            var st = from s in dc.pemiliks where s.idPemilik == txtCariPemilik.Text select s;
            dgPemilik.DataSource = st;

            string jeniskelamin = null;

            try
            {
                //string myConnectionString = @"Data Source=LAPTOP-L1AODT95;Initial Catalog=REALESTATE;Integrated Security=True";
                string myConnectionString = @"Data Source=WINDOWS-LD56BQV;Initial Catalog=REALESTATE;Integrated Security=True";
                SqlConnection myConnection = new SqlConnection(myConnectionString);
                myConnection.Open();

                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("SELECT * FROM pemilik WHERE idPemilik ='" + txtCariPemilik.Text + "'", myConnection);
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                da.Fill(dt);

                txtID.Text = dt.Rows[0]["idPemilik"].ToString();
                txtNama.Text = dt.Rows[0]["nama"].ToString();
                jeniskelamin = dt.Rows[0]["jeniskelamin"].ToString();
                txtTelepon.Text = dt.Rows[0]["telepon"].ToString();
                txtEmail.Text = dt.Rows[0]["email"].ToString();
                txtAlamat.Text = dt.Rows[0]["alamat"].ToString();

                if (jeniskelamin == rbLaki.Text)
                {
                    rbLaki.Checked = true;
                }
                else if (jeniskelamin == rbPerempuan.Text)
                {
                    rbPerempuan.Checked = true;
                }

                rbLaki.Enabled = true;
                rbPerempuan.Enabled = true;

                myConnection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Pemilik Tidak Ditemukan! " + ex);
            }
        }

        private void FormPemilik_Load(object sender, EventArgs e)
        {
            txtID.Text = IDOtomatis();
            LoadData();
        }

        void LoadData()
        {
            var sp = from tb in dc.pemiliks select tb;
            dgPemilik.DataSource = sp;
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

        private void btnJabatan_Click(object sender, EventArgs e)
        {
            FormJabatan jabat = new FormJabatan();
            jabat.Show();
            this.Hide();
        }

        private void btnX_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
