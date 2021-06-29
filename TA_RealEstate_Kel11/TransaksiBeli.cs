﻿using System;
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
    public partial class TransaksiBeli : Form
    {
        //string myConnectionString = @"Data Source=LAPTOP-L1AODT95;Initial Catalog=REALESTATE;Integrated Security=True";
        string myConnectionString = @"Data Source=WINDOWS-LD56BQV;Initial Catalog=REALESTATE;Integrated Security=True";

        public TransaksiBeli()
        {
            InitializeComponent();
        }

        REALESTATEDataContext dc = new REALESTATEDataContext();

        private string IDOtomatis()
        {
            string autoid = null;

            SqlConnection myConnection = new SqlConnection(myConnectionString);
            myConnection.Open();

            string sqlQuery = "SELECT TOP 1 idTBeli FROM tPembelian ORDER BY idTBeli DESC";
            SqlCommand cmd = new SqlCommand(sqlQuery, myConnection);
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                string input = dr["idTBeli"].ToString();
                string angka = input.Substring(input.Length - Math.Min(3, input.Length));
                int number = Convert.ToInt32(angka);
                number += 1;
                string str = number.ToString("D3");

                autoid = "TRB" + str;
            }

            if (autoid == null)
            {
                autoid = "TRB001";
            }

            myConnection.Close();

            return autoid;
        }

        private void btnCariTransaksi_Click(object sender, EventArgs e)
        {

        }

        private void btnBatal_Click(object sender, EventArgs e)
        {
            clear();
        }

        public void clear()
        {
            txtCariTransaksi.Clear();
            txtIDBeli.Clear();
            dateTimePicker1.ResetText();
            cbProperty.SelectedIndex = -1;
            txtPemilik.Clear();
            cbClient.SelectedIndex = -1;
            txtTotal.Clear();

            cbPropertyDetail.SelectedIndex = -1;
            txtHargaProperty.Clear();
            cbPembayaran.SelectedIndex = -1;
            cbCicilan.SelectedIndex = -1;
            txtLamaCicilan.Clear();
            txtTotal.Clear();
            txtDP.Clear();
            txtTotalBayar.Clear();
            
            txtIDBeli.Text = IDOtomatis();
            //LoadData();
        }

        private void cbProperty_SelectedIndexChanged(object sender, EventArgs e)
        {
            SqlConnection myConnection = new SqlConnection(myConnectionString);
            string sql = "SELECT * FROM property p INNER JOIN pemilik b ON p.idPemilik = b.idPemilik WHERE p.idProperty  = '" + cbProperty.SelectedValue + "' ";
            SqlCommand cmd = new SqlCommand(sql, myConnection);
            SqlDataReader myreader;
            try
            {
                myConnection.Open();
                myreader = cmd.ExecuteReader();
                while (myreader.Read())
                {
                    //string harga = myreader.GetInt32(0).ToString();
                    string pemilik = myreader.GetString(9);
                    txtPemilik.Text = pemilik;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Occured" + ex.Message);
            }
        }

        private void cbPropertyDetail_SelectedIndexChanged(object sender, EventArgs e)
        {
            SqlConnection myConnection = new SqlConnection(myConnectionString);
            //string sql = "SELECT * FROM property WHERE idProperty = ";
            string sql = "SELECT * FROM property WHERE idProperty  = '" + cbPropertyDetail.SelectedValue + "' ";
            SqlCommand cmd = new SqlCommand(sql, myConnection);
            SqlDataReader myreader;
            try
            {
                myConnection.Open();
                myreader = cmd.ExecuteReader();
                while (myreader.Read())
                {
                    string harga = myreader.GetInt32(6).ToString();
                    txtHargaProperty.Text = harga;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Occured" + ex.Message);
            }
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

        private void btnPembelian_Click(object sender, EventArgs e)
        {
            TransaksiBeli beli = new TransaksiBeli();
            beli.Show();
            this.Hide();
        }

        private void btnPenyewaan_Click(object sender, EventArgs e)
        {
            TransaksiSewa sewa = new TransaksiSewa();
            sewa.Show();
            this.Hide();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            Login log = new Login();
            log.Visible = true;
            this.Hide();
        }

        private void TransaksiBeli_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'rEALESTATEDataSet.kategoriCicilan' table. You can move, or remove it, as needed.
            this.kategoriCicilanTableAdapter.Fill(this.rEALESTATEDataSet.kategoriCicilan);
            // TODO: This line of code loads data into the 'rEALESTATEDataSet.kategoriBayar' table. You can move, or remove it, as needed.
            this.kategoriBayarTableAdapter.Fill(this.rEALESTATEDataSet.kategoriBayar);
            // TODO: This line of code loads data into the 'rEALESTATEDataSet.client' table. You can move, or remove it, as needed.
            this.clientTableAdapter.Fill(this.rEALESTATEDataSet.client);
            // TODO: This line of code loads data into the 'rEALESTATEDataSet.property' table. You can move, or remove it, as needed.
            this.propertyTableAdapter.Fill(this.rEALESTATEDataSet.property);
           
            txtIDBeli.Text = IDOtomatis();
        }

        private void btnSimpan_Click_1(object sender, EventArgs e)
        {
            
        }
    }
}
