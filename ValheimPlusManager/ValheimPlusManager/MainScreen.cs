using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using ValheimPlusManager.SupportClasses;

namespace ValheimPlusManager
{
    public partial class MainScreen : Form
    {
		public MainScreen()
		{
			InitializeComponent();
			ConfigManager.ReadConfigFile();
		}

		private void MainScreen_Load(object sender, EventArgs e)
		{

		}
	}
}
