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
                    ItemDrop.ItemData item = __instance.m_character.GetCurrentWeapon();
                    Skills.SkillType skillType;
                    if (item == null)
                    {
                        skillType = Skills.SkillType.Unarmed;
                    }
                    else 
                    { 
                        skillType = item.m_shared.m_skillType; 
                    }

                    switch (skillType)
                    {
                        case Skills.SkillType.Swords:
                            __result -= __result * Configuration.Current.StaminaUsage.swords / 100;
                            break;
                        case Skills.SkillType.Knives:
                            __result -= __result * Configuration.Current.StaminaUsage.knives / 100;
                            break;
                        case Skills.SkillType.Clubs:
                            __result -= __result * Configuration.Current.StaminaUsage.clubs / 100;
                            break;
                        case Skills.SkillType.Polearms:
                            __result -= __result * Configuration.Current.StaminaUsage.polearms / 100;
                            break;
                        case Skills.SkillType.Spears:
                            __result -= __result * Configuration.Current.StaminaUsage.spears / 100;
                            break;
                        case Skills.SkillType.Axes:
                            __result -= __result * Configuration.Current.StaminaUsage.axes / 100;
                            break;
                        case Skills.SkillType.Unarmed:
                            __result -= __result * Configuration.Current.StaminaUsage.unarmed / 100;
                            break;
                        case Skills.SkillType.Pickaxes:
                            __result -= __result * Configuration.Current.StaminaUsage.pickaxes / 100;
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }

    [HarmonyPatch(typeof(Player), "UseStamina")]
    public static class ChangeStaminaUsageOfToolsBowsAndBlocking
    {
        private static void Prefix(ref Player __instance, ref float v)
        {
            if (Configuration.Current.StaminaUsage.IsEnabled)
            {
                string methodName = new StackTrace().GetFrame(2).GetMethod().Name;
                if (methodName.Equals(nameof(Player.UpdatePlacement)) || methodName.Equals(nameof(Player.Repair)))
                {
                    string itemName = __instance.GetRightItem()?.m_shared.m_name;
                    if (itemName == "$item_hammer")
                    {
                        v -= v * Configuration.Current.StaminaUsage.hammer / 100;
                    }
                    else if (itemName == "$item_hoe")
                    {
                        v -= v * Configuration.Current.StaminaUsage.hoe / 100;
                    }
                    else if (itemName == "$item_cultivator")
                    {
                        v -= v * Configuration.Current.StaminaUsage.cultivator / 100;
                    }
                }
                else if (methodName.Equals(nameof(Player.PlayerAttackInput)))
                {
                    ItemDrop.ItemData item = __instance.GetCurrentWeapon();
                    if (item?.m_shared.m_skillType == Skills.SkillType.Bows)
                    {
                        v -= v * Configuration.Current.StaminaUsage.bows / 100;
                    }
                }
                else if (methodName.Equals(nameof(Player.BlockAttack)))
                {
                    v -= v * Configuration.Current.StaminaUsage.blocking / 100;
                }
            }
        }
    }
}
