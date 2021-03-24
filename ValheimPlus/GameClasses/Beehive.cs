using HarmonyLib;
using ValheimPlus.Configurations;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

namespace ValheimPlus.GameClasses
{
    /// <summary>
    /// Apply Beehive class changes
    /// </summary>
    [HarmonyPatch(typeof(Beehive), "Awake")]
    public static class Beehive_Awake_Patch
    {
        private static bool Prefix(ref float ___m_secPerUnit, ref int ___m_maxHoney)
        {
            if (Configuration.Current.Beehive.IsEnabled)
            {
                ___m_secPerUnit = Configuration.Current.Beehive.honeyProductionSpeed;
                ___m_maxHoney = Configuration.Current.Beehive.maximumHoneyPerBeehive;
            }

            return true;
        }
    }

    /// <summary>
    /// Altering the hover text to display the time until the next honey is produced
    /// </summary>
    [HarmonyPatch(typeof(Beehive), "GetHoverText")]
    public static class Beehive_GetHoverText_Patch
    {
        private static bool Prefix(Beehive __instance, ref string __result)
        {

            if (!Configuration.Current.Beehive.IsEnabled || !Configuration.Current.Beehive.showDuration)
                return true;

            if (!PrivateArea.CheckAccess(__instance.transform.position, 0f, false))
            {
                __result = Localization.instance.Localize(__instance.m_name + "\n$piece_noaccess");
                return false;
            }
            int honeyLevel = __instance.GetHoneyLevel();
            if (honeyLevel > 0)
            {
                __result = Localization.instance.Localize(string.Concat(new object[]
                {
                __instance.m_name,
                " ( ",
                __instance.m_honeyItem.m_itemData.m_shared.m_name,
                " x ",
                honeyLevel,
                " ) " + calculateTimeLeft(__instance) + "\n[<color=yellow><b>$KEY_Use</b></color>] $piece_beehive_extract"
                }));
                return false;
            }
            __result = Localization.instance.Localize(__instance.m_name + " ( $piece_container_empty ) " + calculateTimeLeft(__instance) + "\n[<color=yellow><b>$KEY_Use</b></color>] $piece_beehive_check");
            return false;

        }

        private static string calculateTimeLeft(Beehive BeehiveInstance)
        {
            string info = "";

            if (BeehiveInstance.GetHoneyLevel() == BeehiveInstance.m_maxHoney)
                return info;

            float num = BeehiveInstance.m_nview.GetZDO().GetFloat("product");

            float durationUntilDone = BeehiveInstance.m_secPerUnit - num;
            int minutes = (int)durationUntilDone / 60;

            if (((int)durationUntilDone) >= 120)
                info = minutes + " minutes";
            else
                info = (int)durationUntilDone + " seconds";

            return " (" + info + ")";
        }

    }

    /// <summary>
    /// Auto Deposit for Beehives
    /// </summary>
    [HarmonyPatch(typeof(Beehive), "RPC_Extract")]
    public static class Beehive_UpdateBees_Patch
    {
        private static bool Prefix(long caller, ref Beehive __instance)
        {
            // Allows for access for linq
            Beehive beehive = __instance; // allowing access to local function

            if (!Configuration.Current.Beehive.autoDeposit || !Configuration.Current.Beehive.IsEnabled || !beehive.m_nview.IsOwner())
                return true;

            // if behive is empty
            if (__instance.GetHoneyLevel() <= 0)
                return true;

            if (Configuration.Current.Beehive.autoDepositRange > 50)
                Configuration.Current.Beehive.autoDepositRange = 50;

            if (Configuration.Current.Beehive.autoDepositRange < 1)
                Configuration.Current.Beehive.autoDepositRange = 1;

            // find nearby chests
            List<Container> nearbyChests = InventoryAssistant.GetNearbyChests(beehive.gameObject, Configuration.Current.Beehive.autoDepositRange);
            if (nearbyChests.Count == 0)
                return true;

            while (beehive.GetHoneyLevel() > 0)
            {
                GameObject itemPrefab = ObjectDB.instance.GetItemPrefab(__instance.m_honeyItem.gameObject.name);

                ZNetView.m_forceDisableInit = true;
                GameObject honeyObject = UnityEngine.Object.Instantiate<GameObject>(itemPrefab);
                ZNetView.m_forceDisableInit = false;

                ItemDrop comp = honeyObject.GetComponent<ItemDrop>();

                bool result = spawnNearbyChest(comp, true);
                UnityEngine.Object.Destroy(honeyObject);
                
                if (!result)
                {
                    // Couldn't drop in chest, letting original code handle things
                    return true;
                }
            }

            if (beehive.GetHoneyLevel() == 0)
                beehive.m_spawnEffect.Create(beehive.m_spawnPoint.position, Quaternion.identity);

            bool spawnNearbyChest(ItemDrop item, bool mustHaveItem)
            {
                foreach (Container chest in nearbyChests)
                {
                    Inventory cInventory = chest.GetInventory();
                    if (mustHaveItem && !cInventory.HaveItem(item.m_itemData.m_shared.m_name))
                        continue;

                    if (!cInventory.AddItem(item.m_itemData))
                    {
                        //Chest full, move to the next
                        continue;
                    }
                    beehive.m_nview.GetZDO().Set("level", beehive.GetHoneyLevel() - 1);
                    InventoryAssistant.ConveyContainerToNetwork(chest);
                    return true;
                }
                
                if (mustHaveItem)
                    return spawnNearbyChest(item, false);

                return false;
            }

            return true;
        }
    }
}
