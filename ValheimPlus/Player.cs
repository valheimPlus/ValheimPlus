using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BepInEx;
using Unity;
using UnityEngine;
using System.IO;
using System.Reflection;
using System.Runtime;
using IniParser;
using IniParser.Model;
using HarmonyLib;
using System.Globalization;
using Steamworks;
using ValheimPlus;


namespace ValheimPlus
{
    [HarmonyPatch(typeof(Player), "GetMaxCarryWeight")]
    public static class ModifyMaximumCarryWeight
    {
        private static void Postfix(ref float __result)
        {
            if (Settings.isEnabled("Player"))
            {
                bool Megingjord = false;
                float carryWeight = __result;

                if (carryWeight > 300)
                {
                    Megingjord = true;
                    carryWeight -= 150;
                }

                carryWeight = Settings.getFloat("Player", "baseMaximumWeight");
                if (Megingjord)
                {
                    carryWeight = carryWeight + Settings.getFloat("Player", "baseMegingjordBuff");
                }

                __result = carryWeight;
            }
        }
    }

    // ToDo have item tooltips be affected.
    [HarmonyPatch(typeof(Player), "UpdateFood")]
    public static class ApplyFoodChanges
    {
        private static Boolean Prefix(ref Player __instance,float dt,ref bool forceUpdate)
        {
            __instance.m_foodUpdateTimer += dt;

            float defaultDeltaTimeTarget = 1f;
            float newDetalTimeTarget = 1f;

            if (Settings.isEnabled("Food"))
            {
                float food_multiplier = Settings.getFloat("Food", "foodDuration");

                if (food_multiplier >= 0)
                {
                    newDetalTimeTarget = defaultDeltaTimeTarget + ((defaultDeltaTimeTarget / 100) * food_multiplier);
                }
                else
                {
                    newDetalTimeTarget = defaultDeltaTimeTarget - ((defaultDeltaTimeTarget / 100) * (food_multiplier * -1));
                }

                if (__instance.m_foodUpdateTimer >= newDetalTimeTarget || forceUpdate)
                {
                    __instance.m_foodUpdateTimer = defaultDeltaTimeTarget;
                    return true;
                }

                return false;
            }
            return true;
        }

    }

}
