using HarmonyLib;
using UnityEngine;
using ValheimPlus.Configurations;

namespace ValheimPlus
{
    [HarmonyPatch(typeof(Smelter), "Awake")]
    public static class ApplyFurnaceChanges
    {
        
        private static void Prefix(ref Smelter __instance)
        {
            Debug.Log("Not first Awake");
            
            


            if (!__instance.m_addWoodSwitch)
            {
                // is kiln
                if (Configuration.Current.Kiln.IsEnabled)
                {
                    __instance.m_maxOre = Configuration.Current.Kiln.MaximumWood;
                    __instance.m_secPerProduct = Configuration.Current.Kiln.ProductionSpeed;
                }
            }
            else
            {
                // is furnace
                if (Configuration.Current.Furnace.IsEnabled)
                {
                    __instance.m_maxOre = Configuration.Current.Furnace.MaximumOre;
                    __instance.m_maxFuel = Configuration.Current.Furnace.MaximumCoal;
                    __instance.m_secPerProduct = Configuration.Current.Furnace.ProductionSpeed;
                    __instance.m_fuelPerProduct = Configuration.Current.Furnace.CoalUsedPerProduct;
                }
            }
        }

    }

    [HarmonyPatch(typeof(Smelter), "Spawn")]
    public static class AutoFurnaceDrop
    {
        private static bool Prefix(string ore,int stack, ref Smelter __instance) 
        {

            if (__instance.gameObject.name.Contains("kiln")) 
            {
                if (Configuration.Current.Kiln.IsEnabled) 
                {
                    if (Configuration.Current.Kiln.autoDeposit) {
                        Collider[] hitColliders = hitColliders = Physics.OverlapSphere(__instance.gameObject.transform.localPosition, Configuration.Current.Furnace.autoDepositRange);
                        foreach (var hitCollider in hitColliders) {
                            if (hitCollider.gameObject.GetComponentInParent<Container>() != null) {
                                GameObject itemPrefab = ObjectDB.instance.GetItemPrefab(__instance.GetItemConversion(ore).m_to.gameObject.name);
                                ZNetView.m_forceDisableInit = true;
                                GameObject spawnedOre = UnityEngine.Object.Instantiate<GameObject>(itemPrefab);
                                ZNetView.m_forceDisableInit = false;
                                ItemDrop comp = spawnedOre.GetComponent<ItemDrop>();
                                comp.m_itemData.m_stack = stack;
                                var result = hitCollider.gameObject.GetComponentInParent<Container>().m_inventory.AddItem(comp.m_itemData);
                                if(!result) 
                                {
                                    continue;
                                }
                                __instance.m_produceEffects.Create(__instance.transform.position, __instance.transform.rotation, null, 1f);
                                UnityEngine.Object.Destroy(spawnedOre);
                                return false;
                            }
                        }

                    }
                }
            }
            else
            {
                if (Configuration.Current.Furnace.IsEnabled) {
                    if (Configuration.Current.Furnace.autoDeposit) {
                        Collider[] hitColliders = hitColliders = Physics.OverlapSphere(__instance.gameObject.transform.localPosition, Configuration.Current.Furnace.autoDepositRange);
                        foreach (var hitCollider in hitColliders) {
                            if (hitCollider.gameObject.GetComponentInParent<Container>() != null) {
                                GameObject itemPrefab = ObjectDB.instance.GetItemPrefab(__instance.GetItemConversion(ore).m_to.gameObject.name);
                                ZNetView.m_forceDisableInit = true;
                                GameObject spawnedOre = UnityEngine.Object.Instantiate<GameObject>(itemPrefab);
                                ZNetView.m_forceDisableInit = false;
                                ItemDrop comp = spawnedOre.GetComponent<ItemDrop>();
                                comp.m_itemData.m_stack = stack;
                                var result = hitCollider.gameObject.GetComponentInParent<Container>().m_inventory.AddItem(comp.m_itemData);
                                if (!result) {
                                    continue;
                                }
                                __instance.m_produceEffects.Create(__instance.transform.position, __instance.transform.rotation, null, 1f);
                                UnityEngine.Object.Destroy(spawnedOre);
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        }

    }
}
