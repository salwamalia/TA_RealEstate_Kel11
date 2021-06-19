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
        SqlCommand cmd;

        REALESTATEDataContext dt = new REALESTATEDataContext();

        private string IDOtomatis()
        {
            string autoid = null;

            //string myConnectionString = @"Data Source=LAPTOP-L1AODT95;Initial Catalog=REALESTATE;Integrated Security=True";
            string myConnectionString = @"Data Source=WINDOWS-LD56BQV;Initial Catalog=REALESTATE;Integrated Security=True";
            SqlConnection myConnection = new SqlConnection(myConnectionString);
            myConnection.Open();

            string sqlQuery = "SELECT TOP 1 idProperty FROM property ORDER BY idProperty DESC";
            SqlCommand cmd = new SqlCommand(sqlQuery, myConnection);
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                string input = dr["idProperty"].ToString();
                string angka = input.Substring(input.Length - Math.Min(2, input.Length));
                int number = Convert.ToInt32(angka);
                number += 1;
                string str = number.ToString("D2");

                autoid = "PTY" + str;
            }

            if (autoid == null)
            {
                autoid = "PTY01";
            }
            myConnection.Close();
            return autoid;
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
            //string myConnectionString = @"Data Source=LAPTOP-L1AODT95;Initial Catalog=REALESTATE;Integrated Security=True";
            string myConnectionString = @"Data Source=WINDOWS-LD56BQV;Initial Catalog=REALESTATE;Integrated Security=True";
            SqlConnection myConnection = new SqlConnection(myConnectionString);

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

            if (txtID.Text == "" || txtNama.Text == "" || cbTipe.Text == "" || cbPemilik.Text == "" || txtUkuran.Text == "" || txtFasilitas.Text == "" || txtHarga.Text == "")
            {
                MessageBox.Show("Data tersebut Harus diisi !!", "Add Property", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                    MessageBox.Show("Unable to save " + ex.Message);
                }
            }
            txtID.Text = IDOtomatis();
            LoadData();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            //string myConnectionString = @"Data Source=LAPTOP-L1AODT95;Initial Catalog=REALESTATE;Integrated Security=True";
            string myConnectionString = @"Data Source=WINDOWS-LD56BQV;Initial Catalog=REALESTATE;Integrated Security=True";
            SqlConnection myConnection = new SqlConnection(myConnectionString);

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

            try
            {
                myConnection.Open();
                Update.ExecuteNonQuery();
                MessageBox.Show("Update Pegawai Succesfully", "Update Pegawai", MessageBoxButtons.OK, MessageBoxIcon.Information);
                clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to save " + ex.Message);
            }

            txtID.Text = IDOtomatis();
            LoadData();
        }

        private void btnHapus_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Lanjut ingin Menghapus?", "Delete Property", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                //string myConnectionString = @"Data Source=LAPTOP-L1AODT95;Initial Catalog=REALESTATE;Integrated Security=True";
                string myConnectionString = @"Data Source=WINDOWS-LD56BQV;Initial Catalog=REALESTATE;Integrated Security=True";
                SqlConnection myConnection = new SqlConnection(myConnectionString);

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
                    LoadData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Unable to save " + ex.Message);
                }
                txtID.Text = IDOtomatis();
            }
        }

        private void btnBatal_Click(object sender, EventArgs e)
        {
            clear();
            txtID.Text = IDOtomatis();
            LoadData();
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
        }

        private void btnCari_Click(object sender, EventArgs e)
        {
            try
            {
                //string myConnectionString = @"Data Source=LAPTOP-L1AODT95;Initial Catalog=REALESTATE;Integrated Security=True";
                string myConnectionString = @"Data Source=WINDOWS-LD56BQV;Initial Catalog=REALESTATE;Integrated Security=True";
                SqlConnection myConnection = new SqlConnection(myConnectionString);
                myConnection.Open();

                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("SELECT * FROM property WHERE idProperty ='" + txtCariProperty.Text + "'", myConnection);
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                da.Fill(dt);

                txtID.Text = dt.Rows[0]["idProperty"].ToString();
                txtNama.Text = dt.Rows[0]["namaProperty"].ToString();
                cbTipe.SelectedValue = dt.Rows[0]["idTipe"].ToString();
                cbPemilik.SelectedValue = dt.Rows[0]["idPemilik"].ToString();
                txtUkuran.Text = dt.Rows[0]["ukuran"].ToString();
                txtFasilitas.Text = dt.Rows[0]["fasilitas"].ToString();
                txtHarga.Text = dt.Rows[0]["harga"].ToString();

                byte[] img = (byte[])(dt.Rows[0]["gambar"]);
                if (img == null)
                {
                    PBGambar.Image = null;
                }
                else
                {
                    MemoryStream ms = new MemoryStream(img);
                    PBGambar.Image = Image.FromStream(ms);
                }

                myConnection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Pegawai Tidak Ditemukan! " + ex);
            }
            txtID.Text = IDOtomatis();
            LoadData();
        }

        void LoadData()
        {
            var st = from tb in dt.properties select tb;
            dgProperty.DataSource = st;
        }

        private void FormProperty_Load(object sender, EventArgs e)
        {
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
    }
}
