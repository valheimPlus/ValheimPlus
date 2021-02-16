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
    [HarmonyPatch(typeof(Beehive), "Awake")]
    public static class ApplyBeehiveChanges
    {
        private static bool Prefix(ref float ___m_secPerUnit, ref int ___m_maxHoney)
        {
            if (Settings.isEnabled("Beehive"))
            {
                ___m_secPerUnit = Settings.getFloat("Beehive", "honeyProductionSpeed");
                ___m_maxHoney = Settings.getInt("Beehive", "maximumHoneyPerBeehive");
            }
            return true;
        }

    }
}
