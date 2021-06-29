using Bunifu.Framework.UI;
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
    public partial class Splash : Form
    {
        public Splash()
        {
            InitializeComponent();
        }

        int startpoint = 0;

        private void timer1_Tick(object sender, EventArgs e)
        {
            startpoint += 1;
            MyProgress.Value = startpoint;
            if (MyProgress.Value == 100)
            {
                MyProgress.Value = 0;
                timer1.Stop();
                Login Login = new Login();
                this.Hide();
                Login.Show();
            }
        }

        private void Splash_Load(object sender, EventArgs e)
        {
            //Bunifu.UI.WinForms.BunifuCircleProgress bunifuCircleProgressbar1 = new Bunifu.UI.WinForms.bunifuCircleProgressbar1();
            bunifuCircleProgressbar1.Value = 20;
            bunifuCircleProgressbar1.LineProgressThickness = 10;
            bunifuCircleProgressbar1.LineThickness = 20;
            bunifuCircleProgressbar1.Text = bunifuCircleProgressbar1.Value.ToString();
            this.Controls.Add(bunifuCircleProgressbar1);


            //timer1.Start();
        }

    private void bunifuCircleProgressbar1_Load(object sender, EventArgs e)
        {
            startpoint += 1;
            MyProgress.Value = startpoint;
            if (MyProgress.Value == 100)
            {
                MyProgress.Value = 0;
                timer1.Stop();
                Login Login = new Login();
                this.Hide();
                Login.Show();
            }
        }

        private void bunifuProgressBar1_progressChanged(object sender, EventArgs e)
        {

        }
    }
}
