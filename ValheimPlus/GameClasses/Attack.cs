using HarmonyLib;
using ValheimPlus.Configurations;

namespace ValheimPlus
{
    /// <summary>
    /// Alters stamina of weapons
    /// </summary>
    [HarmonyPatch(typeof(Attack), "GetStaminaUsage")]
    public static class Attack_GetStaminaUsage_Patch
    {
        private static void Postfix(ref Attack __instance, ref float __result)
        {
            if (Configuration.Current.StaminaUsage.IsEnabled)
            {
                if (__instance.m_character.IsPlayer())
                {
                    ItemDrop.ItemData item = __instance.m_character.GetCurrentWeapon();
                    Skills.SkillType skillType;
                    if (item == null)
                    {
                        skillType = Skills.SkillType.Unarmed;
                    }
                    else
                    {
                        skillType = item.m_shared.m_skillType;
                    }

                    switch (skillType)
                    {
                        case Skills.SkillType.Swords:
                            __result = Helper.applyModifierValue(__result, Configuration.Current.StaminaUsage.swords);
                            break;
                        case Skills.SkillType.Knives:
                            __result = Helper.applyModifierValue(__result, Configuration.Current.StaminaUsage.knives);
                            break;
                        case Skills.SkillType.Clubs:
                            __result = Helper.applyModifierValue(__result, Configuration.Current.StaminaUsage.clubs);
                            break;
                        case Skills.SkillType.Polearms:
                            __result = Helper.applyModifierValue(__result, Configuration.Current.StaminaUsage.polearms);
                            break;
                        case Skills.SkillType.Spears:
                            __result = Helper.applyModifierValue(__result, Configuration.Current.StaminaUsage.spears);
                            break;
                        case Skills.SkillType.Axes:
                            __result = Helper.applyModifierValue(__result, Configuration.Current.StaminaUsage.axes);
                            break;
                        case Skills.SkillType.Unarmed:
                            __result = Helper.applyModifierValue(__result, Configuration.Current.StaminaUsage.unarmed);
                            break;
                        case Skills.SkillType.Pickaxes:
                            __result = Helper.applyModifierValue(__result, Configuration.Current.StaminaUsage.pickaxes);
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }

    /// <summary>
    /// Alter projectile velocity and accuracy without affecting damage
    /// </summary>
    [HarmonyPatch(typeof(Attack), "ProjectileAttackTriggered")]
    public static class Attack_ProjectileAttackTriggered
    {
        private const float DEFAULT_PROJECTILE_VEL_MIN_CHARGE = 2f;
        private const float DEFAULT_PROJECTILE_VEL_MAX_CHARGE = 50f;

        private const float DEFAULT_PROJECTILE_VARIANCE_MIN_CHARGE = 20f;
        private const float DEFAULT_PROJECTILE_VARIANCE_MAX_CHARGE = 1f;

        private const float MAX_VALUE = 1e+6f;

        private static void Prefix(ref Attack __instance)
        {
            if (Configuration.Current.ProjectileFired.IsEnabled)
            {
                if (__instance.m_character is Player)
                {
                    __instance.m_projectileVelMin = Helper.applyModifierValue(DEFAULT_PROJECTILE_VEL_MIN_CHARGE, Configuration.Current.ProjectileFired.playerProjectileVelMinCharge);
                    __instance.m_projectileVel = Helper.applyModifierValue(DEFAULT_PROJECTILE_VEL_MAX_CHARGE, Configuration.Current.ProjectileFired.playerProjectileVelMaxCharge);

                    __instance.m_projectileAccuracyMin = Helper.applyModifierValue(DEFAULT_PROJECTILE_VARIANCE_MIN_CHARGE, Configuration.Current.ProjectileFired.playerProjectileVarMinCharge);
                    __instance.m_projectileAccuracy = Helper.applyModifierValue(DEFAULT_PROJECTILE_VARIANCE_MAX_CHARGE, Configuration.Current.ProjectileFired.playerProjectileVarMaxCharge);
                }
                else
                {
                    __instance.m_projectileVelMin = Helper.applyModifierValue(DEFAULT_PROJECTILE_VEL_MIN_CHARGE, Configuration.Current.ProjectileFired.projectileVelMinCharge);
                    __instance.m_projectileVel = Helper.applyModifierValue(DEFAULT_PROJECTILE_VEL_MAX_CHARGE, Configuration.Current.ProjectileFired.projectileVelMaxCharge);

                    __instance.m_projectileAccuracyMin = Helper.applyModifierValue(DEFAULT_PROJECTILE_VARIANCE_MIN_CHARGE, Configuration.Current.ProjectileFired.projectileVarMinCharge);
                    __instance.m_projectileAccuracy = Helper.applyModifierValue(DEFAULT_PROJECTILE_VARIANCE_MAX_CHARGE, Configuration.Current.ProjectileFired.projectileVarMaxCharge);
                }

                __instance.m_projectileVelMin = Mathf.Clamp(__instance.m_projectileVelMin, 0f, MAX_VALUE);
                __instance.m_projectileVel = Mathf.Clamp(__instance.m_projectileVel, 0f, MAX_VALUE);

                __instance.m_projectileAccuracyMin = Mathf.Clamp(__instance.m_projectileAccuracyMin, 0f, MAX_VALUE);
                __instance.m_projectileAccuracy = Mathf.Clamp(__instance.m_projectileAccuracy, 0f, MAX_VALUE);
            }
        }
    }
}
