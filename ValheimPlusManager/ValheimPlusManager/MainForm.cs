using System;
using System.Drawing;
using System.Windows.Forms;
using ValheimPlusManager.Data;
using ValheimPlusManager.Models;
using ValheimPlusManager.SupportClasses;

namespace ValheimPlusManager
{
    public partial class MainForm : Form
    {
        private bool ValheimPlusInstalledClient { get; set; }
        private bool ValheimPlusInstalledServer { get; set; }
        private Settings settings { get; set; }

        public MainForm()
        {
            InitializeComponent();
            this.Icon = Properties.Resources.valheim_plus;

            // Fetching path settings
            settings = SettingsDAL.GetSettings();

            // Checking installation status
            ValheimPlusInstalledClient = Validation.CheckInstallationStatus(settings.ClientInstallationPath);
            ValheimPlusInstalledServer = Validation.CheckInstallationStatus(settings.ServerInstallationPath);

            if (ValheimPlusInstalledClient)
            {
                clientInstalledLabel.Text = String.Format("ValheimPlus {0} installed on client", settings.ValheimPlusVersion);
                clientInstalledLabel.ForeColor = Color.Green;
                installClientButton.Text = "Reinstall ValheimPlus on client";
            }
            else
            {
                clientInstalledLabel.Text = "ValheimPlus not installed on client";
                clientInstalledLabel.ForeColor = Color.Red;
            }

            if (ValheimPlusInstalledServer)
            {
                serverInstalledLabel.Text = String.Format("ValheimPlus {0} installed on server", settings.ValheimPlusVersion);
                serverInstalledLabel.ForeColor = Color.Green;
                installServerButton.Text = "Reinstall ValheimPlus on server";
            }
            else
            {
                serverInstalledLabel.Text = "ValheimPlus not installed on server";
                serverInstalledLabel.ForeColor = Color.Red;
            }

            installServerUpdateIconButton.Hide();
        }

        private void installServerButton_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult;

            if (!ValheimPlusInstalledServer)
            {
                dialogResult = MessageBox
                    .Show("Are you sure you wish to install ValheimPlus on your server?", "Confirm", MessageBoxButtons.YesNo);
            }
            else
            {
                dialogResult = MessageBox
                    .Show("Are you sure you wish to update/reinstall ValheimPlus on your server? This will overwrite your current configurations!", "Confirm", MessageBoxButtons.YesNo);
            }

            if (dialogResult == DialogResult.Yes)
            {
                try
                {
                    FileManager.InstallValheimPlus(settings.ServerPath, settings.ServerInstallationPath);
                    ValheimPlusInstalledServer = Validation.CheckInstallationStatus(settings.ServerInstallationPath);
                    if (ValheimPlusInstalledServer)
                    {
                        serverInstalledLabel.Text = String.Format("ValheimPlus {0} installed on server", settings.ValheimPlusVersion);
                        serverInstalledLabel.ForeColor = Color.Green;
                        installServerButton.Text = "Reinstall ValheimPlus on server";
                    }
                }
                catch (Exception)
                {
                    throw new Exception(); // ToDo - handling of errors
                }
            }
        }

        private void manageServerButton_Click(object sender, EventArgs e)
        {
            new ConfigEditor(false).Show(); // Bool determines if user will manage conf. for server or game client
        }


        private void manageClientButton_Click(object sender, EventArgs e)
        {
            new ConfigEditor(true).Show(); // Bool determines if user will manage conf. for server or game client
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void iconButton1_Click(object sender, EventArgs e)
        {
            UpdateManager.CheckForValheimPlusUpdates(settings.ValheimPlusVersion);
        }

        private void checkServerUpdatesIconButton_Click_1(object sender, EventArgs e)
        {
            ValheimPlusUpdate valheimPlusUpdate = UpdateManager.CheckForValheimPlusUpdates(settings.ValheimPlusVersion);

            if (valheimPlusUpdate.NewVersion)
            {
                installServerUpdateIconButton.Text = String.Format("Install update {0}", valheimPlusUpdate.Version);
                installServerUpdateIconButton.Show();
            }
        }
    }
}
