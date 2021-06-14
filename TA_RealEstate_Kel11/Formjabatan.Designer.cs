namespace TA_RealEstate_Kel11
{
    partial class FormJabatan
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormJabatan));
            this.charTextBox1 = new TA_RealEstate_Kel11.CharTextBox();
            this.SuspendLayout();
            // 
            // charTextBox1
            // 
            this.charTextBox1.Location = new System.Drawing.Point(251, 63);
            this.charTextBox1.Name = "charTextBox1";
            this.charTextBox1.Size = new System.Drawing.Size(100, 20);
            this.charTextBox1.TabIndex = 0;
            // 
            // FormJabatan
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(486, 332);
            this.Controls.Add(this.charTextBox1);
            this.Name = "FormJabatan";
            this.Text = "FormJabatan";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private CharTextBox charTextBox1;
    }
}