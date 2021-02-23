using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ValheimPlusManager.SupportClasses;

namespace ValheimPlusManager
{
    public partial class Form1 : Form
    {
        private string ClientInstallationPath = "C:/Program Files (x86)/Steam/steamapps/common/Valheim/";
        //private string ServerInstallationPath = "C:/Program Files (x86)/Steam/steamapps/common/Valheim dedicated server/BepInEx/plugins/";
        private string ServerInstallationPath = "C:/Users/msn/Desktop/ServerTest/";

        private string ValheimPlusClientSource = "C:/Users/msn/Downloads/WindowsClient";
        private bool ValheimPlusInstalledClient { get; set; }
        private bool ValheimPlusInstalledServer { get; set; }

        public Form1()
        {
            InitializeComponent();

            ValheimPlusInstalledClient = Validation.CheckInstallationStatus(ClientInstallationPath);
            ValheimPlusInstalledServer = Validation.CheckInstallationStatus(ServerInstallationPath);

            if(ValheimPlusInstalledClient)
            {
                clientInstalledLabel.Text = "ValheimPlus installed on client";
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
                serverInstalledLabel.Text = "ValheimPlus installed on server";
                serverInstalledLabel.ForeColor = Color.Green;
                installServerButton.Text = "Update/reinstall ValheimPlus on server";
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
                    FileManager.InstallValheimPlus(ValheimPlusClientSource, ServerInstallationPath);
                    ValheimPlusInstalledServer = Validation.CheckInstallationStatus(ServerInstallationPath);
                    if (ValheimPlusInstalledServer)
                    {
                        serverInstalledLabel.Text = "ValheimPlus installed on server";
                        serverInstalledLabel.ForeColor = Color.Green;
                        installServerButton.Text = "Update/reinstall ValheimPlus on server";
                    }
                }
                catch (Exception)
                {
                    throw new Exception();
                }
            }
        }
    }
}
