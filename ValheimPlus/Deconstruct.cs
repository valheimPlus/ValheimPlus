using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using ValheimPlus.Configurations;
using ValheimPlus.Configurations.Sections;

namespace ValheimPlus
{
/*
    public static class Deconstruct
    {
        public static InventoryGui __inventoryGui;
        public static Button m_tabDeconstruct;
        private const string deconstructText = "Deconstruct";
        private static Recipe m_deconstructRecipe;
        private static ItemDrop.ItemData m_deconstructItem;
        private static readonly string[] nonDestructableItems = new[] { "$item_oozebomb" };
        private static readonly ItemDrop.ItemData.ItemType[] nonDestructableTypes = new[]
        {
            ItemDrop.ItemData.ItemType.Ammo,
            ItemDrop.ItemData.ItemType.Consumable,
            ItemDrop.ItemData.ItemType.Trophie,
            ItemDrop.ItemData.ItemType.Material
        };
        public static readonly string[] nonDestructableCraftingStations = new[] { "$piece_cauldron" };
        private static readonly string[] unreturnableResources = new[]
        {
            "$item_resin",
            "$item_trophy_deer",
            "$item_trophy_hatchling",
            "$item_trophy_wolf",
        };

        public static void Setup(ref InventoryGui __inventoryGui)
        {
            Deconstruct.__inventoryGui = __inventoryGui;

            // set up deconstruct inventory tab
            m_tabDeconstruct = Object.Instantiate<Button>(Deconstruct.__inventoryGui.m_tabUpgrade);
            m_tabDeconstruct.GetComponentInChildren<Text>().text = deconstructText.ToUpper(); // needs localization
            m_tabDeconstruct.gameObject.SetActive(true);
            m_tabDeconstruct.transform.SetParent(Deconstruct.__inventoryGui.m_tabUpgrade.transform.parent, false);

            RectTransform tabWidth = (RectTransform)Deconstruct.__inventoryGui.m_tabUpgrade.transform;
            m_tabDeconstruct.transform.position += new Vector3(6f + tabWidth.rect.width, 0, 0);
            m_tabDeconstruct.onClick.AddListener(OnTabDeconstructPressed);
        }

        public static void Deconstruct_UpdateRecipeList(ref Player __player)
        {
            Player localPlayer = __player;
            List<ItemDrop.ItemData> m_playerInventory = localPlayer.GetInventory().GetAllItems();

            __inventoryGui.m_availableRecipes.Clear();

            // clear any existing recipe list
            foreach (Object recipe in __inventoryGui.m_recipeList)
            {
                Object.Destroy(recipe);
            }
            __inventoryGui.m_recipeList.Clear();

            // get items in player inventory that have a recipe
            foreach (ItemDrop.ItemData craftableItemInInventory in m_playerInventory)
            {
                if (craftableItemInInventory == null) continue;

                Recipe itemRecipe = ObjectDB.instance.GetRecipe(craftableItemInInventory);

                if (itemRecipe != null && !nonDestructableTypes.Contains(itemRecipe.m_item.m_itemData.m_shared.m_itemType))
                {
                    // don't return recipes for nondestructable crafting stations
                    if (!localPlayer.RequiredCraftingStation(itemRecipe, itemRecipe.m_minStationLevel, true)) continue;

                    // don't return recipes that can't be deconstructed
                    if (nonDestructableItems.Any(x => x.Equals(craftableItemInInventory.m_shared.m_name))) continue;

                    // only add recipe to list if player is at required crafitng station with the correct level, and recipe type is permitted to be deconstructed
                    __inventoryGui.AddRecipeToList(localPlayer, itemRecipe, craftableItemInInventory, true);
                }
            };

            // set size of recipe list in crafting panel
            __inventoryGui.m_recipeListRoot.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Mathf.Max(__inventoryGui.m_recipeListBaseSize, (float)__inventoryGui.m_recipeList.Count * __inventoryGui.m_recipeListSpace));
        }

        public static void Deconstruct_UpdateRecipe(ref Player __localPlayer, float dt)
        {
            CraftingStation currentCraftingStation = __localPlayer.GetCurrentCraftingStation();
            if (currentCraftingStation == null) return;

            // set up crafting station requirements UI
            __inventoryGui.m_craftingStationName.text = Localization.instance.Localize(currentCraftingStation.m_name);
            __inventoryGui.m_craftingStationIcon.gameObject.SetActive(true);
            __inventoryGui.m_craftingStationIcon.sprite = currentCraftingStation.m_icon;
            __inventoryGui.m_craftingStationLevel.text = currentCraftingStation.GetLevel().ToString();
            __inventoryGui.m_craftingStationLevelRoot.gameObject.SetActive(true);

            if (__inventoryGui.m_selectedRecipe.Key != null)
            {
                // set recipe data
                __inventoryGui.m_recipeIcon.enabled = true;
                __inventoryGui.m_recipeName.enabled = true;
                __inventoryGui.m_recipeDecription.enabled = true;
                ItemDrop.ItemData itemData = __inventoryGui.m_selectedRecipe.Value;

                // set item quality
                int itemQuality = itemData != null ? itemData.m_quality : 1;

                // set sprite
                __inventoryGui.m_recipeIcon.sprite =
                    __inventoryGui.m_selectedRecipe.Key.m_item.m_itemData.m_shared.m_icons[itemData != null ? itemData.m_variant : __inventoryGui.m_craftVariant];

                // get localized item name
                string str = Localization.instance.Localize(__inventoryGui.m_selectedRecipe.Key.m_item.m_itemData.m_shared.m_name);

                // get recipe amount
                if (__inventoryGui.m_selectedRecipe.Key.m_amount > 1)
                {
                    str = str + " x" + (object)__inventoryGui.m_selectedRecipe.Key.m_amount;
                }

                // set recipe text
                __inventoryGui.m_recipeName.text = str;

                // set recipe description 
                __inventoryGui.m_recipeDecription.text = Localization.instance.Localize(Deconstruct_GetTooltip(__inventoryGui.m_selectedRecipe.Key.m_item.m_itemData, itemQuality)); // need to localize

                if (itemData != null)
                {
                    // setting text above deconstruct button
                    __inventoryGui.m_itemCraftType.text = "Resources returned";
                    __inventoryGui.m_itemCraftType.gameObject.SetActive(true);
                }
                else
                {
                    __inventoryGui.m_itemCraftType.gameObject.SetActive(false);
                }

                // if there are more than 1 variant and the selected item is null, variantButton should be active
                bool shouldVariantBeActive = __inventoryGui.m_selectedRecipe.Key.m_item.m_itemData.m_shared.m_variants > 1 && __inventoryGui.m_selectedRecipe.Value == null;
                __inventoryGui.m_variantButton.gameObject.SetActive(shouldVariantBeActive);

                // set up crafting requirements
                Deconstruct_SetupRequirementList(itemQuality);

                // get required crafting station and station level in order to craft the recipe
                int requiredStationLevel = __inventoryGui.m_selectedRecipe.Key.GetRequiredStationLevel(itemQuality);
                CraftingStation requiredStation = __inventoryGui.m_selectedRecipe.Key.GetRequiredStation(itemQuality);

                // set active min crafting station icon and text
                __inventoryGui.m_minStationLevelIcon.gameObject.SetActive(true);
                __inventoryGui.m_minStationLevelText.text = requiredStationLevel.ToString();

                if (currentCraftingStation == null || currentCraftingStation.GetLevel() < requiredStationLevel)
                {
                    __inventoryGui.m_minStationLevelText.color = (double)Mathf.Sin(Time.time * 10f) > 0.0 ? Color.red : __inventoryGui.m_minStationLevelBasecolor;
                }
                else
                {
                    __inventoryGui.m_minStationLevelText.color = __inventoryGui.m_minStationLevelBasecolor;
                }

                bool playerHasInventorySpace = true;

                // check for inventory space on player
                foreach (Piece.Requirement resource in __inventoryGui.m_selectedRecipe.Key.m_resources)
                {
                    int amount = Deconstruct_GetAmount(resource, itemQuality);

                    // don't check inventory space for unreturnable resources
                    if (unreturnableResources.Any(x => x.Equals(resource.m_resItem.m_itemData.m_shared.m_name)))
                    {
                        amount = 0;
                        continue;
                    }

                    // if player doesn't have space, break out of check completely
                    if (!__localPlayer.GetInventory().CanAddItem(resource.m_resItem.m_itemData, amount))
                    {
                        playerHasInventorySpace = false;
                        break;
                    }
                }

                bool playerHasRequirements = Deconstruct_HaveRequirements(ref __localPlayer, __inventoryGui.m_selectedRecipe.Key, itemQuality);
                bool craftingStationIsValid = !requiredStation || (currentCraftingStation && currentCraftingStation.CheckUsable(__localPlayer, false));

                // should deconstruct button be clickable
                __inventoryGui.m_craftButton.interactable = (((playerHasRequirements && craftingStationIsValid) || __localPlayer.NoCostCheat()) && playerHasInventorySpace);
                __inventoryGui.m_craftButton.GetComponentInChildren<Text>().text = deconstructText;
                UITooltip component = __inventoryGui.m_craftButton.GetComponent<UITooltip>();

                if (!playerHasInventorySpace)
                {
                    component.m_text = Localization.instance.Localize("$inventory_full");
                }
                else if (!playerHasRequirements)
                {
                    component.m_text = Localization.instance.Localize("$msg_missingrequirement");
                }
                else if (!craftingStationIsValid)
                {
                    component.m_text = Localization.instance.Localize("$msg_missingstation");
                }
                else
                {
                    component.m_text = "";
                }
            }
            else // if no recipe is selected
            {
                __inventoryGui.m_recipeIcon.enabled = false;
                __inventoryGui.m_recipeName.enabled = false;
                __inventoryGui.m_recipeDecription.enabled = false;
                __inventoryGui.m_qualityPanel.gameObject.SetActive(false);
                __inventoryGui.m_minStationLevelIcon.gameObject.SetActive(false);
                __inventoryGui.m_craftButton.GetComponent<UITooltip>().m_text = "";
                __inventoryGui.m_variantButton.gameObject.SetActive(false);
                __inventoryGui.m_itemCraftType.gameObject.SetActive(false);

                for (int index = 0; index < __inventoryGui.m_recipeRequirementList.Length; ++index)
                {
                    InventoryGui.HideRequirement(__inventoryGui.m_recipeRequirementList[index].transform);
                }

                __inventoryGui.m_craftButton.interactable = false;
            }

            // if deconstruct button has not been pressed
            if ((double)__inventoryGui.m_craftTimer < 0.0)
            {
                __inventoryGui.m_craftProgressPanel.gameObject.SetActive(false);
                __inventoryGui.m_craftButton.gameObject.SetActive(true);
            }
            else
            {
                __inventoryGui.m_craftButton.gameObject.SetActive(false);
                __inventoryGui.m_craftProgressPanel.gameObject.SetActive(true);
                __inventoryGui.m_craftProgressPanel.GetComponentInChildren<Text>().text = "Deconstructing...";
                __inventoryGui.m_craftProgressBar.SetMaxValue(__inventoryGui.m_craftDuration);
                __inventoryGui.m_craftProgressBar.SetValue(__inventoryGui.m_craftTimer);

                __inventoryGui.m_craftTimer += dt;
                if ((double)__inventoryGui.m_craftTimer < (double)__inventoryGui.m_craftDuration) return;

                Deconstruct_Deconstruct(ref __localPlayer);

                __inventoryGui.m_craftTimer = -1f;
                __inventoryGui.m_craftProgressPanel.GetComponentInChildren<Text>().text = Localization.instance.Localize("$inventory_craftingprog");
            }
        }

        private static void Deconstruct_SetupRequirementList(int quality)
        {
            int index = 0;

            // setup and show recipe resources if resource meets requirement
            foreach (Piece.Requirement resource in __inventoryGui.m_selectedRecipe.Key.m_resources)
            {
                bool resourceRequirementSetupSuccessfully = Deconstruct_SetupRequirement(__inventoryGui.m_recipeRequirementList[index].transform, resource, quality);
                if (resourceRequirementSetupSuccessfully)
                {
                    ++index;
                }
            }

            // hide requirements that weren't in the selected recipe resources list
            for (; index < __inventoryGui.m_recipeRequirementList.Length; ++index)
            {
                InventoryGui.HideRequirement(__inventoryGui.m_recipeRequirementList[index].transform);
            }
        }

        private static bool Deconstruct_SetupRequirement(Transform elementRoot, Piece.Requirement itemResourceRequirement, int quality)
        {
            if (itemResourceRequirement.m_resItem != null)
            {
                // set up resource object
                Image resourceIcon = elementRoot.transform.Find("res_icon").GetComponent<Image>();
                Text resourceName = elementRoot.transform.Find("res_name").GetComponent<Text>();
                Text resourceAmount = elementRoot.transform.Find("res_amount").GetComponent<Text>();
                UITooltip resourceTooltip = elementRoot.GetComponent<UITooltip>();

                // display resource object
                resourceIcon.gameObject.SetActive(true);
                resourceName.gameObject.SetActive(true);
                resourceAmount.gameObject.SetActive(true);

                resourceIcon.sprite = itemResourceRequirement.m_resItem.m_itemData.GetIcon();
                resourceIcon.color = Color.white;

                // get resource name and text
                string resourceText = Localization.instance.Localize(itemResourceRequirement.m_resItem.m_itemData.m_shared.m_name);
                resourceTooltip.m_text = resourceText;
                resourceName.text = resourceText;

                int amount;

                // get amount of resources returned based on quality of item and check if there is a user-defined percentage of resources returned
                amount = Deconstruct_GetAmount(itemResourceRequirement, quality);

                if (amount <= 0)
                {
                    // if nothing will be returned, hide the resource
                    InventoryGui.HideRequirement(elementRoot);
                    return false;
                }

                if (unreturnableResources.Any(x => x.Equals(itemResourceRequirement.m_resItem.m_itemData.m_shared.m_name)))
                {
                    amount = 0;
                }

                resourceAmount.text = amount.ToString();
                resourceAmount.color = Color.white;
            }

            return true;
        }

        private static bool Deconstruct_HaveRequirements(ref Player __player, Recipe recipe, int qualityLevel)
        {
            if (!__player.RequiredCraftingStation(recipe, qualityLevel, true)) return false;

            return recipe.m_item.m_itemData.m_shared.m_dlc.Length <= 0 || DLCMan.instance.IsDLCInstalled(recipe.m_item.m_itemData.m_shared.m_dlc);
        }

        private static string Deconstruct_GetTooltip(ItemDrop.ItemData item, int qualityLevel)
        {
            StringBuilder text = new StringBuilder(256);
            text.Append(item.m_shared.m_description);
            text.Append("\n\n");

            if (item.m_shared.m_dlc.Length > 0)
                text.Append("\n<color=aqua>$item_dlc</color>");
            if (item.m_crafterID != 0L)
                text.AppendFormat("\n$item_crafter: <color=orange>{0}</color>", (object)item.m_crafterName);
            if (!item.m_shared.m_teleportable)
                text.Append("\n<color=orange>$item_noteleport</color>");
            if (item.m_shared.m_value > 0)
                text.AppendFormat("\n$item_value: <color=orange>{0}  ({1})</color>", (object)item.GetValue(), (object)item.m_shared.m_value);
            if (item.m_shared.m_maxQuality > 1)
                text.AppendFormat("\n$item_quality: <color=orange>{0}</color>", (object)qualityLevel);

            float num;
            if (item.m_shared.m_useDurability)
            {
                float maxDurability = item.GetMaxDurability(qualityLevel);
                float durability = item.m_durability;
                StringBuilder stringBuilder = text;
                num = item.GetDurabilityPercentage() * 100f;
                string str1 = num.ToString("0");
                string str2 = durability.ToString("0");
                string str3 = maxDurability.ToString("0");
                stringBuilder.AppendFormat("\n$item_durability: <color=orange>{0}%</color> <color=yellow>({1}/{2})</color>", (object)str1, (object)str2, (object)str3);
            }

            switch (item.m_shared.m_itemType)
            {
                case ItemDrop.ItemData.ItemType.OneHandedWeapon:
                case ItemDrop.ItemData.ItemType.Bow:
                case ItemDrop.ItemData.ItemType.TwoHandedWeapon:
                case ItemDrop.ItemData.ItemType.Torch:
                    text.Append(item.GetDamage(qualityLevel).GetTooltipString(item.m_shared.m_skillType));

                    string statusEffectTooltip2 = item.GetStatusEffectTooltip();
                    if (statusEffectTooltip2.Length > 0)
                    {
                        text.Append("\n\n");
                        text.Append(statusEffectTooltip2);
                        break;
                    }

                    break;
                case ItemDrop.ItemData.ItemType.Shield:
                    break;
                case ItemDrop.ItemData.ItemType.Helmet:
                case ItemDrop.ItemData.ItemType.Chest:
                case ItemDrop.ItemData.ItemType.Legs:
                case ItemDrop.ItemData.ItemType.Shoulder:
                    string statusEffectTooltip3 = item.GetStatusEffectTooltip();
                    if (statusEffectTooltip3.Length > 0)
                    {
                        text.Append("\n\n");
                        text.Append(statusEffectTooltip3);
                        break;
                    }

                    break;
                case ItemDrop.ItemData.ItemType.Ammo:
                    break;
            }

            string statusEffectTooltip4 = item.GetSetStatusEffectTooltip();
            if (statusEffectTooltip4.Length > 0)
            {
                text.AppendFormat("\n\n$item_seteffect (<color=orange>{0}</color> $item_parts):<color=orange>{1}</color>", (object)item.m_shared.m_setSize, (object)statusEffectTooltip4);
            }

            return text.ToString();
        }

        private static void Deconstruct_Deconstruct(ref Player __localPlayer)
        {
            if (m_deconstructRecipe == null) return;

            int num = m_deconstructItem != null ? m_deconstructItem.m_quality : 1;

            if (!Deconstruct_HaveRequirements(ref __localPlayer, m_deconstructRecipe, num) && !__localPlayer.NoCostCheat() || !__localPlayer.GetInventory().ContainsItem(m_deconstructItem) || m_deconstructItem == null)
            {
                return;
            }

            if (m_deconstructRecipe.m_item.m_itemData.m_shared.m_dlc.Length > 0 && !DLCMan.instance.IsDLCInstalled(m_deconstructRecipe.m_item.m_itemData.m_shared.m_dlc))
            {
                __localPlayer.Message(MessageHud.MessageType.Center, "$msg_dlcrequired", 0, (Sprite)null);
                return;
            }

            foreach (Piece.Requirement resource in m_deconstructRecipe.m_resources)
            {
                // if unreturnable resource, continue without adding item to player inventory
                if (unreturnableResources.Any(x => x.Equals(resource.m_resItem.m_itemData.m_shared.m_name)))
                {
                    continue;
                }

                // get amount of resource to return, if any
                resource.m_resItem.m_itemData.m_stack = Deconstruct_GetAmount(resource, num);

                // add returned resources to player's inventory
                __localPlayer.GetInventory().AddItem(resource.m_resItem.m_itemData);
            };

            if (m_deconstructItem != null)
            {
                // remove player's deconstructed item
                __localPlayer.UnequipItem(m_deconstructItem);
                __localPlayer.GetInventory().RemoveItem(m_deconstructItem);
            }

            __inventoryGui.UpdateCraftingPanel();

            CraftingStation currentCraftingStation = __localPlayer.GetCurrentCraftingStation();

            if (currentCraftingStation != null)
            {
                currentCraftingStation.m_craftItemDoneEffects.Create(__localPlayer.transform.position, Quaternion.identity);
            }
            else
            {
                __inventoryGui.m_craftItemDoneEffects.Create(__localPlayer.transform.position, Quaternion.identity);
            }

            Gogan.LogEvent("Game", "Deconstructed", m_deconstructRecipe.m_item.m_itemData.m_shared.m_name, (long)num);
        }

        private static int Deconstruct_GetAmount(Piece.Requirement itemResource, int qualityLevel)
        {
            int multiplier = Configuration.Current.Deconstruct.percentageOfReturnedResource;
            if (multiplier > 100)
            {
                multiplier = 100;
            }
            else if (multiplier < 0)
            {
                multiplier = 0;
            }

            // if materials returned is default percentage, use base game GetAmount
            if (multiplier.Equals(100))
            {
                // get amount of resources returned based on quality of item
                return itemResource.GetAmount(qualityLevel);
            }

            int amountToReturn;

            if (qualityLevel <= 1)
            {
                amountToReturn = itemResource.m_amount;
            }
            else
            {
                amountToReturn = (qualityLevel - 1) * itemResource.m_amountPerLevel;
            }

            return (amountToReturn * multiplier) / 100;
        }

        public static void OnDeconstructPressed()
        {
            if (__inventoryGui.m_selectedRecipe.Key == null) return;

            Player localPlayer = Player.m_localPlayer;
            m_deconstructRecipe = __inventoryGui.m_selectedRecipe.Key;
            m_deconstructItem = __inventoryGui.m_selectedRecipe.Value;
            __inventoryGui.m_craftVariant = __inventoryGui.m_selectedVariant;
            __inventoryGui.m_craftTimer = 0.0f;

            if (m_deconstructRecipe.m_craftingStation != null)
            {
                CraftingStation currentCraftingStation = localPlayer.GetCurrentCraftingStation();
                if (currentCraftingStation == null) return;

                currentCraftingStation.m_craftItemEffects.Create(localPlayer.transform.position, Quaternion.identity);
            }
            else
            {
                __inventoryGui.m_craftItemEffects.Create(localPlayer.transform.position, Quaternion.identity);
            }
        }

        private static void OnTabDeconstructPressed()
        {
            __inventoryGui.m_tabCraft.interactable = true;
            __inventoryGui.m_tabUpgrade.interactable = true;
            m_tabDeconstruct.interactable = false;

            __inventoryGui.UpdateCraftingPanel();
        }

        public static bool InDeconstructTab() => !m_tabDeconstruct.interactable;

        public static void SetDeconstructTab(bool isInteractable) => m_tabDeconstruct.interactable = isInteractable;
    }
*/
}