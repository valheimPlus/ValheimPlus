using HarmonyLib;
using UnityEngine;
using ValheimPlus.Configurations;

namespace ValheimPlus.GameClasses
{
    class WardPrivateArea
    {
        /// <summary>
        /// Alter ward range
        /// </summary>
        [HarmonyPatch(typeof(PrivateArea), "Awake")]
        public static class ModifyWardRange
        {
            private static void Prefix(ref PrivateArea __instance)
            {
                if (Configuration.Current.Ward.IsEnabled && Configuration.Current.Ward.wardRange > 0) 
                {
                    __instance.m_radius = Configuration.Current.Ward.wardRange;

                    // Apply this change to the child GameObject's EffectArea collision.
                    // Various other systems query this collision instead of the PrivateArea radius for permissions (notably, enemy spawning).
                    Helper.ResizeChildEffectArea(__instance, EffectArea.Type.PlayerBase, Configuration.Current.Ward.wardRange);
                }
            }
        }
    }
}
