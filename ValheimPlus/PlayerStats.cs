using HarmonyLib;
using ValheimPlus.Configurations;

namespace ValheimPlus
{
    [HarmonyPatch(typeof(Player), "Awake")]
    public static class ModifyPlayerValues
    {
        private static void Postfix(Player __instance)
        {
            if (Configuration.Current.Stamina.IsEnabled)
            {
                __instance.m_dodgeStaminaUsage = Configuration.Current.Stamina.DodgeStaminaUsage;;
                __instance.m_encumberedStaminaDrain = Configuration.Current.Stamina.EncumberedStaminaDrain;
                __instance.m_sneakStaminaDrain = Configuration.Current.Stamina.SneakStaminaDrain;
                __instance.m_runStaminaDrain = Configuration.Current.Stamina.RunStaminaDrain;
                __instance.m_staminaRegenDelay = Configuration.Current.Stamina.StaminaRegenDelay;
                __instance.m_staminaRegen = Configuration.Current.Stamina.StaminaRegen;
                __instance.m_swimStaminaDrainMinSkill = Configuration.Current.Stamina.SwimStaminaDrain;
                __instance.m_jumpStaminaUsage = Configuration.Current.Stamina.JumpStaminaUsage;
            }
            if (Configuration.Current.Player.IsEnabled)
            {
                __instance.m_autoPickupRange = Configuration.Current.Player.BaseAutoPickUpRange;
                __instance.m_baseCameraShake = Configuration.Current.Player.DisableCameraShake ? 0f : 4f;
            }
            if (Configuration.Current.Building.IsEnabled)
            {
                __instance.m_maxPlaceDistance = Configuration.Current.Building.MaximumPlacementDistance;
            }
        }


    }

    [HarmonyPatch(typeof(Attack), "GetStaminaUsage")]
        public static class SelectiveWeaponStaminaDescrease {
            private static void Postfix(ref float __result, ItemDrop.ItemData ___m_weapon) {
                if (Configuration.Current.WeaponsStamina.IsEnabled)
	            {
                    string weaponType = ___m_weapon.m_shared.m_skillType.ToString();

                    switch (weaponType)
                    {
                        case "Swords":
                            __result = __result - ( __result * (Configuration.Current.WeaponsStamina.swords) / 100);
                            break;
                        case "Knives":
                            __result = __result - ( __result * (Configuration.Current.WeaponsStamina.knives) / 100);
                        break;
                        case "Clubs":
                            __result = __result - ( __result * (Configuration.Current.WeaponsStamina.clubs) / 100);
                        break;
                        case "Polearms":
                            __result = __result - ( __result * (Configuration.Current.WeaponsStamina.polearms) / 100);
                        break;
                        case "Spears":
                            __result = __result - ( __result * (Configuration.Current.WeaponsStamina.spears) / 100);
                        break;
                        case "Axes":
                            __result = __result - ( __result * (Configuration.Current.WeaponsStamina.axes) / 100);
                        break;
                        case "Bows":
                            __result = __result - ( __result * (Configuration.Current.WeaponsStamina.bows) / 100);
                        break;
                        case "Unarmed":
                            __result = __result - ( __result * (Configuration.Current.WeaponsStamina.unarmed) / 100);
                        break;
                        case "Pickaxes":
                            __result = __result - ( __result * (Configuration.Current.WeaponsStamina.pickaxes) / 100);
                        break;
                        default:
                            break;
                    } 
	            }
                
            }
        }


}
