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
                __instance.m_jumpStaminaUsage = Settings.getFloat("Stamina", "jumpStaminaUsage");
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

    [HarmonyPatch(typeof(Attack), "GetStaminaUsage")]
        public static class SelectiveWeaponStaminaDescrease {
            private static void Postfix(ref float __result, ItemDrop.ItemData ___m_weapon) {
                if (Settings.isEnabled("WeaponsStamina"))
	            {
                    string weaponType = ___m_weapon.m_shared.m_skillType.ToString();

                    switch (weaponType)
                    {
                        case "Swords":
                            __result = __result - ( __result * (Settings.getFloat("WeaponsStamina", "Swords")/100) );
                            break;
                        case "Knives":
                            __result = __result - ( __result * (Settings.getFloat("WeaponsStamina", "Knives")/100) );
                            break;
                        case "Clubs":
                            __result = __result - ( __result * (Settings.getFloat("WeaponsStamina", "Clubs")/100) );
                            break;
                        case "Polearms":
                            __result = __result - ( __result * (Settings.getFloat("WeaponsStamina", "Polearms")/100) );
                            break;
                        case "Spears":
                            __result = __result - ( __result * (Settings.getFloat("WeaponsStamina", "Spears")/100) );
                            break;
                        case "Axes":
                            __result = __result - ( __result * (Settings.getFloat("WeaponsStamina", "Axes")/100) );
                            break;
                        case "Bows":
                            __result = __result - ( __result * (Settings.getFloat("WeaponsStamina", "Bows")/100) );
                            break;
                        case "Unarmed":
                            __result = __result - ( __result * (Settings.getFloat("WeaponsStamina", "Unarmed")/100) );
                            break;
                        case "Pickaxes":
                            __result = __result - ( __result * (Settings.getFloat("WeaponsStamina", "Pickaxes")/100) );
                            break;
                        default:
                            break;
                    } 
	            }
                
            }
        }


}
