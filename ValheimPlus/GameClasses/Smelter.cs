using HarmonyLib;
using System.Linq;
using UnityEngine;
using ValheimPlus.Configurations;

namespace ValheimPlus
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

            if (__instance.m_name.Equals(SmelterDefinitions.KilnName))
            {
                if (Configuration.Current.Kiln.IsEnabled)
                {
                    if (Configuration.Current.Kiln.autoDeposit)
                    {
                        bool result = spawn(Configuration.Current.Kiln.autoDepositRange);
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
                        bool result = spawn(Configuration.Current.Smelter.autoDepositRange);
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
                        bool result = spawn(Configuration.Current.Furnace.autoDepositRange);
                        return result;
                    }
                }
            }

            bool spawn(float autoDepositRange)
            {
                if (autoDepositRange >= 50)
                {
                    autoDepositRange = 50;
                }

                // SphereCast grabbing all overlaps (didn't bother trying to find a mask, so this might be "heavy")
                Collider[] hitColliders = Physics.OverlapSphere(smelter.gameObject.transform.localPosition, autoDepositRange);

                // Order the found objects to select the nearest first instead of the farthest inventory.
                IOrderedEnumerable<Collider> orderedColliders = hitColliders.OrderBy(x => Vector3.Distance(smelter.gameObject.transform.localPosition, x.transform.position));

                foreach (Collider hitCollider in orderedColliders)
                {
                    // Search for Containers components
                    if (hitCollider.gameObject.GetComponentInParent<Container>() != null)
                    {
                        // Replicating original code, just "spawning/adding" the item inside the chest makes it "not have a prefab"
                        GameObject itemPrefab = ObjectDB.instance.GetItemPrefab(smelter.GetItemConversion(ore).m_to.gameObject.name);

                        // Also replication of original code, really have no idead what it is for, didn't bother look
                        ZNetView.m_forceDisableInit = true;
                        GameObject spawnedOre = UnityEngine.Object.Instantiate<GameObject>(itemPrefab);
                        ZNetView.m_forceDisableInit = false;

                        // assign stack size, nobody wants a 0/20 stack of metals (its not very usefull)
                        ItemDrop comp = spawnedOre.GetComponent<ItemDrop>();
                        comp.m_itemData.m_stack = stack;

                        bool result = hitCollider.gameObject.GetComponentInParent<Container>().m_inventory.AddItem(comp.m_itemData);
                        if (!result)
                        {
                            // Chest full, move to the next
                            continue;
                        }

                        smelter.m_produceEffects.Create(smelter.transform.position, smelter.transform.rotation, null, 1f);
                        UnityEngine.Object.Destroy(spawnedOre);

                        return false;
                    }
                }

                return true;
            }

            return true;
        }
    }

    public static class SmelterDefinitions
    {
        public static readonly string KilnName = "$piece_charcoalkiln";
        public static readonly string SmelterName = "$piece_smelter";
        public static readonly string FurnaceName = "$piece_blastfurnace";
    }
}
