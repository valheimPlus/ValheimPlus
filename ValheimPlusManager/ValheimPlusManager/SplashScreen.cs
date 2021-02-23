using System;
using System.Windows.Forms;

namespace ValheimPlusManager
{
    public partial class SplashScreen : Form
    {
        int timeLeft = 100;
        public SplashScreen()
        {
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (timeLeft > 0)
            {
                timeLeft = timeLeft - 1;
            }
            else
            {
                timer1.Stop();
                new MainForm().Show();
                this.Hide();
            }
        }
        private void SplashScreen_Load(object sender, EventArgs e)
        {
            timer1.Start();
        }
    }
}
