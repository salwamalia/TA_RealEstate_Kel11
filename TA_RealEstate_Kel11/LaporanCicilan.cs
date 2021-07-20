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
    public partial class LaporanCicilan : Form
    {
        public LaporanCicilan()
        {
            InitializeComponent();
        }

        private void LaporanCicilan_Load(object sender, EventArgs e)
        {
            this.reportViewer1.RefreshReport();
        }

        private void btnLihat_Click(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'DataSetCicilan.Cicilan' table. You can move, or remove it, as needed.
            this.CicilanTableAdapter.Fill(this.DataSetCicilan.Cicilan, txtID.Text);
            this.reportViewer1.RefreshReport();
        }

        private void btnLapBeli_Click(object sender, EventArgs e)
        {
            LaporanPenjualan beli = new LaporanPenjualan();
            beli.Show();
            this.Hide();
        }

        private void btnLapSewa_Click(object sender, EventArgs e)
        {
            LaporanPenyewaan sewa = new LaporanPenyewaan();
            sewa.Show();
            this.Hide();
        }

        private void btnLapCicil_Click(object sender, EventArgs e)
        {
            LaporanCicilan cicil = new LaporanCicilan();
            cicil.Show();
            this.Hide();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            Login log = new Login();
            log.Visible = true;
            this.Hide();
        }

        private void bunifuTileButton1_Click(object sender, EventArgs e)
        {
            MenuManager manager = new MenuManager();
            manager.Show();
            this.Hide();
        }

        private void btnX_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
