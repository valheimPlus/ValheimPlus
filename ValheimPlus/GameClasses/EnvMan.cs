using HarmonyLib;
using UnityEngine;
using ValheimPlus.Configurations;

namespace ValheimPlus
{
    public class TimeManipulation
    {
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

        public static double GetDayProgression()
        {
            double timeSecondsFromStart = ZNet.instance.GetTimeSeconds();
            double timeSecondsFromDay = timeSecondsFromStart % Configuration.Current.Time.totalDayTimeInSeconds;
            return timeSecondsFromDay / Configuration.Current.Time.totalDayTimeInSeconds;
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

        [HarmonyPatch(typeof(EnvMan), "IsDay")]
        class EnvMan_IsDay_Patch
        {
            static bool Postfix(bool result)
            {
                if (ZNet.instance.IsServer())
                {
                    double progression = GetDayProgression();
                    result = progression >= 0.25f && progression <= 0.75f;
                }
                return result;
            }
        }

        [HarmonyPatch(typeof(EnvMan), "IsNight")]
        class EnvMan_IsNight_Patch
        {
            static bool Postfix(bool result)
            {
                if (ZNet.instance.IsServer())
                {
                    double progression = GetDayProgression();
                    result = progression <= 0.25f || progression >= 0.75f;
                }
                return result;
            }
        }

        [HarmonyPatch(typeof(EnvMan), "IsAfternoon")]
        class EnvMan_IsAfternoon_Patch
        {
            static bool Postfix(bool result)
            {
                if (ZNet.instance.IsServer())
                {
                    double progression = GetDayProgression();
                    result = progression >= 0.5f && progression <= 0.75f;
                }
                return result;
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
                if (!Configuration.Current.Time.IsEnabled)
                {
                    return;
                }
                if (ZNet.instance.IsServer() && ZNet.instance.GetNrOfPlayers() > 0 && !__instance.IsTimeSkipping() && __instance.IsNight())
                {
                    double timeSeconds = ZNet.instance.GetTimeSeconds();
                    double num = timeSeconds + Helper.applyModifierValue(Time.fixedDeltaTime, Configuration.Current.Time.nightTimeSpeedMultiplier);
                    ZNet.instance.SetNetTime(num);
                }
            }
        }
    }
}
