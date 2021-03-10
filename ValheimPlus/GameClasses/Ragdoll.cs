using HarmonyLib;
using ValheimPlus.Configurations;

namespace ValheimPlus.GameClasses
{
    class Ragdoll
    {
        /// <summary>
        /// Altering time for loot appear
        /// </summary>
        [HarmonyPatch(typeof(global::Ragdoll), "Awake")]
        public static class Ragdoll_Awake_Patch
        {
            public static void Prefix(global::Ragdoll __instance)
            {
                if (Configuration.Current.Monster.IsEnabled)
                {
                    __instance.m_ttl = Configuration.Current.Monster.dropDelay;
                }
            }
        }
    }
}