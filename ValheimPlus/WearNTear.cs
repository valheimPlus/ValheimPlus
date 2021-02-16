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
    [HarmonyPatch(typeof(WearNTear), "ApplyDamage")]
    public static class RemoveWearNTear
    {
        private static Boolean Prefix()
        {
            if (Settings.isEnabled("Building") && Settings.getBool("Building", "noWeatherDamage"))
            {
                return false;
            }
            return true;
        }
    }
}
