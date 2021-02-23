﻿using HarmonyLib;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using ValheimPlus.Configurations;

namespace ValheimPlus
{
    [HarmonyPatch(typeof(Humanoid), "GetCurrentWeapon")]
    public static class ModifyCurrentWeapon
    {
        private static ItemDrop.ItemData Postfix(ItemDrop.ItemData __weapon, ref Character __instance)
        {
            if (Configuration.Current.Unarmed.IsEnabled)
            {
                if (__weapon != null)
                {
                    if (__weapon.m_shared.m_name == "Unarmed")
                    { 
                        __weapon.m_shared.m_damages.m_blunt = __instance.GetSkillFactor(Skills.SkillType.Unarmed) * Configuration.Current.Unarmed.scaleDamage;
                    }
                }
            }

            return __weapon;
        }
    }

    [HarmonyPatch(typeof(Humanoid), MethodType.Constructor)]
    public static class ModifyPlayerInventorySize
    {
        private static void Postfix(ref Humanoid __instance)
        {
            if (Configuration.Current.Inventory.IsEnabled)
            {
                __instance.m_inventory = new Inventory("Inventory", (Sprite)null, Configuration.Current.Inventory.width, Configuration.Current.Inventory.height);
            }
        }
    }
}
