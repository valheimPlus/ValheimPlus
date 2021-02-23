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

		TabControl.TabPageCollection tabSetup { get; set; }

		public ConfigEditor(bool manageClient)
		{
			InitializeComponent();
			valheimPlusConf = ConfigManager.ReadConfigFile();
			tabSetup = tabControl1.TabPages;

			enterAdvancedBuildingModeTextBox.Text = valheimPlusConf.enterAdvancedBuildingMode;
			exitAdvancedBuildingModeTextBox.Text = valheimPlusConf.exitAdvancedBuildingMode;

			if(valheimPlusConf.advancedBuildingModeEnabled)
            {
				configCheckedListBox.SetItemChecked(0, valheimPlusConf.advancedBuildingModeEnabled);
				advancedBuildingModeTab.Enabled = true;
			}

			for (int i = 0; i < configCheckedListBox.Items.Count; i++)
			{
				if (configCheckedListBox.GetItemChecked(i))
				{
					tabSetup[i].Enabled = true;
				}
				else if (!configCheckedListBox.GetItemChecked(i))
				{
					tabSetup[i].Enabled = false;
				}
			}
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

        private void configCheckedListBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
			this.BeginInvoke(new Action(() =>
			{
                for (int i = 0; i < configCheckedListBox.Items.Count; i++)
                {
					if (configCheckedListBox.GetItemChecked(i))
					{
						tabSetup[i].Enabled = true;
					}
					else if (!configCheckedListBox.GetItemChecked(i))
					{
						tabSetup[i].Enabled = false;
					}
				}
			}));
		}
    }
}
