using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ValheimPlus
{
    /*[HarmonyPatch(typeof(ObjectDB), MethodType.Constructor)]
    public static class ObjectDB_Constructor
    {
        private static void Postfix(ref ObjectDB __instance)
        {
            Debug.Log("ObjectDB Constructor Patched");

            Debug.Log("GameObjects: " + __instance.m_items.Count);

            foreach (GameObject gameObject in __instance.m_items) {
                Debug.Log(gameObject.name);
            }
        }
    }

    [HarmonyPatch(typeof(ObjectDB), "Awake")]
    public static class ObjectDB_Awake
    {
        private static void Postfix(ref ObjectDB __instance)
        {
            Debug.Log("ObjectDB Awake Patched");

            Debug.Log("GameObjects: " + __instance.m_items.Count);

            foreach (GameObject gameObject in __instance.m_items)
            {
                Debug.Log(gameObject.name);
            }
        }
    }

    [HarmonyPatch(typeof(ObjectDB), "GetItemPrefab", typeof(String))]
    public static class ObjectDB_GetItemPrefab
    {
        private static GameObject Postfix(GameObject __result)
        {
            //Debug.Log("ObjectDB GetItemPrefab Patched");

            Debug.Log("ObjectDB Retrieved: " + __result.name);

            return __result;
        }
    }

    [HarmonyPatch(typeof(ObjectDB), "GetAllItems")]
    public static class ObjectDB_GetAllItems
    {
        private static List<ItemDrop> Postfix(List<ItemDrop> __result)
        {
            Debug.Log("ObjectDB GetAllItems Patched");

            return __result;
        }
    }*/
}
