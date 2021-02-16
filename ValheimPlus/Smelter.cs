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
    [HarmonyPatch(typeof(Smelter), "Awake")]
    public static class ApplyFurnaceChanges
    {
        private static void Prefix(ref Smelter __instance)
        {
            if (!__instance.m_addWoodSwitch && Settings.isEnabled("Kiln"))
            {
                // is kiln
                __instance.m_maxOre = Settings.getInt("Kiln", "maximumWood");
                __instance.m_secPerProduct = Settings.getFloat("Kiln", "productionSpeed");
            }
            else
            {
                // is furnace
                if (Settings.isEnabled("Furnace"))
                {
                    __instance.m_maxOre = Settings.getInt("Furnace", "maximumOre");
                    __instance.m_maxFuel = Settings.getInt("Furnace", "maximumCoal");
                    __instance.m_secPerProduct = Settings.getFloat("Furnace", "productionSpeed");
                    __instance.m_fuelPerProduct = Settings.getInt("Furnace", "coalUsedPerProduct");
                }
            }
        }
    }
}
