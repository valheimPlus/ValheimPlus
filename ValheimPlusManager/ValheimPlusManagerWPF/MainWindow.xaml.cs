using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Media;
using ValheimPlusManager.Data;
using ValheimPlusManager.Models;
using ValheimPlusManager.SupportClasses;

namespace ValheimPlusManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool ValheimPlusInstalledClient { get; set; } = false;
        private bool ValheimPlusInstalledServer { get; set; } = false;
        private Settings Settings { get; set; }

        public void UISettingsInit(bool clientPathCorrect, bool serverPathCorrect)
        {
            if (clientPathCorrect)
            {
                if (ValheimPlusInstalledClient)
                {
                    clientInstalledLabel.Content = String.Format("ValheimPlus {0} installed on client", Settings.ValheimPlusGameClientVersion);
                    clientInstalledLabel.Foreground = Brushes.Green;
                    installClientButton.Content = "Reinstall ValheimPlus on client";

                    var modActive = File.Exists(String.Format("{0}winhttp.dll", Settings.ClientInstallationPath));
                    if (modActive)
                    {
                        enableDisableValheimPlusGameClientButton.Content = "Disable ValheimPlus";
                        enableDisableValheimPlusGameClientButton.Style = Application.Current.TryFindResource("MaterialDesignOutlinedButton") as Style;
                    }
                    else
                    {
                        enableDisableValheimPlusGameClientButton.Content = "Enable ValheimPlus";
                        enableDisableValheimPlusGameClientButton.Style = Application.Current.TryFindResource("MaterialDesignRaisedButton") as Style;
                    }

                    installClientButton.Visibility = Visibility.Visible;
                    manageClientButton.Visibility = Visibility.Visible;
                    installClientUpdateButton.Visibility = Visibility.Visible;
                    checkClientUpdatesButtons.Visibility = Visibility.Visible;
                    enableDisableValheimPlusGameClientButton.Visibility = Visibility.Visible;
                    setClientPath.Visibility = Visibility.Hidden;
                }
                else
                {
                    clientInstalledLabel.Content = "ValheimPlus not installed on client";
                    clientInstalledLabel.Foreground = Brushes.Red;

                    manageClientButton.Visibility = Visibility.Hidden;
                    installClientUpdateButton.Visibility = Visibility.Hidden;
                    checkClientUpdatesButtons.Visibility = Visibility.Hidden;
                    enableDisableValheimPlusGameClientButton.Visibility = Visibility.Hidden;
                    setClientPath.Visibility = Visibility.Hidden;
                }
            }
            else
            {
                clientInstalledLabel.Content = "Valheim installation not found, please select installation path by locating and choosing 'valheim.exe'";
                clientInstalledLabel.Foreground = Brushes.Red;

                manageClientButton.Visibility = Visibility.Hidden;
                installClientUpdateButton.Visibility = Visibility.Hidden;
                checkClientUpdatesButtons.Visibility = Visibility.Hidden;
                enableDisableValheimPlusGameClientButton.Visibility = Visibility.Hidden;
                installClientButton.Visibility = Visibility.Hidden;
                setClientPath.Margin = new Thickness(16, 78, 0, 0);
            }
            if (serverPathCorrect)
            {
                if (ValheimPlusInstalledServer)
                {
                    serverInstalledLabel.Content = String.Format("ValheimPlus {0} installed on server", Settings.ValheimPlusServerClientVersion);
                    serverInstalledLabel.Foreground = Brushes.Green;
                    installServerButton.Content = "Reinstall ValheimPlus on server";
                }
                else
                {
                    serverInstalledLabel.Content = "ValheimPlus not installed on server";
                    serverInstalledLabel.Foreground = Brushes.Red;
                }
            }
            else
            {

            }
        }

        public void FetchSettings()
        {
            // Fetching path settings
            try
            {
                Settings = SettingsDAL.GetSettings();

                // Checking installation status
                if (ValidationManager.CheckClientInstallationPath(Settings.ClientInstallationPath))
                {
                    ValheimPlusInstalledClient = ValidationManager.CheckInstallationStatus(Settings.ClientInstallationPath);
                    ValheimPlusInstalledServer = ValidationManager.CheckInstallationStatus(Settings.ServerInstallationPath);

                    UISettingsInit(true, true);
                }
                else
                {
                    ValheimPlusInstalledClient = ValidationManager.CheckInstallationStatus(Settings.ClientInstallationPath);
                    ValheimPlusInstalledServer = ValidationManager.CheckInstallationStatus(Settings.ServerInstallationPath);

                    UISettingsInit(false, true);
                }
            }
            catch (Exception)
            {
                statusLabel.Foreground = Brushes.Red;
                statusLabel.Content = "Error! Settings file not found, reinstall manager.";
            }
        }

        public MainWindow()
        {
            InitializeComponent();

            FetchSettings();
        }

        private async void installClientButton_Click(object sender, RoutedEventArgs e)
        {
            installClientUpdateButton.IsEnabled = false;

            ValheimPlusUpdate valheimPlusUpdate = await UpdateManager.CheckForValheimPlusUpdatesAsync(Settings.ValheimPlusGameClientVersion);

            if (valheimPlusUpdate.NewVersion)
            {
                bool success = await UpdateManager.DownloadValheimPlusUpdateAsync(Settings.ValheimPlusGameClientVersion, true);

                if (success)
                {
                    Settings = SettingsDAL.GetSettings();
                    clientInstalledLabel.Content = String.Format("ValheimPlus {0} installed on client", Settings.ValheimPlusGameClientVersion);
                    statusLabel.Foreground = Brushes.Green;
                    statusLabel.Content = "Success! Game client updated to latest version.";
                    installClientUpdateButton.IsEnabled = false;
                }
            }
        }

        private async void checkClientUpdatesButtons_Click(object sender, RoutedEventArgs e)
        {
            ValheimPlusUpdate valheimPlusUpdate = await UpdateManager.CheckForValheimPlusUpdatesAsync(Settings.ValheimPlusGameClientVersion);

            if (valheimPlusUpdate.NewVersion)
            {
                installClientUpdateButton.Content = String.Format("Install update {0}", valheimPlusUpdate.Version);
                installClientUpdateButton.IsEnabled = true;
                statusLabel.Foreground = Brushes.Green;
                statusLabel.Content = String.Format("Update {0} available for game client", valheimPlusUpdate.Version);
            }
            else
            {
                statusLabel.Foreground = Brushes.Red;
                statusLabel.Content = "No new game client updates available";
            }
        }

        private void installClientUpdateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                FileManager.InstallValheimPlus(Settings.ClientPath, Settings.ClientInstallationPath);
                ValheimPlusInstalledClient = ValidationManager.CheckInstallationStatus(Settings.ClientInstallationPath);
                if (ValheimPlusInstalledClient)
                {
                    clientInstalledLabel.Content = String.Format("ValheimPlus {0} installed on game client", Settings.ValheimPlusGameClientVersion);
                    clientInstalledLabel.Foreground = Brushes.Green;
                    installClientButton.Content = "Reinstall ValheimPlus on game client";
                    statusLabel.Foreground = Brushes.Green;
                    statusLabel.Content = "Success! Game client has been installed.";
                }
            }
            catch (Exception)
            {
                throw new Exception(); // ToDo - handling of errors
            }
        }

        private void manageClientButton_Click(object sender, RoutedEventArgs e)
        {
            new ConfigurationManagerWindow(true).Show(); // Bool determines if user will manage conf. for server or game client
        }


        private void enableDisableValheimPlusGameClientButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var modActive = File.Exists(String.Format("{0}winhttp.dll", Settings.ClientInstallationPath));
                if (modActive)
                {
                    System.IO.File.Move(String.Format("{0}winhttp.dll", Settings.ClientInstallationPath), String.Format("{0}winhttp_.dll", Settings.ClientInstallationPath));
                    enableDisableValheimPlusGameClientButton.Content = "Enable ValheimPlus";
                    enableDisableValheimPlusGameClientButton.Style = Application.Current.TryFindResource("MaterialDesignRaisedButton") as Style;
                }
                else
                {
                    System.IO.File.Move(String.Format("{0}winhttp_.dll", Settings.ClientInstallationPath), String.Format("{0}winhttp.dll", Settings.ClientInstallationPath));
                    enableDisableValheimPlusGameClientButton.Content = "Disable ValheimPlus";
                    enableDisableValheimPlusGameClientButton.Style = Application.Current.TryFindResource("MaterialDesignOutlinedButton") as Style;
                }
            }
            catch (Exception)
            {
                //
            }
        }

        private void setClientPath_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "Executeable|valheim.exe"
            };

            if (dialog.ShowDialog() == true)
            {
                var fullPath = dialog.FileName;
                string formattedPath = String.Format("{0}\\", Path.GetDirectoryName(fullPath));
                string uriPath = new Uri(formattedPath).AbsolutePath.ToString();
                uriPath = Uri.UnescapeDataString(uriPath);
                Settings.ClientInstallationPath = uriPath;

                SettingsDAL.UpdateSettings(Settings, true);

                FetchSettings();
            }
        }

        private void installServerButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult;

            if (!ValheimPlusInstalledServer)
            {
                messageBoxResult = MessageBox.Show("Are you sure you wish to install ValheimPlus on your server?", "Confirm", MessageBoxButton.YesNo);
            }
            else
            {
                messageBoxResult = MessageBox
                    .Show("Are you sure you wish to reinstall ValheimPlus on your server? This will overwrite your current configurations!", "Confirm");
            }

            if (messageBoxResult == MessageBoxResult.OK)
            {
                try
                {
                    FileManager.InstallValheimPlus(Settings.ServerPath, Settings.ServerInstallationPath);
                    ValheimPlusInstalledServer = ValidationManager.CheckInstallationStatus(Settings.ServerInstallationPath);
                    if (ValheimPlusInstalledServer)
                    {
                        serverInstalledLabel.Content = String.Format("ValheimPlus {0} installed on server", Settings.ValheimPlusServerClientVersion);
                        serverInstalledLabel.Foreground = Brushes.Green;
                        installServerButton.Content = "Reinstall ValheimPlus on server";
                        statusLabel.Foreground = Brushes.Green;
                        statusLabel.Content = "Success! Server client has been installed.";
                    }
                }
                catch (Exception)
                {
                    throw new Exception(); // ToDo - handling of errors
                }
            }
        }

        private async void checkServerUpdatesButton_Click(object sender, RoutedEventArgs e)
        {
            ValheimPlusUpdate valheimPlusUpdate = await UpdateManager.CheckForValheimPlusUpdatesAsync(Settings.ValheimPlusServerClientVersion);

            if (valheimPlusUpdate.NewVersion)
            {
                installServerUpdateButton.Content = String.Format("Install update {0}", valheimPlusUpdate.Version);
                installServerUpdateButton.IsEnabled = true;
                statusLabel.Foreground = Brushes.Green;
                statusLabel.Content = String.Format("Update {0} available for server client", valheimPlusUpdate.Version);
            }
            else
            {
                statusLabel.Foreground = Brushes.Red;
                statusLabel.Content = "No new server updates available";
            }
        }

        private async void installServerUpdateButton_Click(object sender, RoutedEventArgs e)
        {
            installServerUpdateButton.IsEnabled = false;

            ValheimPlusUpdate valheimPlusUpdate = await UpdateManager.CheckForValheimPlusUpdatesAsync(Settings.ValheimPlusServerClientVersion);

            if (valheimPlusUpdate.NewVersion)
            {
                bool success = await UpdateManager.DownloadValheimPlusUpdateAsync(Settings.ValheimPlusServerClientVersion, false);

                if (success)
                {
                    Settings = SettingsDAL.GetSettings();
                    serverInstalledLabel.Content = String.Format("ValheimPlus {0} installed on server", Settings.ValheimPlusServerClientVersion);
                    statusLabel.Foreground = Brushes.Green;
                    statusLabel.Content = "Success! Server client updated to latest version.";
                    installServerUpdateButton.IsEnabled = false;
                }
            }
        }

        private void manageServerButton_Click(object sender, RoutedEventArgs e)
        {
            new ConfigurationManagerWindow(false).Show(); // Bool determines if user will manage conf. for server or game client
        }

        private void launchGameButton_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(@"C:\Program Files (x86)\Steam\steam.exe", "steam://rungameid/892970");
            //Process.Start(String.Format("{0}valheim.exe", Settings.ClientInstallationPath));
        }
    }
}
