using HarmonyLib;
using System.Linq;
using ValheimPlus.Configurations;

namespace ValheimPlus
{
    class FireplaceFuel
    {
        [HarmonyPatch(typeof(Fireplace), "Awake")]
        public static class Fireplace_Awake_Patch
        {
            /// <summary>
            /// When fire source is created, check for configurations and set its start fuel to max fuel
            /// </summary>
            private static void Prefix(ref Fireplace __instance)
            {
                if (!Configuration.Current.FireSource.IsEnabled) return;

                if (Configuration.Current.FireSource.onlyTorches)
                {
                    if (FireplaceExtensions.IsTorch(__instance.m_name))
                    {
                        __instance.m_startFuel = __instance.m_maxFuel;
                    }
                }
                else
                {
                    __instance.m_startFuel = __instance.m_maxFuel;
                }
            }
        }

        [HarmonyPatch(typeof(Fireplace), "GetTimeSinceLastUpdate")]
        public static class Fireplace_GetTimeSinceLastUpdate_Patch
        {
            /// <summary>
            /// If fire source is configured to keep fire source lit, reset time since being lit to 0
            /// </summary>
            private static void Postfix(ref double __result, ref Fireplace __instance)
            {
                if (!Configuration.Current.FireSource.IsEnabled) return;

                if (Configuration.Current.FireSource.onlyTorches)
                {
                    // if configuration is set to only keep torches lit, check that our current instance is a torch and only then intercept and overwrite result
                    if (FireplaceExtensions.IsTorch(__instance.m_name))
                    {
                        __result = 0.0;
                    }
                }
                else
                {
                    __result = 0.0;
                }
            }
        }
    }

    public static class FireplaceExtensions
    {
        static readonly string[] torchItemNames = new[]
        {
            "$piece_groundtorchwood", // standing wood torch
            "$piece_groundtorch", // standing iron torch
            "$piece_groundtorchgreen", // standing green torch
            "$piece_sconce", // sconce torch
            "$piece_brazierceiling01" // brazier
        };

        internal static bool IsTorch(string itemName)
        {
            return torchItemNames.Any(x => x.Equals(itemName));
        }
    }
}