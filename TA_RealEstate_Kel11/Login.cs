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
    public partial class FormLogin : Form
    {
        public FormLogin()
        {
            InitializeComponent();
        }

        SqlConnection Connection = new SqlConnection(@"Data Source=LAPTOP-L1AODT95;Initial Catalog=TARealEstateKel11;Integrated Security=True");

        private void btnKeluar_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            Connection.Open();
            if (txtUser.Text == "" || txtPass.Text == "")
            {
                MessageBox.Show("Silahkan Masukkan Username atau Password Terlebih Dahulu!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                SqlDataAdapter sda = new SqlDataAdapter("Select username, password, jabatan from pegawai where username = '" + txtUser.Text + "' and password = '" + txtPass.Text + "'", Connection);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (dr["jabatan"].ToString() == "JBT02")
                        {
                            this.Hide();
                            MenuAdmin admin = new MenuAdmin();
                            admin.Show();
                        }
                        else if (dr["jabatan"].ToString() == "JBT03")
                        {
                            this.Hide();
                            MenuKasir kasir = new MenuKasir();
                            kasir.Show();
                        }
                        else if (dr["jabatan"].ToString() == "JBT01")
                        {
                            this.Hide();
                            MenuManager manajer = new MenuManager();
                            manajer.Show();
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Username atau Password salah", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }
            Connection.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            txtUser.Clear();
            txtPass.Clear();
        }
    }
}
