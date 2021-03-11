using System;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

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
        /// Check if the current enviroment is skipping time and return true or false
        /// </summary>
        public static bool isTimeSkipping()
        {
            if (EnvMan.instance.IsTimeSkipping())
                return true;

            return false;
        }

        /// <summary>
        /// Get all valid nearby chests
        /// </summary>
        public static List<Container> GetNearbyChests(GameObject target, float range)
        {
            Collider[] hitColliders = Physics.OverlapSphere(target.transform.localPosition, range, LayerMask.GetMask(new string[] { "piece" }));

            // Order the found objects to select the nearest first instead of the farthest inventory.
            IOrderedEnumerable<Collider> orderedColliders = hitColliders.OrderBy(x => Vector3.Distance(target.transform.localPosition, x.transform.position));

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

        // function to get all items in nearby chests
        // function to get the amount of a specific item in all nearby chests
        // function to remove items in the amount from all nearby chests

        /// <summary>
        /// Removes the specified amount of a item found by m_shared.m_name by the declared amount
        /// </summary>
        public static bool RemoveItemFromChest(Container chest, ItemDrop.ItemData needle, int amount = 1)
        {
            if (!ChestContainsItem(chest, needle))
            {
                return false;
            }

            // find item
            List<ItemDrop.ItemData> allItems = chest.GetInventory().GetAllItems();
            foreach (ItemDrop.ItemData itemData in allItems)
            {
                if (itemData.m_shared.m_name == needle.m_shared.m_name)
                {
                    int num = Mathf.Min(itemData.m_stack, amount);
                    itemData.m_stack -= num;
                    amount -= num;
                    if (amount <= 0)
                    {
                        break;
                    }
                }
            }
            allItems.RemoveAll((ItemDrop.ItemData x) => x.m_stack <= 0);
            chest.m_inventory.m_inventory = allItems;
            chest.GetInventory().Changed();

            // note how many items removed
            // add function to see if there are enough items in all chests
            // add function to see remove all the available items for the demand

            return true;
        }

        
    }
}
