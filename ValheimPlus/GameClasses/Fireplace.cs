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
                    if (ZNet.instance == null) return;
                    ZNet znet = ZNet.instance;
                    FireplaceExtension.ApplyFuel(ref __instance, ref znet);
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

        internal static void ApplyFuel(ref Fireplace __instance, ref ZNet __znet)
        {
            Fireplace localFireplace = __instance;

            if (Configuration.Current.FireSource.onlyTorches)
            {
                if (torchItemNames.Any(x => x.Equals(localFireplace.m_piece.m_name)))
                {
                    __instance.m_nview.GetZDO().Set("fuel", __instance.m_maxFuel); // setting to max won't waste rss on fill attempts
                    __instance.m_nview.GetZDO().Set("lastTime", __znet.GetTime().Ticks);
                }
            }
            else
            {
                __instance.m_nview.GetZDO().Set("fuel", __instance.m_maxFuel); // setting to max won't waste rss on fill attempts
                __instance.m_nview.GetZDO().Set("lastTime", __znet.GetTime().Ticks);
            }
        }
    }
}