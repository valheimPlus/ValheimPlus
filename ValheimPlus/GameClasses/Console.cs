using HarmonyLib;

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
}
