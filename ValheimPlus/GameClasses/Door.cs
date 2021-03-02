using HarmonyLib;
using System;
using System.Collections;
using UnityEngine;
using ValheimPlus.Configurations;

namespace ValheimPlus
{
    [HarmonyPatch(typeof(Door), "Interact")]
    public static class DoorAutoClose
    {
        private static void Postfix(ZNetView ___m_nview)
        {
            if (!Configuration.Current.Door.IsEnabled)
            {
                return;
            }

            IEnumerator enumerator = DoorCloseDelay(Configuration.Current.Door.waitTimeForDoorClosing, () =>
            {
                ___m_nview.GetZDO().Set("state", 0);
            });

            ___m_nview.StopAllCoroutines();
            ___m_nview.StartCoroutine(enumerator);
        }

        private static IEnumerator DoorCloseDelay(float wait_time, Action action)
        {
            yield return new WaitForSeconds(wait_time);

            action?.Invoke();
        }
    }
}
