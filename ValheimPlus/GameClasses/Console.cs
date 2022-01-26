using HarmonyLib;
using ValheimPlus.Configurations;

namespace ValheimPlus.GameClasses
{
    /// <summary>
    /// Adding version data to console
    /// </summary>
    [HarmonyPatch(typeof(Console), "Awake")]
    public static class Console_Awake_Patch
    {
        private static void Postfix(ref Console __instance)
        {
            __instance.AddString("ValheimPlus [" + ValheimPlusPlugin.version + "] is loaded.");
            if (!ValheimPlusPlugin.isUpToDate && ValheimPlusPlugin.newestVersion != "Unknown")
            {
                __instance.AddString("ValheimPlus [" + ValheimPlusPlugin.version + "] is outdated, version [" + ValheimPlusPlugin.newestVersion + "] is available.");
                __instance.AddString("Please visit " + ValheimPlusPlugin.Repository + ".");
            }
            else
            {
                __instance.AddString("ValheimPlus [" + ValheimPlusPlugin.version + "] is up to date.");
            }

            __instance.AddString("");
        }
    }


    /// <summary>
    /// Change the console enabled status
    /// </summary>
    [HarmonyPatch(typeof(Console), "IsConsoleEnabled")]
    public static class Console_IsConsoleEnabled_Patch
    {
        private static bool Prefix(ref Console __instance, ref bool __result)
        {
            if (Configuration.Current.Game.IsEnabled && Configuration.Current.Game.disableConsole)
            {
                __result = false;
                return false;
            }
                
            return true;
        }
    }

}
