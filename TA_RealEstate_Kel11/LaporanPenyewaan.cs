﻿using System;
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
    public partial class LaporanPenyewaan : Form
    {
        public LaporanPenyewaan()
        {
            InitializeComponent();
        }

        private void btnX_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void bunifuTileButton1_Click(object sender, EventArgs e)
        {
            MenuManager manager = new MenuManager();
            manager.Show();
            this.Hide();
        }

        private void btnLapBeli_Click(object sender, EventArgs e)
        {
            LaporanPembelian beli = new LaporanPembelian();
            beli.Show();
            this.Hide();
        }

        private void btnLapSewa_Click(object sender, EventArgs e)
        {
            LaporanPenyewaan sewa = new LaporanPenyewaan();
            sewa.Show();
            this.Hide();
        }

        private void bunifuFlatButton1_Click(object sender, EventArgs e)
        {
            Login log = new Login();
            log.Visible = true;
            this.Hide();
        }
    }
}
