using HarmonyLib;
using System;
using UnityEngine;
using ValheimPlus.Configurations;

namespace ValheimPlus
{

    [HarmonyPatch(typeof(Player))]
    public class hookDodgeRoll
    {
        [HarmonyReversePatch]
        [HarmonyPatch(typeof(Player), "Dodge", new Type[] { typeof(Vector3) })]
        public static void Dodge(object instance, Vector3 dodgeDir) => throw new NotImplementedException();
    }
    [HarmonyPatch(typeof(Player), "Update")]
    public static class ApplyDodgeHotkeys
    {
        private static void Postfix(ref Player __instance, ref Vector3 ___m_moveDir, ref Vector3 ___m_lookDir, ref GameObject ___m_placementGhost, Transform ___m_eye)
        {
            if (!Configuration.Current.Hotkeys.IsEnabled) return;

            KeyCode rollKeyForward = Configuration.Current.Hotkeys.rollForwards;
            KeyCode rollKeyBackwards = Configuration.Current.Hotkeys.rollBackwards;

            if (Input.GetKeyDown(rollKeyBackwards))
            {
                Vector3 dodgeDir = ___m_moveDir;
                if (dodgeDir.magnitude < 0.1f)
                {
                    dodgeDir = -___m_lookDir;
                    dodgeDir.y = 0f;
                    dodgeDir.Normalize();
                }
                hookDodgeRoll.Dodge(__instance, dodgeDir);
            }
            if (Input.GetKeyDown(rollKeyForward))
            {
                Vector3 dodgeDir = ___m_moveDir;
                if (dodgeDir.magnitude < 0.1f)
                {
                    dodgeDir = ___m_lookDir;
                    dodgeDir.y = 0f;
                    dodgeDir.Normalize();
                }
                hookDodgeRoll.Dodge(__instance, dodgeDir);
            }

        }

    }

}
