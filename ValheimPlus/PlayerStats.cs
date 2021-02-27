using HarmonyLib;
using ValheimPlus.Configurations;
using System.Diagnostics;

namespace ValheimPlus
{
    [HarmonyPatch(typeof(Player), "Awake")]
    public static class ModifyPlayerValues
    {
        private static void Postfix(Player __instance)
        {
            if (Configuration.Current.Stamina.IsEnabled)
            {
                __instance.m_dodgeStaminaUsage = Configuration.Current.Stamina.dodgeStaminaUsage;;
                __instance.m_encumberedStaminaDrain = Configuration.Current.Stamina.encumberedStaminaDrain;
                __instance.m_sneakStaminaDrain = Configuration.Current.Stamina.sneakStaminaDrain;
                __instance.m_runStaminaDrain = Configuration.Current.Stamina.runStaminaDrain;
                __instance.m_staminaRegenDelay = Configuration.Current.Stamina.staminaRegenDelay;
                __instance.m_staminaRegen = Configuration.Current.Stamina.staminaRegen;
                __instance.m_swimStaminaDrainMinSkill = Configuration.Current.Stamina.swimStaminaDrain;
                __instance.m_jumpStaminaUsage = Configuration.Current.Stamina.jumpStaminaDrain;
            }
            if (Configuration.Current.Player.IsEnabled)
            {
                __instance.m_autoPickupRange = Configuration.Current.Player.baseAutoPickUpRange;
                __instance.m_baseCameraShake = Configuration.Current.Player.disableCameraShake ? 0f : 4f;
            }
            if (Configuration.Current.Building.IsEnabled)
            {
                __instance.m_maxPlaceDistance = Configuration.Current.Building.maximumPlacementDistance;
            }
        }
    }

    [HarmonyPatch(typeof(Attack), "GetStaminaUsage")]
    public static class ChangeStaminaUsageOfWeapons
    {
        private static void Postfix(ref Attack __instance, ref float __result)
        {
            if (Configuration.Current.StaminaUsage.IsEnabled)
            {
                if (__instance.m_character.IsPlayer())
                {
                    ItemDrop.ItemData item = __instance.m_character.GetRightItem();
                    string weaponType = "";

                    if (item == null)
                    {
                        weaponType = "Unarmed";
                    }
                    else if (item.IsWeapon())
                    {
                        try
                        {
                            weaponType = item.m_shared.m_skillType.ToString();
                        }
                        catch (System.Exception e) { }
                    }

                    if (weaponType != "")
                    {
                        switch (weaponType)
                        {
                            case "Swords":
                                __result -= __result * Configuration.Current.StaminaUsage.swords / 100;
                                break;
                            case "Knives":
                                __result -= __result * Configuration.Current.StaminaUsage.knives / 100;
                                break;
                            case "Clubs":
                                __result -= __result * Configuration.Current.StaminaUsage.clubs / 100;
                                break;
                            case "Polearms":
                                __result -= __result * Configuration.Current.StaminaUsage.polearms / 100;
                                break;
                            case "Spears":
                                __result -= __result * Configuration.Current.StaminaUsage.spears / 100;
                                break;
                            case "Axes":
                                __result -= __result * Configuration.Current.StaminaUsage.axes / 100;
                                break;
                            case "Bows":
                                __result -= __result * Configuration.Current.StaminaUsage.bows / 100;
                                break;
                            case "Unarmed":
                                __result -= __result * Configuration.Current.StaminaUsage.unarmed / 100;
                                break;
                            case "Pickaxes":
                                __result -= __result * Configuration.Current.StaminaUsage.pickaxes / 100;
                                break;
                            default:
                                break;
                        }
                    }       
                }
            }
        }
    }

    [HarmonyPatch(typeof(Player), "UseStamina")]
    public static class ChangeStaminaUsageOfTools
    {
        private static void Prefix(ref Player __instance, ref float v)
        {
            if (Configuration.Current.StaminaUsage.IsEnabled)
            {
                string methodName = new StackTrace().GetFrame(2).GetMethod().Name;
                if (methodName == "UpdatePlacement" || methodName == "Repair")
                {
                    ItemDrop.ItemData item = __instance.GetRightItem();
                    if (item != null)
                    {
                        string itemName = item.m_shared.m_name;
                        bool isHoe = itemName == "$item_hoe";
                        bool isHammer = itemName == "$item_hammer";
                        bool isCultivator = itemName == "$item_cultivator";

                        if (isHammer)
                        {
                            v -= v * Configuration.Current.StaminaUsage.hammer / 100;
                        }
                        else if (isHoe)
                        {
                            v -= v * Configuration.Current.StaminaUsage.hoe / 100;
                        }
                        else if (isCultivator)
                        {
                            v -= v * Configuration.Current.StaminaUsage.cultivator / 100;
                        }
                    }
                }
            }
        }
    }
}
