using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ValheimPlus.Configurations;

// ToDo add packet system to convey map markers
namespace ValheimPlus
{
    /// <summary>
    /// Hooks base explore method
    /// </summary>
    [HarmonyPatch(typeof(Minimap))]
    public class HookExplore
    {
        [HarmonyReversePatch]
        [HarmonyPatch(typeof(Minimap), "Explore", new Type[] { typeof(Vector3), typeof(float) })]
        public static void call_Explore(object instance, Vector3 p, float radius) => throw new NotImplementedException();
    }

    /// <summary>
    /// Update exploration for all players
    /// </summary>
    [HarmonyPatch(typeof(Minimap), "UpdateExplore")]
    public static class ChangeMapBehavior
    {
        private static void Prefix(ref float dt, ref Player player, ref Minimap __instance, ref float ___m_exploreTimer, ref float ___m_exploreInterval, ref List<ZNet.PlayerInfo> ___m_tempPlayerInfo) // Set after awake function
        {
            if (Configuration.Current.Map.exploreRadius > 10000) Configuration.Current.Map.exploreRadius = 10000;

            if (!Configuration.Current.Map.IsEnabled) return;

            if (Configuration.Current.Map.shareMapProgression)
            {
                float explorerTime = ___m_exploreTimer;
                explorerTime += Time.deltaTime;
                if (explorerTime > ___m_exploreInterval)
                {
                    ___m_tempPlayerInfo.Clear();
                    HookZNet.GetOtherPublicPlayers(ZNet.instance, ___m_tempPlayerInfo); // inconsistent returns but works

                    if (___m_tempPlayerInfo.Count() > 0)
                    {
                        foreach (ZNet.PlayerInfo m_Player in ___m_tempPlayerInfo)
                        {
                            HookExplore.call_Explore(__instance, m_Player.m_position, Configuration.Current.Map.exploreRadius);
                        }
                    }
                }
            }

            // Always reveal for your own, we do this non the less to apply the potentially bigger exploreRadius
            HookExplore.call_Explore(__instance, player.transform.position, Configuration.Current.Map.exploreRadius);
        }
    }
}
