using SetupDevEnvironment.IO;
using System.Reflection;

namespace SetupDevEnvironment;
internal class InstallScript
{
    public InstallScript()
    {
        Logger.Start();
    }

    public async Task Install()
    {
        //1.Download the[BepInEx for Valheim package](https://valheim.thunderstore.io/package/download/denikson/BepInExPack_Valheim/5.4.1901/).
        //1.Extract zip contents and copy the contents inside `/ BepInExPack_Valheim /` and paste them in your Valheim root folder and overwrite every file when asked.
        //1.Copy over all the DLLs from Valheim/ unstripped_corlib to Valheim / valheim_Data / Managed * (overwrite when asked) *
        //1.Download the[AssemblyPublicizer package](https://mega.nz/file/oQxEjCJI#_XPXEjwLfv9zpcF2HRakYzepMwaUXflA9txxhx4tACA).
        //1.Drag and drop all `assembly_ *.dll` files from "\Valheim\valheim_Data\Managed\" folder onto "AssemblyPublicizer.exe".
        //1.Define Environment Variable `VALHEIM_INSTALL` with path to Valheim Install Directory

        CopyValheimFiles();
        await InstallBepInEx542100();
        await InstallUnstrippedFiles();
        PublicizeAssembliesDirectly();
        await SetupDnSpy();
        ConfigureEnvironment();
        Logger.Log("All Done, have a nice dev.");
    }

    private static void CopyValheimFiles()
    {
        Logger.Log("Copying existing Valheim files to Dev Environment...");
        FileCopier.CopyFiles(Settings.ValheimInstallDir, Settings.ValheimPlusDevInstallDir);
        Logger.Log("Done!");
    }

    private static void ConfigureEnvironment()
    {
        Logger.Log("Configuring environment...");
        Environment.SetEnvironmentVariable("VALHEIM_INSTALL", Settings.ValheimPlusDevInstallDir, EnvironmentVariableTarget.Machine);
        var configPath = Path.Combine(Settings.ValheimPlusDevInstallDir, "BepInEx\\config\\");
        
        Directory.CreateDirectory(configPath);
        var configFile = Path.Combine(configPath, "valheim_plus.cfg");
        if (!File.Exists(configFile))
        {
            var defaultConfig = ResourceHelper.GetResource("valheim_plus.cfg");
            File.WriteAllText(configFile, defaultConfig);
        }
        Logger.Log("Done!");
    }

    private static async Task SetupDnSpy()
    {
        Logger.Log("Installing Debug helper DnSpy...");
        var tempFolder = DirectoryHelper.CreateTempFolder();
        var dnSpyZip = await Downloader.Download(Links.DnSpy64, Path.Combine(tempFolder, "dnSpy.zip"));
        var dnSpyFiles = Unzipper.Unzip(dnSpyZip, Links.DnSpy64TargetFolder);
        var dnSpyExecutable = dnSpyFiles.Single(file => file.EndsWith("dnSpy.exe"));

        Logger.Log($"DnSpy installed to '{dnSpyExecutable}'");
        Logger.Log("Done!");

        Logger.Log("Creating DnSpy configuration...");
        var template = ResourceHelper.GetResource("DnSpyConfiguration.xml");
        template = template.Replace(@"%%DNSPYDIR%%", Links.DnSpy64TargetFolder);
        template = template.Replace(@"%%VALHEIMPLUSINSTALLDIR%%", Settings.ValheimPlusDevInstallDir);

        var configurationFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "dnSpy", "dnSpy.xml");
        if (File.Exists(configurationFile))
        {
            Logger.Log("backing up existing dnSpy config");
            File.Copy(configurationFile, configurationFile + ".ValheimPlus.bak", true);
        }
        File.WriteAllText(configurationFile, template);
        Logger.Log("Done!");

        Logger.Log("Installing patched mono dll...");
        var executablePath = Path.GetDirectoryName(Assembly.GetEntryAssembly()!.Location);
        var libPath = Path.Combine(executablePath!, "../../../../libraries/Debug/mono-2.0-bdwgc.dll");
        var destinationPath = Path.Combine(Settings.ValheimPlusDevInstallDir, "MonoBleedingEdge\\EmbedRuntime\\mono-2.0-bdwgc.dll");
        File.Copy(destinationPath, destinationPath + ".bak", true);
        File.Copy(libPath, destinationPath, true);
        Logger.Log("Done!");
    }

    public static async Task InstallBepInEx541900()
    {
        Logger.Log("Installing BepInEx...");
        var tempFolder = DirectoryHelper.CreateTempFolder();
        var bepInExZip = await Downloader.Download(Links.BepInEx, Path.Combine(tempFolder, "bepInExPack.zip"));
        var bepInExFiles = Unzipper.Unzip(bepInExZip);
        File.Delete(bepInExZip);

        var sourceFolder = bepInExFiles.Single(file => file.EndsWith("BepInExPack_Valheim/"));
        FileCopier.CopyFiles(sourceFolder, Settings.ValheimPlusDevInstallDir);
    }

    public static async Task InstallUnstrippedFiles()
    {
        var tempFolder = DirectoryHelper.CreateTempFolder();
        var unstrippedZip = await Downloader.Download(Links.Unstripped_Corlib, Path.Combine(tempFolder, "unstripped.zip"));
        var unstripped_corlibFiles = Unzipper.Unzip(unstrippedZip);

        var managedFolder = Path.Combine(Settings.ValheimPlusDevInstallDir, $"valheim_Data\\Managed\\");
        //var coreLibFolder = Path.Combine(Settings.ValheimPlusDevInstallDir, $"valheim_Data\\unstripped_corlib\\");
        Directory.CreateDirectory(managedFolder);
        foreach (var source in unstripped_corlibFiles)
        {
            if (File.GetAttributes(source).HasFlag(FileAttributes.Directory)) { continue; }
            
            var filename = Path.GetFileName(source);

            var managedDestination = Path.Combine(managedFolder, filename);
            File.Copy(source, managedDestination, true);
        }

        Logger.Log("Done!");
    }

    public static async Task InstallBepInEx542100()
    {
        Logger.Log("Installing BepInEx...");
        var tempFolder = DirectoryHelper.CreateTempFolder();
        var bepInExZip = await Downloader.Download(Links.BepInEx21, Path.Combine(tempFolder, "bepInExPack.zip"));
        var bepInExFiles = Unzipper.Unzip(bepInExZip);
        File.Delete(bepInExZip);

        var sourceFolder = Path.GetDirectoryName(bepInExFiles.First());
        FileCopier.CopyFiles(sourceFolder!, Settings.ValheimPlusDevInstallDir);
        Logger.Log("Done!");
    }

    public static void PublicizeAssembliesDirectly()
    {
        Logger.Log("Publicizing assemblies...");
        var pubber = new AssemblyPublicizer();
        pubber.ProcessAssemblies(Settings.ValheimPlusDevInstallDir);
        Logger.Log("Done!");
    }
}
