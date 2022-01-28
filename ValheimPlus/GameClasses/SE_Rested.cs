using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;
using ValheimPlus.Configurations;
using HarmonyLib.Tools;
using MonoMod.Utils;

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
                ___m_TTLPerComfortLevel = Configuration.Current.Player.restSecondsPerComfortLevel;
            }
        }
    }


    /// <summary>
    /// Changes the radius in which pieces contribute to the rested bonus.
    /// </summary>
    [HarmonyPatch(typeof(SE_Rested), nameof(SE_Rested.GetNearbyPieces))]
    public static class SE_Rested_GetNearbyPieces_Transpiler
    {
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            if (!Configuration.Current.Building.IsEnabled || Configuration.Current.Building.pieceComfortRadius == 10) return instructions;

            List<CodeInstruction> il = instructions.ToList();

            for (int i = 0; i < il.Count; ++i)
            {
                if (il[i].opcode == OpCodes.Ldc_R4)
                    il[i].operand = Mathf.Clamp(Configuration.Current.Building.pieceComfortRadius, 1, 300);
            }

            return il.AsEnumerable();
        }

    }

    /// <summary>
    /// Changes the radius in which pieces contribute to the rested bonus.
    /// </summary>
    [HarmonyPatch(typeof(SE_Rested), nameof(SE_Rested.GetNearbyComfortPieces))]
    public static class SE_Rested_GetNearbyComfortPieces_Transpiler
    {
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            if (!Configuration.Current.Building.IsEnabled || Configuration.Current.Building.pieceComfortRadius == 10) return instructions;

            List<CodeInstruction> il = instructions.ToList();

            for (int i = 0; i < il.Count; ++i)
            {
                if (il[i].opcode == OpCodes.Ldc_R4)
                    il[i].operand = Mathf.Clamp(Configuration.Current.Building.pieceComfortRadius, 1, 300);
            }
            return il.AsEnumerable();
        }

    }

}
