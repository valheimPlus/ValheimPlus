using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ValheimPlus.Configurations;

namespace ValheimPlus
{
    class TimeManipulation
    {


        [HarmonyPatch(typeof(EnvMan), "FixedUpdate")]
        public static class TimeUpdateHook
        {
            private static void Prefix(ref EnvMan __instance)
            {
                if (Configuration.Current.Time.IsEnabled)
                {
                    if (ZNet.instance.IsServer() && __instance.IsNight() && !__instance.IsTimeSkipping())
                    {
                        double timeSeconds = ZNet.instance.GetTimeSeconds();
                        double num = timeSeconds + (((double)Time.deltaTime / 50) * Configuration.Current.Time.nightTimeSpeedMultiplier);
                        double time = timeSeconds - (double)((float)__instance.m_dayLengthSec * 0.25f);
                        int day = __instance.GetDay(time);
                        double morningStartSec = __instance.GetMorningStartSec(day + 1);
                        // Make sure we don't go over the morning time
                        if (num > morningStartSec)
                        {
                           num = morningStartSec;
                        }
                        ZNet.instance.SetNetTime(num);
                    }
                }

            }
        }

        [HarmonyPatch(typeof(EnvMan), "Awake")]
        public static class TimeInitHook
        {
            private static void Prefix(ref EnvMan __instance)
            {
                if (Configuration.Current.Time.IsEnabled)
                {
                    __instance.m_dayLengthSec = (long)Configuration.Current.Time.totalDayTimeInSeconds;
                }
            }
        }
    }
}
