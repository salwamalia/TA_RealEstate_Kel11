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
using System.IO;
using System.Drawing.Imaging;

namespace TA_RealEstate_Kel11
{
    public partial class FormProperty : Form
    {
        public FormProperty()
        {
            InitializeComponent();
        }

        string imgLocation = "";

        //koneksi
        koneksi connection = new koneksi();
        REALESTATEDataContext dt = new REALESTATEDataContext();

        private string IDOtomatis()
        {
            int autoid = 0;
            string kode = null;

            SqlConnection myConnection = connection.Getcon();
            myConnection.Open();

            string sqlQuery = "SELECT TOP (1) MAX(RIGHT (idProperty,2))+1 AS idProperty FROM property";
            SqlCommand cmd = new SqlCommand(sqlQuery, myConnection);
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                if (dr["idProperty"].ToString() == "")
                {
                    autoid = 1;
                }
                else
                {
                    autoid = Int32.Parse(dr["idProperty"].ToString());
                }
            }

            if (autoid < 10)
            {
                kode = "PT00" + autoid;
            }
            else if (autoid < 100)
            {
                kode = "PT" + autoid;
            }

            myConnection.Close();

            return kode;
        }

        private void btnBrowser_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "png files(*.png)|*.png|jpg files(*jpg)|*.jpg|All files(*.*)|*.*";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                imgLocation = dialog.FileName.ToString();
                PBGambar.ImageLocation = imgLocation;
            }
        }

        private void btnSimpan_Click(object sender, EventArgs e)
        {
            SqlConnection myConnection = connection.Getcon();

            Image img;
            byte[] photo_aray;
            if (PBGambar.Image != null)
            {
                img = PBGambar.Image;
                MemoryStream ms = new MemoryStream();
                PBGambar.Image.Save(ms, ImageFormat.Jpeg);
                photo_aray = new byte[ms.Length];
                ms.Position = 0;
                ms.Read(photo_aray, 0, photo_aray.Length);
            }
            else
            {
                byte[] kosong = { 0 };
                photo_aray = kosong;
            }

            SqlCommand insert = new SqlCommand("sp_InputProperty", myConnection);
            insert.CommandType = CommandType.StoredProcedure;

            insert.Parameters.AddWithValue("idProperty", IDOtomatis());
            insert.Parameters.AddWithValue("namaProperty", txtNama.Text);
            insert.Parameters.AddWithValue("idTipe", cbTipe.SelectedValue.ToString());
            insert.Parameters.AddWithValue("idPemilik", cbPemilik.SelectedValue.ToString());
            insert.Parameters.AddWithValue("ukuran", txtUkuran.Text);
            insert.Parameters.AddWithValue("fasilitas", txtFasilitas.Text);
            insert.Parameters.AddWithValue("harga", txtHarga.Text);
            insert.Parameters.AddWithValue("gambar", photo_aray);
            insert.Parameters.AddWithValue("idInterior", cbInterior.SelectedValue.ToString());
            insert.Parameters.AddWithValue("status", cbStatus.SelectedItem.ToString());

            if (txtID.Text == "" || txtNama.Text == "" || cbTipe.Text == "" || cbPemilik.Text == "" || txtUkuran.Text == "" || txtFasilitas.Text == "" || txtHarga.Text == "" || cbInterior.Text == "")
            {
                MessageBox.Show("Semua Data Harus diisi !!", "Add Property", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                try
                {
                    myConnection.Open();
                    insert.ExecuteNonQuery();
                    MessageBox.Show("Property Telah Ditambahkan", "Add Property", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    clear();
                }
                catch (Exception ex)
                {
                    //MessageBox.Show("Unable to save ", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    MessageBox.Show("Unable to save "+ex);
                }
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            SqlConnection myConnection = connection.Getcon();

            Image img;
            byte[] photo_aray;
            if (PBGambar.Image != null)
            {
                img = PBGambar.Image;
                MemoryStream ms = new MemoryStream();
                PBGambar.Image.Save(ms, ImageFormat.Jpeg);
                photo_aray = new byte[ms.Length];
                ms.Position = 0;
                ms.Read(photo_aray, 0, photo_aray.Length);
            }
            else
            {
                byte[] kosong = { 0 };
                photo_aray = kosong;
            }

            //Update command
            SqlCommand Update = new SqlCommand("sp_UpdateProperty", myConnection);
            Update.CommandType = CommandType.StoredProcedure;

            Update.Parameters.AddWithValue("idProperty", txtID.Text);
            Update.Parameters.AddWithValue("namaProperty", txtNama.Text);
            Update.Parameters.AddWithValue("idTipe", cbTipe.SelectedValue);
            Update.Parameters.AddWithValue("idPemilik", cbPemilik.SelectedValue);
            Update.Parameters.AddWithValue("ukuran", txtUkuran.Text);
            Update.Parameters.AddWithValue("fasilitas", txtFasilitas.Text);
            Update.Parameters.AddWithValue("harga", txtHarga.Text);
            Update.Parameters.AddWithValue("gambar", photo_aray);
            Update.Parameters.AddWithValue("idInterior", cbInterior.SelectedValue);
            Update.Parameters.AddWithValue("status", cbStatus.SelectedItem.ToString());

            if (txtID.Text == "" || txtNama.Text == "" || cbTipe.Text == "" || cbPemilik.Text == "" || txtUkuran.Text == "" || txtFasilitas.Text == "" || txtHarga.Text == "" || cbInterior.Text == "")
            {
                MessageBox.Show("Semua Data Harus diisi !!", "Add Property", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                try
                {
                    myConnection.Open();
                    Update.ExecuteNonQuery();
                    MessageBox.Show("Update Property Succesfully", "Update Property", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    clear();
                }
                catch (Exception ex)
                {
                    //MessageBox.Show("Unable to save ", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    MessageBox.Show("Unable to save "+ex);
                }
            }
        }

        private void btnHapus_Click(object sender, EventArgs e)
        {
            SqlConnection myConnection = connection.Getcon();

            if (txtID.Text == "" || txtNama.Text == "" || cbTipe.Text == "" || cbPemilik.Text == "" || txtUkuran.Text == "" || txtFasilitas.Text == "" || txtHarga.Text == "" || cbInterior.Text == "")
            {
                MessageBox.Show("Semua Data Harus diisi !!", "Delete Property", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                if (MessageBox.Show("Lanjut ingin Menghapus?", "Delete Property", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    //delete command
                    SqlCommand delete = new SqlCommand("sp_DeleteProperty", myConnection);
                    delete.CommandType = CommandType.StoredProcedure;

                    delete.Parameters.AddWithValue("idProperty", txtID.Text);

                    try
                    {
                        myConnection.Open();
                        delete.ExecuteNonQuery();
                        MessageBox.Show("Property Delete Succesfully", "Delete Property", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        clear();
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Unable to save ", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
        }

        private void btnBatal_Click(object sender, EventArgs e)
        {
            clear();
        }

        private void clear()
        {
            txtCariProperty.Clear();
            txtID.Clear();
            txtNama.Clear();
            cbTipe.SelectedIndex = -1;
            cbPemilik.SelectedIndex = -1;
            txtUkuran.Clear();
            txtFasilitas.Clear();
            txtHarga.Clear();
            PBGambar.Image = null;
            cbInterior.SelectedIndex = -1;
            cbStatus.SelectedIndex = -1;
            txtID.Text = IDOtomatis();
            LoadData();
        }

        private void btnCari_Click(object sender, EventArgs e)
        {
            if (txtCariProperty.Text == "")
            {
                MessageBox.Show("Masukkan ID Untuk Cari !!", "Add Property", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                try
                {
                    SqlConnection myConnection = connection.Getcon();
                    myConnection.Open();
                    
                    SqlCommand cmd = new SqlCommand("SELECT * FROM property WHERE idProperty ='" + txtCariProperty.Text + "'", myConnection);
                    SqlDataReader dr = cmd.ExecuteReader();
                    SqlDataReader sqlDataReader = dr;

                    dr.Read();

                    txtID.Text = sqlDataReader["idProperty"].ToString();
                    txtNama.Text = sqlDataReader["namaProperty"].ToString();
                    cbTipe.SelectedValue = sqlDataReader["idTipe"].ToString();
                    cbPemilik.SelectedValue = sqlDataReader["idPemilik"].ToString();
                    cbInterior.SelectedValue = sqlDataReader["idInterior"].ToString();
                    txtUkuran.Text = sqlDataReader["ukuran"].ToString();
                    txtFasilitas.Text = sqlDataReader["fasilitas"].ToString();
                    txtHarga.Text = sqlDataReader["harga"].ToString();
                    byte[] img = (byte[])(sqlDataReader["gambar"]);
                    cbStatus.Text = dr["statusProperty"].ToString();

                    try
                    {
                        if (img == null)
                        {
                            PBGambar.Image = null;
                        }
                        else
                        {
                            MemoryStream ms = new MemoryStream(img);
                            PBGambar.Image = Image.FromStream(ms);
                        }
                    }
                    catch
                    { }

                    dr.Close();
                    sqlDataReader.Close();
                    myConnection.Close();
                    
                }
                catch (Exception ex)
                {
                }
            }
        }

        void LoadData()
        {
            try
            {
                SqlConnection con = connection.Getcon();
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM property", con);
                SqlDataReader dr = cmd.ExecuteReader();
                SqlDataReader sqlDataReader = dr;
                DataTable dataTable = new DataTable();
                dataTable.Columns.Add("ID");
                dataTable.Columns.Add("Nama Property");
                dataTable.Columns.Add("ID Tipe");
                dataTable.Columns.Add("ID Pemilik");
                dataTable.Columns.Add("Ukuran");
                dataTable.Columns.Add("Fasilitas");
                dataTable.Columns.Add("Harga");
                dataTable.Columns.Add("ID Interior");
                dataTable.Columns.Add("Status");
                while (dr.Read())
                {
                    DataRow row = dataTable.NewRow();
                    row["ID"] = sqlDataReader["idProperty"];
                    row["Nama Property"] = sqlDataReader["namaProperty"];
                    row["ID Tipe"] = sqlDataReader["idTipe"];
                    row["ID Pemilik"] = sqlDataReader["idPemilik"];
                    row["Ukuran"] = sqlDataReader["ukuran"];
                    row["Fasilitas"] = sqlDataReader["fasilitas"];
                    row["Harga"] = sqlDataReader["harga"];
                    row["ID Interior"] = sqlDataReader["idInterior"];
                    row["Status"] = sqlDataReader["statusProperty"];
                    dataTable.Rows.Add(row);
                }
                //this.dataGridView1.DataSource = con.table;
                dgProperty.DataSource = dataTable;
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Occured" + ex.Message);
            }
        }

        private void FormProperty_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'rEALESTATEDataSet.property' table. You can move, or remove it, as needed.
            this.propertyTableAdapter.Fill(this.rEALESTATEDataSet.property);
            // TODO: This line of code loads data into the 'rEALESTATEDataSet.desainInterior' table. You can move, or remove it, as needed.
            this.desainInteriorTableAdapter.Fill(this.rEALESTATEDataSet.desainInterior);
            // TODO: This line of code loads data into the 'rEALESTATEDataSet.pemilik' table. You can move, or remove it, as needed.
            this.pemilikTableAdapter.Fill(this.rEALESTATEDataSet.pemilik);
            // TODO: This line of code loads data into the 'rEALESTATEDataSet.propertyTipe' table. You can move, or remove it, as needed.
            this.propertyTipeTableAdapter.Fill(this.rEALESTATEDataSet.propertyTipe);
            txtID.Text = IDOtomatis();
            LoadData();
        }
        
        private void btnX_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void bunifuTileButton1_Click(object sender, EventArgs e)
        {
            MenuAdmin admin = new MenuAdmin();
            admin.Show();
            this.Hide();
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
            FormInterior byr = new FormInterior();
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
