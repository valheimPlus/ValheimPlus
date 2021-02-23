using HarmonyLib;
using ValheimPlus.Configurations;
using UnityEngine;

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


    [HarmonyPatch(typeof(Player), "UseStamina")]
    public static class ChangeStaminaUsageOfToolsAndWeapons
    {
        private static void Prefix(ref Player __instance, ref float v)
        {

            if (Configuration.Current.StaminaUsage.IsEnabled)
            {
                string weaponType = "";
                bool isHoe = false;
                bool isHammer = false;

                if (__instance.GetRightItem() == null)
                    weaponType = "Unarmed";

                if(weaponType != "Unarmed") 
                {
                    try
                    {
                        weaponType = __instance.GetRightItem().m_shared.m_skillType.ToString();
                    }
                    catch (System.Exception e)
                    { }

                    isHoe = (__instance.GetRightItem().m_shared.m_name == "$item_hoe" ? true : false);
                    isHammer = (__instance.GetRightItem().m_shared.m_name == "$item_hammer" ? true : false);
                }
                
                if(weaponType != "")
                switch (weaponType)
                {
                    case "Swords":
                        v = v - (v * (Configuration.Current.StaminaUsage.swords) / 100);
                        break;
                    case "Knives":
                        v = v - (v * (Configuration.Current.StaminaUsage.knives) / 100);
                        break;
                    case "Clubs":
                        v = v - (v * (Configuration.Current.StaminaUsage.clubs) / 100);
                        break;
                    case "Polearms":
                        v = v - (v * (Configuration.Current.StaminaUsage.polearms) / 100);
                        break;
                    case "Spears":
                        v = v - (v * (Configuration.Current.StaminaUsage.spears) / 100);
                        break;
                    case "Axes":
                        v = v - (v * (Configuration.Current.StaminaUsage.axes) / 100);
                            break;
                    case "Bows":
                        v = v - (v * (Configuration.Current.StaminaUsage.bows) / 100);
                        break;
                    case "Unarmed":
                        v = v - (v * (Configuration.Current.StaminaUsage.unarmed) / 100);
                        break;
                    case "Pickaxes":
                        v = v - (v * (Configuration.Current.StaminaUsage.pickaxes) / 100);
                        break;
                    default:
                        break;
                }

                if (isHammer)
                {
                    v = v - (v * (Configuration.Current.StaminaUsage.hammer) / 100);
                }
                if (isHoe)
                {
                    v = v - (v * (Configuration.Current.StaminaUsage.hoe) / 100);
                }
            }

        }
    }


}
