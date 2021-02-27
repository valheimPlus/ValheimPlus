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
}