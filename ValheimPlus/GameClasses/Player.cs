using HarmonyLib;
using System;
using System.Collections.Generic;
using UnityEngine;
using ValheimPlus.Configurations;
using ValheimPlus.RPC;
using ValheimPlus.Utility;

namespace ValheimPlus.GameClasses
{
    /// <summary>
    /// Advanced Editing hook
    /// </summary>
    [HarmonyPatch(typeof(Player), "Update")]
    public static class AdvancedEditingModeHook
    {
        private static void Postfix(Player __instance)
        {
            if (Configuration.Current.AdvancedEditingMode.IsEnabled)
            {
                AEM.PlayerInstance = __instance;
                AEM.run();
            }
        }
    }

    /// <summary>
    /// Hook base Dodge method
    /// </summary>
    [HarmonyPatch(typeof(Player))]
    public class HookDodgeRoll
    {
        [HarmonyReversePatch]
        [HarmonyPatch(typeof(Player), "Dodge", new Type[] { typeof(Vector3) })]
        public static void Dodge(object instance, Vector3 dodgeDir) => throw new NotImplementedException();
    }

    /// <summary>
    /// Update maximum carry weight based on baseMaximumWeight and baseMegingjordBuff configurations.
    /// </summary>
    [HarmonyPatch(typeof(Player), "GetMaxCarryWeight")]
    public static class ModifyMaximumCarryWeight
    {
        private static void Postfix(ref float __result)
        {
            if (Configuration.Current.Player.IsEnabled)
            {
                bool Megingjord = false;
                float carryWeight = __result;

                if (carryWeight > 300)
                {
                    Megingjord = true;
                    carryWeight -= 150;
                }

                carryWeight = Configuration.Current.Player.baseMaximumWeight;
                if (Megingjord)
                {
                    carryWeight = carryWeight + Configuration.Current.Player.baseMegingjordBuff;
                }

                __result = carryWeight;
            }
        }
    }

    /// <summary>
    /// Add ValheimPlus intro to compendium.
    /// </summary>
    [HarmonyPatch(typeof(Player), "OnSpawned")]
    public static class ModifyOnSpawned
    {
        private static void Prefix()
        {
            //Show VPlus tutorial raven if not yet seen by the player's character.
            Tutorial.TutorialText introTutorial = new Tutorial.TutorialText()
            {
                m_label = "ValheimPlus Intro",
                m_name = "vplus",
                m_text = "We hope you enjoy the mod, please support our Patreon so we can continue to provide new updates!",
                m_topic = "Welcome to Valheim+"
            };

            if (!Tutorial.instance.m_texts.Contains(introTutorial))
            {
                Tutorial.instance.m_texts.Add(introTutorial);
            }

            Player.m_localPlayer.ShowTutorial("vplus");

            //Send map data to the server
            VPlusMapSync.SendMapToServer();
        }
    }

    /// <summary>
    /// Apply Dodge hotkeys
    /// </summary>
    [HarmonyPatch(typeof(Player), "Update")]
    public static class ApplyDodgeHotkeys
    {
        private static void Postfix(ref Player __instance, ref Vector3 ___m_moveDir, ref Vector3 ___m_lookDir, ref GameObject ___m_placementGhost, Transform ___m_eye)
        {
            if (!Configuration.Current.Hotkeys.IsEnabled) return;

            KeyCode rollKeyForward = Configuration.Current.Hotkeys.rollForwards;
            KeyCode rollKeyBackwards = Configuration.Current.Hotkeys.rollBackwards;

            if (Input.GetKeyDown(rollKeyBackwards))
            {
                Vector3 dodgeDir = ___m_moveDir;
                if (dodgeDir.magnitude < 0.1f)
                {
                    dodgeDir = -___m_lookDir;
                    dodgeDir.y = 0f;
                    dodgeDir.Normalize();
                }

                HookDodgeRoll.Dodge(__instance, dodgeDir);
            }
            if (Input.GetKeyDown(rollKeyForward))
            {
                Vector3 dodgeDir = ___m_moveDir;
                if (dodgeDir.magnitude < 0.1f)
                {
                    dodgeDir = ___m_lookDir;
                    dodgeDir.y = 0f;
                    dodgeDir.Normalize();
                }

                HookDodgeRoll.Dodge(__instance, dodgeDir);
            }
        }
    }

    /// <summary>
    /// Updates food duration.
    /// </summary>
    // ToDo have item tooltips be affected.
    [HarmonyPatch(typeof(Player), "UpdateFood")]
    public static class ApplyFoodChanges
    {
        private static bool Prefix(ref Player __instance, ref float dt, ref bool forceUpdate)
        {
            if (!Configuration.Current.Food.IsEnabled) return true;

            __instance.m_foodUpdateTimer += dt;
            if (__instance.m_foodUpdateTimer >= getModifiedDeltaTime(ref __instance, ref dt) || forceUpdate)
            {
                __instance.m_foodUpdateTimer = 0f;
                foreach (Player.Food food in __instance.m_foods)
                {
                    food.m_health -= food.m_item.m_shared.m_food / food.m_item.m_shared.m_foodBurnTime;
                    food.m_stamina -= food.m_item.m_shared.m_foodStamina / food.m_item.m_shared.m_foodBurnTime;
                    if (food.m_health < 0f)
                    {
                        food.m_health = 0f;
                    }
                    if (food.m_stamina < 0f)
                    {
                        food.m_stamina = 0f;
                    }
                    if (food.m_health <= 0f)
                    {
                        __instance.Message(MessageHud.MessageType.Center, "$msg_food_done", 0, null);
                        __instance.m_foods.Remove(food);
                        break;
                    }
                }

                __instance.GetTotalFoodValue(out float health, out float stamina);
                __instance.SetMaxHealth(health, true);
                __instance.SetMaxStamina(stamina, true);
            }
            if (!forceUpdate)
            {
                __instance.m_foodRegenTimer += dt;
                if (__instance.m_foodRegenTimer >= 10f)
                {
                    __instance.m_foodRegenTimer = 0f;
                    float num = 0f;
                    foreach (Player.Food food2 in __instance.m_foods)
                    {
                        num += food2.m_item.m_shared.m_foodRegen;
                    }
                    if (num > 0f)
                    {
                        float num2 = 1f;
                        __instance.m_seman.ModifyHealthRegen(ref num2);
                        num *= num2;
                        Helper.getPlayerCharacter(__instance).Heal(num, true);
                    }
                }
            }

            return false;
        }

        private static float getModifiedDeltaTime(ref Player __instance, ref float dt)
        {
            float defaultDeltaTimeTarget = 1f;
            float newDetalTimeTarget = 1f;

            float food_multiplier = Configuration.Current.Food.foodDurationMultiplier;
            if (food_multiplier == 50) food_multiplier = 51; // Decimal issue
            if (food_multiplier == -50) food_multiplier = -51; // Decimal issue

            if (food_multiplier >= 0)
            {
                newDetalTimeTarget = defaultDeltaTimeTarget + ((defaultDeltaTimeTarget / 100) * food_multiplier);
            }
            else
            {
                newDetalTimeTarget = defaultDeltaTimeTarget - ((defaultDeltaTimeTarget / 100) * (food_multiplier * -1));
            }

            return newDetalTimeTarget;
        }
    }

    /// <summary>
    /// Alter player stamina values
    /// </summary>
    [HarmonyPatch(typeof(Player), "Awake")]
    public static class ModifyPlayerValues
    {
        private static void Postfix(Player __instance)
        {
            if (Configuration.Current.Stamina.IsEnabled)
            {
                __instance.m_dodgeStaminaUsage = Configuration.Current.Stamina.dodgeStaminaUsage; ;
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

    /// <summary>
    /// Alters stamina of tools and weapons
    /// </summary>
    [HarmonyPatch(typeof(Player), "UseStamina")]
    public static class ChangeStaminaUsageOfToolsAndWeapons
    {
        private static void Prefix(ref Player __instance, ref float v)
        {
            // TODO add a check for the origin of the call of this function to restrict it to not reduce stamina drain of running/jumping/swimming etc.

            if (Configuration.Current.StaminaUsage.IsEnabled)
            {
                string weaponType = "";
                bool isHoe = false;
                bool isHammer = false;

                if (__instance.GetRightItem() == null)
                    weaponType = "Unarmed";

                if (weaponType != "Unarmed")
                {
                    try
                    {
                        weaponType = __instance.GetRightItem().m_shared.m_skillType.ToString();
                    }
                    catch
                    {
                    }

                    isHoe = (__instance.GetRightItem().m_shared.m_name == "$item_hoe" ? true : false);
                    isHammer = (__instance.GetRightItem().m_shared.m_name == "$item_hammer" ? true : false);
                }

                if (weaponType != "")
                {
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
