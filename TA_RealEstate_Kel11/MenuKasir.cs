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

        private void btnPembelian_Click(object sender, EventArgs e)
        {
            TransaksiPembelian Beli = new TransaksiPembelian();
            Beli.Show();
            this.Dispose();
        }

        private void btnPenyewaan_Click(object sender, EventArgs e)
        {
            TransaksiPenyewaan Sewa = new TransaksiPenyewaan();
            Sewa.Show();
            this.Dispose();
        }

        private void btnPembayaran_Click(object sender, EventArgs e)
        {
            TransaksiPembayaran Bayar = new TransaksiPembayaran();
            Bayar.Show();
            this.Dispose();
        }

        private void btnPelunasan_Click(object sender, EventArgs e)
        {
            TransaksiPelunasan Lunas = new TransaksiPelunasan();
            Lunas.Show();
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
