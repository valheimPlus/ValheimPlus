using HarmonyLib;
using ValheimPlus.Configurations;

namespace ValheimPlus
{
    [HarmonyPatch(typeof(Humanoid), "GetCurrentWeapon")]
    public static class ModifyCurrentWeapon
    {
        private static ItemDrop.ItemData Postfix(ItemDrop.ItemData __weapon, ref Character __instance)
        {
            if (__weapon != null)
            {
                if (__weapon.m_shared.m_name == "Unarmed")
                {
                    // Set default value first
                    __weapon.m_shared.m_damages.m_blunt = __instance.GetSkillFactor(Skills.SkillType.Unarmed) * Configuration.Current.Player.GetDefault<float>(nameof(Configuration.Player.baseUnarmedDamage));
                    if (Configuration.Current.Player.IsEnabled)
                    {
                        __weapon.m_shared.m_damages.m_blunt = __instance.GetSkillFactor(Skills.SkillType.Unarmed) * Configuration.Current.Player.baseUnarmedDamage;
                    }
                }
            }

            return __weapon;
        }
    }
}
