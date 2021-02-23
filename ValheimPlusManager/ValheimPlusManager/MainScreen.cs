using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace ValheimPlusManager
{
    public partial class MainScreen : Form
    {
		public MainScreen()
		{
			Thread t = new Thread(new ThreadStart(StartForm));
			t.Start();
			Thread.Sleep(5000);
			InitializeComponent();
		}

		public void StartForm()
		{
			Application.Run(new SplashScreen());
		}

		private void MainScreen_Load(object sender, EventArgs e)
		{

		}
	}
}
