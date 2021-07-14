using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TA_RealEstate_Kel11
{
    public partial class MenuKasir : Form
    {
        public MenuKasir()
        {
            InitializeComponent();
        }

        private void btnX_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void bunifuTileButton1_Click(object sender, EventArgs e)
        {
            MenuKasir kasir = new MenuKasir();
            kasir.Show();
            this.Hide();
        }

        private void btnPenyewaan_Click(object sender, EventArgs e)
        {
            TSewaProperty sewa = new TSewaProperty();
            sewa.Show();
            this.Hide();
        }
        
        private void MenuKasir_Load(object sender, EventArgs e)
        {

        }

        private void btnJualProperty_Click(object sender, EventArgs e)
        {
            TJualPropertyLunas lunas = new TJualPropertyLunas();
            lunas.Show();
            this.Hide();
        }

        private void btnPelunasanCicilan_Click(object sender, EventArgs e)
        {
            TPelunasanCicilanProperty lunas = new TPelunasanCicilanProperty();
            lunas.Show();
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
