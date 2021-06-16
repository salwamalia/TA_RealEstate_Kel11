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
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }
        
        SqlConnection Connection = new SqlConnection(@"Data Source=LAPTOP-L1AODT95;Initial Catalog=REALESTATE;Integrated Security=True");

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (txtUser.Text == "" || txtPass.Text == "")
            {
                MessageBox.Show("Silahkan Masukkan Username atau Password Terlebih Dahulu!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                SqlDataAdapter sda = new SqlDataAdapter("Select username, password, idJabatan from pegawai where username= '" + txtUser.Text + "' and password = '" + txtPass.Text + "'", Connection);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (dr["idJabatan"].ToString() == "JBT02")
                        {
                            this.Hide();
                            MenuAdmin dash = new MenuAdmin();
                            dash.Show();
                        }
                        else if (dr["idJabatan"].ToString() == "JBT03")
                        {
                            this.Hide();
                            MenuKasir cash = new MenuKasir();
                            cash.Show();
                        }
                        else if (dr["idJabatan"].ToString() == "JBT01")
                        {
                            this.Hide();
                            MenuManager Manage = new MenuManager();
                            Manage.Show();
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Username atau Password salah", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void btnKeluar_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
