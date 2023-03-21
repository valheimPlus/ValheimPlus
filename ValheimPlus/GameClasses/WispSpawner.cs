using HarmonyLib;
using System;
using System.Runtime.Remoting.Messaging;
using UnityEngine;
using ValheimPlus.Configurations;
using ValheimPlus.Utility;
using static HarmonyLib.AccessTools;

namespace ValheimPlus.GameClasses
{
    [HarmonyPatch(typeof(WispSpawner), "Start")]
    static class WispSpawnerModification
    {
        [HarmonyPrefix]
        static void Prefix(WispSpawner __instance)
        {
            if (Configuration.Current.WispSpawner.IsEnabled)
            {
                FieldRef<WispSpawner, int> m_maxSpawned = FieldRefAccess<WispSpawner, int>("m_maxSpawned");
                FieldRef<WispSpawner, float> m_spawnChance = FieldRefAccess<WispSpawner, float>("m_spawnChance");
                FieldRef<WispSpawner, float> m_spawnInterval = FieldRefAccess<WispSpawner, float>("m_spawnInterval");
                FieldRef<WispSpawner, bool> m_onlySpawnAtNight = FieldRefAccess<WispSpawner, bool>("m_onlySpawnAtNight");

                m_onlySpawnAtNight(__instance) = Configuration.Current.WispSpawner.onlySpawnAtNight;
                m_maxSpawned(__instance) = Configuration.Current.WispSpawner.maximumWisps;
                m_spawnChance(__instance) = Helper.applyModifierValue(m_spawnChance(__instance), Configuration.Current.WispSpawner.wispSpawnChanceMultiplier);
                m_spawnInterval(__instance) = Helper.applyModifierValue(m_spawnInterval(__instance), Configuration.Current.WispSpawner.wispSpawnIntervalMultiplier);
            }
        }
    }
}
