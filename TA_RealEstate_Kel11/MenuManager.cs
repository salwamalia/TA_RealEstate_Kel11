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
    public partial class MenuManager : Form
    {
        public MenuManager()
        {
            InitializeComponent();
        }

        private void btnPembelian_Click(object sender, EventArgs e)
        {
            LaporanPembelian beli = new LaporanPembelian();
            beli.Show();
            this.Dispose();
        }

        private void btnPenyewaan_Click(object sender, EventArgs e)
        {
            LaporanPenyewaan sewa = new LaporanPenyewaan();
            sewa.Show();
            this.Dispose();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            FormLogin login = new FormLogin();
            login.Visible = true;
            this.Dispose();
        }
    }
}
