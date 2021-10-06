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
    public static class PickableYieldState
    {
        public static Dictionary<string, float> yieldModifier;
    }

    /// <summary>
    /// Allow tweaking of Pickable item yield. (E.g. berries, flowers, branches, stones, gemstones.)
    /// </summary>
    [HarmonyPatch(typeof(Pickable), "RPC_Pick")]
    public static class Pickable_RPC_Pick_Transpiler
    {
        // Our method and its arguments that we need to patch in
        private static readonly MethodInfo method_calculateYield = AccessTools.Method(typeof(Pickable_RPC_Pick_Transpiler), nameof(calculateYield));
        private static readonly FieldInfo field_ItemPrefab = AccessTools.Field(typeof(Pickable), "m_itemPrefab");
        private static readonly FieldInfo field_amount = AccessTools.Field(typeof(Pickable), "m_amount");

        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            if (!Configuration.Current.Pickable.IsEnabled)
                return instructions;

            List<CodeInstruction> il = instructions.ToList();

            LocalBuilder newYieldLocal = null;
            for (int i = 0; i < il.Count; i++)
            {
                if (newYieldLocal == null && i > 0 && il[i].opcode == OpCodes.Stloc_1 && il[i - 1].opcode == OpCodes.Ldc_I4_0)
                {
                    // Step 1: Call calculateYield() and store the result as a new local variable.
                    //
                    // Calling calculateYield takes several instructions:
                    // LdArg.0 (load the "this" pointer), LdFld (load m_itemPrefab from this), LdArg.0 (this), LdFld (load m_amount from this), Call.
                    // We then store the return value as our new local, used in the second patch below.
                    newYieldLocal = generator.DeclareLocal(typeof(int));
                    newYieldLocal.SetLocalSymInfo("newYieldLocal");
                    il.Insert(++i, new CodeInstruction(OpCodes.Ldarg_0));
                    il.Insert(++i, new CodeInstruction(OpCodes.Ldfld, field_ItemPrefab));
                    il.Insert(++i, new CodeInstruction(OpCodes.Ldarg_0));
                    il.Insert(++i, new CodeInstruction(OpCodes.Ldfld, field_amount));
                    il.Insert(++i, new CodeInstruction(new CodeInstruction(OpCodes.Call, method_calculateYield)));
                    il.Insert(++i, new CodeInstruction(OpCodes.Stloc_S, newYieldLocal.LocalIndex));
                }

                if (newYieldLocal != null && il[i].opcode == OpCodes.Ldfld && il[i].operand.ToString().Contains("m_amount"))
                {
                    // Step 2: Patch the for loop to loop calculateYield() iterations instead of m_amount iterations,
                    // by replacing m_amount with our new local variable, created above.
                    //
                    // Get rid of LdArg.1 (i-1) and LdFld (i) and replace them with loading our previously calculated yield.
                    il[i] = new CodeInstruction(OpCodes.Ldloc_S, newYieldLocal.LocalIndex);
                    il.RemoveRange(i - 1, 1);

                    ZLog.Log("Successfully transpiled Pickable.RPC_Pick to patch item yields");

                    // NOTE: This transpiler may be called multiple times, e.g. when starting the game, when connecting to a server and when disconnecting.
                    // We need to re-do the initial setup every time, since the modifier values may have changed (as the server config will be used instead of the client config).
                    initialSetup();

                    return il.AsEnumerable();
                }
            }

            ZLog.Log("Unable to transpile Pickable.RPC_Pick to patch item yields");
            return instructions;
        }

        private static int calculateYield(GameObject item, int originalAmount)
        {
            try
            {
                return (int)Helper.applyModifierValue(originalAmount, PickableYieldState.yieldModifier[item.name]);
            }
            catch
            {
                return originalAmount;
            }
        }

        private static void initialSetup()
        {
            // Called from the transpiler, so this will be run when the game starts, plus when you connect to or disconnect from a server.

            var edibles = new List<string>
            {
                "Carrot",
                "Blueberries",
                "Cloudberry",
                "Raspberry",
                "Mushroom",
                "MushroomBlue",
                "MushroomYellow",
                "Onion"
            };

            var flowersAndIngredients = new List<string>
            {
                "Barley",
                "CarrotSeeds",
                "Dandelion",
                "Flax",
                "Thistle",
                "TurnipSeeds",
                "Turnip",
                "OnionSeeds"
            };

            var materials = new List<string>
            {
                "BoneFragments",
                "Flint",
                "Stone",
                "Wood"
            };

            var valuables = new List<string>
            {
                "Amber",
                "AmberPearl",
                "Coins",
                "Ruby"
            };

            var surtlingCores = new List<string>
            {
                "SurtlingCore"
            };

            PickableYieldState.yieldModifier = new Dictionary<string, float>();

            foreach (var item in edibles)
                PickableYieldState.yieldModifier.Add(item, Configuration.Current.Pickable.edibles);
            foreach (var item in flowersAndIngredients)
                PickableYieldState.yieldModifier.Add(item, Configuration.Current.Pickable.flowersAndIngredients);
            foreach (var item in materials)
                PickableYieldState.yieldModifier.Add(item, Configuration.Current.Pickable.materials);
            foreach (var item in valuables)
                PickableYieldState.yieldModifier.Add(item, Configuration.Current.Pickable.valuables);
            foreach (var item in surtlingCores)
                PickableYieldState.yieldModifier.Add(item, Configuration.Current.Pickable.surtlingCores);
        }
    }
}
