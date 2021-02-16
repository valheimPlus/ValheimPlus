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
    [HarmonyPatch(typeof(Fermenter), "Awake")]
    public static class ApplyFermenterChanges
    {
        private static bool Prefix(ref float ___m_fermentationDuration, ref Fermenter __instance)
        {
            if (Settings.isEnabled("Fermenter"))
            {
                float fermenterDuration = Settings.getFloat("Fermenter", "fermenterDuration");
                if (fermenterDuration > 0)
                {
                    ___m_fermentationDuration = fermenterDuration;
                }
            }
            return true;
        }

    }
    [HarmonyPatch(typeof(Fermenter), "GetItemConversion")]
    public static class ApplyFermenterItemCountChanges
    {
        private static void Postfix(ref Fermenter.ItemConversion __result)
        {
            if (Settings.isEnabled("Fermenter"))
            {
                int fermenterItemCount = Settings.getInt("Fermenter", "fermenterItemsProduced");
                if (fermenterItemCount > 0)
                {
                    __result.m_producedItems = fermenterItemCount;
                }
            }

        }

    }
}
