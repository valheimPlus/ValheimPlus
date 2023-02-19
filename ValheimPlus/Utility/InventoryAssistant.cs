using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ValheimPlus.Configurations;

namespace ValheimPlus
{
    static class InventoryAssistant
    {

        /// <summary>
        /// Get all valid nearby chests
        /// </summary>
        public static List<Container> GetNearbyChests(GameObject target, float range, bool checkWard = true)
        {
            // item == cart layermask
            // vehicle == cart&ship layermask

            string[] layerMask = { "piece" };

            if (Configuration.Current.CraftFromChest.allowCraftingFromCarts || Configuration.Current.CraftFromChest.allowCraftingFromShips)
                layerMask = new string[] { "piece", "item", "vehicle" };

            Collider[] hitColliders = Physics.OverlapSphere(target.transform.position, range, LayerMask.GetMask(layerMask));

            // Order the found objects to select the nearest first instead of the farthest inventory.
            IOrderedEnumerable<Collider> orderedColliders = hitColliders.OrderBy(x => Vector3.Distance(x.gameObject.transform.position, target.transform.position));

            List<Container> validContainers = new List<Container>();
            foreach (var hitCollider in orderedColliders)
            {
                try
                {
                    Container foundContainer = hitCollider.GetComponentInParent<Container>();
                    bool hasAccess = foundContainer.CheckAccess(Player.m_localPlayer.GetPlayerID());
                    if (checkWard) hasAccess = hasAccess && PrivateArea.CheckAccess(hitCollider.gameObject.transform.position, 0f, false, true);
                    var piece = foundContainer.GetComponentInParent<Piece>();
                    var isVagon = foundContainer.GetComponentInParent<Vagon>() != null;
                    var isShip = foundContainer.GetComponentInParent<Ship>() != null;

                    if (piece != null
                        /*&& piece.IsPlacedByPlayer() Prevents detection of ship storage */
                        && hasAccess
                        && foundContainer.GetInventory() != null)
                    {

                        if (isVagon && !Configuration.Current.CraftFromChest.allowCraftingFromCarts)
                            continue;
                        if (isShip && !Configuration.Current.CraftFromChest.allowCraftingFromShips)
                            continue;

                        if (piece.IsPlacedByPlayer() || (isShip && Configuration.Current.CraftFromChest.allowCraftingFromShips))
                            validContainers.Add(foundContainer);
                    }
                }
                catch { }
            }

            return validContainers;
        }

        /// <summary>
        /// Get a chests that contain the specified itemInfo.m_shared.m_name (item name)
        /// </summary>
        public static List<Container> GetNearbyChestsWithItem(GameObject target, float range, ItemDrop.ItemData itemInfo, bool checkWard = true)
        {
            List<Container> nearbyChests = GetNearbyChests(target, range, checkWard);

            List<Container> validChests = new List<Container>();
            foreach (Container chest in nearbyChests)
            {
                if (ChestContainsItem(chest, itemInfo))
                {
                    validChests.Add(chest);
                }
            }

            return validChests;
        }

        /// <summary>
        /// Check if the container contains the itemInfo.m_shared.name (item name)
        /// </summary>
        public static bool ChestContainsItem(Container chest, ItemDrop.ItemData itemDataToFind)
        {
            List<ItemDrop.ItemData> items = chest.GetInventory().GetAllItems();

            foreach (ItemDrop.ItemData item in items)
            {
                if (item.m_shared.m_name == itemDataToFind.m_shared.m_name)
                    return true;
            }

            return false;
        }

        public static bool ChestContainsItem(Container chest, string itemNameToFind)
        {
            List<ItemDrop.ItemData> items = chest.GetInventory().GetAllItems();

            foreach (ItemDrop.ItemData item in items)
            {
                if (item.m_shared.m_name == itemNameToFind)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// function to get all items in nearby chests by range
        /// </summary>
        public static List<ItemDrop.ItemData> GetNearbyChestItems(GameObject target, float range = 10, bool checkWard = true)
        {
            List<ItemDrop.ItemData> itemList = new List<ItemDrop.ItemData>();
            List<Container> nearbyChests = GetNearbyChests(target, range, checkWard);

            foreach (Container chest in nearbyChests)
            {
                List<ItemDrop.ItemData> chestItemList = chest.GetInventory().GetAllItems();
                foreach (ItemDrop.ItemData item in chestItemList)
                    itemList.Add(item);
            }

            return itemList;
        }

        /// <summary>
        /// function to get all items in chest list
        /// </summary>
        public static List<ItemDrop.ItemData> GetNearbyChestItemsByContainerList(List<Container> nearbyChests)
        {
            List<ItemDrop.ItemData> itemList = new List<ItemDrop.ItemData>();

            foreach (Container chest in nearbyChests)
            {
                List<ItemDrop.ItemData> chestItemList = chest.GetInventory().GetAllItems();
                foreach (ItemDrop.ItemData item in chestItemList)
                    itemList.Add(item);
            }

            return itemList;
        }

        /// <summary>
        /// function to get the amount of a specific item in a list of <ItemDrop.ItemData>
        /// </summary>
        public static int GetItemAmountInItemList(List<ItemDrop.ItemData> itemList, ItemDrop.ItemData itemDataToFind)
        {
            int amount = 0;
            foreach (ItemDrop.ItemData item in itemList)
            {
                if (item.m_shared.m_name == itemDataToFind.m_shared.m_name)
                    amount += item.m_stack;
            }

            return amount;
        }

        public static int GetItemAmountInItemList(List<ItemDrop.ItemData> itemList, string itemNameToFind)
        {
            int amount = 0;
            foreach (ItemDrop.ItemData item in itemList)
            {
                if (item.m_shared.m_name == itemNameToFind) amount += item.m_stack;
            }

            return amount;
        }

        // function to remove items in the amount from all nearby chests
        public static int RemoveItemInAmountFromAllNearbyChests(GameObject target, float range, ItemDrop.ItemData itemDataToRemove, int amount, bool checkWard = true)
        {
            List<Container> nearbyChests = GetNearbyChests(target, range, checkWard);

            // check if there are enough items nearby
            List<ItemDrop.ItemData> allItems = GetNearbyChestItemsByContainerList(nearbyChests);

            // get amount of item
            int availableAmount = GetItemAmountInItemList(allItems, itemDataToRemove);

            // check if there are enough items
            if (amount == 0)
                return 0;

            // iterate all chests and remove as many items as possible for the respective chest
            int itemsRemovedTotal = 0;
            foreach (Container chest in nearbyChests)
            {
                if (itemsRemovedTotal != amount)
                {
                    int removedItems = RemoveItemFromChest(chest, itemDataToRemove, amount);
                    itemsRemovedTotal += removedItems;
                    amount -= removedItems;
                }
            }

            return itemsRemovedTotal;
        }

        public static int RemoveItemInAmountFromAllNearbyChests(GameObject target, float range, string itemNameToRemove, int amount, bool checkWard = true)
        {
            List<Container> nearbyChests = GetNearbyChests(target, range, checkWard);

            // check if there are enough items nearby
            List<ItemDrop.ItemData> allItems = GetNearbyChestItemsByContainerList(nearbyChests);

            // get amount of item
            int availableAmount = GetItemAmountInItemList(allItems, itemNameToRemove);

            // check if there are enough items
            if (amount == 0)
                return 0;

            // iterate all chests and remove as many items as possible for the respective chest
            int itemsRemovedTotal = 0;
            foreach (Container chest in nearbyChests)
            {
                if (itemsRemovedTotal != amount)
                {
                    int removedItems = RemoveItemFromChest(chest, itemNameToRemove, amount);
                    itemsRemovedTotal += removedItems;
                    amount -= removedItems;
                }
            }

            return itemsRemovedTotal;
        }

        // function to add a item by name/ItemDrop.ItemData to a specified chest

        /// <summary>
        /// Removes the specified amount of a item found by m_shared.m_name by the declared amount
        /// </summary>
        public static int RemoveItemFromChest(Container chest, ItemDrop.ItemData itemDataToRemove, int amount = 1)
        {
            if (chest.IsInUse() || !ChestContainsItem(chest, itemDataToRemove))
            {
                return 0;
            }

            using (InventoryAssistant.lockContainer(chest))
            {
                int totalRemoved = 0;
                // find item
                List<ItemDrop.ItemData> allItems = chest.GetInventory().GetAllItems();
                foreach (ItemDrop.ItemData itemData in allItems)
                {
                    if (itemData.m_shared.m_name == itemDataToRemove.m_shared.m_name)
                    {
                        int num = Mathf.Min(itemData.m_stack, amount);
                        itemData.m_stack -= num;
                        amount -= num;
                        totalRemoved += num;
                        if (amount <= 0)
                        {
                            break;
                        }
                    }
                }

                // We don't want to send chest content through network
                if (totalRemoved == 0) return 0;

                allItems.RemoveAll((ItemDrop.ItemData x) => x.m_stack <= 0);
                chest.m_inventory.m_inventory = allItems;

                ConveyContainerToNetwork(chest);

                return totalRemoved;
            }
        }

        public static int RemoveItemFromChest(Container chest, string itemNameToRemove, int amount = 1)
        {
            if (chest.IsInUse() || !ChestContainsItem(chest, itemNameToRemove))
            {
                return 0;
            }

            using (InventoryAssistant.lockContainer(chest))
            {
                int totalRemoved = 0;
                // find item
                List<ItemDrop.ItemData> allItems = chest.GetInventory().GetAllItems();
                foreach (ItemDrop.ItemData itemData in allItems)
                {
                    if (itemData.m_shared.m_name == itemNameToRemove)
                    {
                        int num = Mathf.Min(itemData.m_stack, amount);
                        itemData.m_stack -= num;
                        amount -= num;
                        totalRemoved += num;
                        if (amount <= 0)
                        {
                            break;
                        }
                    }
                }

                // We don't want to send chest content through network
                if (totalRemoved == 0) return 0;

                allItems.RemoveAll((ItemDrop.ItemData x) => x.m_stack <= 0);
                chest.m_inventory.m_inventory = allItems;

                ConveyContainerToNetwork(chest);

                return totalRemoved;
            }
        }

        /// <summary>
        /// Function to convey the changes of a container to the network
        /// </summary>
        public static void ConveyContainerToNetwork(Container c)
        {
            c.Save();
            c.GetInventory().Changed();
        }

        /// <summary>
        /// To be used in a using statement during mutating operations.
        /// This will set the container to in use for the duration of the code within the using block.
        /// This will lock players out of the container while the code in the using statement is ran so that 
        /// their changes don't get overwritten by mod changes. Make sure to check that the container is not in use 
        /// before locking it.
        /// 
        /// Sample:
        /// <code>
        /// if (chest.IsInUse()) 
        /// 	continue;
        /// using (InventoryAssistant.lockContainer(chest))
        /// {
        /// 	// modify container
        /// 	InventoryAssistant.ConveyContainerToNetwork(chest);
        /// }
        /// </code>
        /// 
        /// </summary>
        /// <param name="container">the container to lock</param>
        /// <returns>a disposable that will unlock the container at the end of the using statement</returns>
        public static IDisposable lockContainer(Container container)
        {
            return new ContainerDisposable(container);
        }

        private class ContainerDisposable : IDisposable
        {
            private readonly Container m_container;

            public ContainerDisposable(Container container)
            {
                m_container = container;
                if (m_container.IsInUse())
                    ZLog.LogWarning("Locking a container that was already locked. This may result in overwriting container changes.");
                else
                    m_container.SetInUse(true);
            }

            public void Dispose()
            {
                if (!m_container.IsInUse())
                    ZLog.LogWarning("Unlocking a container that was already unlocked. There may have been overwriting container changes.");
                else
                    m_container.SetInUse(false);
            }
        }
    }
}
