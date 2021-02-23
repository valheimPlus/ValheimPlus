using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ValheimPlusManager.Data;
using ValheimPlusManager.Models;
using ValheimPlusManager.SupportClasses;

namespace ValheimPlusManager
{
    public partial class Form1 : Form
    {
        private bool ValheimPlusInstalledClient { get; set; }
        private bool ValheimPlusInstalledServer { get; set; }
        private Settings settings { get; set; }

        public Form1()
        {
            InitializeComponent();

            // Fetching path settings
            settings = SettingsDAL.GetSettings();

            // Checking installation status
            ValheimPlusInstalledClient = Validation.CheckInstallationStatus(settings.ClientInstallationPath);
            ValheimPlusInstalledServer = Validation.CheckInstallationStatus(settings.ServerInstallationPath);

            if(ValheimPlusInstalledClient)
            {
                clientInstalledLabel.Text = "ValheimPlus v0.8.5 installed on client";
                clientInstalledLabel.ForeColor = Color.Green;
                installClientButton.Enabled = false;
            }
            else
            {
                clientInstalledLabel.Text = "ValheimPlus not installed on client";
                clientInstalledLabel.ForeColor = Color.Red;
                installClientButton.Enabled = true;
            }

            if(ValheimPlusInstalledServer)
            {
                serverInstalledLabel.Text = "ValheimPlus v0.8.5 installed on server";
                serverInstalledLabel.ForeColor = Color.Green;
                installServerButton.Text = "Reinstall ValheimPlus on server";
            }
            else
            {
                serverInstalledLabel.Text = "ValheimPlus not installed on server";
                serverInstalledLabel.ForeColor = Color.Red;
            }
        }

        private void installServerButton_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult;

            if (!ValheimPlusInstalledServer)
            {
                dialogResult = MessageBox.Show("Are you sure you wish to install ValheimPlus on your server?", "Confirm", MessageBoxButtons.YesNo);
            }
            else
            {
                dialogResult = MessageBox.Show("Are you sure you wish to update/reinstall ValheimPlus on your server?", "Confirm", MessageBoxButtons.YesNo);
            }

            if (dialogResult == DialogResult.Yes)
            {
                try
                {
                    FileManager.InstallValheimPlus(settings.ServerPath, settings.ServerInstallationPath);
                    ValheimPlusInstalledServer = Validation.CheckInstallationStatus(settings.ServerInstallationPath);
                    if (ValheimPlusInstalledServer)
                    {
                        serverInstalledLabel.Text = "ValheimPlus v0.8.5 installed on server";
                        serverInstalledLabel.ForeColor = Color.Green;
                        installServerButton.Text = "Reinstall ValheimPlus on server";
                    }
                }
                catch (Exception)
                {
                    throw new Exception();
                }
            }
        }

        private void manageServerButton_Click(object sender, EventArgs e)
        {
            new ConfigEditor(false).Show();
        }

        private void manageClientButton_Click(object sender, EventArgs e)
        {
            new ConfigEditor(true).Show();
        }
    }
}
