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
    [HarmonyPatch(typeof(Inventory), "IsTeleportable")]
    public static class noItemTeleportPrevention
    {
        private static void Postfix(ref Boolean __result)
        {
            if (Settings.isEnabled("Items"))
            {
                if (Settings.getBool("Items", "noTeleportPrevention"))
                    __result = true;
            }
        }
    }
}
