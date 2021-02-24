using HarmonyLib;
using System.Collections.Generic;
using ValheimPlus.Configurations;
using UnityEngine;

namespace ValheimPlus
{
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

                    if (sleepingCount >= Configuration.Current.Sleep.numberOfPlayersToSleep)
                    {
                        return true;
                    }
                }
            }

            return _result;
        }
    }
}
