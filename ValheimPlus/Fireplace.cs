using HarmonyLib;
using ValheimPlus.Configurations;

namespace ValheimPlus 
{
    class FireplaceFuel 
    {
        [HarmonyPatch(typeof(Fireplace), "UpdateFireplace")]
        public static class TorchesNoFuel 
        {
            /// <summary>
            /// Prefix which returns false every time to skip the original method and other prefixes so that we're not
            /// needlessly setting fuel value twice
            /// </summary>
            /// <param name="__instance"></param>
            /// <param name="___m_nview"></param>
            /// <returns>false</returns>
            private static bool Prefix(ref Fireplace __instance, ref ZNetView ___m_nview)
            {
                if (!__instance.m_nview.IsValid())
                    return false; // don't run other prefixes or original
                if (__instance.m_nview.IsOwner())
                {
                    if (Configuration.Current.Fireplace.IsEnabled)
                    {
                        FireplaceExtension.ApplyFuel(__instance, ref ___m_nview);
                    }
                    else
                    {
                        return true;
                    }
                }
                __instance.UpdateState();
                return false; // don't run other prefixes or original
            }
        }

        public static class FireplaceExtension
        {
            const string woodTorchName = "$piece_groundtorchwood";
            const string ironTorchName = "$piece_groundtorch";
            const string greenTorchName = "$piece_groundtorchgreen";
            const string sconceName = "$piece_sconce";
            const string brazierName = "$piece_brazierceiling01";

            internal static void ApplyFuel(Fireplace __instance, ref ZNetView ___m_nview)
            {
                if (Configuration.Current.Fireplace.onlyTorches)
                {
                    if (__instance.m_piece.m_name.Equals(woodTorchName) ||
                        __instance.m_piece.m_name.Equals(sconceName) ||
                        __instance.m_piece.m_name.Equals(ironTorchName) ||
                        __instance.m_piece.m_name.Equals(brazierName) ||
                        __instance.m_piece.m_name.Equals(greenTorchName))
                    {
                        //___m_nview.GetZDO().Set("fuel", __instance.m_maxFuel); // setting to max won't waste rss on fill attempts
                        ___m_nview.InvokeRPC("AddFuel", new object[] { });
                    }
                }
                else
                {
                    //___m_nview.GetZDO().Set("fuel", __instance.m_maxFuel); // setting to max won't waste rss on fill attempts
                    ___m_nview.InvokeRPC("AddFuel", new object[] { });
                }
            }
        }
    }
}
