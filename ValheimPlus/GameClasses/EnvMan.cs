using HarmonyLib;
using UnityEngine;
using ValheimPlus.Configurations;

namespace ValheimPlus.GameClasses
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

    [HarmonyPatch(typeof(EnvMan), "SetEnv")]
    public static class EnvMan_SetEnv_Patch
    {
        private static void Prefix(ref EnvMan __instance, ref EnvSetup env)
        {
            if (Configuration.Current.Game.IsEnabled && Configuration.Current.Game.disableFog)
            {
                env.m_fogDensityNight = 0.0001f;
                env.m_fogDensityMorning = 0.0001f;
                env.m_fogDensityDay = 0.0001f;
                env.m_fogDensityEvening = 0.0001f;
            }

            if (Configuration.Current.Brightness.IsEnabled)
            {
                applyEnvModifier(env);
            }
        }

        private static void applyEnvModifier(EnvSetup env)
        {
            /* changing brightness during a period of day had a coupling affect with other period, need further development
            env.m_fogColorMorning = applyBrightnessModifier(env.m_fogColorMorning, Configuration.Current.Brightness.morningBrightnessMultiplier);
            env.m_fogColorSunMorning = applyBrightnessModifier(env.m_fogColorSunMorning, Configuration.Current.Brightness.morningBrightnessMultiplier);
            env.m_sunColorMorning = applyBrightnessModifier(env.m_sunColorMorning, Configuration.Current.Brightness.morningBrightnessMultiplier);

            env.m_ambColorDay = applyBrightnessModifier(env.m_ambColorDay, Configuration.Current.Brightness.dayBrightnessMultiplier);
            env.m_fogColorDay = applyBrightnessModifier(env.m_fogColorDay, Configuration.Current.Brightness.dayBrightnessMultiplier);
            env.m_fogColorSunDay = applyBrightnessModifier(env.m_fogColorSunDay, Configuration.Current.Brightness.dayBrightnessMultiplier);
            env.m_sunColorDay = applyBrightnessModifier(env.m_sunColorDay, Configuration.Current.Brightness.dayBrightnessMultiplier);

            env.m_fogColorEvening = applyBrightnessModifier(env.m_fogColorEvening, Configuration.Current.Brightness.eveningBrightnessMultiplier);
            env.m_fogColorSunEvening = applyBrightnessModifier(env.m_fogColorSunEvening, Configuration.Current.Brightness.eveningBrightnessMultiplier);
            env.m_sunColorEvening = applyBrightnessModifier(env.m_sunColorEvening, Configuration.Current.Brightness.eveningBrightnessMultiplier);
            */

            env.m_ambColorNight = applyBrightnessModifier(env.m_ambColorNight, Configuration.Current.Brightness.nightBrightnessMultiplier);
            env.m_fogColorNight = applyBrightnessModifier(env.m_fogColorNight, Configuration.Current.Brightness.nightBrightnessMultiplier);
            env.m_fogColorSunNight = applyBrightnessModifier(env.m_fogColorSunNight, Configuration.Current.Brightness.nightBrightnessMultiplier);
            env.m_sunColorNight = applyBrightnessModifier(env.m_sunColorNight, Configuration.Current.Brightness.nightBrightnessMultiplier);
        }

        private static Color applyBrightnessModifier(Color color, float multiplier)
        {
            float h, s, v;
            Color.RGBToHSV(color, out h, out s, out v);
            float scaleFunc;
            if (multiplier >= 0)
            {
                scaleFunc = (Mathf.Sqrt(multiplier) * 1.069952679E-4f) + 1f;
            }
            else
            {
                scaleFunc = 1f - (Mathf.Sqrt(Mathf.Abs(multiplier)) * 1.069952679E-4f);
            }
            v = Mathf.Clamp01(v * scaleFunc);
            return Color.HSVToRGB(h, s, v);
        }
    }
}
