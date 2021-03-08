using BepInEx;
using HarmonyLib;
using ValheimPlus.Configurations;

namespace ValheimPlus.GameClasses
{
    public class BepInExPlugin : BaseUnityPlugin
    {
        [HarmonyPatch(typeof(Ragdoll), "Awake")]
        static class Ragdoll_Awake_Patch
        {
            static void Prefix(Ragdoll __instance)
            {
                __instance.m_ttl = Configuration.Current.Monster.dropDelay;
            }
        }
    }
}