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
                    if (__instance.IsNight() && !__instance.IsTimeSkipping())
                    {
                        addToTime((long)Configuration.Current.Time.nightTimeSpeedMultiplier, Time.deltaTime);
                    }
                }

            }
            private static void addToTime(float amount, float deltaTime)
            {
                double timeSeconds = ZNet.instance.GetTimeSeconds();
                timeSeconds += (((double)deltaTime / 50) * amount);
                ZNet.instance.SetNetTime(timeSeconds);
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
