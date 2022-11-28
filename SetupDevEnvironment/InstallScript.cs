using SetupDevEnvironment.IO;
using System.ComponentModel;
using System.Diagnostics;

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
            await ConfigureEnvironment();
        }

        private void CopyValheimFiles()
        {
            Log("Copying existing Valheim files to Dev Environment...");
            FileMover.CopyFiles(_valheimInstallFolder, _devInstallFolder);
            Log("Done!");
        }

        private async Task ConfigureEnvironment()
        {
            Log("Configuring environment...");
            Environment.SetEnvironmentVariable("VALHEIM_INSTALL", _devInstallFolder, EnvironmentVariableTarget.Machine);
            await Task.CompletedTask;
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
            FileMover.CopyFiles(sourceFolder, _devInstallFolder);

            var unstripped_corlibFiles = bepInExFiles
                .Where(path => path.Contains("unstripped_corlib"));

            var managedFolder = Path.Combine(_devInstallFolder, $"valheim_Data\\Managed\\");
            Directory.CreateDirectory(managedFolder);
            foreach (var source in unstripped_corlibFiles)
            {
                var file = Path.GetFileName(source);
                if (string.IsNullOrEmpty(file))
                {
                    continue;
                }

                var destination = Path.Combine($"{managedFolder}{file}");
                File.Copy(source, destination, true);
            }
            Log("Done!");
        }

        /// <summary>
        /// Install a plugin that automatically reads any file that matches "assembly_*.dll" and
        /// creates a new dll where all properties, classes and fields are made public.
        /// This allows devs to include these publicized dlls and use the correct consts, methods, etc.
        /// </summary>
        /// <returns></returns>
        public async Task PublicizeAssemblies()
        {
            var publicizerZip = await Downloader.Download(Links.AssemblyPublicizer);
            var publicizerFiles = Unzipper.Unzip(publicizerZip);

            var file = "BepInEx-Publicizer.dll";
            var pluginDll = publicizerFiles.Single(file => file.EndsWith(
                @"plugins/Bepinex-Publicizer/Bepinex-Publicizer.dll", StringComparison.InvariantCultureIgnoreCase));

            File.Copy(pluginDll, Path.Combine(_devInstallFolder, $"BepInEx\\plugins\\{file}"), true);
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
