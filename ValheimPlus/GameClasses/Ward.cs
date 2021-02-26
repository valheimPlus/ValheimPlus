using HarmonyLib;
using ValheimPlus.Configurations;

namespace ValheimPlus
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
                   __instance.m_radius  = Configuration.Current.Ward.wardRange;
                }
            }
        }
    }
}
