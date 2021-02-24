using System;
using HarmonyLib;
using Steamworks;
using ValheimPlus.Configurations;
using UnityEngine;
using System.Collections.Generic;

namespace ValheimPlus
{
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
    
    [HarmonyPatch(typeof(Game), "EverybodyIsTryingToSleep")]
    public static class Game_EverybodyIsTryingToSleep
    {
        private static bool Postfix(bool _result, ref ZDO __instance)
        {
            if (Configuration.Current.Sleep.IsEnabled)
            {
                if (Configuration.Current.Server.IsEnabled)
                {
                    List<ZDO> allCharacterZdos = ZNet.instance.GetAllCharacterZDOS();
                    double sleepingCount = 0;
                    if (allCharacterZdos.Count == 0)
                        return false;
                    foreach (ZDO zdo in allCharacterZdos)
                    {
                        if (zdo.GetBool("inBed"))
                        {
                            sleepingCount++;
                        }
                    }

                    if (Configuration.Current.Sleep.byPlayers)
                    {
                        if (sleepingCount >= Configuration.Current.Sleep.numberOfPlayersToSleep)
                        {
                            return true;
                        }
                    }

                    if(Configuration.Current.Sleep.byPercentage)
                    {
                        double percentSleeping = sleepingCount / allCharacterZdos.Count * 100;
                        if (percentSleeping >= Configuration.Current.Sleep.percentageOfPlayersToSleep)
                        {
                            return true;
                        }
                    }
                }
            }

            return _result;
        }
    }
}
