using HarmonyLib;
using System.Diagnostics;
using System.Collections.Generic;
using UnityEngine;
using ValheimPlus.Configurations;

namespace ValheimPlus.GameClasses
{
    /// <summary>
    /// Alters smelter input, output, and production speed configurations
    /// </summary>
    [HarmonyPatch(typeof(Smelter), "Awake")]
    public static class Smelter_Awake_Patch
    {
        private static void Prefix(ref Smelter __instance)
        {
            if (__instance.m_name.Equals(SmelterDefinitions.KilnName))
            {
                if (Configuration.Current.Kiln.IsEnabled)
                {
                    __instance.m_maxOre = Configuration.Current.Kiln.maximumWood;
                    __instance.m_secPerProduct = Configuration.Current.Kiln.productionSpeed;
                }
            }
            else if (__instance.m_name.Equals(SmelterDefinitions.SmelterName))
            {
                if (Configuration.Current.Smelter.IsEnabled)
                {
                    __instance.m_maxOre = Configuration.Current.Smelter.maximumOre;
                    __instance.m_maxFuel = Configuration.Current.Smelter.maximumCoal;
                    __instance.m_secPerProduct = Configuration.Current.Smelter.productionSpeed;
                    __instance.m_fuelPerProduct = Configuration.Current.Smelter.coalUsedPerProduct;
                }
            }
            else if (__instance.m_name.Equals(SmelterDefinitions.FurnaceName))
            {
                if (Configuration.Current.Furnace.IsEnabled)
                {
                    __instance.m_maxOre = Configuration.Current.Furnace.maximumOre;
                    __instance.m_maxFuel = Configuration.Current.Furnace.maximumCoal;
                    __instance.m_secPerProduct = Configuration.Current.Furnace.productionSpeed;
                    __instance.m_fuelPerProduct = Configuration.Current.Furnace.coalUsedPerProduct;
                }
            }
        }

    }

    [HarmonyPatch(typeof(Smelter), "Spawn")]
    public static class Smelter_Spawn_Patch
    {
        private static bool Prefix(string ore, int stack, ref Smelter __instance)
        {
            Smelter smelter = __instance; // allowing access to local function
            if (!smelter.m_nview.IsOwner())
                return true;

            if (__instance.m_name.Equals(SmelterDefinitions.KilnName))
            {
                if (Configuration.Current.Kiln.IsEnabled)
                {
                    if (Configuration.Current.Kiln.autoDeposit)
                    {
                        bool result = spawn(Helper.Clamp(Configuration.Current.Kiln.autoRange, 1, 50), Configuration.Current.Kiln.ignorePrivateAreaCheck);
                        return result;
                    }
                }
            }
            else if (__instance.m_name.Equals(SmelterDefinitions.SmelterName))
            {
                if (Configuration.Current.Smelter.IsEnabled)
                {
                    if (Configuration.Current.Smelter.autoDeposit)
                    {
                        bool result = spawn(Helper.Clamp(Configuration.Current.Smelter.autoRange, 1, 50), Configuration.Current.Smelter.ignorePrivateAreaCheck);
                        return result;
                    }
                }
            }
            else if (__instance.m_name.Equals(SmelterDefinitions.FurnaceName))
            {
                if (Configuration.Current.Furnace.IsEnabled)
                {
                    if (Configuration.Current.Furnace.autoDeposit)
                    {
                        bool result = spawn(Helper.Clamp(Configuration.Current.Furnace.autoRange, 1, 50), Configuration.Current.Furnace.ignorePrivateAreaCheck);
                        return result;
                    }
                }
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
                UnityEngine.Object.Destroy(spawnedOre);

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

    [HarmonyPatch(typeof(Smelter), "FixedUpdate")]
    public static class Smelter_FixedUpdate_Patch
    {
        static void Postfix(Smelter __instance)
        {
            if (__instance == null || !Player.m_localPlayer || __instance.m_nview == null || !__instance.m_nview.IsOwner())
                return;

            Smelter smelter = __instance;

            Stopwatch delta = GameObjectAssistant.GetStopwatch(smelter.gameObject);
            if (delta.IsRunning && delta.ElapsedMilliseconds < 1000) return;
            delta.Restart();

            float autoFuelRange = 0f;
            bool ignorePrivateAreaCheck = false;
            if (smelter.m_name.Equals(SmelterDefinitions.KilnName))
            {
                if (!Configuration.Current.Kiln.IsEnabled || !Configuration.Current.Kiln.autoFuel)
                    return;
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
    public static class SmelterDefinitions
    {
        public static readonly string KilnName = "$piece_charcoalkiln";
        public static readonly string SmelterName = "$piece_smelter";
        public static readonly string FurnaceName = "$piece_blastfurnace";
    }
}
