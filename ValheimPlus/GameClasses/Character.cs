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
    /// <summary>
    /// Determines what happens when a tamed creature takes damage.
    /// </summary>
    [HarmonyPatch(typeof(Character), nameof(Character.Damage))]
    public static class Character_Damage_Patch
    {
        public static void Prefix(ref Character __instance, ref HitData hit)
        {
            if (Configuration.Current.Tameable.IsEnabled)
            {
                // if immortal
                if (isMortality(TameableMortalityTypes.Immortal))
                {
                    // Network & Tameable component
                    ZDO zdo = __instance.m_nview.GetZDO();
                    Tameable tamed = __instance.GetComponent<Tameable>();

                    // Is tamed, has network, has valid hit data, tamed component is present.
                    if (!__instance.IsTamed() || zdo == null || hit == null || tamed == null)
                        return;

                    // Check if it should ignore the hit damage (includes stunned status check)
                    if (ShouldIgnoreDamage(__instance, hit, zdo))
                        hit = new HitData();
                }


            }
        }

        public static void Postfix(ref Character __instance, ref HitData hit)
        {
            if (Configuration.Current.Tameable.IsEnabled)
            {
                // if essential
                if (isMortality(TameableMortalityTypes.Essential))
                {
                    // Network & Tameable component
                    ZDO zdo = __instance.m_nview.GetZDO();
                    Tameable tamed = __instance.GetComponent<Tameable>();

                    // Is tamed, has network, has valid hit data, tamed component is present.
                    if (!__instance.IsTamed() || zdo == null || hit == null || tamed == null)
                        return;

                    // if killed on this hit
                    if (__instance.GetHealth() <= 5f)
                    {
                        // Allow players to kill the tamed creature with ownerDamageOverride
                        if(ShouldIgnoreDamage(__instance, hit, zdo)){
                            __instance.SetHealth(__instance.GetMaxHealth());
                            __instance.m_animator.SetBool("sleeping", true);
                            zdo.Set("sleeping", true);
                            zdo.Set("isRecoveringFromStun", true);
                        }
                    }
                }

            }
        }


        private static bool isMortality(TameableMortalityTypes type)
        {
            TameableMortalityTypes setting = (TameableMortalityTypes)Mathf.Clamp(Configuration.Current.Tameable.mortality, 0, 2);
            if (setting == type)
            {
                return true;
            }
            return false;
        }

        private static bool ShouldIgnoreDamage(Character __instance, HitData hit, ZDO zdo)
        {
            // The only valid attack from a player is with a butcher knife from a player.
            if (Configuration.Current.Tameable.ownerDamageOverride)
            {
                Character attacker = hit.GetAttacker();
                // Attacker is player
                if (attacker == __instance.GetComponent<Tameable>().GetPlayer(attacker.GetZDOID()))
                    return false;
            }

            return true;
        }



    }

    /// <summary>
    /// Allow tweaking of fall damage
    /// </summary>
    [HarmonyPatch(typeof(Character), nameof(Character.UpdateGroundContact))]
    public static class Character_UpdateGroundContact_Transpiler
    {
        private static readonly MethodInfo method_calculateFallDamage = AccessTools.Method(typeof(Character_UpdateGroundContact_Transpiler), nameof(calculateFallDamage));

        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            if (!Configuration.Current.Player.IsEnabled)
                return instructions;

            List<CodeInstruction> il = instructions.ToList();

            bool found = false;

            for (int i = 0; i < il.Count; i++)
            {
                if (!found && il[i].opcode != OpCodes.Newobj)
                {
                    continue;
                }
                else if (il[i].opcode == OpCodes.Newobj)
                {
                    found = true;
                    continue;
                }
                if (il[i].opcode == OpCodes.Ldloc_2)
                {
                    il[i].opcode = OpCodes.Ldloc_0;
                    il.Insert(i + 1, new CodeInstruction(OpCodes.Call, method_calculateFallDamage));
                }
                else continue;

                return il.AsEnumerable();
            }

            ZLog.Log("Unable to transpile Character::UpdateGroundContact to patch fall damage calculation");
            return instructions;
        }

        private static float calculateFallDamage(float fallDistance)
        {
            if (fallDistance < 4f)
                return 0f;

            float linearFallDamage = ((fallDistance - 4f) / 16f) * 100f;
            float scaledFallDamage = Helper.applyModifierValue(linearFallDamage, Configuration.Current.Player.fallDamageScalePercent);
            float fallDamage = Math.Min(scaledFallDamage, Configuration.Current.Player.maxFallDamage);

            return fallDamage;
        }
    }
}
