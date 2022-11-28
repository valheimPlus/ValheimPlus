using SetupDevEnvironment.IO;
using System.ComponentModel;
using System.Diagnostics;
using static System.Net.Mime.MediaTypeNames;

namespace SetupDevEnvironment
{
    public partial class SetupForm : Form
    {
        private string? ValheimInstallPath { get; set; } = null;
        private string? ValheimPlusInstallPath { get; set; } = null;
        private Task _installTask = Task.CompletedTask;

        public SetupForm()
        {
            InitializeComponent();
        }

        private void EnableStartButton()
        {
            if (Directory.Exists(tbValheimInstallDir.Text) &&
                Directory.Exists(tbValheimPlusInstallDir.Text))
            {
                btStartInstallation.Enabled = true;
            } else
            {
                btStartInstallation.Enabled = false;
            }
        }

        private void btBrowseValheimInstallDir_Click(object sender, EventArgs e)
        {
            var path = GetFolder(Links.DefaultValheimInstallFolder);
            if (path == string.Empty)
                return;

            ValheimInstallPath = path;
            tbValheimInstallDir.Text = ValheimInstallPath;
            tbValheimInstallDir.Invalidate();

            if (string.IsNullOrEmpty(ValheimPlusInstallPath))
            {
                var steamRoot = Directory.GetParent(path);
                ValheimPlusInstallPath = Path.Combine(steamRoot.FullName, "Valheim Plus Development");
                tbValheimPlusInstallDir.Text = ValheimPlusInstallPath;
                tbValheimPlusInstallDir.Invalidate();
            }

            EnableStartButton();
        }

        private void btBrowseVPlusInstallDir_Click(object sender, EventArgs e)
        {
            var path = GetFolder(Links.DefaultValheimPlusDevInstallFolder);
            if (path == string.Empty)
                return;

            ValheimPlusInstallPath = path;
            tbValheimPlusInstallDir.Text = ValheimPlusInstallPath;
            tbValheimPlusInstallDir.Invalidate();

            EnableStartButton();
        }

        private async void btStartInstallation_Click(object sender, EventArgs e)
        {
            btStartInstallation.Enabled = false;
            tbLog.Enabled = false;
            tbLog.Visible = true;

            var script = new InstallScript(ValheimInstallPath, ValheimPlusInstallPath);
            script.OnLogMessage += OnLogMessage;
            await script.Install();

            btEditConfig.Enabled = true;

            EnableStartButton();
        }

        private static string GetFolder(string initialPath)
        {
            var odd = new FolderBrowserDialog
            {
                InitialDirectory = initialPath,
            };

            var result = odd.ShowDialog();

            if (result != DialogResult.OK)
            {
                return string.Empty;
            }

            return odd.SelectedPath;
        }


        private void OnLogMessage(object? sender, ProgressChangedEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke((Action<string>)UpdateLog, (string)e.UserState);
                return;
            }
            UpdateLog((string)e.UserState);
        }

        private void UpdateLog(string text)
        {
            tbLog.AppendText(text + Environment.NewLine);
        }

        private void btEditConfig_Click(object sender, EventArgs e)
        {
            var process = new Process();
            process.StartInfo = new ProcessStartInfo()
            {
                UseShellExecute = true,
                FileName = Path.Combine(ValheimPlusInstallPath, "BepInEx\\config\\BepInEx.cfg")
            };

            process.Start();
            process.WaitForExit();
        }
    }
}