using HarmonyLib;
using ValheimPlus.Configurations;
using UnityEngine;

namespace ValheimPlus
{
    [HarmonyPatch(typeof(Humanoid), "GetCurrentWeapon")]
    public static class ModifyCurrentWeapon
    {
        private static ItemDrop.ItemData Postfix(ItemDrop.ItemData __weapon, ref Character __instance)
        {
            if (Configuration.Current.Player.IsEnabled)
            {
                if (__weapon != null)
                {
                    if (__weapon.m_shared.m_name == "Unarmed")
                    {
                        Player CharacterPlayerInstance = (Player)__instance;
                        __weapon.m_shared.m_damages.m_blunt = CharacterPlayerInstance.GetSkillFactor(Skills.SkillType.Unarmed) * Configuration.Current.Player.baseUnarmedDamage;
                    }
                }
            }

            return __weapon;
        }
    }
}
