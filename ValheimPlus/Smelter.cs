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
            if (!__instance.m_addWoodSwitch)
            {
                // is kiln
                if (Configuration.Current.Kiln.IsEnabled)
                {
                    __instance.m_maxOre = Configuration.Current.Kiln.maximumWood;
                    __instance.m_secPerProduct = Configuration.Current.Kiln.productionSpeed;
                }
            }
            else
            {
                // is furnace
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
    public static class AutoFurnaceDrop
    {
        private static bool Prefix(string ore,int stack, ref Smelter __instance) 
        {
            var smelter = __instance; //allowing access to local function

            if (__instance.gameObject.name.Contains("kiln")) 
            {
                if (Configuration.Current.Kiln.IsEnabled) 
                {
                    if (Configuration.Current.Kiln.autoDeposit) 
                    {
                        var result = spawn(true);
                        return result; 
                    }
                }
            }
            else
            {
                if (Configuration.Current.Furnace.IsEnabled) 
                {
                    if (Configuration.Current.Furnace.autoDeposit) 
                    {
                        var result = spawn(false);
                        return result;
                    }
                }
            }

            bool spawn(bool isKiln) {

                //SphereCast grabbing all overlaps (didn't bother trying to find a mask, so this might be "heavy")
                Collider[] hitColliders = Physics.OverlapSphere(smelter.gameObject.transform.localPosition, isKiln ? Configuration.Current.Kiln.autoDepositRange : Configuration.Current.Furnace.autoDepositRange);
                
                foreach (var hitCollider in hitColliders) 
                {
                    //Search for Containers components
                    if (hitCollider.gameObject.GetComponentInParent<Container>() != null) 
                    {

                        //Replicating original code, just "spawning/adding" the item inside the chest makes it "not have a prefab"
                        GameObject itemPrefab = ObjectDB.instance.GetItemPrefab(smelter.GetItemConversion(ore).m_to.gameObject.name);

                        //Also replication of original code, really have no idead what it is for, didn't bother look
                        ZNetView.m_forceDisableInit = true;
                        GameObject spawnedOre = UnityEngine.Object.Instantiate<GameObject>(itemPrefab);
                        ZNetView.m_forceDisableInit = false;

                        //assign stack size, nobody wants a 0/20 stack of metals (its not very usefull)
                        ItemDrop comp = spawnedOre.GetComponent<ItemDrop>();
                        comp.m_itemData.m_stack = stack;

                        var result = hitCollider.gameObject.GetComponentInParent<Container>().m_inventory.AddItem(comp.m_itemData);
                        if (!result) 
                        {
                            //Chest full, move to the next
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
}
