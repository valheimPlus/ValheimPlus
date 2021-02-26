using System;
using HarmonyLib;
using ValheimPlus.RPC;
using ValheimPlus.Configurations;
using UnityEngine;
using ValheimPlus.ConsoleCommands;

namespace ValheimPlus
{
    [HarmonyPatch(typeof(Game), "Start")]
    public static class GameStartPatch
    {
        private static void Prefix()
        {
            //Config Sync
            ZRoutedRpc.instance.Register("VPlusConfigSync", new Action<long, ZPackage>(VPlusConfigSync.RPC_VPlusConfigSync));

            // Configuration console command RPC
            ZRoutedRpc.instance.Register("SetConfigurationValue", new Action<long, ZPackage>(RPC.SetConfigurationValueRPC.RPC_SetConfigurationValue));

            // register all console commands
            BaseConsoleCommand.InitializeCommand<SetConfigurationValue>();
        }
    }

    [HarmonyPatch(typeof(Game), "GetDifficultyDamageScale")]
    public static class ChangeDifficultyScaleDamage
    {
        private static Boolean Prefix(ref Game __instance, ref Vector3 pos, ref float __result)
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

    [HarmonyPatch(typeof(Game), "GetDifficultyHealthScale")]
    public static class ChangeDifficultyScaleHealth
    {
        private static Boolean Prefix(ref Game __instance, ref Vector3 pos, ref float __result)
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


    [HarmonyPatch(typeof(Game), "GetPlayerDifficulty")]
    public static class ChangePlayerDifficultyCount
    {
        private static Boolean Prefix(ref Game __instance, ref Vector3 pos, ref int __result)
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