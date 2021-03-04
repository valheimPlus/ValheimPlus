using HarmonyLib;
using UnityEngine;
using ValheimPlus.Configurations;

namespace ValheimPlus
{
    public class TimeManipulation
    {
        /*
        /// <summary>
        /// Alter the total day length in seconds base on configuration file
        /// </summary>
        public static void SetupDayLength()
        {
            if (Configuration.Current.Time.IsEnabled)
            {
                EnvMan instance = EnvMan.m_instance;
                if (instance)
                {
                    instance.m_dayLengthSec = (long)Configuration.Current.Time.totalDayTimeInSeconds;
                }
                else
                {
                    Debug.LogWarning("EnvMan instance not loaded");
                }
            }
        }

        /// <summary>
        /// Hook on EnvMan init to alter total day length
        /// </summary>
        [HarmonyPatch(typeof(EnvMan), "Awake")]
        public static class TimeInitHook
        {
            private static void Prefix(ref EnvMan __instance)
            {
                SetupDayLength();
            }
        }

        /// <summary>
        /// Hook on EnvMan to alter night speed
        /// </summary>
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
                        double num = timeSeconds + Helper.applyModifierValue(Configuration.Current.Time.nightTimeSpeedMultiplier, Time.deltaTime);
                        double time = timeSeconds - (double)((float)__instance.m_dayLengthSec * 0.25f);
                        int day = __instance.GetDay(time);
                        double morningStartSec = __instance.GetMorningStartSec(day + 1);
                        // Make sure we don't go over the morning time
                        if (num < morningStartSec)
                        {
                            ZNet.instance.SetNetTime(num);
                        }
                    }
                }
            }
        }
        */
    }
}
