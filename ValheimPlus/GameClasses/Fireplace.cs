using HarmonyLib;
using System.Linq;
using ValheimPlus.Configurations;

namespace ValheimPlus
{
    class FireplaceFuel
    {
        [HarmonyPatch(typeof(Fireplace), "UpdateFireplace")]
        public static class Fireplace_UpdateFireplace_Patch
        {
            /// <summary>
            /// Prefix which returns false every time to skip the original method and other prefixes so that we're not
            /// needlessly setting fuel value twice
            /// </summary>
            private static void Prefix(ref Fireplace __instance)
            {
                if (!Configuration.Current.FireSource.IsEnabled) return;

                if (!__instance.m_nview.IsValid()) return;

                if (__instance.m_nview.IsOwner())
                {
                    FireplaceExtension.ApplyFuel(ref __instance);
                }
            }
        }
    }

    public static class FireplaceExtension
    {
        static readonly string[] torchItemNames = new[]
        {
            "$piece_groundtorchwood", // standing wood torch
            "$piece_groundtorch", // standing iron torch
            "$piece_groundtorchgreen", // standing green torch
            "$piece_sconce", // sconce torch
            "$piece_brazierceiling01" // brazier
        };

        internal static void ApplyFuel(ref Fireplace __instance)
        {
            Fireplace localFireplace = __instance;
            if (Configuration.Current.FireSource.onlyTorches)
            {
                if (torchItemNames.Any(x => x.Equals(localFireplace.m_piece.m_name)))
                {
                    __instance.m_nview.GetZDO().Set("fuel", __instance.m_maxFuel); // setting to max won't waste rss on fill attempts
                }
            }
            else
            {
                __instance.m_nview.GetZDO().Set("fuel", __instance.m_maxFuel); // setting to max won't waste rss on fill attempts
            }
        }
    }
}