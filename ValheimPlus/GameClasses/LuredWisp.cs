using HarmonyLib;
using System;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using UnityEngine;
using ValheimPlus.Configurations;
using ValheimPlus.Utility;
using static HarmonyLib.AccessTools;

namespace ValheimPlus.GameClasses
{
    class LuredWispModification
    {
        [HarmonyPatch(typeof(LuredWisp), "Awake")]
        public static class LuredWispPatch
        {

            [HarmonyPrefix]
            static void Prefix(LuredWisp __instance)
            {
                if (Configuration.Current.WispSpawner.IsEnabled)
                {
                    FieldRef<LuredWisp, bool> m_despawnInDaylight = FieldRefAccess<LuredWisp, bool>("m_despawnInDaylight");
                    m_despawnInDaylight(__instance) = Configuration.Current.WispSpawner.onlySpawnAtNight;
                }
            }
        }
    }
}