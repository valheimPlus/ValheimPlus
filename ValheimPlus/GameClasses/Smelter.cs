using HarmonyLib;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;
using ValheimPlus.Configurations;

namespace ValheimPlus.GameClasses
{
    /// <summary>
    /// Alters smelter input, output, and production speed configurations
    /// </summary>
    [HarmonyPatch(typeof(Smelter), nameof(Smelter.Awake))]
    public static class Smelter_Awake_Patch
    {
        private static void Prefix(ref Smelter __instance)
        {
            if (__instance.m_name.Equals(SmelterDefinitions.KilnName) && Configuration.Current.Kiln.IsEnabled)
            {
                __instance.m_maxOre = Configuration.Current.Kiln.maximumWood;
                __instance.m_secPerProduct = Configuration.Current.Kiln.productionSpeed;
            }
            else if (__instance.m_name.Equals(SmelterDefinitions.SmelterName) && Configuration.Current.Smelter.IsEnabled)
            {
                __instance.m_maxOre = Configuration.Current.Smelter.maximumOre;
                __instance.m_maxFuel = Configuration.Current.Smelter.maximumCoal;
                __instance.m_secPerProduct = Configuration.Current.Smelter.productionSpeed;
                __instance.m_fuelPerProduct = Configuration.Current.Smelter.coalUsedPerProduct;
            }
            else if (__instance.m_name.Equals(SmelterDefinitions.FurnaceName) && Configuration.Current.Furnace.IsEnabled)
            {
                __instance.m_maxOre = Configuration.Current.Furnace.maximumOre;
                __instance.m_maxFuel = Configuration.Current.Furnace.maximumCoal;
                __instance.m_secPerProduct = Configuration.Current.Furnace.productionSpeed;
                __instance.m_fuelPerProduct = Configuration.Current.Furnace.coalUsedPerProduct;

                if (Configuration.Current.Furnace.allowAllOres)
                {
                    __instance.m_conversion.AddRange(FurnaceDefinitions.AdditionalConversions);
                }
            }
            else if (__instance.m_name.Equals(SmelterDefinitions.WindmillName) && Configuration.Current.Windmill.IsEnabled)
            {
                __instance.m_maxOre = Configuration.Current.Windmill.maximumBarley;
                __instance.m_secPerProduct = Configuration.Current.Windmill.productionSpeed;
            }
            else if (__instance.m_name.Equals(SmelterDefinitions.SpinningWheelName) && Configuration.Current.SpinningWheel.IsEnabled)
            {
                __instance.m_maxOre = Configuration.Current.SpinningWheel.maximumFlax;
                __instance.m_secPerProduct = Configuration.Current.SpinningWheel.productionSpeed;
            }
            else if (__instance.m_name.Equals(SmelterDefinitions.EitrRefineryName) && Configuration.Current.EitrRefinery.IsEnabled)
            {
                __instance.m_maxOre = Configuration.Current.EitrRefinery.maximumSoftTissue;
                __instance.m_maxFuel = Configuration.Current.EitrRefinery.maximumSap;
                __instance.m_secPerProduct = Configuration.Current.EitrRefinery.productionSpeed;
            }
        }

    }

    [HarmonyPatch(typeof(Smelter), nameof(Smelter.Spawn))]
    public static class Smelter_Spawn_Patch
    {
        private static bool Prefix(string ore, int stack, ref Smelter __instance)
        {
            Smelter smelter = __instance; // allowing access to local function
            if (!smelter.m_nview.IsOwner())
                return true;

            if (__instance.m_name.Equals(SmelterDefinitions.KilnName) && Configuration.Current.Kiln.IsEnabled && Configuration.Current.Kiln.autoDeposit)
            {
                return spawn(Helper.Clamp(Configuration.Current.Kiln.autoRange, 1, 50), Configuration.Current.Kiln.ignorePrivateAreaCheck);
            }
            if (__instance.m_name.Equals(SmelterDefinitions.SmelterName) && Configuration.Current.Smelter.IsEnabled && Configuration.Current.Smelter.autoDeposit)
            {
                return spawn(Helper.Clamp(Configuration.Current.Smelter.autoRange, 1, 50), Configuration.Current.Smelter.ignorePrivateAreaCheck);
            }
            if (__instance.m_name.Equals(SmelterDefinitions.FurnaceName) && Configuration.Current.Furnace.IsEnabled && Configuration.Current.Furnace.autoDeposit)
            {
                return spawn(Helper.Clamp(Configuration.Current.Furnace.autoRange, 1, 50), Configuration.Current.Furnace.ignorePrivateAreaCheck);
            }
            if (__instance.m_name.Equals(SmelterDefinitions.WindmillName) && Configuration.Current.Windmill.IsEnabled && Configuration.Current.Windmill.autoDeposit)
            {
                return spawn(Helper.Clamp(Configuration.Current.Windmill.autoRange, 1, 50), Configuration.Current.Windmill.ignorePrivateAreaCheck);
            }
            if (__instance.m_name.Equals(SmelterDefinitions.SpinningWheelName) && Configuration.Current.SpinningWheel.IsEnabled && Configuration.Current.SpinningWheel.autoDeposit)
            {
                return spawn(Helper.Clamp(Configuration.Current.SpinningWheel.autoRange, 1, 50), Configuration.Current.Windmill.ignorePrivateAreaCheck);
            }
            if (__instance.m_name.Equals(SmelterDefinitions.EitrRefineryName) && Configuration.Current.EitrRefinery.IsEnabled && Configuration.Current.EitrRefinery.autoDeposit)
            {
                return spawn(Helper.Clamp(Configuration.Current.EitrRefinery.autoRange, 1, 50), Configuration.Current.Windmill.ignorePrivateAreaCheck);
            }
            bool spawn(float autoDepositRange, bool ignorePrivateAreaCheck)
            {
                List<Container> nearbyChests = InventoryAssistant.GetNearbyChests(smelter.gameObject, autoDepositRange, !ignorePrivateAreaCheck);
                if (nearbyChests.Count == 0)
                    return true;

                if (autoDepositRange > 50)
                    autoDepositRange = 50;
                else if (autoDepositRange < 1)
                    autoDepositRange = 1;

                // Replicating original code, just "spawning/adding" the item inside the chest makes it "not have a prefab"
                GameObject itemPrefab = ObjectDB.instance.GetItemPrefab(smelter.GetItemConversion(ore).m_to.gameObject.name);

                // Also replication of original code, really have no idead what it is for, didn't bother look
                ZNetView.m_forceDisableInit = true;
                GameObject spawnedOre = Object.Instantiate<GameObject>(itemPrefab);
                ZNetView.m_forceDisableInit = false;

                // assign stack size, nobody wants a 0/20 stack of metals (its not very usefull)
                ItemDrop comp = spawnedOre.GetComponent<ItemDrop>();
                comp.m_itemData.m_stack = stack;

                bool result = spawnNearbyChest(true);
                Object.Destroy(spawnedOre);

                bool spawnNearbyChest(bool mustHaveItem)
                {
                    foreach (Container chest in nearbyChests)
                    {
                        Inventory cInventory = chest.GetInventory();
                        if (mustHaveItem && !cInventory.HaveItem(comp.m_itemData.m_shared.m_name))
                            continue;

                        bool added = cInventory.AddItem(comp.m_itemData);
                        if (!added)
                        {
                            // Chest full, move to the next
                            continue;
                        }

                        smelter.m_produceEffects.Create(smelter.transform.position, smelter.transform.rotation, null, 1f);
                        InventoryAssistant.ConveyContainerToNetwork(chest);
                        return false;
                    }

                    if (mustHaveItem)
                        return spawnNearbyChest(false);

                    return true;
                }
                return result;
            }


            return true;
        }
    }

    [HarmonyPatch(typeof(Smelter), nameof(Smelter.UpdateSmelter))]
    public static class Smelter_UpdateSmelter_Patch
    {
        static void Prefix(Smelter __instance)
        {
            if (__instance == null || !Player.m_localPlayer || __instance.m_nview == null || !__instance.m_nview.IsOwner())
                return;

            Smelter smelter = __instance;

            Stopwatch delta = GameObjectAssistant.GetStopwatch(smelter.gameObject);
            if (delta.IsRunning && delta.ElapsedMilliseconds < 1000) return;
            delta.Restart();

            float autoFuelRange = 0f;
            bool ignorePrivateAreaCheck = false;
            bool isKiln = false;
            if (smelter.m_name.Equals(SmelterDefinitions.KilnName))
            {
                if (!Configuration.Current.Kiln.IsEnabled || !Configuration.Current.Kiln.autoFuel)
                    return;
                isKiln = true;
                autoFuelRange = Configuration.Current.Kiln.autoRange;
                ignorePrivateAreaCheck = Configuration.Current.Kiln.ignorePrivateAreaCheck;
            }
            else if (smelter.m_name.Equals(SmelterDefinitions.SmelterName))
            {
                if (!Configuration.Current.Smelter.IsEnabled || !Configuration.Current.Smelter.autoFuel)
                    return;
                autoFuelRange = Configuration.Current.Smelter.autoRange;
                ignorePrivateAreaCheck = Configuration.Current.Smelter.ignorePrivateAreaCheck;
            }
            else if (smelter.m_name.Equals(SmelterDefinitions.FurnaceName))
            {
                if (!Configuration.Current.Furnace.IsEnabled || !Configuration.Current.Furnace.autoFuel)
                    return;
                autoFuelRange = Configuration.Current.Furnace.autoRange;
                ignorePrivateAreaCheck = Configuration.Current.Furnace.ignorePrivateAreaCheck;
            }
            else if (__instance.m_name.Equals(SmelterDefinitions.WindmillName))
            {
                if (!Configuration.Current.Windmill.IsEnabled || !Configuration.Current.Windmill.autoFuel)
                    return;
                autoFuelRange = Configuration.Current.Windmill.autoRange;
                ignorePrivateAreaCheck = Configuration.Current.Windmill.ignorePrivateAreaCheck;
            }
            else if (__instance.m_name.Equals(SmelterDefinitions.SpinningWheelName))
            {
                if (!Configuration.Current.SpinningWheel.IsEnabled || !Configuration.Current.SpinningWheel.autoFuel)
                    return;
                autoFuelRange = Configuration.Current.SpinningWheel.autoRange;
                ignorePrivateAreaCheck = Configuration.Current.SpinningWheel.ignorePrivateAreaCheck;
            }
            else if (__instance.m_name.Equals(SmelterDefinitions.EitrRefineryName))
            {
                if (!Configuration.Current.EitrRefinery.IsEnabled || !Configuration.Current.EitrRefinery.autoFuel)
                    return;
                autoFuelRange = Configuration.Current.EitrRefinery.autoRange;
                ignorePrivateAreaCheck = Configuration.Current.EitrRefinery.ignorePrivateAreaCheck;
            }

            autoFuelRange = Helper.Clamp(autoFuelRange, 1, 50);

            int toMaxOre = smelter.m_maxOre - smelter.GetQueueSize();
            int toMaxFuel = smelter.m_maxFuel - (int)System.Math.Ceiling(smelter.GetFuel());

            if (smelter.m_fuelItem && toMaxFuel > 0)
            {
                ItemDrop.ItemData fuelItemData = smelter.m_fuelItem.m_itemData;

                // Check for fuel in nearby containers
                int addedFuel = InventoryAssistant.RemoveItemInAmountFromAllNearbyChests(smelter.gameObject, autoFuelRange, fuelItemData, toMaxFuel, !ignorePrivateAreaCheck);
                for (int i = 0; i < addedFuel; i++)
                {
                    smelter.m_nview.InvokeRPC("AddFuel", new object[] { });
                }
                if (addedFuel > 0)
                    ZLog.Log("Added " + addedFuel + " fuel(" + fuelItemData.m_shared.m_name + ") in " + smelter.m_name);
            }
            if (toMaxOre > 0)
            {
                List<Container> nearbyChests = InventoryAssistant.GetNearbyChests(smelter.gameObject, autoFuelRange);
                foreach (Container c in nearbyChests)
                {
                    foreach (Smelter.ItemConversion itemConversion in smelter.m_conversion)
                    {
                        if (isKiln)
                        {
                            if (Configuration.Current.Kiln.dontProcessFineWood && itemConversion.m_from.m_itemData.m_shared.m_name.Equals(WoodDefinitions.FineWoodName)) continue;
                            if (Configuration.Current.Kiln.dontProcessRoundLog && itemConversion.m_from.m_itemData.m_shared.m_name.Equals(WoodDefinitions.RoundLogName)) continue;

                            int threshold = Configuration.Current.Kiln.stopAutoFuelThreshold < 0 ? 0 : Configuration.Current.Kiln.stopAutoFuelThreshold;
                            if (threshold > 0 && InventoryAssistant.GetItemAmountInItemList(InventoryAssistant.GetNearbyChestItemsByContainerList(nearbyChests), itemConversion.m_to.m_itemData) >= threshold) return;
                        }

                        ItemDrop.ItemData oreItem = itemConversion.m_from.m_itemData;
                        int addedOres = InventoryAssistant.RemoveItemFromChest(c, oreItem, toMaxOre);
                        if (addedOres > 0)
                        {
                            GameObject orePrefab = ObjectDB.instance.GetItemPrefab(itemConversion.m_from.gameObject.name);

                            for (int i = 0; i < addedOres; i++)
                            {
                                smelter.m_nview.InvokeRPC("AddOre", new object[] { orePrefab.name });
                            }
                            toMaxOre -= addedOres;
                            if (addedOres > 0)
                                ZLog.Log("Added " + addedOres + " ores(" + oreItem.m_shared.m_name + ") in " + smelter.m_name);
                            if (toMaxOre == 0)
                                return;
                        }
                    }
                }
            }
        }
    }

    [HarmonyPatch(typeof(Smelter), nameof(Smelter.FindCookableItem))]
    public static class Smelter_FindCookableItem_Transpiler
    {
        private static MethodInfo method_PreventUsingSpecificWood = AccessTools.Method(typeof(Smelter_FindCookableItem_Transpiler), nameof(Smelter_FindCookableItem_Transpiler.PreventUsingSpecificWood));

        /// <summary>
        /// Patches out the function that check for Cookable items.
        /// It prevent putting Fine Wood and/or Round Log items in the Smelter when enabled.
        /// </summary>
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            if (!Configuration.Current.Kiln.IsEnabled) return instructions;

            int pos = -1;

            List<CodeInstruction> il = instructions.ToList();
            for (int i = 0; i < il.Count; i++)
            {
                if (il[i].opcode == OpCodes.Stloc_1)
                {
                    il.Insert(++i, new CodeInstruction(OpCodes.Ldarg_0));
                    il.Insert(++i, new CodeInstruction(OpCodes.Ldloc_1));
                    il.Insert(++i, new CodeInstruction(OpCodes.Call, method_PreventUsingSpecificWood));
                    pos = i;
                }
                else if (pos != -1 && il[i].opcode == OpCodes.Brfalse)
                {
                    il.Insert(++pos, new CodeInstruction(OpCodes.Brtrue, il[i].operand));
                    return il.AsEnumerable();
                }
            }

            ZLog.LogError("Failed to apply Smelter_FindCookableItem_Transpiler");

            return instructions;
        }

        private static bool PreventUsingSpecificWood(Smelter smelter, Smelter.ItemConversion itemConversion)
        {
            if (smelter.m_name.Equals(SmelterDefinitions.KilnName))
            {
                if (Configuration.Current.Kiln.dontProcessFineWood && itemConversion.m_from.m_itemData.m_shared.m_name.Equals(WoodDefinitions.FineWoodName) ||
                    Configuration.Current.Kiln.dontProcessRoundLog && itemConversion.m_from.m_itemData.m_shared.m_name.Equals(WoodDefinitions.RoundLogName))
                    return true;
            }

            return false;
        }
    }

    [HarmonyPatch(typeof(Smelter), nameof(Smelter.UpdateSmelter))]
    public static class Smelter_UpdaterSmelter_Transpiler
    {
        private static MethodInfo method_Windmill_GetPowerOutput = AccessTools.Method(typeof(Windmill), nameof(Windmill.GetPowerOutput));
        private static MethodInfo method_GetPowerOutput = AccessTools.Method(typeof(Smelter_UpdaterSmelter_Transpiler), nameof(Smelter_UpdaterSmelter_Transpiler.GetPowerOutput));

        /// <summary>
        /// Patches out the code that get the power output of a windmill.
        /// If ignoreWindIntensity is enabled, the power output will always be set to 1;
        /// </summary>
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            if (!Configuration.Current.Windmill.IsEnabled || !Configuration.Current.Windmill.ignoreWindIntensity) return instructions;

            List<CodeInstruction> il = instructions.ToList();
            for (int i = 0; i < il.Count; i++)
            {
                if (il[i].Calls(method_Windmill_GetPowerOutput))
                {
                    il[i].operand = method_GetPowerOutput;
                    return il.AsEnumerable();
                }
            }

            return instructions;
        }

        private static float GetPowerOutput(Windmill __instance)
        {
            return 1f;
        }
    }

    public static class SmelterDefinitions
    {
        public static readonly string KilnName = "$piece_charcoalkiln";
        public static readonly string SmelterName = "$piece_smelter";
        public static readonly string FurnaceName = "$piece_blastfurnace";
        public static readonly string WindmillName = "$piece_windmill";
        public static readonly string SpinningWheelName = "$piece_spinningwheel";
        public static readonly string EitrRefineryName = "$piece_eitrrefinery";
    }

    public static class FurnaceDefinitions
    {
        public static readonly string SapPrefabName = "Sap";
        public static readonly string CopperOrePrefabName = "CopperOre";
        public static readonly string ScrapIronPrefabName = "IronScrap";
        public static readonly string SilverOrePrefabName = "SilverOre";
        public static readonly string TinOrePrefabName = "TinOre";

        public static readonly string RefinedEitrPrefabName = "Eitr";
        public static readonly string CopperPrefabName = "Copper";
        public static readonly string IronPrefabName = "Iron";
        public static readonly string SilverPrefabName = "Silver";
        public static readonly string TinPrefabName = "Tin";

        public static readonly List<Smelter.ItemConversion> AdditionalConversions = new List<Smelter.ItemConversion>
        {
            new Smelter.ItemConversion() { m_from = ObjectDB.instance.GetItemPrefab(CopperOrePrefabName).GetComponent<ItemDrop>(), m_to = ObjectDB.instance.GetItemPrefab(CopperPrefabName).GetComponent<ItemDrop>().GetComponent<ItemDrop>() },
            new Smelter.ItemConversion() { m_from = ObjectDB.instance.GetItemPrefab(ScrapIronPrefabName).GetComponent<ItemDrop>(), m_to = ObjectDB.instance.GetItemPrefab(IronPrefabName).GetComponent<ItemDrop>().GetComponent<ItemDrop>() },
            new Smelter.ItemConversion() { m_from = ObjectDB.instance.GetItemPrefab(SilverOrePrefabName).GetComponent<ItemDrop>(), m_to = ObjectDB.instance.GetItemPrefab(SilverPrefabName).GetComponent<ItemDrop>().GetComponent<ItemDrop>() },
            new Smelter.ItemConversion() { m_from = ObjectDB.instance.GetItemPrefab(TinOrePrefabName).GetComponent<ItemDrop>(), m_to = ObjectDB.instance.GetItemPrefab(TinPrefabName).GetComponent<ItemDrop>().GetComponent<ItemDrop>() },
            new Smelter.ItemConversion() { m_from = ObjectDB.instance.GetItemPrefab(SapPrefabName).GetComponent<ItemDrop>(), m_to = ObjectDB.instance.GetItemPrefab(RefinedEitrPrefabName).GetComponent<ItemDrop>().GetComponent<ItemDrop>() }
        };
    }

    public static class WoodDefinitions
    {
        public static readonly string FineWoodName = "$item_finewood";
        public static readonly string RoundLogName = "$item_roundlog";
    }
}
