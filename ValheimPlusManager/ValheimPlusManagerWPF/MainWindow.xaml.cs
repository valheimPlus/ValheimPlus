using System;
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
        private bool ValheimPlusInstalledClient { get; set; }
        private bool ValheimPlusInstalledServer { get; set; }
        private Settings Settings { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            // Fetching path settings
            try
            {
                Settings = SettingsDAL.GetSettings();

                // Checking installation status
                ValheimPlusInstalledClient = ValidationManager.CheckInstallationStatus(Settings.ClientInstallationPath);
                ValheimPlusInstalledServer = ValidationManager.CheckInstallationStatus(Settings.ServerInstallationPath);

                if (ValheimPlusInstalledClient)
                {
                    clientInstalledLabel.Content = String.Format("ValheimPlus {0} installed on client", Settings.ValheimPlusGameClientVersion);
                    clientInstalledLabel.Foreground = Brushes.Green;
                    installClientButton.Content = "Reinstall ValheimPlus on client";
                }
                else
                {
                    clientInstalledLabel.Content = "ValheimPlus not installed on client";
                    clientInstalledLabel.Foreground = Brushes.Red;
                }

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
            catch (Exception)
            {
                statusLabel.Foreground = Brushes.Red;
                statusLabel.Content = "Error! Settings file not found, reinstall manager.";
            }
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
            //try
            //{
            //    FileManager.InstallValheimPlus(Settings.ClientPath, Settings.ClientInstallationPath);
            //    ValheimPlusInstalledClient = ValidationManager.CheckInstallationStatus(Settings.ClientInstallationPath);
            //    if (ValheimPlusInstalledClient)
            //    {
            //        clientInstalledLabel.Content = String.Format("ValheimPlus {0} installed on game client", Settings.ValheimPlusGameClientVersion);
            //        clientInstalledLabel.Foreground = Brushes.Green;
            //        installClientButton.Content = "Reinstall ValheimPlus on game client";
            //        statusLabel.Foreground = Brushes.Green;
            //        statusLabel.Content = "Success! Game client has been installed.";
            //    }
            //}
            //catch (Exception)
            //{
            //    throw new Exception(); // ToDo - handling of errors
            //}
        }

        private void manageClientButton_Click(object sender, RoutedEventArgs e)
        {
            new ConfigurationManagerWindow(true).Show(); // Bool determines if user will manage conf. for server or game client
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
    }
}
