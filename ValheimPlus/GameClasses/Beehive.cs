using HarmonyLib;
using ValheimPlus.Configurations;
using UnityEngine;

namespace ValheimPlus
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
    [HarmonyPatch(typeof(Beehive), "UpdateBees")]
    public static class Beehive_UpdateBees_Patch
    {
        private static void Postfix(ref Beehive __instance)
        {
            return;

            if (!Configuration.Current.Beehive.autoDeposit || !Configuration.Current.Beehive.IsEnabled)
                return;

            // if max level honey
            if (__instance.GetHoneyLevel() <= 0)
                return;

            if (Configuration.Current.Beehive.autoDepositRange >= 50)
                Configuration.Current.Beehive.autoDepositRange = 50;

            if (Configuration.Current.Beehive.autoDepositRange <= 1)
                Configuration.Current.Beehive.autoDepositRange = 1;

            // place in nearby chest
            Collider[] hitColliders = Physics.OverlapSphere(__instance.gameObject.transform.localPosition, Configuration.Current.Beehive.autoDepositRange);

            // Reverse the found objects to select the nearest first instead of the farthest inventory.
            System.Array.Reverse(hitColliders);

            foreach (var hitCollider in hitColliders)
            {
                //Search for Containers components
                if (hitCollider.gameObject.GetComponentInParent<Container>() != null)
                {


                    GameObject itemPrefab = ObjectDB.instance.GetItemPrefab(__instance.m_honeyItem.gameObject.name);

                    ZNetView.m_forceDisableInit = true;
                    GameObject honeyObject = UnityEngine.Object.Instantiate<GameObject>(itemPrefab);
                    ZNetView.m_forceDisableInit = false;

                    ItemDrop comp = honeyObject.GetComponent<ItemDrop>();
                    var result = hitCollider.gameObject.GetComponentInParent<Container>().m_inventory.AddItem(comp.m_itemData);
                    UnityEngine.Object.Destroy(honeyObject);

                    if (!result)
                    {
                        //Chest full, move to the next
                        continue;
                    }

                    if (result)
                    {
                        __instance.m_nview.GetZDO().Set("level", __instance.GetHoneyLevel() - 1);
                    }


                }
            }


        }
    }
}
