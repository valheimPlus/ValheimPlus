using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using ValheimPlusManager.Models;
using ValheimPlusManager.SupportClasses;

namespace ValheimPlusManager
{
    public partial class ConfigEditor : Form
    {
		private ValheimPlusConf valheimPlusConf { get; set; }

		public ConfigEditor(bool manageClient)
		{
			InitializeComponent();
			valheimPlusConf = ConfigManager.ReadConfigFile();
			enterAdvancedBuildingModeTextBox.Text = valheimPlusConf.enterAdvancedBuildingMode;
		}

		private void MainScreen_Load(object sender, EventArgs e)
		{

		}

        private void configCheckedListBox_Click(object sender, EventArgs e)
        {

        }

        private void saveConfigButton_Click(object sender, EventArgs e)
        {

        }
    }
}
