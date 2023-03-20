using HarmonyLib;
using System;
using System.Runtime.Remoting.Messaging;
using UnityEngine;
using ValheimPlus.Configurations;
using ValheimPlus.Utility;

namespace ValheimPlus.GameClasses
{
    class LuredWispModification
    {
        [HarmonyPatch(typeof(LuredWisp), "UpdateTarget")]
        public static class ModifyLuredWisp
        {
            // "WispSpawner" is from base game
            private static bool Prefix(ref LuredWisp __instance)
            {
                LuredWisp luredWisp = __instance;


                if (Configuration.Current.WispSpawner.IsEnabled && !Configuration.Current.WispSpawner.onlySpawnAtNight)
                {
                    luredWisp.m_despawnInDaylight = false;
                }

                return false;
            }

        }
    }
}