using HarmonyLib;
using UnityEngine;
using ValheimPlus.Configurations;

namespace ValheimPlus
{
    public class TimeManipulation
    {
        /// <summary>
        /// Hook on EnvMan FixedUpdate to adjust dayLengthSec
        /// </summary>
        [HarmonyPatch(typeof(EnvMan), "FixedUpdate")]
        class EnvMan_FixedUpdate_Patch
        {
            private static void Prefix(ref EnvMan __instance)
            {
                if (Configuration.Current.Time.IsEnabled)
                {
                    __instance.m_dayLengthSec = (long)(Configuration.Current.Time.totalDayTimeInSeconds * (2f / 3f)); // A day length in valhein is 2/3 of a human full day cycle
                } else
                {
                    __instance.m_dayLengthSec = 1200L; // Default valheim value
                }
            }
        }


        /// <summary>
        /// Hook on EnvMan RescaleDayFraction to adjust nightTime + dayLengthSec
        /// </summary>
        [HarmonyPatch(typeof(EnvMan), "RescaleDayFraction")]
        class EnvMan_RescaleDayFraction_Patch
        {
            static void Prefix(ref EnvMan __instance, float fraction, ref float __result)
            {
                if (ZNet.instance != null && ZNet.instance.IsServer() && ZNet.instance.GetNrOfPlayers() > 0 && !__instance.IsTimeSkipping() && Configuration.Current.Time.IsEnabled)
                {
                    // If Night (late night / early morning) or Night (early night)
                    if (fraction < 0.15f || fraction > 0.85f) 
                    {
                        ZNet.instance.m_netTime += Helper.applyModifierValue(Time.fixedDeltaTime, Configuration.Current.Time.nightTimeSpeedMultiplier); // If out of sync; server every 2s resync netTime
                    }
                }
            }
        }
    }
}
