// ValheimPlus

using HarmonyLib;

namespace ValheimPlus.Configurations
{
    [HarmonyPatch(typeof(ZNet), "RPC_Save")]
    public static class ConfigurationHooks
    {
        public static void Postfix()
        {
            // Just save configuration after a save command is issued
            // Server side only
            Configuration.Current.SaveConfiguration();
        }
    }

    [HarmonyPatch(typeof(ZNet), "OnDestroy")]
    public static class ConfigurationHooks2
    {
        private static void Prefix()
        {
            ZLog.Log("Saving local configuration");
            Configuration.Current.SaveConfiguration();
        }
    }
}