using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;
using ValheimPlus;
using ValheimPlus.Configurations;
using ValheimPlus.Utility;

namespace ValheimPlus.GameClasses
{
    public enum TameableMortalityTypes
    {
        Normal,
        Essential,
        Immortal
    }

    /// <summary>
    /// Adds a text indicator so player's know when an animal they've tamed has been stunned.
    /// </summary>
    [HarmonyPatch(typeof(Tameable), "GetHoverText")]
    public static class Tameable_GetHoverText_Patch
    {
        public static void Postfix(Tameable __instance, ref string __result)
        {
            if (Configuration.Current.Tameable.IsEnabled)
            {
                Tameable tameable = __instance;

                // If tamed creature is recovering from a stun, then add Stunned to hover text.
                if (tameable.m_character.m_nview.GetZDO().GetBool("isRecoveringFromStun"))
                    __result = __result.Insert(__result.IndexOf(" )"), ", Stunned");
            }
        }
    }
}
