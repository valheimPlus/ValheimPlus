using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using ValheimPlus.Configurations;

namespace ValheimPlus
{
    [HarmonyPatch(typeof(SE_Rested), nameof(SE_Rested.UpdateTTL))]
    public static class SE_Rested_UpdateTTL_Patch
    {
        /// <summary>
        /// Updates the time per rested comfort level.
        /// </summary>
        [HarmonyPrefix]
        public static void Prefix(ref float ___m_TTLPerComfortLevel)
        {
            if (Configuration.Current.Player.IsEnabled)
            {
                const float ORIGINAL = 60;
                ___m_TTLPerComfortLevel = Helper.applyModifierValue(ORIGINAL, Configuration.Current.Player.restSecondsPerComfortLevel);
            }
        }
    }

    [HarmonyPatch(typeof(SE_Rested), nameof(SE_Rested.CalculateComfortLevel))]
    public static class SE_Rested_CalculateComfortLevel_Transpiler
    {
        private static MethodInfo method_SE_Rested_GetNearbyPieces = AccessTools.Method(typeof(SE_Rested), nameof(SE_Rested.GetNearbyPieces));
        private static MethodInfo method_GetNearbyPieces = AccessTools.Method(typeof(SE_Rested_CalculateComfortLevel_Transpiler), nameof(SE_Rested_CalculateComfortLevel_Transpiler.GetNearbyPieces));

        /// <summary>
        /// Changes the radius in which pieces contribute to the rested bonus.
        /// </summary>
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            if (!Configuration.Current.Building.IsEnabled) return instructions;

            List<CodeInstruction> il = instructions.ToList();

            for (int i = 0; i < il.Count; ++i)
            {
                if (il[i].Calls(method_SE_Rested_GetNearbyPieces))
                {
                    il[i].operand = method_GetNearbyPieces;
                    break;
                }
            }

            return il.AsEnumerable();
        }

        public static List<Piece> GetNearbyPieces(Vector3 point)
        {
            List<Piece> pieces = new List<Piece>();
            const float ORIGINAL = 10;
            Piece.GetAllPiecesInRadius(point, Helper.applyModifierValue(ORIGINAL, Configuration.Current.Building.pieceComfortRadius), pieces);
            return pieces;
        }
    }
}
