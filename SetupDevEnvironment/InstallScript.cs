using SetupDevEnvironment.IO;
using System.ComponentModel;
using System.Reflection;

namespace SetupDevEnvironment
{
    internal class InstallScript
    {

        public event ProgressChangedEventHandler? OnLogMessage;

        private void Log(string msg)
        {
            if (OnLogMessage != null)
            {
                OnLogMessage(this, new ProgressChangedEventArgs(0, msg ));
            }
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
            FileCopier.CopyFiles(Settings.ValheimInstallDir, Settings.ValheimPlusDevInstallDir);
            Log("Done!");
        }

        private void ConfigureEnvironment()
        {
            Log("Configuring environment...");
            Environment.SetEnvironmentVariable("VALHEIM_INSTALL", Settings.ValheimPlusDevInstallDir, EnvironmentVariableTarget.Machine);
            Log("Done!");
        }

        private async Task SetupDnSpy()
        {
            Log("Installing Debug helper DnSpy...");
            var tempFolder = DirectoryHelper.CreateTempFolder();
            var dnSpyZip = await Downloader.Download(Links.DnSpy64, Path.Combine(tempFolder, "dnSpy.zip"));
            var dnSpyFiles = Unzipper.Unzip(dnSpyZip, Links.DnSpy64TargetFolder);
            var dnSpyExecutable = dnSpyFiles.Single(file => file.EndsWith("dnSpy.exe"));

            Log($"DnSpy installed to '{dnSpyExecutable}'");
            Log("Done!");

            Log("Creating DnSpy configuration...");
            var template = ResourceHelper.GetResource("DnSpyConfiguration.xml");
            template = template.Replace(@"%%DNSPYDIR%%", Links.DnSpy64TargetFolder);
            template = template.Replace(@"%%VALHEIMPLUSINSTALLDIR%%", Settings.ValheimPlusDevInstallDir);
            
            var configurationFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "dnSpy", "dnSpy.xml");
            File.WriteAllText(configurationFile, template);
            Log("Done!");

            Log("Installing patched mono dll...");
            var executablePath = Assembly.GetEntryAssembly().Location;
            var libPath = Path.Combine(executablePath, "../../../../../libraries/Debug/mono-2.0-bdwgc.dll");
            var destinationPath = Path.Combine(Settings.ValheimPlusDevInstallDir, "MonoBleedingEdge\\EmbedRuntime\\mono-2.0-bdwgc.dll");
            File.Copy(destinationPath, destinationPath + ".bak", true);
            File.Copy(libPath, destinationPath, true);
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
            FileCopier.CopyFiles(sourceFolder, Settings.ValheimPlusDevInstallDir);

            var unstripped_corlibFiles = bepInExFiles
                .Where(path => path.Contains("unstripped_corlib"));

            var managedFolder = Path.Combine(Settings.ValheimPlusDevInstallDir, $"valheim_Data\\Managed\\");
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
            pubber.ProcessAssemblies(Settings.ValheimPlusDevInstallDir);
            Log("Done!");
        }
    }
}
