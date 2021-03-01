using System;
using HarmonyLib;
using ValheimPlus.RPC;
using ValheimPlus.Configurations;
using UnityEngine;

namespace ValheimPlus
{
    /// <summary>
    /// Sync server config to clients
    /// </summary>
    [HarmonyPatch(typeof(Game), "Start")]
    public static class GameStartPatch
    {
        private static void Prefix()
        {
            ZRoutedRpc.instance.Register("VPlusConfigSync", new Action<long, ZPackage>(VPlusConfigSync.RPC_VPlusConfigSync));
        }
    }


    /// <summary>
    /// Alter game difficulty damage scale
    /// </summary>
    [HarmonyPatch(typeof(Game), "GetDifficultyDamageScale")]
    public static class ChangeDifficultyScaleDamage
    {
        private static bool Prefix(ref Game __instance, ref Vector3 pos, ref float __result)
        {
            if (Configuration.Current.Game.IsEnabled)
            {
                int playerDifficulty = __instance.GetPlayerDifficulty(pos);
                __result = 1f + (float)(playerDifficulty - 1) * Configuration.Current.Game.gameDifficultyDamageScale;
                return false;
            }

            return true;
        }
    }

    /// <summary>
    /// Alter game difficulty health scale
    /// </summary>
    [HarmonyPatch(typeof(Game), "GetDifficultyHealthScale")]
    public static class ChangeDifficultyScaleHealth
    {
        private static bool Prefix(ref Game __instance, ref Vector3 pos, ref float __result)
        {
            if (Configuration.Current.Game.IsEnabled)
            {
                int playerDifficulty = __instance.GetPlayerDifficulty(pos);
                __result = 1f + (float)(playerDifficulty - 1) * Configuration.Current.Game.gameDifficultyHealthScale;
                return false;
            }

            return true;
        }
    }

    /// <summary>
    /// Alter player difficulty scale
    /// </summary>
    [HarmonyPatch(typeof(Game), "GetPlayerDifficulty")]
    public static class ChangePlayerDifficultyCount
    {
        private static bool Prefix(ref Game __instance, ref Vector3 pos, ref int __result)
        {
            if (Configuration.Current.Game.IsEnabled)
            {
                if (Configuration.Current.Game.setFixedPlayerCountTo > 0)
                {
                    __result = Configuration.Current.Game.setFixedPlayerCountTo + Configuration.Current.Game.extraPlayerCountNearby;
                    return false;
                }

                int num = Player.GetPlayersInRangeXZ(pos, Configuration.Current.Game.difficultyScaleRange);

                if (num < 1)
                {
                    num = 1;
                }

                __result = num + Configuration.Current.Game.extraPlayerCountNearby;
                return false;
            }

            return true;
        }
    }
}