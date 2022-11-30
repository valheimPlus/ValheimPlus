using SetupDevEnvironment.IO;
using SetupDevEnvironment.Properties;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;

namespace SetupDevEnvironment
{
    internal class InstallScript
    {
        private readonly string _valheimInstallFolder;
        private readonly string _devInstallFolder;

        public event ProgressChangedEventHandler? OnLogMessage;

        private void Log(string msg)
        {
            if (OnLogMessage != null)
            {
                OnLogMessage(this, new ProgressChangedEventArgs(0, msg ));
            }
        }

        public InstallScript(string valheimFolder, string valheimPlusFolder)
        {
            _valheimInstallFolder = valheimFolder;
            _devInstallFolder = valheimPlusFolder;
        }

        public async Task Install()
        {
            CopyValheimFiles();
            await InstallBepInEx();
            PublicizeAssembliesDirectly();
            ConfigureEnvironment();
            await SetupDnSpy();
        }

        private void CopyValheimFiles()
        {
            Log("Copying existing Valheim files to Dev Environment...");
            FileCopier.CopyFiles(_valheimInstallFolder, _devInstallFolder);
            Log("Done!");
        }

        private void ConfigureEnvironment()
        {
            Log("Configuring environment...");
            Environment.SetEnvironmentVariable("VALHEIM_INSTALL", _devInstallFolder, EnvironmentVariableTarget.Machine);
            Log("Done!");
        }

        private async Task SetupDnSpy()
        {
            Log("Installing Debug helper DnSpy...");
            var tempFolder = DirectoryHelper.CreateTempFolder();
            var dnSpyZip = await Downloader.Download(Links.DnSpy64, Path.Combine(tempFolder, "dnSpy.zip"));
            var dnSpyFiles = Unzipper.Unzip(dnSpyZip, Links.DnSpy64TargetFolder);
            var dnSpyExecutable = dnSpyFiles.Single(file => file.EndsWith("dnSpy.exe"));

            Log("* * * * * * * * * * * *");
            Log($"DnSpy installed to '{dnSpyExecutable}'");
            Log("* * * * * * * * * * * *");
            
            Log("Creating DnSpy configuration...");
            var template = ResourceHelper.GetResource("DnSpyConfiguration.xml");
            template.Replace(@"%%DNSPYDIR%%", Links.DnSpy64TargetFolder);
            template.Replace(@"%%VALHEIMPLUSINSTALLDIR%%", _devInstallFolder);
            
            var configurationFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "dnSpy", "dnSpy.xml");
            File.WriteAllText(configurationFile, template);

            Log("Done!");
        }

        public async Task InstallBepInEx()
        {
            Log("Installing BepInEx...");
            var tempFolder = DirectoryHelper.CreateTempFolder();
            var bepInExZip = await Downloader.Download(Links.BepInEx, Path.Combine(tempFolder, "bepInExPack.zip"));
            var bepInExFiles = Unzipper.Unzip(bepInExZip);
            File.Delete(bepInExZip);

            var sourceFolder = bepInExFiles.Single(file => file.EndsWith("BepInExPack_Valheim/"));
            FileCopier.CopyFiles(sourceFolder, _devInstallFolder);

            var unstripped_corlibFiles = bepInExFiles
                .Where(path => path.Contains("unstripped_corlib"));

            var managedFolder = Path.Combine(_devInstallFolder, $"valheim_Data\\Managed\\");
            Directory.CreateDirectory(managedFolder);
            foreach (var source in unstripped_corlibFiles)
            {
                var file = Path.GetFileName(source);
                if (string.IsNullOrEmpty(file))
                {
                    // ignore folders
                    continue;
                }

                var destination = Path.Combine(managedFolder, file);
                File.Copy(source, destination, true);
            }
            Log("Done!");
        }

        public void PublicizeAssembliesDirectly()
        {
            Log("Publicizing assemblies...");
            var pubber = new AssemblyPublicizer();
            pubber.OnLogMessage += this.OnLogMessage;
            pubber.ProcessAssemblies(_devInstallFolder);
            Log("Done!");
        }
    }
}
