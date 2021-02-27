using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ValheimPlus.Configurations;

namespace ValheimPlus {
    class TimeManipulation {

        
        [HarmonyPatch(typeof(EnvMan), "FixedUpdate")]
        public static class TimeUpdateHook {
            private static void Prefix(ref EnvMan __instance) {
                if (Configuration.Current.Time.IsEnabled)
                {
                    Boolean isNight = __instance.IsNight();
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
                // Debug.Log("Is not server.");
                return;
            }
            double num = ZNet.instance.GetTimeSeconds();
            // We target math to be aligned with the config: Increase the speed at which time passes at night by %. The value 50 would result in a 50% reduced amount of night time.
            // If 50% tends to reduce night by 2, we need to make 50% reach the tick rate which is 0.02 (deltaTime)
            num += (((double)deltaTime / 50) * amount);
            ZNet.instance.SetNetTime(num);
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
