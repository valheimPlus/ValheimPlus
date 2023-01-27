using HarmonyLib;
using UnityEngine;
using System.Collections.Generic;
using ValheimPlus.Configurations;
using System;

namespace ValheimPlus
{
    /// <summary>
    /// Modify drops to increase their amount and chances.
    /// </summary>
    [HarmonyPatch(typeof(CharacterDrop), nameof(CharacterDrop.GenerateDropList))]
    public static class CharacterDrop_GenerateDropList_Patch
    {
        private static bool Prefix(ref List<CharacterDrop.Drop> ___m_drops, ref List<CharacterDrop.Drop> __state)
        {
            if (Configuration.Current.LootDrop.IsEnabled)
            {
                __state = ___m_drops;
                ___m_drops = ___m_drops.ConvertAll(originalDrop => {
                    var newDrop = new CharacterDrop.Drop
                    {
                        m_prefab = originalDrop.m_prefab,
                        m_amountMin = (int)Helper.applyModifierValue(originalDrop.m_amountMin, Configuration.Current.LootDrop.lootDropAmountMultiplier),
                        m_amountMax = (int)Helper.applyModifierValue(originalDrop.m_amountMax, Configuration.Current.LootDrop.lootDropAmountMultiplier),
                        m_chance = Helper.applyModifierValue(originalDrop.m_chance, Configuration.Current.LootDrop.lootDropChanceMultiplier),
                        m_onePerPlayer = originalDrop.m_onePerPlayer,
                        m_levelMultiplier = originalDrop.m_levelMultiplier
                    };
                    return newDrop;
                });
            }
            return true;
        }

        private static void Postfix(ref List<CharacterDrop.Drop> ___m_drops, List<CharacterDrop.Drop> __state)
        {
            if (Configuration.Current.LootDrop.IsEnabled)
            {
                ___m_drops = __state;
            }
        }
    }
}