namespace SetupDevEnvironment.IO;

internal static class Links
{
    internal static string DefaultValheimInstallFolder =
        @"C:\Program Files (x86)\Steam\steamapps\common\Valheim";
    internal static string DefaultValheimPlusDevInstallFolder =
        @"C:\Program Files (x86)\Steam\steamapps\common\Valheim Plus Development";
    internal static readonly string DnSpy64TargetFolder =
        @"C:\Program Files\dnSpy64";

    internal static readonly string BepInEx = 
        @"https://valheim.thunderstore.io/package/download/denikson/BepInExPack_Valheim/5.4.1900/";
    internal static readonly string BepInEx21 =
        @"https://github.com/BepInEx/BepInEx/releases/download/v5.4.21/BepInEx_x64_5.4.21.0.zip";

    [Obsolete("This should be the correct source, but you can't run valheim with it", true)]
    internal static readonly string Unstripped_Corlib =
        @"https://cdn.discordapp.com/attachments/624272422295568435/841990037935882250/unstripped_corlib.7z";
    internal static readonly string Unstripped_Corlib_ValheimPlusSource =
        @"https://github.com/valheimPlus/ValheimPlus/releases/download/0.9.9.9/WindowsClient.zip";

    internal static readonly string DnSpy64 =
        @"https://github.com/dnSpy/dnSpy/releases/download/v6.1.8/dnSpy-net-win64.zip";

    [Obsolete("we're doing this ourselves now", true)]
    internal static readonly string Assemblyinternalizer = 
        @"https://github.com/elliotttate/Bepinex-Tools/releases/download/1.0.0-internalizer/Bepinex-internalizer.zip";
}
