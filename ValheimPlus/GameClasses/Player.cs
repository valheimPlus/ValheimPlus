using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;
using ValheimPlus.Configurations;

namespace ValheimPlus
{
    /// <summary>
    /// Alter player stamina values
    /// </summary>
    [HarmonyPatch(typeof(Player), "Awake")]
    public static class Player_Awake_Patch
    {
        private static void Postfix(ref Player __instance)
        {
            if (Configuration.Current.Stamina.IsEnabled)
            {
                __instance.m_dodgeStaminaUsage = Helper.applyModifierValue(__instance.m_dodgeStaminaUsage, Configuration.Current.Stamina.dodgeStaminaUsage);
                __instance.m_encumberedStaminaDrain = Helper.applyModifierValue(__instance.m_encumberedStaminaDrain, Configuration.Current.Stamina.encumberedStaminaDrain);
                __instance.m_sneakStaminaDrain = Helper.applyModifierValue(__instance.m_sneakStaminaDrain, Configuration.Current.Stamina.sneakStaminaDrain);
                __instance.m_runStaminaDrain = Helper.applyModifierValue(__instance.m_runStaminaDrain, Configuration.Current.Stamina.runStaminaDrain);
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
    /// Hooks for ABM and AEM
    /// </summary>
    [HarmonyPatch(typeof(Player), "Update")]
    public static class Player_Update_Patch
    {
        private static void Postfix(ref Player __instance, ref Vector3 ___m_moveDir, ref Vector3 ___m_lookDir, ref GameObject ___m_placementGhost, Transform ___m_eye)
        {
            if (!__instance.m_nview.IsValid() || !__instance.m_nview.IsOwner()) return;

            if (Configuration.Current.AdvancedEditingMode.IsEnabled)
            {
                AEM.PlayerInstance = __instance;
                AEM.run();
            }

            if (Configuration.Current.AdvancedBuildingMode.IsEnabled)
            {
                ABM.Run(ref __instance);
            }

            if (Configuration.Current.Hotkeys.IsEnabled)
            {
                ApplyDodgeHotkeys(ref __instance, ref ___m_moveDir, ref ___m_lookDir);
            }
        }

        private static void ApplyDodgeHotkeys(ref Player __instance, ref Vector3 ___m_moveDir, ref Vector3 ___m_lookDir)
        {
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

                Player_Dodge_ReversePatch.call_Dodge(__instance, dodgeDir);
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

                Player_Dodge_ReversePatch.call_Dodge(__instance, dodgeDir);
            }
        }
    }

    /// <summary>
    /// Hook base Dodge method
    /// </summary>
    [HarmonyPatch(typeof(Player))]
    public class Player_Dodge_ReversePatch
    {
        [HarmonyReversePatch]
        [HarmonyPatch(typeof(Player), "Dodge", new Type[] { typeof(Vector3) })]
        public static void call_Dodge(object instance, Vector3 dodgeDir) => throw new NotImplementedException();
    }

    /// <summary>
    /// Update maximum carry weight based on baseMaximumWeight and baseMegingjordBuff configurations.
    /// </summary>
    [HarmonyPatch(typeof(Player), "GetMaxCarryWeight")]
    public static class Player_GetMaxCarryWeight_Patch
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
    public static class Player_OnSpawned_Patch
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

    // ToDo have item tooltips be affected.
    [HarmonyPatch(typeof(Player), nameof(Player.UpdateFood))]
    public static class Player_UpdateFood_Transpiler
    {
        private static FieldInfo field_Player_m_foodUpdateTimer = AccessTools.Field(typeof(Player), nameof(Player.m_foodUpdateTimer));
        private static MethodInfo method_ComputeModifiedDt = AccessTools.Method(typeof(Player_UpdateFood_Transpiler), nameof(Player_UpdateFood_Transpiler.ComputeModifiedDT));

        /// <summary>
        /// Replaces the first load of dt inside Player::UpdateFood with a modified dt that is scaled
        /// by the food duration scaling multiplier. This ensures the food lasts longer while maintaining
        /// the same rate of regeneration.
        /// </summary>
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            if (!Configuration.Current.Food.IsEnabled) return instructions;

            List<CodeInstruction> il = instructions.ToList();

            for (int i = 0; i < il.Count - 2; ++i)
            {
                if (il[i].LoadsField(field_Player_m_foodUpdateTimer) &&
                    il[i + 1].opcode == OpCodes.Ldarg_1 /* dt */ &&
                    il[i + 2].opcode == OpCodes.Add)
                {
                    // We insert after Ldarg_1 (push dt) a call to our function, which computes the modified DT and returns it.
                    il.Insert(i + 2, new CodeInstruction(OpCodes.Call, method_ComputeModifiedDt));
                }
            }

            return il.AsEnumerable();
        }

        private static float ComputeModifiedDT(float dt)
        {
            return dt / Helper.applyModifierValue(1.0f, Configuration.Current.Food.foodDurationMultiplier);
        }
    }

    [HarmonyPatch(typeof(Player), nameof(Player.GetTotalFoodValue))]
    public static class Player_GetTotalFoodValue_Transpiler
    {
        private static FieldInfo field_Food_m_health = AccessTools.Field(typeof(Player.Food), nameof(Player.Food.m_health));
        private static FieldInfo field_Food_m_stamina = AccessTools.Field(typeof(Player.Food), nameof(Player.Food.m_stamina));
        private static FieldInfo field_Food_m_item = AccessTools.Field(typeof(Player.Food), nameof(Player.Food.m_item));
        private static FieldInfo field_ItemData_m_shared = AccessTools.Field(typeof(ItemDrop.ItemData), nameof(ItemDrop.ItemData.m_shared));
        private static FieldInfo field_SharedData_m_food = AccessTools.Field(typeof(ItemDrop.ItemData.SharedData), nameof(ItemDrop.ItemData.SharedData.m_food));
        private static FieldInfo field_SharedData_m_foodStamina = AccessTools.Field(typeof(ItemDrop.ItemData.SharedData), nameof(ItemDrop.ItemData.SharedData.m_foodStamina));

        /// <summary>
        /// Replaces loads to the current health/stamina for food with loads to the original health/stamina for food
        /// inside Player::GetTotalFoodValue. This disables food degradation.
        /// </summary>
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            if (!Configuration.Current.Food.IsEnabled) return instructions;

            List<CodeInstruction> il = instructions.ToList();

            if (Configuration.Current.Food.disableFoodDegradation)
            {
                for (int i = 0; i < il.Count; ++i)
                {
                    bool loads_health = il[i].LoadsField(field_Food_m_health);
                    bool loads_stamina = il[i].LoadsField(field_Food_m_stamina);

                    if (loads_health || loads_stamina)
                    {
                        il[i].operand = field_Food_m_item;
                        il.Insert(++i, new CodeInstruction(OpCodes.Ldfld, field_ItemData_m_shared));
                        il.Insert(++i, new CodeInstruction(OpCodes.Ldfld, loads_health ? field_SharedData_m_food : field_SharedData_m_foodStamina));
                    }
                }
            }

            return il.AsEnumerable();
        }
    }

    /// <summary>
    /// Alters stamina of tools, bows and blocking
    /// </summary>
    [HarmonyPatch(typeof(Player), "UseStamina")]
    public static class Player_UseStamina_Patch
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
    public static class Player_UpdatePlacementGhost_Patch
    {
        private static bool Prefix(ref Player __instance, bool flashGuardStone)
        {
            if (Configuration.Current.AdvancedBuildingMode.IsEnabled)
            {
                ABM.Run(ref __instance);
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
                catch
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
                catch
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
