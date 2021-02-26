using MaterialSkin2DotNet;
using MaterialSkin2DotNet.Controls;
using System;
using System.Drawing;
using System.Windows.Forms;
using ValheimPlusManager.Data;
using ValheimPlusManager.Models;
using ValheimPlusManager.SupportClasses;

namespace ValheimPlusManager
{
    public partial class MainForm : MaterialForm
    {
        private bool ValheimPlusInstalledClient { get; set; }
        private bool ValheimPlusInstalledServer { get; set; }
        private Settings Settings { get; set; }

        public MainForm()
        {
            InitializeComponent();

            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            materialSkinManager.ColorScheme = new ColorScheme(Primary.BlueGrey800, Primary.BlueGrey900, Primary.BlueGrey500, Accent.LightBlue400, TextShade.WHITE);
            this.Icon = Properties.Resources.valheim_plus;
            statusLabel.Text = "";

            // Fetching path settings
            try
            {
                Settings = SettingsDAL.GetSettings();

                // Checking installation status
                ValheimPlusInstalledClient = Validation.CheckInstallationStatus(Settings.ClientInstallationPath);
                ValheimPlusInstalledServer = Validation.CheckInstallationStatus(Settings.ServerInstallationPath);

                if (ValheimPlusInstalledClient)
                {
                    clientInstalledMaterialLabel.Text = String.Format("ValheimPlus {0} installed on client", Settings.ValheimPlusGameClientVersion);
                    clientInstalledMaterialLabel.ForeColor = Color.Green;
                    installClientMaterialButton.Text = "Reinstall ValheimPlus on client";
                }
                else
                {
                    clientInstalledMaterialLabel.Text = "ValheimPlus not installed on client";
                    clientInstalledMaterialLabel.ForeColor = Color.Red;
                }

                if (ValheimPlusInstalledServer)
                {
                    serverInstalledMaterialLabel.Text = String.Format("ValheimPlus {0} installed on server", Settings.ValheimPlusServerClientVersion);
                    serverInstalledMaterialLabel.ForeColor = Color.Green;
                    installServerMaterialButton.Text = "Reinstall ValheimPlus on server";
                }
                else
                {
                    installServerMaterialButton.Text = "ValheimPlus not installed on server";
                    installServerMaterialButton.ForeColor = Color.Red;
                }

                installServerUpdateMaterialButton.Enabled = false;
                installClientUpdateMaterialButton.Enabled = false;
            }
            catch (Exception)
            {
                clientMaterialCard.Hide();
                serverMaterialCard.Hide();
                statusLabel.ForeColor = Color.Red;
                statusLabel.Text = "ERROR! Settings file not found, reinstall manager.";
            }
        }

        private void installClientMaterialButton_Click(object sender, EventArgs e)
        {

        }

        private async void checkCLientUpdatesMaterialButton_Click(object sender, EventArgs e)
        {
            ValheimPlusUpdate valheimPlusUpdate = await UpdateManager.CheckForValheimPlusUpdatesAsync(Settings.ValheimPlusGameClientVersion);

            if (valheimPlusUpdate.NewVersion)
            {
                installClientUpdateMaterialButton.Text = String.Format("Install update {0}", valheimPlusUpdate.Version);
                installClientUpdateMaterialButton.Enabled = true;
            }
            else
            {
                statusLabel.ForeColor = Color.Red;
                statusLabel.Text = "No new game client updates available";
            }
        }

        private void manageClientMaterialButton_Click(object sender, EventArgs e)
        {
            new ConfigEditor(true).Show(); // Bool determines if user will manage conf. for server or game client
        }

        private async void installClientUpdateMaterialButton_Click(object sender, EventArgs e)
        {
            installClientUpdateMaterialButton.Enabled = false;

            ValheimPlusUpdate valheimPlusUpdate = await UpdateManager.CheckForValheimPlusUpdatesAsync(Settings.ValheimPlusGameClientVersion);

            if (valheimPlusUpdate.NewVersion)
            {
                bool success = await UpdateManager.DownloadValheimPlusUpdateAsync(Settings.ValheimPlusGameClientVersion, true);

                if (success)
                {
                    Settings = SettingsDAL.GetSettings();
                    clientInstalledMaterialLabel.Text = String.Format("ValheimPlus {0} installed on client", Settings.ValheimPlusGameClientVersion);
                    statusLabel.ForeColor = Color.Green;
                    statusLabel.Text = "Success! Game client updated to latest version.";
                    installClientUpdateMaterialButton.Hide();
                }
            }
        }

        private void installServerMaterialButton_Click(object sender, EventArgs e)
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
                        serverInstalledMaterialLabel.Text = String.Format("ValheimPlus {0} installed on server", Settings.ValheimPlusServerClientVersion);
                        serverInstalledMaterialLabel.ForeColor = Color.Green;
                        installServerMaterialButton.Text = "Reinstall ValheimPlus on server";
                        statusLabel.ForeColor = Color.Green;
                        statusLabel.Text = "Success! Server client updated to latest version.";
                    }
                }
                catch (Exception)
                {
                    throw new Exception(); // ToDo - handling of errors
                }
            }
        }

        private void manageServerMaterialButton_Click(object sender, EventArgs e)
        {
            new ConfigEditor(false).Show(); // Bool determines if user will manage conf. for server or game client
        }

        private async void checkServerUpdatesMaterialButton_Click(object sender, EventArgs e)
        {
            ValheimPlusUpdate valheimPlusUpdate = await UpdateManager.CheckForValheimPlusUpdatesAsync(Settings.ValheimPlusServerClientVersion);

            if (valheimPlusUpdate.NewVersion)
            {
                installServerUpdateMaterialButton.Text = String.Format("Install update {0}", valheimPlusUpdate.Version);
                installServerUpdateMaterialButton.Enabled = true;
            }
            else
            {
                statusLabel.ForeColor = Color.Red;
                statusLabel.Text = "No new server updates available";
            }
        }

        private async void installServerUpdateMaterialButton_Click(object sender, EventArgs e)
        {
            installServerUpdateMaterialButton.Enabled = false;

            ValheimPlusUpdate valheimPlusUpdate = await UpdateManager.CheckForValheimPlusUpdatesAsync(Settings.ValheimPlusServerClientVersion);

            if (valheimPlusUpdate.NewVersion)
            {
                bool success = await UpdateManager.DownloadValheimPlusUpdateAsync(Settings.ValheimPlusServerClientVersion, false);

                if (success)
                {
                    Settings = SettingsDAL.GetSettings();
                    serverInstalledMaterialLabel.Text = String.Format("ValheimPlus {0} installed on server", Settings.ValheimPlusServerClientVersion);
                    statusLabel.ForeColor = Color.Green;
                    statusLabel.Text = "Success! Server client updated to latest version.";
                    installServerUpdateMaterialButton.Enabled = false;
                }
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
