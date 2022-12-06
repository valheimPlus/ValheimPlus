using HarmonyLib;
using System;
using UnityEngine;
using ValheimPlus.Configurations;

namespace ValheimPlus.GameClasses
{
    // Modify length of in multiplayer and singleplayer casted guradian powers including around the player.
    [HarmonyPatch(typeof(SEMan), nameof(SEMan.AddStatusEffect), new Type[] { typeof(StatusEffect), typeof(bool), typeof(int), typeof(float) })]
    public static class SEMan_AddStatusEffect_Patch
    {
        private static void Postfix(ref SEMan __instance, ref StatusEffect statusEffect, bool resetTime = false, int itemLevel = 0, float skillLevel = 0)
        {

            if (!Configuration.Current.Player.IsEnabled )
                return;

            // Don't execute if the affected person is not the player
            if (__instance.m_character.IsPlayer())
                return;

            // Every guardian power starts with GP_
            if (statusEffect.name.StartsWith("GP_"))
            {
                foreach (StatusEffect buff in __instance.m_statusEffects)
                {
                    if (buff.m_name == __instance.GetStatusEffect(statusEffect.name).m_name)
                    {
                        Player fromCharacter = (Player)__instance.m_character;
                        if (fromCharacter.m_guardianSE)
                        {
                            fromCharacter.m_guardianSE.m_ttl = Configuration.Current.Player.guardianBuffDuration;
                        }
                    }
                }
            }
        }
    }
}
