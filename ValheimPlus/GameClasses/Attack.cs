using HarmonyLib;
using UnityEngine;
using ValheimPlus.Configurations;

namespace ValheimPlus.GameClasses
{
    /// <summary>
    /// Alters stamina of weapons
    /// </summary>
    [HarmonyPatch(typeof(Attack), "GetAttackStamina")]
    public static class Attack_GetAttackStamina_Patch
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
                        case Skills.SkillType.Bows:
                            __result = Helper.applyModifierValue(__result, Configuration.Current.StaminaUsage.bows);
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
    public static class Attack_ProjectileAttackTriggered_Patch
    {
        private static float maxClampValue = 1E+6f;

        private static void Prefix(ref Attack __instance)
        {
            if (Configuration.Current.PlayerProjectile.IsEnabled && __instance.m_character.IsPlayer())
            {
                // This might not be required but i wanted to add this non the less to be sure that no missing properties are causing issues.
                if (__instance?.m_projectileVel == null || __instance?.m_projectileAccuracy == null || __instance?.m_projectileVelMin == null || __instance?.m_projectileAccuracyMin == null)
                    return;

                float playerProjVelMinMod = Helper.applyModifierValue(__instance.m_projectileVelMin, Configuration.Current.PlayerProjectile.playerMinChargeVelocityMultiplier);
                float playerProjVelMaxMod = Helper.applyModifierValue(__instance.m_projectileVel, Configuration.Current.PlayerProjectile.playerMaxChargeVelocityMultiplier);

                // negate value to handle increasing accuracy means decreasing variance
                float playerProjAccuMinMod = Helper.applyModifierValue(__instance.m_projectileAccuracyMin, -Configuration.Current.PlayerProjectile.playerMinChargeAccuracyMultiplier);
                float playerProjAccuMaxMod = Helper.applyModifierValue(__instance.m_projectileAccuracy, -Configuration.Current.PlayerProjectile.playerMaxChargeAccuracyMultiplier);

                if (Configuration.Current.PlayerProjectile.enableScaleWithSkillLevel)
                {

                    Skills.SkillType skillType = __instance.m_weapon.m_shared.m_skillType;
                    if (skillType != Skills.SkillType.None) // https://github.com/valheimPlus/ValheimPlus/issues/758
                    {
                        Player player = (Player)__instance.m_character;
                        Skills.Skill skill = player.m_skills.GetSkill(skillType);
                        float maxLevelPercentage = skill.m_level * 0.01f;

                        __instance.m_projectileVelMin = Mathf.Lerp(__instance.m_projectileVelMin, playerProjVelMinMod, maxLevelPercentage);
                        __instance.m_projectileVel = Mathf.Lerp(__instance.m_projectileVel, playerProjVelMaxMod, maxLevelPercentage);

                        __instance.m_projectileAccuracyMin = Mathf.Lerp(__instance.m_projectileAccuracyMin, playerProjAccuMinMod, maxLevelPercentage);
                        __instance.m_projectileAccuracy = Mathf.Lerp(__instance.m_projectileAccuracy, playerProjAccuMaxMod, maxLevelPercentage);
                    }
                }
                else
                {
                    __instance.m_projectileVelMin = playerProjVelMinMod;
                    __instance.m_projectileVel = playerProjVelMaxMod;

                    __instance.m_projectileAccuracyMin = playerProjAccuMinMod;
                    __instance.m_projectileAccuracy = playerProjAccuMaxMod;
                }
            }

            if (Configuration.Current.MonsterProjectile.IsEnabled && !__instance.m_character.IsPlayer())
            {

                // This might not be required but i wanted to add this non the less to be sure that no missing properties are causing issues.
                if (__instance?.m_projectileVel == null || __instance?.m_projectileAccuracy == null || __instance?.m_projectileVelMin == null || __instance?.m_projectileAccuracyMin == null)
                    return;

                __instance.m_projectileVel = Helper.applyModifierValue(__instance.m_projectileVel, Configuration.Current.MonsterProjectile.monsterMaxChargeVelocityMultiplier);

                // negate value to handle increasing accuracy means decreasing variance
                __instance.m_projectileAccuracy = Helper.applyModifierValue(__instance.m_projectileAccuracy, -Configuration.Current.MonsterProjectile.monsterMaxChargeAccuracyMultiplier);

                __instance.m_projectileVelMin = Mathf.Clamp(__instance.m_projectileVelMin, 0f, maxClampValue);
                __instance.m_projectileVel = Mathf.Clamp(__instance.m_projectileVel, 0f, maxClampValue);

                __instance.m_projectileAccuracyMin = Mathf.Clamp(__instance.m_projectileAccuracyMin, 0f, maxClampValue);
                __instance.m_projectileAccuracy = Mathf.Clamp(__instance.m_projectileAccuracy, 0f, maxClampValue);
            }

            
        }
    }
}