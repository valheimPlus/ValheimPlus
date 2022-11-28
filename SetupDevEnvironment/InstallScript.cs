using SetupDevEnvironment.IO;
using System.ComponentModel;

namespace SetupDevEnvironment
{
    internal class InstallScript
    {
        readonly string _installFolder;

        public event ProgressChangedEventHandler? OnLogMessage;

        private void Log(string msg)
        {
            if (OnLogMessage != null)
            {
                OnLogMessage(this, new ProgressChangedEventArgs(0, msg ));
            }
        }

        public InstallScript(string? installFolder)
        {
            _installFolder = installFolder;
        }

        public async Task Install()
        {
            await InstallBepInEx();
            //await PublicizeAssemblies();
            PublicizeAssembliesDirectly();
            await ConfigureEnvironment();
        }

        private async Task ConfigureEnvironment()
        {
            Log("Configuring environment...");
            Environment.SetEnvironmentVariable("VALHEIM_INSTALL", _installFolder, EnvironmentVariableTarget.Machine);
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
            FileMover.CopyFiles(sourceFolder, _installFolder);

            var unstripped_corlibFiles = bepInExFiles
                .Where(path => path.Contains("unstripped_corlib"));
            
            foreach(var source in unstripped_corlibFiles)
            {
                var file = Path.GetFileName(source);
                if (string.IsNullOrEmpty(file))
                {
                    continue;
                }

                var destination = Path.Combine(_installFolder, $"valheim_Data\\Managed\\{file}");
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

            File.Copy(pluginDll, Path.Combine(_installFolder, $"BepInEx\\plugins\\{file}"), true);
        }

        public void PublicizeAssembliesDirectly()
        {
            Log("Publicizing assemblies...");
            var pubber = new AssemblyPublicizer();
            pubber.OnLogMessage += this.OnLogMessage;
            pubber.ProcessAssemblies(_installFolder);
            Log("Done!");
        }
    }
}
