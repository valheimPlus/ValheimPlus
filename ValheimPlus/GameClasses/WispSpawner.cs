using HarmonyLib;
using System;
using System.Runtime.Remoting.Messaging;
using UnityEngine;
using ValheimPlus.Configurations;
using ValheimPlus.Utility;

namespace ValheimPlus.GameClasses
{
    class WispSpawnerModification
    {
        [HarmonyPatch(typeof(WispSpawner), "TrySpawn")]
        public static class ModifyWispSpawner
        {
            // "WispSpawner" is from base game
            private static bool Prefix(ref WispSpawner __instance)
            {
                WispSpawner wispSpawner = __instance;
                if (!wispSpawner.m_nview.IsValid() || !wispSpawner.m_nview.IsOwner())
                {
                    return false;
                }

                if (Configuration.Current.WispSpawner.IsEnabled)
                {
                    wispSpawner.m_maxSpawned = Configuration.Current.WispSpawner.maximumWisps;
                    wispSpawner.m_spawnChance = Helper.applyModifierValue(wispSpawner.m_spawnChance, Configuration.Current.WispSpawner.wispSpawnChanceMultiplier);
                    wispSpawner.m_spawnInterval = Helper.applyModifierValue(wispSpawner.m_spawnInterval, Configuration.Current.WispSpawner.wispSpawnIntervalMultiplier);
                }

                return true;
            }

        }
    }
}