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
    [HarmonyPatch(typeof(Player), "Awake")]
    public static class ModifyPlayerValues
    {
        private static void Postfix(Player __instance)
        {
            if (Settings.isEnabled("Stamina"))
            {
                __instance.m_dodgeStaminaUsage = Settings.getFloat("Stamina", "dodgeStaminaUsage");
                __instance.m_encumberedStaminaDrain = Settings.getFloat("Stamina", "encumberedStaminaDrain");
                __instance.m_sneakStaminaDrain = Settings.getFloat("Stamina", "sneakStaminaDrain");
                __instance.m_runStaminaDrain = Settings.getFloat("Stamina", "runStaminaDrain");
                __instance.m_staminaRegenDelay = Settings.getFloat("Stamina", "staminaRegenDelay");
                __instance.m_staminaRegen = Settings.getFloat("Stamina", "staminaRegen");
                __instance.m_swimStaminaDrainMinSkill = Settings.getFloat("Stamina", "swimStaminaDrain");
            }
            if (Settings.isEnabled("Player"))
            {
                __instance.m_autoPickupRange = Settings.getFloat("Player", "baseAutoPickUpRange");
                __instance.m_baseCameraShake = Settings.getBool("Player", "disableCameraShake") ? 0f : 4f;
            }
            if (Settings.isEnabled("Building"))
            {
                __instance.m_maxPlaceDistance = Settings.getFloat("Building", "maximumPlacementDistance");
            }
        }
    }


}
