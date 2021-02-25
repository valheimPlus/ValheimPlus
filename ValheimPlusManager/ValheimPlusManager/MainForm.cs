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
        private Settings Settings { get; set; }

        public MainForm()
        {
            InitializeComponent();
            this.Icon = Properties.Resources.valheim_plus;

            // Fetching path settings
            try
            {
                Settings = SettingsDAL.GetSettings();

                // Checking installation status
                ValheimPlusInstalledClient = Validation.CheckInstallationStatus(Settings.ClientInstallationPath);
                ValheimPlusInstalledServer = Validation.CheckInstallationStatus(Settings.ServerInstallationPath);

                if (ValheimPlusInstalledClient)
                {
                    clientInstalledLabel.Text = String.Format("ValheimPlus {0} installed on client", Settings.ValheimPlusGameClientVersion);
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
                    serverInstalledLabel.Text = String.Format("ValheimPlus {0} installed on server", Settings.ValheimPlusServerClientVersion);
                    serverInstalledLabel.ForeColor = Color.Green;
                    installServerButton.Text = "Reinstall ValheimPlus on server";
                }
                else
                {
                    serverInstalledLabel.Text = "ValheimPlus not installed on server";
                    serverInstalledLabel.ForeColor = Color.Red;
                }

                installServerUpdateIconButton.Hide();
                installClientUpdateIconButton.Hide();
            }
            catch (Exception)
            {
                clientPanel.Hide();
                serverPanel.Hide();
                statusLabel.ForeColor = Color.Red;
                statusLabel.Text = "ERROR! Settings file not found, reinstall manager.";
            }
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
                    .Show("Are you sure you wish to reinstall ValheimPlus on your server? This will overwrite your current configurations!", "Confirm", MessageBoxButtons.YesNo);
            }

            if (dialogResult == DialogResult.Yes)
            {
                try
                {
                    FileManager.InstallValheimPlus(Settings.ServerPath, Settings.ServerInstallationPath);
                    ValheimPlusInstalledServer = Validation.CheckInstallationStatus(Settings.ServerInstallationPath);
                    if (ValheimPlusInstalledServer)
                    {
                        serverInstalledLabel.Text = String.Format("ValheimPlus {0} installed on server", Settings.ValheimPlusServerClientVersion);
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

        private async void checkCLientUpdatesIconButton_ClickAsync(object sender, EventArgs e)
        {
            ValheimPlusUpdate valheimPlusUpdate = await UpdateManager.CheckForValheimPlusUpdatesAsync(Settings.ValheimPlusGameClientVersion);

            if (valheimPlusUpdate.NewVersion)
            {
                installClientUpdateIconButton.Text = String.Format("Install update {0}", valheimPlusUpdate.Version);
                installClientUpdateIconButton.Show();
            }
            else
            {
                statusLabel.ForeColor = Color.Red;
                statusLabel.Text = "No new client updates available";
            }
        }

        private async void checkServerUpdatesIconButton_Click_1Async(object sender, EventArgs e)
        {
            ValheimPlusUpdate valheimPlusUpdate = await UpdateManager.CheckForValheimPlusUpdatesAsync(Settings.ValheimPlusServerClientVersion);

            if (valheimPlusUpdate.NewVersion)
            {
                installServerUpdateIconButton.Text = String.Format("Install update {0}", valheimPlusUpdate.Version);
                installServerUpdateIconButton.Show();
            }
            else
            {
                statusLabel.ForeColor = Color.Red;
                statusLabel.Text = "No new server updates available";
            }
        }

        private async void installServerUpdateIconButton_ClickAsync(object sender, EventArgs e)
        {
            installServerUpdateIconButton.Enabled = false;

            ValheimPlusUpdate valheimPlusUpdate = await UpdateManager.CheckForValheimPlusUpdatesAsync(Settings.ValheimPlusServerClientVersion);

            if (valheimPlusUpdate.NewVersion)
            {
                bool success = await UpdateManager.DownloadValheimPlusUpdateAsync(Settings.ValheimPlusServerClientVersion, false);

                if (success)
                {
                    Settings = SettingsDAL.GetSettings();
                    serverInstalledLabel.Text = String.Format("ValheimPlus {0} installed on server", Settings.ValheimPlusServerClientVersion);
                    statusLabel.ForeColor = Color.Green;
                    statusLabel.Text = "Success! Server updated to latest version.";
                    installServerUpdateIconButton.Hide();
                }
            }
        }

        private async void installClientUpdateIconButton_Click(object sender, EventArgs e)
        {
            installClientUpdateIconButton.Enabled = false;

            ValheimPlusUpdate valheimPlusUpdate = await UpdateManager.CheckForValheimPlusUpdatesAsync(Settings.ValheimPlusGameClientVersion);

            if (valheimPlusUpdate.NewVersion)
            {
                bool success = await UpdateManager.DownloadValheimPlusUpdateAsync(Settings.ValheimPlusGameClientVersion, true);

                if(success)
                {
                    Settings = SettingsDAL.GetSettings();
                    clientInstalledLabel.Text = String.Format("ValheimPlus {0} installed on client", Settings.ValheimPlusGameClientVersion);
                    statusLabel.ForeColor = Color.Green;
                    statusLabel.Text = "Success! Client updated to latest version.";
                    installClientUpdateIconButton.Hide();
                }
            }
        }
    }
}
