using MaterialSkin2DotNet;
using MaterialSkin2DotNet.Controls;
using System;
using System.Windows.Forms;

namespace ValheimPlusManager
{
    public partial class SplashScreen : MaterialForm
    {
        int timeLeft = 10;
        public SplashScreen()
        {
            InitializeComponent();

            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            materialSkinManager.ColorScheme = new ColorScheme(Primary.BlueGrey800, Primary.BlueGrey900, Primary.BlueGrey500, Accent.LightBlue200, TextShade.WHITE);
            this.Icon = Properties.Resources.valheim_plus;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (timeLeft > 0)
            {
                timeLeft -= 1;
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
