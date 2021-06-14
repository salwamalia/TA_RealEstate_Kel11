namespace TA_RealEstate_Kel11
{
    partial class FormProperty
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormProperty));
            this.button1 = new System.Windows.Forms.Button();
            this.txtAlamat = new System.Windows.Forms.TextBox();
            this.txtID = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.cbTipe = new System.Windows.Forms.ComboBox();
            this.btnCariID = new System.Windows.Forms.Button();
            this.btnCariOwner = new System.Windows.Forms.Button();
            this.btnSemuaProperty = new System.Windows.Forms.Button();
            this.btnGambarProperty = new System.Windows.Forms.Button();
            this.dgProperty = new System.Windows.Forms.DataGridView();
            this.btnBatal = new System.Windows.Forms.Button();
            this.btnHapus = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.Simpan = new System.Windows.Forms.Button();
            this.txtUkuran = new System.Windows.Forms.TextBox();
            this.txtOwner = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgProperty)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(649, -1);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(31, 26);
            this.button1.TabIndex = 38;
            this.button1.Text = "X";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // txtAlamat
            // 
            this.txtAlamat.Location = new System.Drawing.Point(102, 250);
            this.txtAlamat.Multiline = true;
            this.txtAlamat.Name = "txtAlamat";
            this.txtAlamat.Size = new System.Drawing.Size(178, 69);
            this.txtAlamat.TabIndex = 33;
            // 
            // txtID
            // 
            this.txtID.Location = new System.Drawing.Point(102, 76);
            this.txtID.Name = "txtID";
            this.txtID.Size = new System.Drawing.Size(97, 20);
            this.txtID.TabIndex = 31;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(22, 151);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(61, 20);
            this.label4.TabIndex = 30;
            this.label4.Text = "Ukuran";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(22, 114);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(39, 20);
            this.label3.TabIndex = 29;
            this.label3.Text = "Tipe";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(22, 76);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(26, 20);
            this.label2.TabIndex = 28;
            this.label2.Text = "ID";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(286, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(109, 20);
            this.label1.TabIndex = 27;
            this.label1.Text = "Form Property";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(22, 184);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(55, 20);
            this.label6.TabIndex = 40;
            this.label6.Text = "Owner";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(22, 218);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(53, 20);
            this.label7.TabIndex = 41;
            this.label7.Text = "Harga";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(22, 248);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(59, 20);
            this.label8.TabIndex = 42;
            this.label8.Text = "Alamat";
            // 
            // cbTipe
            // 
            this.cbTipe.FormattingEnabled = true;
            this.cbTipe.Location = new System.Drawing.Point(102, 113);
            this.cbTipe.Name = "cbTipe";
            this.cbTipe.Size = new System.Drawing.Size(178, 21);
            this.cbTipe.TabIndex = 45;
            // 
            // btnCariID
            // 
            this.btnCariID.Location = new System.Drawing.Point(205, 76);
            this.btnCariID.Name = "btnCariID";
            this.btnCariID.Size = new System.Drawing.Size(75, 23);
            this.btnCariID.TabIndex = 46;
            this.btnCariID.Text = "Cari ID";
            this.btnCariID.UseVisualStyleBackColor = true;
            // 
            // btnCariOwner
            // 
            this.btnCariOwner.Location = new System.Drawing.Point(205, 184);
            this.btnCariOwner.Name = "btnCariOwner";
            this.btnCariOwner.Size = new System.Drawing.Size(75, 23);
            this.btnCariOwner.TabIndex = 47;
            this.btnCariOwner.Text = "Pilih Owner";
            this.btnCariOwner.UseVisualStyleBackColor = true;
            // 
            // btnSemuaProperty
            // 
            this.btnSemuaProperty.Location = new System.Drawing.Point(355, 272);
            this.btnSemuaProperty.Name = "btnSemuaProperty";
            this.btnSemuaProperty.Size = new System.Drawing.Size(130, 23);
            this.btnSemuaProperty.TabIndex = 59;
            this.btnSemuaProperty.Text = "Lihat Semua Property";
            this.btnSemuaProperty.UseVisualStyleBackColor = true;
            // 
            // btnGambarProperty
            // 
            this.btnGambarProperty.Location = new System.Drawing.Point(508, 272);
            this.btnGambarProperty.Name = "btnGambarProperty";
            this.btnGambarProperty.Size = new System.Drawing.Size(130, 23);
            this.btnGambarProperty.TabIndex = 60;
            this.btnGambarProperty.Text = "Lihat Gambar Property";
            this.btnGambarProperty.UseVisualStyleBackColor = true;
            // 
            // dgProperty
            // 
            this.dgProperty.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgProperty.Location = new System.Drawing.Point(26, 372);
            this.dgProperty.Name = "dgProperty";
            this.dgProperty.Size = new System.Drawing.Size(630, 173);
            this.dgProperty.TabIndex = 115;
            // 
            // btnBatal
            // 
            this.btnBatal.Location = new System.Drawing.Point(226, 336);
            this.btnBatal.Name = "btnBatal";
            this.btnBatal.Size = new System.Drawing.Size(54, 23);
            this.btnBatal.TabIndex = 152;
            this.btnBatal.Text = "Batal";
            this.btnBatal.UseVisualStyleBackColor = true;
            // 
            // btnHapus
            // 
            this.btnHapus.Location = new System.Drawing.Point(162, 336);
            this.btnHapus.Name = "btnHapus";
            this.btnHapus.Size = new System.Drawing.Size(54, 23);
            this.btnHapus.TabIndex = 151;
            this.btnHapus.Text = "Hapus";
            this.btnHapus.UseVisualStyleBackColor = true;
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(96, 336);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(54, 23);
            this.btnUpdate.TabIndex = 150;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.UseVisualStyleBackColor = true;
            // 
            // Simpan
            // 
            this.Simpan.Location = new System.Drawing.Point(33, 336);
            this.Simpan.Name = "Simpan";
            this.Simpan.Size = new System.Drawing.Size(54, 23);
            this.Simpan.TabIndex = 149;
            this.Simpan.Text = "Tambah";
            this.Simpan.UseVisualStyleBackColor = true;
            // 
            // txtUkuran
            // 
            this.txtUkuran.Location = new System.Drawing.Point(102, 148);
            this.txtUkuran.Name = "txtUkuran";
            this.txtUkuran.Size = new System.Drawing.Size(178, 20);
            this.txtUkuran.TabIndex = 153;
            // 
            // txtOwner
            // 
            this.txtOwner.Location = new System.Drawing.Point(102, 186);
            this.txtOwner.Name = "txtOwner";
            this.txtOwner.Size = new System.Drawing.Size(100, 20);
            this.txtOwner.TabIndex = 155;
            // 
            // FormProperty
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(681, 563);
            this.Controls.Add(this.txtOwner);
            this.Controls.Add(this.txtUkuran);
            this.Controls.Add(this.btnBatal);
            this.Controls.Add(this.btnHapus);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.Simpan);
            this.Controls.Add(this.dgProperty);
            this.Controls.Add(this.btnGambarProperty);
            this.Controls.Add(this.btnSemuaProperty);
            this.Controls.Add(this.btnCariOwner);
            this.Controls.Add(this.btnCariID);
            this.Controls.Add(this.cbTipe);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.txtAlamat);
            this.Controls.Add(this.txtID);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormProperty";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FormProperty";
            ((System.ComponentModel.ISupportInitialize)(this.dgProperty)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox txtAlamat;
        private System.Windows.Forms.TextBox txtID;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox cbTipe;
        private System.Windows.Forms.Button btnCariID;
        private System.Windows.Forms.Button btnCariOwner;
        private System.Windows.Forms.Button btnSemuaProperty;
        private System.Windows.Forms.Button btnGambarProperty;
        private System.Windows.Forms.DataGridView dgProperty;
        private System.Windows.Forms.Button btnBatal;
        private System.Windows.Forms.Button btnHapus;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button Simpan;
        private System.Windows.Forms.TextBox txtUkuran;
        private System.Windows.Forms.TextBox txtOwner;
    }
}