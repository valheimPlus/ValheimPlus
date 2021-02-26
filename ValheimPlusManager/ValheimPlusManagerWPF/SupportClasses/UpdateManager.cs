using Octokit;
using System;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using ValheimPlusManagerWPF.Data;
using ValheimPlusManagerWPF.Models;

namespace ValheimPlusManagerWPF.SupportClasses
{
    public sealed class UpdateManager
    {
        public static async Task<ValheimPlusUpdate> CheckForValheimPlusUpdatesAsync(string valheimPlusVersion)
        {
            ValheimPlusUpdate valheimPlusUpdate = new ValheimPlusUpdate();

            // Calling Github API to fetch versions of ValheimPlus
            var github = new GitHubClient(new ProductHeaderValue("ValheimPlusManager"));
            var releases = await github.Repository.Release.GetAll("valheimPlus", "ValheimPlus");
            var latest = releases[0];

            // Comparing latest release on ValheimPlus Github to currently installed locally
            var latestVersion = new Version(latest.TagName);
            var currentVersion = new Version(valheimPlusVersion);
            var result = latestVersion.CompareTo(currentVersion);

            if (result > 0) // If a new version is available
            {
                valheimPlusUpdate.NewVersion = true;
                valheimPlusUpdate.Version = latest.TagName;
                valheimPlusUpdate.WindowsServerClientDownloadURL = latest.Assets.Single(x => x.Name == "WindowsServer.zip").BrowserDownloadUrl;
                valheimPlusUpdate.WindowsGameClientDownloadURL = latest.Assets.Single(x => x.Name == "WindowsClient.zip").BrowserDownloadUrl;
                return valheimPlusUpdate;
            }
            else if (result < 0)
            {
                valheimPlusUpdate.NewVersion = false;
                valheimPlusUpdate.Version = latest.TagName;
                return valheimPlusUpdate;
            }
            else
            {
                valheimPlusUpdate.NewVersion = false;
                valheimPlusUpdate.Version = latest.TagName;
                return valheimPlusUpdate;
            }
        }

        public static async Task<bool> DownloadValheimPlusUpdateAsync(string valheimPlusVersion, bool manageClient)
        {
            ValheimPlusUpdate valheimPlusUpdate = await CheckForValheimPlusUpdatesAsync(valheimPlusVersion);

            var wc = new System.Net.WebClient();

            if (manageClient)
            {
                wc.DownloadFile(valheimPlusUpdate.WindowsGameClientDownloadURL, @"Data/ValheimPlusGameClient/WindowsClient.zip");
                await InstallValheimPlusUpdateAsync(true, valheimPlusUpdate.Version);
                return true;
            }
            else
            {
                wc.DownloadFile(valheimPlusUpdate.WindowsServerClientDownloadURL, @"Data/ValheimPlusServerClient/WindowsServer.zip");
                await InstallValheimPlusUpdateAsync(false, valheimPlusUpdate.Version);
                return true;
            }
        }

        public static async Task<bool> InstallValheimPlusUpdateAsync(bool manageClient, string valheimPlusVersion)
        {
            var settings = SettingsDAL.GetSettings();
            if (manageClient)
            {
                string zipPath = @"Data/ValheimPlusGameClient/WindowsClient.zip";
                string extractPath = @"Data/ValheimPlusGameClient/Extracted";

                await Task.Run(() => ZipFile.ExtractToDirectory(zipPath, extractPath, true));

                try
                {
                    FileManager.InstallValheimPlus(settings.ClientPath, settings.ClientInstallationPath);
                    settings.ValheimPlusGameClientVersion = valheimPlusVersion;
                    SettingsDAL.UpdateSettings(settings, false);
                }
                catch (Exception)
                {
                    return false; // ToDo - handling of errors
                }
            }
            else
            {
                string zipPath = @"Data/ValheimPlusServerClient/WindowsServer.zip";
                string extractPath = @"Data/ValheimPlusServerClient/Extracted";

                await Task.Run(() => ZipFile.ExtractToDirectory(zipPath, extractPath, true));

                try
                {
                    FileManager.InstallValheimPlus(settings.ServerPath, settings.ServerInstallationPath);
                    settings.ValheimPlusServerClientVersion = valheimPlusVersion;
                    SettingsDAL.UpdateSettings(settings, false);
                }
                catch (Exception)
                {
                    return false; // ToDo - handling of errors
                }
            }

            return true;
        }

        private UpdateManager()
        {
        }
        private static UpdateManager instance = null;
        public static UpdateManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new UpdateManager();
                }
                return instance;
            }
        }
    }
}
