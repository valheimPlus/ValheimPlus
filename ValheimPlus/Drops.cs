using HarmonyLib;
using UnityEngine;
using System;
using System.Collections.Generic;
using ValheimPlus.Configurations;

namespace ValheimPlus
{
    [HarmonyPatch(typeof(DropTable), "GetDropList", new Type[]
    {

    })]
    public static class GetDropList
    {
        private static bool Prefix(DropTable __instance, ref List<GameObject> __result)
        {
            if (Configuration.Current.Drops.IsEnabled)
            {
                int num = UnityEngine.Random.Range(__instance.m_dropMin, __instance.m_dropMax + 1) * Configuration.Current.Drops.baseIncreasedDropMultiplier;
                __result = __instance.GetDropList(num);
            }
            return false;
        }
    }

    [HarmonyPatch(typeof(Ragdoll), "SpawnLoot")]
    public static class SpawnLoot
    {
        private static bool Prefix(Ragdoll __instance, ref Vector3 center)
        {
            if (Configuration.Current.Drops.IsEnabled)
            {
                ZDO zDO = __instance.m_nview.GetZDO();
                int @int = zDO.GetInt("drops", 0);
                List<KeyValuePair<GameObject, int>> list = new List<KeyValuePair<GameObject, int>>();
                for (int i = 0; i < @int; i++)
                {
                    int int2 = zDO.GetInt("drop_hash" + i, 0);
                    int value = zDO.GetInt("drop_amount" + i, 0) * Configuration.Current.Drops.baseIncreasedDropMultiplier;
                    GameObject prefab = ZNetScene.instance.GetPrefab(int2);
                    list.Add(new KeyValuePair<GameObject, int>(prefab, value));
                }
                CharacterDrop.DropItems(list, center + Vector3.up * 0.75f, 0.5f);
            }
            return false;
        }
    }

    [HarmonyPatch(typeof(Pickable), "RPC_Pick")]
    public static class RPC_Pick
    {
        private static bool Prefix(ref Pickable __instance)
        {
            if (Configuration.Current.Drops.IsEnabled)
            {
                if (!__instance.m_nview.IsOwner())
                {
                    return true;
                }
                if (__instance.m_picked)
                {
                    return true;
                }
                int num = 0;
                int num2 = Configuration.Current.Drops.baseIncreasedDropMultiplier - 1;
                for (int i = 0; i < __instance.m_amount; i++)
                {
                    __instance.Drop(__instance.m_itemPrefab, num++, num2);
                }
            }
            return true;
        }
    }
}
