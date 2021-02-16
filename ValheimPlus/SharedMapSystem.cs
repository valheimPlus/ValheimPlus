using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BepInEx;
using Unity;
using UnityEngine;
using System.IO;
using System.Reflection;
using System.Runtime;
using IniParser;
using IniParser.Model;
using HarmonyLib;
using System.Globalization;
using Steamworks;
using ValheimPlus;

// ToDo add packet system to convey map markers

namespace ValheimPlus
{

    [HarmonyPatch(typeof(Minimap))]
    public class hookExplore
    {
        [HarmonyReversePatch]
        [HarmonyPatch(typeof(Minimap), "Explore", new Type[] { typeof(Vector3), typeof(float) })]
        public static void call_Explore(object instance, Vector3 p, float radius) => throw new NotImplementedException();
    }
    [HarmonyPatch(typeof(ZNet))]
    public class hookZNet
    {
        [HarmonyReversePatch]
        [HarmonyPatch(typeof(ZNet), "GetOtherPublicPlayers", new Type[] { typeof(List<ZNet.PlayerInfo>) })]
        public static void GetOtherPublicPlayers(object instance, List<ZNet.PlayerInfo> playerList) => throw new NotImplementedException();

    }

    [HarmonyPatch(typeof(Minimap), "UpdateExplore")]
    public static class ChangeMapBehavior
    {

        private static void Prefix(ref float dt, ref Player player, ref Minimap __instance, ref float ___m_exploreTimer, ref float ___m_exploreInterval, ref List<ZNet.PlayerInfo> ___m_tempPlayerInfo) // Set after awake function
        {

            float exploreRadius = Settings.getFloat("Map", "exploreRadius");
            if (Settings.isEnabled("Map") && Settings.getBool("Map", "shareMapProgression"))
            {
                float explorerTime = ___m_exploreTimer;
                explorerTime += Time.deltaTime;
                if (explorerTime > ___m_exploreInterval)
                {
                    ___m_tempPlayerInfo.Clear();
                    hookZNet.GetOtherPublicPlayers(ZNet.instance, ___m_tempPlayerInfo); // inconsistent returns but works

                    if (___m_tempPlayerInfo.Count() > 0)
                    {
                        foreach (ZNet.PlayerInfo m_Player in ___m_tempPlayerInfo)
                        {
                            hookExplore.call_Explore(__instance, m_Player.m_position, exploreRadius);
                        }
                    }

                }
            }
            if (Settings.isEnabled("Map"))
            {
                // Always reveal for your own, we do this non the less to apply the potentially bigger exploreRadius
                hookExplore.call_Explore(__instance, player.transform.position, exploreRadius);
            }
        }
    }
}
