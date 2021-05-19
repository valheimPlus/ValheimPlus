using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;
using UnityEngine.UI;
using ValheimPlus.Configurations;
using ValheimPlus.RPC;

namespace ValheimPlus.GameClasses
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
                __instance.m_swimStaminaDrainMaxSkill = Helper.applyModifierValue(__instance.m_swimStaminaDrainMaxSkill, Configuration.Current.Stamina.swimStaminaDrain);
                __instance.m_jumpStaminaUsage = Helper.applyModifierValue(__instance.m_jumpStaminaUsage, Configuration.Current.Stamina.jumpStaminaDrain);
            }
            if (Configuration.Current.Player.IsEnabled)
            {
                __instance.m_autoPickupRange = Configuration.Current.Player.baseAutoPickUpRange;
                __instance.m_baseCameraShake = Configuration.Current.Player.disableCameraShake ? 0f : 4f;
                __instance.m_maxCarryWeight = Configuration.Current.Player.baseMaximumWeight;
            
            }
            if (Configuration.Current.Building.IsEnabled)
            {
                __instance.m_maxPlaceDistance = Configuration.Current.Building.maximumPlacementDistance;
            }
        }
    }

    /// <summary>
    /// Hooks for ABM and AEM, Dodge Hotkeys and Display of the game clock
    /// </summary>
    /// 
    [HarmonyPatch(typeof(Player), "Update")]
    public static class Player_Update_Patch
    {

        private static GameObject timeObj = null;
        private static double savedEnvSeconds = -1;
        private static void Postfix(ref Player __instance, ref Vector3 ___m_moveDir, ref Vector3 ___m_lookDir, ref GameObject ___m_placementGhost, Transform ___m_eye)
        {
            if ((Configuration.Current.Player.IsEnabled && Configuration.Current.Player.queueWeaponChanges) && (ZInput.GetButtonDown("Hide") || ZInput.GetButtonDown("JoyHide")))
            {
                if (__instance.InAttack() && (__instance.GetRightItem() != null || __instance.GetLeftItem() != null))
                {
                    // The game ignores this keypress, so queue it and take care of it when able (Player_FixedUpdate_Patch).
                    EquipPatchState.shouldHideItemsAfterAttack = true;
                }
            }

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


            if (Configuration.Current.GameClock.IsEnabled)
            {
                String hours_str = "";
                String minutes_str = "";
                String amPM_str = "";

                Hud hud = Hud.instance;

                Text timeText;
                if (timeObj == null)
                {
                    MessageHud msgHud = MessageHud.instance;

                    timeObj = new GameObject();
                    timeObj.transform.SetParent(hud.m_statusEffectListRoot.transform.parent);
                    timeObj.AddComponent<RectTransform>();

                    timeText = timeObj.AddComponent<Text>();

                    float rRatio = Mathf.Clamp01((float)Configuration.Current.GameClock.textRedChannel / 255f);
                    float gRatio = Mathf.Clamp01((float)Configuration.Current.GameClock.textGreenChannel / 255f);
                    float bRatio = Mathf.Clamp01((float)Configuration.Current.GameClock.textBlueChannel / 255f);
                    float aRatio = Mathf.Clamp01((float)Configuration.Current.GameClock.textTransparencyChannel / 255f);

                    timeText.color = new Color(rRatio, gRatio, bRatio, aRatio);
                    timeText.font = msgHud.m_messageCenterText.font;
                    timeText.fontSize = Configuration.Current.GameClock.textFontSize;
                    timeText.enabled = true;
                    timeText.alignment = TextAnchor.MiddleCenter;
                    timeText.horizontalOverflow = HorizontalWrapMode.Overflow;
                }
                else timeText = timeObj.GetComponent<Text>();

                EnvMan env = EnvMan.instance;
                if (savedEnvSeconds != env.m_totalSeconds)
                {
                    float minuteFrac = Mathf.Lerp(0, 24, env.GetDayFraction());
                    float hr24 = Mathf.Floor(minuteFrac);
                    minuteFrac = minuteFrac - hr24;
                    float minutes = Mathf.Lerp(0, 60, minuteFrac);

                    int hours_int = Mathf.FloorToInt(hr24);
                    int minutes_int = Mathf.FloorToInt(minutes);

                    if (Configuration.Current.GameClock.useAMPM)
                    {
                        amPM_str = (hours_int < 12) ? " AM" : " PM";
                        if (hours_int > 12) hours_int -= 12;
                    }

                    if (hours_int < 10) hours_str = "0" + hours_int;
                    if (minutes_int < 10) minutes_str = "0" + minutes_int;
                    if (hours_int >= 10) hours_str = hours_int.ToString();
                    if (minutes_int >= 10) minutes_str = minutes_int.ToString();

                    timeText.text = hours_str + ":" + minutes_str + amPM_str;
                    var staminaBarRec = hud.m_staminaBar2Root.transform as RectTransform;
                    var statusEffictBarRec = hud.m_statusEffectListRoot.transform as RectTransform;
                    timeObj.GetComponent<RectTransform>().position = new Vector2(staminaBarRec.position.x, statusEffictBarRec.position.y);
                    timeObj.SetActive(true);
                    savedEnvSeconds = env.m_totalSeconds;
                }
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
    
    /*
    * Moved to Player Awake so that it is only called once
    *
    /// <summary>
    /// Update maximum carry weight based on baseMaximumWeight configurations.
    /// </summary>
    [HarmonyPatch(typeof(Player), "GetMaxCarryWeight")]
    public static class Player_GetMaxCarryWeight_Patch
    {
        private static void Prefix(ref Player __instance)
        {
            if (Configuration.Current.Player.IsEnabled)
            {
                __instance.m_maxCarryWeight = Configuration.Current.Player.baseMaximumWeight;
            }
        }
    }
    */
    
    /// <summary>
    /// Update maximum carry weight based on baseMegingjordBuff configurations.
    /// </summary>
    [HarmonyPatch(typeof(SE_Stats), nameof(SE_Stats.Setup))]
    public static class SE_Stats_Setup_Patch
    {
        private static void Postfix(ref SE_Stats __instance)
        {
            if (Configuration.Current.Player.IsEnabled) 
                if (__instance.m_addMaxCarryWeight != null && __instance.m_addMaxCarryWeight > 0)
                    __instance.m_addMaxCarryWeight = (__instance.m_addMaxCarryWeight - 150) + Configuration.Current.Player.baseMegingjordBuff;
        }
    }


    /// <summary>
    /// Add ValheimPlus intro to compendium.
    /// </summary>
    [HarmonyPatch(typeof(Player), "OnSpawned")]
    public static class Player_OnSpawned_Patch
    {
        private static void Prefix(ref Player __instance)
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

            //Only sync on first spawn
            if (VPlusMapSync.ShouldSyncOnSpawn && Configuration.Current.Map.IsEnabled && Configuration.Current.Map.shareMapProgression)
            {
                //Send map data to the server
                VPlusMapSync.SendMapToServer();
                VPlusMapSync.ShouldSyncOnSpawn = false;
            }

            if(Configuration.Current.Player.IsEnabled && Configuration.Current.Player.skipIntro)
                __instance.m_firstSpawn = false;

        }
    }


    [HarmonyPatch(typeof(Player), nameof(Player.RemovePiece))]
    public static class Player_RemovePiece_Transpiler
    {
        private static MethodInfo modifyIsInsideMythicalZone = AccessTools.Method(typeof(Player_RemovePiece_Transpiler), nameof(Player_RemovePiece_Transpiler.IsInsideNoBuildLocation));

        /// <summary>
        //  Replaces the RemovePiece().Location.IsInsideNoBuildLocation with a stub function
        /// </summary>
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            if (!Configuration.Current.Building.IsEnabled || !Configuration.Current.Building.noMysticalForcesPreventPlacementRestriction)
                return instructions;

            List<CodeInstruction> il = instructions.ToList();
            for (int i = 0; i < il.Count; ++i)
            {
                if (il[i].operand != null)
                    // search for every call to the function
                    if (il[i].operand.ToString().Contains(nameof(Location.IsInsideNoBuildLocation)))
                    {
                        il[i] = new CodeInstruction(OpCodes.Call, modifyIsInsideMythicalZone);
                        // replace every call to the function with the stub
                    }
            }
            return il.AsEnumerable();
        }

        private static bool IsInsideNoBuildLocation(Vector3 point)
        {
            return false;
        }
    }



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
                if (methodName.Contains(nameof(Player.UpdatePlacement)) || methodName.Contains(nameof(Player.Repair)) || methodName.Contains(nameof(Player.RemovePiece)))
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

            if (Configuration.Current.GridAlignment.IsEnabled)
            {
                if (GridAlignment.AlignPressed ^ GridAlignment.AlignToggled)
                    GridAlignment.UpdatePlacementGhost(__instance);
            }

            if (Configuration.Current.Building.IsEnabled && Configuration.Current.Building.noInvalidPlacementRestriction)
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

            if (Configuration.Current.Building.IsEnabled && Configuration.Current.Building.noMysticalForcesPreventPlacementRestriction)
            {
                try
                {
                    if (__instance.m_placementStatus == Player.PlacementStatus.NoBuildZone)
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

    [HarmonyPatch]
    public static class AreaRepair
    {
        private static int m_repair_count;

        [HarmonyPatch(typeof(Player), nameof(Player.UpdatePlacement))]
        public static class Player_UpdatePlacement_Transpiler
        {
            private static MethodInfo method_Player_Repair = AccessTools.Method(typeof(Player), nameof(Player.Repair));
            private static AccessTools.FieldRef<Player, Piece> field_Player_m_hoveringPiece = AccessTools.FieldRefAccess<Player, Piece>(nameof(Player.m_hoveringPiece));
            private static MethodInfo method_RepairNearby = AccessTools.Method(typeof(Player_UpdatePlacement_Transpiler), nameof(Player_UpdatePlacement_Transpiler.RepairNearby));

            /// <summary>
            /// Patches the call to Repair from Player::UpdatePlacement with our own stub which handles repairing multiple pieces rather than just one.
            /// </summary>
            [HarmonyTranspiler]
            public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                if (!Configuration.Current.Building.IsEnabled) return instructions;

                List<CodeInstruction> il = instructions.ToList();

                if (Configuration.Current.Building.enableAreaRepair)
                {
                    // Replace call to Player::Repair with our own stub.
                    // Our stub calls the original repair multiple times, one for each nearby piece.
                    for (int i = 0; i < il.Count; ++i)
                    {
                        if (il[i].Calls(method_Player_Repair))
                        {
                            il[i].operand = method_RepairNearby;
                        }
                    }
                }

                return il.AsEnumerable();
            }

            public static void RepairNearby(Player instance, ItemDrop.ItemData toolItem, Piece _1)
            {
                Piece selected_piece = instance.GetHoveringPiece();
                Vector3 position = selected_piece != null ? selected_piece.transform.position : instance.transform.position;

                List<Piece> pieces = new List<Piece>();
                Piece.GetAllPiecesInRadius(position, Configuration.Current.Building.areaRepairRadius, pieces);

                m_repair_count = 0;

                Piece original_piece = instance.m_hoveringPiece;

                foreach (Piece piece in pieces)
                {
                    bool has_stamina = instance.HaveStamina(toolItem.m_shared.m_attack.m_attackStamina);
                    bool uses_durability = toolItem.m_shared.m_useDurability;
                    bool has_durability = toolItem.m_durability > 0.0f;

                    if (!has_stamina || (uses_durability && !has_durability)) break;

                    // The repair function takes a piece to repair but then completely ignores it and repairs the hovering piece instead...
                    // I really don't like this, but Valheim's spaghetti code makes it required.
                    instance.m_hoveringPiece = piece;
                    instance.Repair(toolItem, _1);
                    instance.m_hoveringPiece = original_piece;
                }

                instance.Message(MessageHud.MessageType.TopLeft, string.Format("{0} pieces repaired", m_repair_count));
            }
        }

        [HarmonyPatch(typeof(Player), nameof(Player.Repair))]
        public static class Player_Repair_Transpiler
        {
            private static MethodInfo method_Character_Message = AccessTools.Method(typeof(Character), nameof(Character.Message));
            private static MethodInfo method_MessageNoop = AccessTools.Method(typeof(Player_Repair_Transpiler), nameof(Player_Repair_Transpiler.MessageNoop));

            /// <summary>
            /// Noops the original message notification when one piece is repaired, and counts them instead - the other transpiler
            /// will dispatch one notification for a batch of repairs using this count.
            /// </summary>
            [HarmonyTranspiler]
            public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                if (!Configuration.Current.Building.IsEnabled) return instructions;

                List<CodeInstruction> il = instructions.ToList();

                if (Configuration.Current.Building.enableAreaRepair)
                {
                    // Replace calls to Character::Message with our own noop stub
                    // We don't want to spam messages for each piece so we patch the messages out here and dispatch our own messages in the other transpiler.
                    // First call pushes 1, then subsequent calls 0 - the first call is the branch where the repair succeeded.
                    int count = 0;
                    for (int i = 0; i < il.Count; ++i)
                    {
                        if (il[i].Calls(method_Character_Message))
                        {
                            il[i].operand = method_MessageNoop;
                            il.Insert(i++, new CodeInstruction(count++ == 0 ? OpCodes.Ldc_I4_1 : OpCodes.Ldc_I4_0, null));
                        }
                    }
                }

                return il.AsEnumerable();
            }

            public static void MessageNoop(Character _0, MessageHud.MessageType _1, string _2, int _3, Sprite _4, int repaired)
            {
                m_repair_count += repaired;
            }
        }
    }

    [HarmonyPatch(typeof(Player), "Update")]
    public static class GridAlignment
    {
        public static int DefaultAlignment = 100;
        public static bool AlignPressed = false;
        public static bool AlignToggled = false;

        private static void Postfix(ref Player __instance)
        {
            if (!Configuration.Current.GridAlignment.IsEnabled)
                return;

            if (Input.GetKeyDown(Configuration.Current.GridAlignment.align))
                AlignPressed = true;
            if (Input.GetKeyUp(Configuration.Current.GridAlignment.align))
                AlignPressed = false;

            if (Input.GetKeyDown(Configuration.Current.GridAlignment.changeDefaultAlignment))
            {
                if (DefaultAlignment == 50)
                    DefaultAlignment = 100;
                else if (DefaultAlignment == 100)
                    DefaultAlignment = 200;
                else if (DefaultAlignment == 200)
                    DefaultAlignment = 400;
                else
                    DefaultAlignment = 50;
                MessageHud.instance.ShowMessage(MessageHud.MessageType.TopLeft, "Default grid alignment set to " + (DefaultAlignment / 100f));
            }

            if (Input.GetKeyDown(Configuration.Current.GridAlignment.alignToggle))
            {
                AlignToggled ^= true;
                MessageHud.instance.ShowMessage(MessageHud.MessageType.TopLeft, "Grid alignment by default " + (AlignToggled ? "enabled" : "disabled"));
            }
        }

        static float FixAlignment(float f)
        {
            int i = (int)Mathf.Round(f * 100f);
            if (i <= 0)
                return DefaultAlignment / 100f;
            if (i <= 50)
                return 0.5f;
            if (i <= 100)
                return 1f;
            if (i <= 200)
                return 2f;
            return 4f;
        }

        public static void GetAlignment(Piece piece, out Vector3 alignment, out Vector3 offset)
        {
            var points = new System.Collections.Generic.List<Transform>();
            piece.GetSnapPoints(points);
            if (points.Count != 0)
            {
                Vector3 min = Vector3.positiveInfinity;
                Vector3 max = Vector3.negativeInfinity;
                foreach (var point in points)
                {
                    var pos = point.localPosition;
                    min = Vector3.Min(min, pos);
                    max = Vector3.Max(max, pos);
                }
                alignment = max - min;
                alignment.x = FixAlignment(alignment.x);
                alignment.y = FixAlignment(alignment.y);
                alignment.z = FixAlignment(alignment.z);
                // Align at top
                offset = max;
                if (piece.name == "iron_grate" || piece.name == "wood_gate")
                {
                    // Align at bottom, not top
                    offset.y = min.y;
                }
                if (piece.name == "wood_gate")
                {
                    alignment.x = 4;
                }
            }
            else
            {
                if (piece.m_notOnFloor || piece.name == "sign" || piece.name == "itemstand")
                {
                    alignment = new Vector3(0.5f, 0.5f, 0);
                    offset = new Vector3(0, 0, 0);
                    if (piece.name == "sign")
                        alignment.y = 0.25f;
                }
                else if (piece.name == "piece_walltorch")
                {
                    alignment = new Vector3(0, 0.5f, 0.5f);
                    offset = new Vector3(0, 0, 0);
                }
                else
                {
                    alignment = new Vector3(0.5f, 0, 0.5f);
                    offset = new Vector3(0, 0, 0);
                }
            }
        }

        public static float Align(float value, out float alpha)
        {
            float result = Mathf.Round(value);
            alpha = value - result;
            return result;
        }


        public static void UpdatePlacementGhost(Player player)
        {
            if (player.m_placementGhost == null)
                return;

            if (ABM.isActive)
                return;

            bool altMode = ZInput.GetButton("AltPlace") || ZInput.GetButton("JoyAltPlace");

            var piece = player.m_placementGhost.GetComponent<Piece>();

            var newVal = piece.transform.position;
            newVal = Quaternion.Inverse(piece.transform.rotation) * newVal;

            Vector3 alignment;
            Vector3 offset;
            GetAlignment(piece, out alignment, out offset);
            newVal += offset;
            var copy = newVal;
            newVal = new Vector3(newVal.x / alignment.x, newVal.y / alignment.y, newVal.z / alignment.z);
            float alphaX, alphaY, alphaZ;
            newVal = new UnityEngine.Vector3(Align(newVal.x, out alphaX), Align(newVal.y, out alphaY), Align(newVal.z, out alphaZ));
            if (altMode)
            {
                float alphaMin = 0.2f;
                if (Mathf.Abs(alphaX) >= alphaMin && Mathf.Abs(alphaX) >= Mathf.Abs(alphaY) && Mathf.Abs(alphaX) >= Mathf.Abs(alphaZ))
                    newVal.x += Mathf.Sign(alphaX);
                else if (Mathf.Abs(alphaY) >= alphaMin && Mathf.Abs(alphaY) >= Mathf.Abs(alphaZ))
                    newVal.y += Mathf.Sign(alphaY);
                else if (Mathf.Abs(alphaZ) >= alphaMin)
                    newVal.z += Mathf.Sign(alphaZ);
            }
            newVal = new Vector3(newVal.x * alignment.x, newVal.y * alignment.y, newVal.z * alignment.z);
            if (alignment.x <= 0)
                newVal.x = copy.x;
            if (alignment.y <= 0)
                newVal.y = copy.y;
            if (alignment.z <= 0)
                newVal.z = copy.z;
            newVal -= offset;

            newVal = piece.transform.rotation * newVal;
            piece.transform.position = newVal;
        }
    }

    /// <summary>
    /// Configures guardian buff duration and cooldown
    /// </summary>
    [HarmonyPatch(typeof(Player), "SetGuardianPower")]
    public static class Player_SetGuardianPower_Patch
    {
        private static void Postfix(ref Player __instance)
        {
            if (Configuration.Current.Player.IsEnabled)
            {
                if (__instance.m_guardianSE)
                {
                    __instance.m_guardianSE.m_ttl = Configuration.Current.Player.guardianBuffDuration;
                    __instance.m_guardianSE.m_cooldown = Configuration.Current.Player.guardianBuffCooldown;
                }
            }
        }
    }

    /// <summary>
    /// Skips the guardian power activation animation
    /// </summary>
    [HarmonyPatch(typeof(Player), "StartGuardianPower")]
    public static class Player_StartGuardianPower_Patch
    {
        private static bool Prefix(ref Player __instance, ref bool __result)
        {
            if (!Configuration.Current.Player.disableGuardianBuffAnimation || !Configuration.Current.Player.IsEnabled)
                return true;

            if (__instance.m_guardianSE == null)
            {
                __result = false;
                return false;
            }
            if (__instance.m_guardianPowerCooldown > 0f)
            {
                __instance.Message(MessageHud.MessageType.Center, "$hud_powernotready", 0, null);
                __result = false;
                return false;
            }
            __instance.ActivateGuardianPower();
            __result = true;
            return false;
        }
    }

    [HarmonyPatch(typeof(Player), nameof(Player.HaveRequirements), new System.Type[] { typeof(Piece.Requirement[]), typeof(bool), typeof(int) })]
    public static class Player_HaveRequirements_Transpiler
    {
        private static MethodInfo method_Inventory_CountItems = AccessTools.Method(typeof(Inventory), nameof(Inventory.CountItems));
        private static MethodInfo method_ComputeItemQuantity = AccessTools.Method(typeof(Player_HaveRequirements_Transpiler), nameof(Player_HaveRequirements_Transpiler.ComputeItemQuantity));

        /// <summary>
        /// Patches out the code that checks if there is enough material to craft a specific object.
        /// The return value of this function is used to set the item as "Craftable" or not in the crafts list.
        /// </summary>
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            if (!Configuration.Current.CraftFromChest.IsEnabled) return instructions;

            List<CodeInstruction> il = instructions.ToList();

            for (int i = 0; i < il.Count; ++i)
            {
                if (il[i].Calls(method_Inventory_CountItems))
                {
                    il.Insert(++i, new CodeInstruction(OpCodes.Ldloc_2));
                    il.Insert(++i, new CodeInstruction(OpCodes.Ldarg_0));
                    il.Insert(++i, new CodeInstruction(OpCodes.Call, method_ComputeItemQuantity));
                }
            }

            return il.AsEnumerable();
        }

        private static int ComputeItemQuantity(int fromInventory, Piece.Requirement item, Player player)
        {
            Stopwatch delta;

            GameObject pos = player.GetCurrentCraftingStation()?.gameObject;
            if (!pos || !Configuration.Current.CraftFromChest.checkFromWorkbench)
            {
                pos = player.gameObject;
                delta = Inventory_NearbyChests_Cache.delta;
            }
            else
                delta = GameObjectAssistant.GetStopwatch(pos);

            int lookupInterval = Helper.Clamp(Configuration.Current.CraftFromChest.lookupInterval, 1, 10) * 1000;
            if (!delta.IsRunning || delta.ElapsedMilliseconds > lookupInterval)
            {
                Inventory_NearbyChests_Cache.chests = InventoryAssistant.GetNearbyChests(pos, Helper.Clamp(Configuration.Current.CraftFromChest.range, 1, 50), !Configuration.Current.CraftFromChest.ignorePrivateAreaCheck);
                delta.Restart();
            }
            return fromInventory + InventoryAssistant.GetItemAmountInItemList(InventoryAssistant.GetNearbyChestItemsByContainerList(Inventory_NearbyChests_Cache.chests), item.m_resItem.m_itemData);
        }
    }

    [HarmonyPatch(typeof(Player), nameof(Player.HaveRequirements), new System.Type[] { typeof(Piece), typeof(Player.RequirementMode) })]
    public static class Player_HaveRequirements_2_Transpiler
    {
        private static MethodInfo method_Inventory_CountItems = AccessTools.Method(typeof(Inventory), nameof(Inventory.CountItems));
        private static MethodInfo method_ComputeItemQuantity = AccessTools.Method(typeof(Player_HaveRequirements_2_Transpiler), nameof(Player_HaveRequirements_2_Transpiler.ComputeItemQuantity));

        /// <summary>
        /// Patches out the code that checks if there is enough material to craft a specific object.
        /// The return value of this function determines if the item should be crafted or not.
        /// </summary>
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            if (!Configuration.Current.CraftFromChest.IsEnabled) return instructions;

            List<CodeInstruction> il = instructions.ToList();

            for (int i = 0; i < il.Count; ++i)
            {
                if (il[i].Calls(method_Inventory_CountItems))
                {
                    il.Insert(++i, new CodeInstruction(OpCodes.Ldloc_2));
                    il.Insert(++i, new CodeInstruction(OpCodes.Ldarg_0));
                    il.Insert(++i, new CodeInstruction(OpCodes.Call, method_ComputeItemQuantity));
                }
            }

            return il.AsEnumerable();
        }

        private static int ComputeItemQuantity(int fromInventory, Piece.Requirement item, Player player)
        {
            Stopwatch delta;

            GameObject pos = player.GetCurrentCraftingStation()?.gameObject;
            if (!pos || !Configuration.Current.CraftFromChest.checkFromWorkbench)
            {
                pos = player.gameObject;
                delta = Inventory_NearbyChests_Cache.delta;
            }
            else
                delta = GameObjectAssistant.GetStopwatch(pos);

            int lookupInterval = Helper.Clamp(Configuration.Current.CraftFromChest.lookupInterval, 1, 10) * 1000;
            if (!delta.IsRunning || delta.ElapsedMilliseconds > lookupInterval)
            {
                Inventory_NearbyChests_Cache.chests = InventoryAssistant.GetNearbyChests(pos, Helper.Clamp(Configuration.Current.CraftFromChest.range, 1, 50), !Configuration.Current.CraftFromChest.ignorePrivateAreaCheck);
                delta.Restart();
            }

            return fromInventory + InventoryAssistant.GetItemAmountInItemList(InventoryAssistant.GetNearbyChestItemsByContainerList(Inventory_NearbyChests_Cache.chests), item.m_resItem.m_itemData);
        }
    }

    [HarmonyPatch(typeof(Player), nameof(Player.ConsumeResources))]
    public static class Player_ConsumeResources_Transpiler
    {
        private static MethodInfo method_Inventory_RemoveItem = AccessTools.Method(typeof(Inventory), nameof(Inventory.RemoveItem), new System.Type[] { typeof(string), typeof(int) });
        private static MethodInfo method_RemoveItemsFromInventoryAndNearbyChests = AccessTools.Method(typeof(Player_ConsumeResources_Transpiler), nameof(Player_ConsumeResources_Transpiler.RemoveItemsFromInventoryAndNearbyChests));

        /// <summary>
        /// Patches out the code that consumes the material required to craft something.
        /// We first remove the amount we can from the player inventory before moving on to the nearby chests.
        /// </summary>
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            if (!Configuration.Current.CraftFromChest.IsEnabled) return instructions;

            List<CodeInstruction> il = instructions.ToList();

            int thisIdx = -1;
            int callIdx = -1;

            for (int i = 0; i < il.Count; ++i)
            {
                if (il[i].opcode == OpCodes.Ldarg_0)
                {
                    thisIdx = i;
                }
                else if (il[i].Calls(method_Inventory_RemoveItem))
                {
                    callIdx = i;
                    break;
                }
            }

            if (thisIdx == -1 || callIdx == -1)
            {
                ZLog.LogError("Failed to apply Player_ConsumeResources_Transpiler");
                return instructions;
            }
            il.RemoveRange(thisIdx + 1, callIdx - thisIdx);

            il.Insert(++thisIdx, new CodeInstruction(OpCodes.Ldloc_2));
            il.Insert(++thisIdx, new CodeInstruction(OpCodes.Ldloc_3));
            il.Insert(++thisIdx, new CodeInstruction(OpCodes.Call, method_RemoveItemsFromInventoryAndNearbyChests));

            return il.AsEnumerable();
        }

        private static void RemoveItemsFromInventoryAndNearbyChests(Player player, Piece.Requirement item, int amount)
        {
            GameObject pos = player.GetCurrentCraftingStation()?.gameObject;
            if (!pos || !Configuration.Current.CraftFromChest.checkFromWorkbench) pos = player.gameObject;

            int inventoryAmount = player.m_inventory.CountItems(item.m_resItem.m_itemData.m_shared.m_name);
            player.m_inventory.RemoveItem(item.m_resItem.m_itemData.m_shared.m_name, amount);
            amount -= inventoryAmount;
            if (amount <= 0) return;

            InventoryAssistant.RemoveItemInAmountFromAllNearbyChests(pos, Helper.Clamp(Configuration.Current.CraftFromChest.range, 1, 50), item.m_resItem.m_itemData, amount, !Configuration.Current.CraftFromChest.ignorePrivateAreaCheck);
        }
    }

    public static class EquipPatchState
    {
        public static bool shouldEquipItemsAfterAttack = false;
        public static bool shouldHideItemsAfterAttack = false;
        public static List<ItemDrop.ItemData> items = null;
    }

    /// <summary>
    /// Queue weapon/item changes until attack is finished, instead of simply ignoring the change entirely
    /// </summary>
    [HarmonyPatch(typeof(Player), "ToggleEquiped")]
    public static class Player_ToggleEquiped_Patch
    {
        private static void Postfix(Player __instance, bool __result, ItemDrop.ItemData item)
        {
            if (!Configuration.Current.Player.IsEnabled || !Configuration.Current.Player.queueWeaponChanges)
                return;

            if (!__result || !item.IsEquipable())
            {
                // Item is not equipable (second case should never happen as the original always returns false if not equipable)
                return;
            }
            if (__instance.InAttack())
            {
                // Store the item(s) to equip when the attack is finished
                if (EquipPatchState.items == null)
                    EquipPatchState.items = new List<ItemDrop.ItemData>();

                if (!EquipPatchState.items.Contains(item))
                    EquipPatchState.items.Add(item);

                EquipPatchState.shouldEquipItemsAfterAttack = true;
            }
        }
    }

    /// <summary>
    /// Queue weapon/item changes until attack is finished, instead of simply ignoring the change entirely
    /// </summary>
    [HarmonyPatch(typeof(Player), "FixedUpdate")]
    public static class Player_FixedUpdate_Patch
    {
        private static void Postfix(Player __instance)
        {
            if (!Configuration.Current.Player.IsEnabled || !Configuration.Current.Player.queueWeaponChanges)
                return;

            // I'm not sure if the m_nview checks are necessary, but the original code performs them.
            // Note that we check !InAttack() to ensure we've waited until the attack has
            if (EquipPatchState.shouldEquipItemsAfterAttack && !__instance.InAttack() &&
                (__instance.m_nview == null || (__instance.m_nview != null && __instance.m_nview.IsOwner())))
            {
                foreach (ItemDrop.ItemData item in EquipPatchState.items)
                {
                    float oldDuration = item.m_shared.m_equipDuration;
                    item.m_shared.m_equipDuration = 0f;
                    __instance.ToggleEquiped(item);
                    item.m_shared.m_equipDuration = oldDuration;
                }

                EquipPatchState.shouldEquipItemsAfterAttack = false;
                EquipPatchState.items.Clear();
            }

            if (EquipPatchState.shouldHideItemsAfterAttack && !__instance.InAttack())
            {
                __instance.HideHandItems();
                EquipPatchState.shouldHideItemsAfterAttack = false;
            }
        }
    }

    /// <summary>
    /// skip all tutorials from now on
    /// </summary>
    [HarmonyPatch(typeof(Player), "HaveSeenTutorial")]
    public class Player_HaveSeenTutorial_Patch
    {
        [HarmonyPrefix]
        private static void Prefix(Player __instance, ref string name)
        {
            if (Configuration.Current.Player.IsEnabled && Configuration.Current.Player.skipTutorials)
            {
                if (!__instance.m_shownTutorials.Contains(name))
                {
                    __instance.m_shownTutorials.Add(name);
                }
            }
        }
    }

    [HarmonyPatch(typeof(Player), nameof(Player.IsEncumbered))]
    public static class Player_DisableEncumbered_Patch
    {
        private static bool Prefix(ref bool __result)
        {
            if (Configuration.Current.Player.IsEnabled && Configuration.Current.Player.disableEncumbered)
            {
                __result = false;
                return false;
            }

            return true;
        }
    }
}
