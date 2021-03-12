using System;
using UnityEngine;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.IO;

namespace ValheimPlus
{
    static class Helper
    {
		public static Character getPlayerCharacter(Player __instance)
		{
			return (Character)__instance;
		}

        public static float tFloat(this float value, int digits)
        {
            double mult = Math.Pow(10.0, digits);
            double result = Math.Truncate(mult * value) / mult;
            return (float)result;
        }
         
        public static float applyModifierValue(float targetValue, float value)
        {
            if (value == 50) value = 51; // Decimal issue
            if (value == -50) value = -51; // Decimal issue

            if (value <= -100)
                value = -100;

            float newValue = targetValue;

            if (value >= 0)
            {
                newValue = targetValue + ((targetValue / 100) * value);
            }
            else
            {
                newValue = targetValue - ((targetValue / 100) * (value * -1));
            }

            return newValue;
        }

        public static Texture2D LoadPng(Stream fileStream)
        {
            Texture2D texture = null;

            if (fileStream != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    fileStream.CopyTo(memoryStream);

                    texture = new Texture2D(2, 2);
                    texture.LoadImage(memoryStream.ToArray()); //This will auto-resize the texture dimensions.
                }
            }

            return texture;
        }

        public static string CreateMD5(string input)
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }

                return sb.ToString();
            }
        }

        
        /// <summary>
        /// Resize child EffectArea's collision that matches the specified type(s).
        /// </summary>
        public static void ResizeChildEffectArea(MonoBehaviour parent, EffectArea.Type includedTypes, float newRadius)
        {
            if (parent != null)
            {
                EffectArea effectArea = parent.GetComponentInChildren<EffectArea>();
                if (effectArea != null)
                {
                    if ((effectArea.m_type & includedTypes) != 0)
                    {
                        SphereCollider collision = effectArea.GetComponent<SphereCollider>();
                        if (collision != null)
                        {
                            collision.radius = newRadius;
                        }
                    }
                }
            }
        }


        /// <summary>
        /// Get all valid nearby chests
        /// </summary>
        public static List<Container> GetNearbyChests(GameObject target, float range)
        {
            Collider[] hitColliders = Physics.OverlapSphere(target.transform.localPosition, range, LayerMask.GetMask(new string[] { "piece" }));

            // Order the found objects to select the nearest first instead of the farthest inventory.
            IOrderedEnumerable<Collider> orderedColliders = hitColliders.OrderBy(x => Vector3.Distance(x.gameObject.transform.position, target.transform.position));

            List<Container> validContainers = new List<Container>();
            foreach (var hitCollider in hitColliders)
            {
                try
                {
                    Container foundContainer = hitCollider.GetComponentInParent<Container>();
                    if (foundContainer.m_name.Contains("piece_chest") && foundContainer.GetInventory() != null)
                    {
                        validContainers.Add(foundContainer);
                    }
                }
                catch{ }
            }

            return validContainers;
        }

        /// <summary>
        /// Get a chests that contain the specified itemInfo.m_shared.m_name (item name)
        /// </summary>
        public static List<Container> GetNearbyChestsWithItem (GameObject target, float range, ItemDrop.ItemData itemInfo)
        {
            List<Container> nearbyChests = GetNearbyChests(target, range);

            List<Container> validChests = new List<Container>();
            foreach(Container chest in nearbyChests)
            {
                if (ChestContainsItem(chest, itemInfo)) { 
                    validChests.Add(chest);
                }
            }

            return validChests;
        }

        /// <summary>
        /// Check if the container contains the itemInfo.m_shared.name (item name)
        /// </summary>
        public static bool ChestContainsItem(Container chest, ItemDrop.ItemData needle)
        {
            List<ItemDrop.ItemData> items = chest.GetInventory().GetAllItems();

            foreach(ItemDrop.ItemData item in items)
            {
                if (item.m_shared.m_name == needle.m_shared.m_name)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// function to get all items in nearby chests by range
        /// </summary>
        public static List<ItemDrop.ItemData> GetNearbyChestItems(GameObject target, float range = 10)
        {
            List<ItemDrop.ItemData> itemList = new List<ItemDrop.ItemData>();
            List<Container> nearbyChests = GetNearbyChests(target, range);

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
        public static int GetItemAmountInItemList(List<ItemDrop.ItemData> itemList, ItemDrop.ItemData needle)
        {
            int amount = 0;
            foreach(ItemDrop.ItemData item in itemList)
            {
                if (item.m_shared.m_name == needle.m_shared.m_name)
                    amount += item.m_stack;
            }

            return amount;
        }

        // function to remove items in the amount from all nearby chests
        public static bool RemoveItemInAmountFromAllNearbyChests(GameObject target, float range, ItemDrop.ItemData needle, int amount)
        {
            List<Container> nearbyChests = GetNearbyChests(target, range);

            // check if there are enough items nearby
            List<ItemDrop.ItemData> allItems = Helper.GetNearbyChestItems(target, range);
            
            // get amount of item
            int availableAmount = GetItemAmountInItemList(allItems, needle);

            // check if there are enough items
            if (availableAmount < amount || amount == 0)
                return false;

            // iterate all chests and remove as many items as possible for the respective chest
            int itemsRemovedTotal = 0;
            foreach(Container chest in nearbyChests)
            {
                if(itemsRemovedTotal != amount) {
                    int removedItems = RemoveItemFromChest(chest, needle, amount);
                    itemsRemovedTotal += removedItems;
                    amount -= removedItems;
                }
            }

            return true;
        }

        // function to add a item by name/ItemDrop.ItemData to a specified chest

        /// <summary>
        /// Removes the specified amount of a item found by m_shared.m_name by the declared amount
        /// </summary>
        public static int RemoveItemFromChest(Container chest, ItemDrop.ItemData needle, int amount = 1)
        {
            if (!ChestContainsItem(chest, needle))
            {
                return 0;
            }

            int totalRemoved = 0;
            // find item
            List<ItemDrop.ItemData> allItems = chest.GetInventory().GetAllItems();
            foreach (ItemDrop.ItemData itemData in allItems)
            {
                if (itemData.m_shared.m_name == needle.m_shared.m_name)
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
            allItems.RemoveAll((ItemDrop.ItemData x) => x.m_stack <= 0);
            chest.m_inventory.m_inventory = allItems;
            chest.GetInventory().Changed();

            return totalRemoved;
        }

        // Clamp value between min and max
        public static int Clamp(int value, int min, int max)
        {
            return Math.Min(max, Math.Max(min, value));
        }
    }
}
