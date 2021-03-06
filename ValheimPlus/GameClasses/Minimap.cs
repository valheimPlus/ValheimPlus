using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using ValheimPlus.Configurations;
using ValheimPlus.RPC;
using Random = UnityEngine.Random;

// ToDo add packet system to convey map markers
namespace ValheimPlus.GameClasses
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
        private static void Prefix(ref float dt, ref Player player, ref Minimap __instance, ref float ___m_exploreTimer, ref float ___m_exploreInterval)
        {
            if (Configuration.Current.Map.exploreRadius > 10000) Configuration.Current.Map.exploreRadius = 10000;

            if (!Configuration.Current.Map.IsEnabled) return;

            if (Configuration.Current.Map.shareMapProgression)
            {
                float explorerTime = ___m_exploreTimer;
                explorerTime += Time.deltaTime;
                if (explorerTime > ___m_exploreInterval)
                {
                    if (ZNet.instance.m_players.Any())
                    {
                        foreach (ZNet.PlayerInfo m_Player in ZNet.instance.m_players)
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

    [HarmonyPatch(typeof(Minimap), "Awake")]
    public static class MinimapAwake
    {
        private static void Postfix()
        {
            if (ZNet.m_isServer)
            {
                //Init map array
                VPlusMapSync.ServerMapData = new bool[Minimap.instance.m_textureSize * Minimap.instance.m_textureSize];

                //Load map data from disk
                //VPlusMapSync.LoadMapDataFromDisk();

                for (int i = 0; i < 10000; i++)
                {
                    int pixelX = Random.Range(0, Minimap.instance.m_textureSize);
                    int pixelY = Random.Range(0, Minimap.instance.m_textureSize);

                    int radiusPixels = (int)Mathf.Ceil(Configuration.Current.Map.exploreRadius / Minimap.instance.m_pixelSize);

                    for (int y = pixelY - radiusPixels; y <= pixelY + radiusPixels; ++y)
                    {
                        for (int x = pixelX - radiusPixels; x <= pixelX + radiusPixels; ++x)
                        {
                            if (x >= 0 && y >= 0 && (x < Minimap.instance.m_textureSize && y < Minimap.instance.m_textureSize) &&
                                ((double)new Vector2((float)(x - pixelX), (float)(y - pixelY)).magnitude <= (double)radiusPixels))
                            {
                                VPlusMapSync.ServerMapData[y * Minimap.instance.m_textureSize + x] = true;
                            }
                        }
                    }
                }

                //Start map data save timer
                //ValheimPlusPlugin.mapSyncSaveTimer.Start();
            }
        }
    }
}
