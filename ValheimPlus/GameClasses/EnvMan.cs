using HarmonyLib;
using System;
using System.Text;
using UnityEngine;
using ValheimPlus.Configurations;

namespace ValheimPlus
{
    class TimeManipulation
    {
        /// <summary>
        /// Alters total day speed
        /// </summary>
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

        /// <summary>
        /// Alter night speed
        /// </summary>
        [HarmonyPatch(typeof(EnvMan), "FixedUpdate")]
        public static class TimeUpdateHook {
            private static void Prefix(ref EnvMan __instance)
            {
                if (Configuration.Current.Time.IsEnabled)
                {
                    bool isNight = __instance.IsNight();
                    if (isNight)
                    {
                        addToTime((long)Configuration.Current.Time.nightTimeSpeedMultiplier, Time.deltaTime);
                    }
                    
                }
            }
        }

        private static void addToTime(float amount, float deltaTime)
        {
            if (!ZNet.instance.IsServer())
            {
                return;
            }

            double num = ZNet.instance.GetTimeSeconds();
            num += (double)deltaTime *  amount;
            ZNet.instance.SetNetTime(num);
        }
    }
}
