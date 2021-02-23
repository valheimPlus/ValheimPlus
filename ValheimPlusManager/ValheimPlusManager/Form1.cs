using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ValheimPlusManager
{
    public partial class Form1 : Form
    {
        private bool valheimPlusInstalledClient { get; set; }
        private bool valheimPlusInstalledServer { get; set; }

        public Form1()
        {
            InitializeComponent();
            // Checking if ValheimPlus is already installed for the client and/or server
            valheimPlusInstalledClient = System.IO.File.Exists("C:/Program Files (x86)/Steam/steamapps/common/Valheim/BepInEx/plugins/ValheimPlus.dll");
            valheimPlusInstalledServer = System.IO.File.Exists("C:/Program Files (x86)/Steam/steamapps/common/Valheim dedicated server/BepInEx/plugins/ValheimPlus.dll");

            if(valheimPlusInstalledClient)
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

            if(valheimPlusInstalledServer)
            {
                serverInstalledLabel.Text = "ValheimPlus installed on server";
                serverInstalledLabel.ForeColor = Color.Green;
                installServerButton.Enabled = false;
            }
            else
            {
                serverInstalledLabel.Text = "ValheimPlus not installed on server";
                serverInstalledLabel.ForeColor = Color.Red;
                installServerButton.Enabled = true;
            }
        }
    }
}
