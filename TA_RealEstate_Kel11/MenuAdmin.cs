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
    public partial class MenuAdmin : Form
    {
        public MenuAdmin()
        {
            InitializeComponent();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            FormLogin login = new FormLogin();
            login.Visible = true;
            this.Dispose();
        }

        private void btnPegawai_Click(object sender, EventArgs e)
        {
            FormPegawai pegawai = new FormPegawai();
            pegawai.Show();
            this.Dispose();
        }

        private void btnJabatan_Click(object sender, EventArgs e)
        {
            FormJabatan jabat = new FormJabatan();
            jabat.Show();
            this.Dispose();
        }

        private void btnOwner_Click(object sender, EventArgs e)
        {
            FormOwner Own = new FormOwner();
            Own.Show();
            this.Dispose();
        }

        private void btnClient_Click(object sender, EventArgs e)
        {
            FormClient klien = new FormClient();
            klien.Show();
            this.Dispose();
        }

        private void btnProperty_Click(object sender, EventArgs e)
        {
            FormProperty Prop = new FormProperty();
            Prop.Show();
            this.Dispose();
        }

        private void btnPropertyType_Click(object sender, EventArgs e)
        {
            FormPropertyTypes PropType = new FormPropertyTypes();
            PropType.Show();
            this.Dispose();
        }

        private void btnPropertyImage_Click(object sender, EventArgs e)
        {
            FormPropertyImages PropImage = new FormPropertyImages();
            PropImage.Show();
            this.Dispose();
        }

        private void btnCicilan_Click(object sender, EventArgs e)
        {
            FormCicilan cicil = new FormCicilan();
            cicil.Show();
            this.Dispose();
        }
    }
}
