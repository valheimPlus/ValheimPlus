using SetupDevEnvironment.IO;
using System.ComponentModel;

#nullable disable
namespace SetupDevEnvironment
{
    public partial class SetupForm : Form
    {
        private string ValheimInstallPath 
        { 
            get => Settings.ValheimInstallDir; 
            set { Settings.ValheimInstallDir = value; } 
        }
        private string ValheimPlusInstallPath 
        { 
            get => Settings.ValheimPlusDevInstallDir; 
            set { Settings.ValheimPlusDevInstallDir = value; } 
        }

        public SetupForm()
        {
            InitializeComponent();
            EnableStartButton();
        }

        private void EnableStartButton()
        {
            if (Directory.Exists(tbValheimInstallDir.Text) &&
                Directory.Exists(tbValheimPlusInstallDir.Text) &&
                tbValheimInstallDir.Text != tbValheimPlusInstallDir.Text)
            {
                btStartInstallation.Enabled = true;
            }
            else
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

            if (ValheimPlusInstallPath == Links.DefaultValheimPlusDevInstallFolder)
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

            if (path == ValheimInstallPath)
            {
                MessageBox.Show("Folders can't be the same. Let's try that again.");
                return;
            }

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

            var script = new InstallScript();
            script.OnLogMessage += OnLogMessage;
            await script.Install();

            btEditConfig.Enabled = true;
            btStartDnSpy.Enabled = true;

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

        /// <summary>
        /// yes, old school. but it works.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnLogMessage(object? sender, ProgressChangedEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(UpdateLog, (string)e.UserState);
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
            ProcessRunner.Run(Path.Combine(ValheimPlusInstallPath, "BepInEx\\config\\BepInEx.cfg"));
        }

        private void btStartDnSpy_Click(object sender, EventArgs e)
        {
            ProcessRunner.Run(Path.Combine(Links.DnSpy64TargetFolder, "dnSpy.exe"));
        }
    }
}