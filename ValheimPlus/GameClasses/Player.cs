using HarmonyLib;
using System;
using System.Diagnostics;
using UnityEngine;
using ValheimPlus.Configurations;

namespace ValheimPlus
{
    /// <summary>
    /// Hooks for ABM and AEM
    /// </summary>
    [HarmonyPatch(typeof(Player), "Update")]
    public static class AdvancedBuildingAndEditingModeHook
    {
        private static void Postfix(Player __instance)
        {
            if (Configuration.Current.AdvancedEditingMode.IsEnabled)
            {
                AEM.PlayerInstance = __instance;
                AEM.run();
            }

            if (Configuration.Current.AdvancedBuildingMode.IsEnabled)
            {
                ABM.PlayerInstance = __instance;
                ABM.run();
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
            float newDetalTimeTarget;

            newDetalTimeTarget = Helper.applyModifierValue(defaultDeltaTimeTarget, Configuration.Current.Food.foodDurationMultiplier);

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
                __instance.m_dodgeStaminaUsage = Helper.applyModifierValue(__instance.m_dodgeStaminaUsage, Configuration.Current.Stamina.dodgeStaminaUsage);
                __instance.m_encumberedStaminaDrain = Helper.applyModifierValue(__instance.m_encumberedStaminaDrain, Configuration.Current.Stamina.encumberedStaminaDrain);
                __instance.m_sneakStaminaDrain = Helper.applyModifierValue(__instance.m_sneakStaminaDrain, Configuration.Current.Stamina.sneakStaminaDrain);
                __instance.m_runStaminaDrain = Helper.applyModifierValue(__instance.m_runStaminaDrain,Configuration.Current.Stamina.runStaminaDrain);
                __instance.m_staminaRegenDelay = Helper.applyModifierValue(__instance.m_staminaRegenDelay, Configuration.Current.Stamina.staminaRegenDelay);
                __instance.m_staminaRegen = Helper.applyModifierValue(__instance.m_staminaRegen, Configuration.Current.Stamina.staminaRegen);
                __instance.m_swimStaminaDrainMinSkill = Helper.applyModifierValue(__instance.m_swimStaminaDrainMinSkill, Configuration.Current.Stamina.swimStaminaDrain);
                __instance.m_jumpStaminaUsage = Helper.applyModifierValue(__instance.m_jumpStaminaUsage, Configuration.Current.Stamina.jumpStaminaDrain);
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
    /// Alters stamina of weapons
    /// </summary>
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
                            __result = Helper.applyModifierValue(__result, Configuration.Current.StaminaUsage.swords);
                            break;
                        case Skills.SkillType.Knives:
                            __result = Helper.applyModifierValue(__result, Configuration.Current.StaminaUsage.knives);
                            break;
                        case Skills.SkillType.Clubs:
                            __result = Helper.applyModifierValue(__result, Configuration.Current.StaminaUsage.clubs);
                            break;
                        case Skills.SkillType.Polearms:
                            __result = Helper.applyModifierValue(__result, Configuration.Current.StaminaUsage.polearms);
                            break;
                        case Skills.SkillType.Spears:
                            __result = Helper.applyModifierValue(__result, Configuration.Current.StaminaUsage.spears);
                            break;
                        case Skills.SkillType.Axes:
                            __result = Helper.applyModifierValue(__result, Configuration.Current.StaminaUsage.axes);
                            break;
                        case Skills.SkillType.Unarmed:
                            __result = Helper.applyModifierValue(__result, Configuration.Current.StaminaUsage.unarmed);
                            break;
                        case Skills.SkillType.Pickaxes:
                            __result = Helper.applyModifierValue(__result, Configuration.Current.StaminaUsage.pickaxes);
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }

    /// <summary>
    /// Alters stamina of tools, bows and blocking
    /// </summary>
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
                        v = Helper.applyModifierValue(v, Configuration.Current.StaminaUsage.hammer);
                    }
                    else if (itemName == "$item_hoe")
                    {
                        v = Helper.applyModifierValue(v, Configuration.Current.StaminaUsage.hoe);
                    }
                    else if (itemName == "$item_cultivator")
                    {
                        v = Helper.applyModifierValue(v, Configuration.Current.StaminaUsage.cultivator);
                    }
                }
                else if (methodName.Equals(nameof(Player.PlayerAttackInput)))
                {
                    ItemDrop.ItemData item = __instance.GetCurrentWeapon();
                    if (item?.m_shared.m_skillType == Skills.SkillType.Bows)
                    {
                        v = Helper.applyModifierValue(v, Configuration.Current.StaminaUsage.bows);
                    }
                }
                else if (methodName.Equals(nameof(Player.BlockAttack)))
                {
                    v = Helper.applyModifierValue(v, Configuration.Current.StaminaUsage.blocking);
                }
            }
        }
    }

	/// <summary>
    /// Starts ABM if not already started
    /// And checks if the player is trying to place a plant/crop too close to another plant/crop
    /// </summary>
    [HarmonyPatch(typeof(Player), "UpdatePlacementGhost")]
    public static class ModifyPlacingRestrictionOfGhost
    {
        private static Boolean Prefix(Player __instance, bool flashGuardStone)
        {
            if (Configuration.Current.AdvancedBuildingMode.IsEnabled)
            {
                ABM.PlayerInstance = __instance;
                ABM.run();
            }
            if (ABM.isActive)
                return false;
            return true;
        }


        private static void Postfix(ref Player __instance)
        {
            if (ABM.exitOnNextIteration)
            {
                try
                {
                    if (__instance.m_placementMarkerInstance)
                    {
                        __instance.m_placementMarkerInstance.SetActive(false);
                    }
                }
                catch (Exception e)
                {

                }
            }

            if (Configuration.Current.Building.noInvalidPlacementRestriction)
            {
                try
                {
                    if (__instance.m_placementStatus == Player.PlacementStatus.Invalid)
                    {
                        __instance.m_placementStatus = Player.PlacementStatus.Valid;
                        __instance.m_placementGhost.GetComponent<Piece>().SetInvalidPlacementHeightlight(false);
                    }
                }
                catch(Exception e)
                {

                }
            }


			if (Configuration.Current.Player.IsEnabled && Configuration.Current.Player.cropNotifier)
            {

                if (__instance.m_placementGhost == null)
                    return;

                // Check to see if the current placement ghost is has a plant component
                Plant plantComponent = __instance.m_placementGhost.GetComponent<Plant>();
                if (plantComponent != null && __instance.m_placementStatus == Player.PlacementStatus.Valid)
                {
                    LayerMask mask = LayerMask.GetMask("Default", "static_solid", "Default_small", "piece", "piece_nonsolid");
                    // Create an array of objects that are within the grow radius of the current placement ghost plant/crop
                    Collider[] array = Physics.OverlapSphere(__instance.m_placementGhost.transform.position, plantComponent.m_growRadius, mask);
                    for (int i = 0; i < array.Length; i++)
                    {
                        // Check if the any of the objects within the array have a plant component
                        // and set the placement status to Need More Space
                        Plant component = array[i].GetComponent<Plant>();
                        if (component != null)
                        {
                            __instance.m_placementStatus = Player.PlacementStatus.MoreSpace;
                        }
                    }
                }
            }

        }


    }


}
