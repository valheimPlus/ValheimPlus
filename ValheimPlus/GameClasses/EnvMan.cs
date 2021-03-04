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
        public static void SetupDayLength() {
            if (Configuration.Current.Time.IsEnabled) {
                EnvMan instance = EnvMan.m_instance;
                if (instance) {
                    instance.m_dayLengthSec = (long)Configuration.Current.Time.totalDayTimeInSeconds;
                } else {
                    Debug.LogWarning("EnvMan instance not loaded");
                }
            }
        }

        // Refer to assembly_valheim/EnvMan IsDay/IsAfternoon/IsNight
        private const float startOfADay = 0.25f;
        private const float startOfNight = 0.75f;
        private const float startOfAfternoon = 0.5f;

        /// <summary>
        /// Return progression of the day on a range of 0 to 1
        /// </summary>
        public static double GetDayProgression() {
            double timeSecondsFromStart = ZNet.instance.GetTimeSeconds();
            double timeSecondsFromDay = timeSecondsFromStart % Configuration.Current.Time.totalDayTimeInSeconds;
            return timeSecondsFromDay / Configuration.Current.Time.totalDayTimeInSeconds;
        }

        /// <summary>
        /// Hook on EnvMan init to alter total day length
        /// </summary>
        [HarmonyPatch(typeof(EnvMan), "Awake")]
        public static class EnvMan_Awake_Patch {
            private static void Prefix(ref EnvMan __instance) {
                SetupDayLength();
            }
        }

        /// <summary>
        /// Hook on EnvMan IsDay to avoid fraction methods on the server side
        /// </summary>
        [HarmonyPatch(typeof(EnvMan), "IsDay")]
        class EnvMan_IsDay_Patch {
            static void Postfix(ref bool __result) {
                if (Configuration.Current.Time.IsEnabled && ZNet.instance != null && ZNet.instance.IsServer()) {
                    double progression = GetDayProgression();
                    __result = progression >= startOfADay && progression <= startOfNight;
                }
            }
        }

        /// <summary>
        /// Hook on EnvMan IsNight to avoid fraction methods on the server side
        /// </summary>
        [HarmonyPatch(typeof(EnvMan), "IsNight")]
        class EnvMan_IsNight_Patch {
            static void Postfix(ref bool __result) {
                if (Configuration.Current.Time.IsEnabled && ZNet.instance != null && ZNet.instance.IsServer()) {
                    double progression = GetDayProgression();
                    __result = progression <= startOfADay || progression >= startOfNight;
                }
            }
        }

        /// <summary>
        /// Hook on EnvMan IsAfternoon to avoid fraction methods on the server side
        /// </summary>
        [HarmonyPatch(typeof(EnvMan), "IsAfternoon")]
        class EnvMan_IsAfternoon_Patch {
            static void Postfix(ref bool __result) {
                if (Configuration.Current.Time.IsEnabled && ZNet.instance != null && ZNet.instance.IsServer()) {
                    double progression = GetDayProgression();
                    __result = progression >= startOfAfternoon && progression <= startOfNight;
                }
            }
        }

        /// <summary>
        /// Hook on EnvMan to alter night speed
        /// </summary>
        [HarmonyPatch(typeof(EnvMan), "FixedUpdate")]
        public static class EnvMan_FixedUpdate_Patch {
            private static void Prefix(ref EnvMan __instance) {
                if (!Configuration.Current.Time.IsEnabled) {
                    return;
                }
                if (ZNet.instance.IsServer() && ZNet.instance.GetNrOfPlayers() > 0 && !__instance.IsTimeSkipping() && __instance.IsNight()) {
                    double timeSeconds = ZNet.instance.GetTimeSeconds();
                    double num = timeSeconds + Helper.applyModifierValue(Time.fixedDeltaTime, Configuration.Current.Time.nightTimeSpeedMultiplier);
                    ZNet.instance.SetNetTime(num);
                }
            }
        }
    }
}
