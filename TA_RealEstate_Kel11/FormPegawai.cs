﻿using System;
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
    public partial class FormPegawai : Form
    {
        public FormPegawai()
        {
            InitializeComponent();
        }

        private string IDOtomatis()
        {
            string autoid = null;

            string myConnectionString = @"Data Source=LAPTOP-L1AODT95;Initial Catalog=REALESTATE;Integrated Security=True";
            SqlConnection myConnection = new SqlConnection(myConnectionString);
            myConnection.Open();

            string sqlQuery = "SELECT TOP 1 idPegawai FROM pegawai ORDER BY idPegawai DESC";
            SqlCommand cmd = new SqlCommand(sqlQuery, myConnection);
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                string input = dr["idPegawai"].ToString();
                string angka = input.Substring(input.Length - Math.Min(2, input.Length));
                int number = Convert.ToInt32(angka);
                number += 1;
                string str = number.ToString("D2");

                autoid = "PGW" + str;
            }

            if (autoid == null)
            {
                autoid = "PGW01";
            }

            myConnection.Close();

            return autoid;
        }

        private void btnSimpan_Click(object sender, EventArgs e)
        {
            string myConnectionString = @"Data Source=LAPTOP-L1AODT95;Initial Catalog=REALESTATE;Integrated Security=True";
            SqlConnection myConnection = new SqlConnection(myConnectionString);

            SqlCommand insert = new SqlCommand("sp_InsertPegawai", myConnection);
            insert.CommandType = CommandType.StoredProcedure;

            insert.Parameters.AddWithValue("idPegawai", txtID.Text);
            insert.Parameters.AddWithValue("nama", txtNama.Text);
            insert.Parameters.AddWithValue("jeniskelamin", txtJenisKel.Text);
            insert.Parameters.AddWithValue("username", txtUser.Text);
            insert.Parameters.AddWithValue("password", txtPass.Text);
            insert.Parameters.AddWithValue("idjabatan", cbJabatan.SelectedItem.ToString());

            if (txtID.Text == "" || txtNama.Text == "" || txtJenisKel.Text == "" || txtUser.Text == "" || txtPass.Text == "" || cbJabatan.Text == "")
            {
                MessageBox.Show("Data tersebut Harus diisi !!", "Add Pegawai", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                try
                {
                    myConnection.Open();
                    insert.ExecuteNonQuery();
                    MessageBox.Show("Pegawai Telah Ditambahkan", "Add Pegawai", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    clear();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Unable to save " + ex.Message);
                }
            }

            txtID.Text = IDOtomatis();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            string myConnectionString = @"Data Source=LAPTOP-L1AODT95;Initial Catalog=REALESTATE;Integrated Security=True";
            SqlConnection myConnection = new SqlConnection(myConnectionString);

            //Update command
            SqlCommand Update = new SqlCommand("sp_UpdatePegawai", myConnection);
            Update.CommandType = CommandType.StoredProcedure;

            Update.Parameters.AddWithValue("idPegawai", txtID.Text);
            Update.Parameters.AddWithValue("nama", txtNama.Text);
            Update.Parameters.AddWithValue("jeniskelamin", txtJenisKel.Text);
            Update.Parameters.AddWithValue("username", txtUser.Text);
            Update.Parameters.AddWithValue("password", txtPass.Text);
            Update.Parameters.AddWithValue("idJabatan", cbJabatan.SelectedItem.ToString());

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
        }

        private void btnHapus_Click(object sender, EventArgs e)
        {
            string myConnectionString = @"Data Source=LAPTOP-L1AODT95;Initial Catalog=REALESTATE;Integrated Security=True";
            SqlConnection myConnection = new SqlConnection(myConnectionString);

            if (MessageBox.Show("Lanjut ingin Menghapus?", "Delete Pegawai", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                //delete command
                SqlCommand delete = new SqlCommand("sp_DeletePegawai", myConnection);
                delete.CommandType = CommandType.StoredProcedure;

                delete.Parameters.AddWithValue("idPegawai", txtID.Text);

                try
                {
                    myConnection.Open();
                    delete.ExecuteNonQuery();
                    MessageBox.Show("Delete Pegawai Succesfully", "Delete Pegawai", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    clear();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Unable to save " + ex.Message);
                }
            }

            txtID.Text = IDOtomatis();
        }

        private void btnCari_Click(object sender, EventArgs e)
        {
            try
            {
                string myConnectionString = @"Data Source=LAPTOP-L1AODT95;Initial Catalog=REALESTATE;Integrated Security=True";
                SqlConnection myConnection = new SqlConnection(myConnectionString);
                myConnection.Open();

                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("SELECT * FROM pegawai WHERE idPegawai ='" + txtID.Text + "'", myConnection);
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                da.Fill(dt);

                txtNama.Text = dt.Rows[0]["nama"].ToString();
                txtJenisKel.Text= dt.Rows[0]["jeniskelamin"].ToString();
                txtUser.Text = dt.Rows[0]["username"].ToString();
                txtPass.Text = dt.Rows[0]["password"].ToString();
                cbJabatan.SelectedItem = dt.Rows[0]["idJabatan"].ToString();

                myConnection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error ! " + ex.Message);
            }
        }

        private void btnBatal_Click(object sender, EventArgs e)
        {
           clear();
        }

        private void clear()
        {
            txtID.Clear();
            txtNama.Clear();
            txtJenisKel.Clear();
            txtUser.Clear();
            txtPass.Clear();
            cbJabatan.SelectedIndex = -1;
        }

        private void FormPegawai_Load(object sender, EventArgs e)
        {
            txtID.Text = IDOtomatis();
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
            FormKategoriBayar byr = new FormKategoriBayar();
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
