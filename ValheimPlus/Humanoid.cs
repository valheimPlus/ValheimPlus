using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ValheimPlus
{
    [HarmonyPatch(typeof(Humanoid), "GetCurrentWeapon")]
    public static class ModifyCurrentWeapon
    {
        private static ItemDrop.ItemData Postfix(ItemDrop.ItemData __weapon, ref Character __instance)
        {
            if (Settings.isEnabled("UnarmedScaling"))
            {
                if (__weapon != null)
                {
                    if (__weapon.m_shared.m_name == "Unarmed")
                    {
                        __weapon.m_shared.m_damages.m_blunt = __instance.GetSkillFactor(Skills.SkillType.Unarmed) * Settings.getFloat("UnarmedScaling", "baseDamage");
                    }
                }
            }

            return __weapon;
        }
    }
}
