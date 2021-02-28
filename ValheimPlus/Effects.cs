using HarmonyLib;
using System;
using System.Collections.Generic;
using ValheimPlus.Configurations;

namespace ValheimPlus
{ 
    class Effects
    {
        const string EikthyrBuff = "$se_eikthyr_name";
        const string TheElderBuff = "$se_theelder_name";
        const string BonemassBuff = "$se_bonemass_name";
        const string YagluthBuff = "$se_yagluth_name";
        const string ModerBuff = "$se_moder_name";

        const string RestedBuff = "$se_rested_name";
        const string RestingBuff = "$se_resting_name";
        const string WarmBuff = "$se_warm_name";
        const string CorpseRunBuff = "$se_corpserun_name";

        const string FreezingDebuff = "$se_freezing_name";
        const string ColdDebuff = "$se_cold_name";
        const string WetDebuff = "$se_wet_name";

        [HarmonyPatch(typeof(ObjectDB), "Awake")]
        public static class ModifyEffects
        {
            private static void Postfix(ObjectDB __instance)
            {
                if (Configuration.Current.Effects.IsEnabled)
                {
                    ConfigureEffects(__instance);
                }                
            }
        }
        private static Skills.SkillType ParseAttackSkill(string attackSkill)
        {
            try
            {
                return (Skills.SkillType)Enum.Parse(typeof(Skills.SkillType), attackSkill.Trim(), true);
            }
            catch (ArgumentException)
            {
                return Skills.SkillType.None;
            }
        }

        private static HitData.DamageType ParseDamageType(string damageType)
        {
            try
            {
                return (HitData.DamageType)Enum.Parse(typeof(HitData.DamageType), damageType.Trim(), true);
            }
            catch (ArgumentException)
            {
                return HitData.DamageType.Physical;
            }
        }
        
        private static HitData.DamageModifier ParseDamageModifier(string damageModifier)
        {
            try
            {
                return (HitData.DamageModifier)Enum.Parse(typeof(HitData.DamageModifier), damageModifier.Trim(), true);
            }
            catch (ArgumentException)
            {
                return HitData.DamageModifier.Ignore;
            }
        }

        private static List<HitData.DamageModPair> ParseDamageTypesModifiers(string damageTypesModifiers)
        {
            string[] damageTypeModifiers;
            List <HitData.DamageModPair> damageTypesModifiersListing = new List<HitData.DamageModPair>();
            damageTypeModifiers = damageTypesModifiers.Split('|');
            foreach (string damageTypeModifier in damageTypeModifiers)
            {
                string[] damageTypeModifierConfiguration = damageTypeModifier.Split(':');
                if (damageTypeModifiers.Length == 2)
                {
                    HitData.DamageModPair damageModPair;
                    damageModPair.m_type = ParseDamageType(damageTypeModifierConfiguration[0]);
                    damageModPair.m_modifier = ParseDamageModifier(damageTypeModifierConfiguration[1]);
                    damageTypesModifiersListing.Add(damageModPair);
                }
            }
            return damageTypesModifiersListing;
        }

        private static void ConfigureBossBuff (
            SE_Stats currentStatus, 
            float healthRegenMultiplier, float staminaRegenMultiplier, float jumpStaminaModifier, float runStaminaModifier, float cooldown, 
            float duration, string damageTypesModifiers, string modifyAttackSkill, float damageModifier, string description
        )
        {
            currentStatus.m_healthRegenMultiplier = 1 + healthRegenMultiplier / 100;
            currentStatus.m_staminaRegenMultiplier = 1 + staminaRegenMultiplier / 100;
            currentStatus.m_jumpStaminaUseModifier = jumpStaminaModifier / 100;
            currentStatus.m_runStaminaDrainModifier = runStaminaModifier / 100;
            currentStatus.m_cooldown = cooldown;
            currentStatus.m_ttl = duration;
            currentStatus.m_mods = ParseDamageTypesModifiers(damageTypesModifiers);
            if (modifyAttackSkill.Length > 0)
            {
                currentStatus.m_modifyAttackSkill = ParseAttackSkill(modifyAttackSkill);
            }
            currentStatus.m_damageModifier = 1 + damageModifier / 100;
            if (description.Length > 0)
            {
                currentStatus.m_tooltip = description;
            }
        }

        private static void ConfigureEffects(ObjectDB __instance)
        {
            foreach (StatusEffect statusEffect in __instance.m_StatusEffects)
            {
                if (statusEffect is SE_Stats currentStatus)
                {
                    switch (statusEffect.m_name)
                    {
                        case EikthyrBuff:
                            ConfigureBossBuff(
                                currentStatus, 
                                Configuration.Current.Effects.eikthyrBuffHealthRegenMultiplier, 
                                Configuration.Current.Effects.eikthyrBuffStaminaRegenMultiplier,
                                Configuration.Current.Effects.eikthyrBuffJumpStaminaModifier,
                                Configuration.Current.Effects.eikthyrBuffRunStaminaModifier,
                                Configuration.Current.Effects.eikthyrBuffCooldown,
                                Configuration.Current.Effects.eikthyrBuffDuration,
                                Configuration.Current.Effects.eikthyrBuffDamageTypesModifiers,
                                Configuration.Current.Effects.eikthyrBuffModifyAttackSkill,
                                Configuration.Current.Effects.eikthyrBuffDamageModifier,
                                Configuration.Current.Effects.eikthyrBuffDescription
                            );
                            break;
                        case TheElderBuff:
                            ConfigureBossBuff(
                                currentStatus,
                                Configuration.Current.Effects.theElderBuffHealthRegenMultiplier,
                                Configuration.Current.Effects.theElderBuffStaminaRegenMultiplier,
                                Configuration.Current.Effects.theElderBuffJumpStaminaModifier,
                                Configuration.Current.Effects.theElderBuffRunStaminaModifier,
                                Configuration.Current.Effects.theElderBuffCooldown,
                                Configuration.Current.Effects.theElderBuffDuration,
                                Configuration.Current.Effects.theElderBuffDamageTypesModifiers,
                                Configuration.Current.Effects.theElderBuffModifyAttackSkill,
                                Configuration.Current.Effects.theElderBuffDamageModifier,
                                Configuration.Current.Effects.theElderBuffDescription
                            );
                            break;
                        case BonemassBuff:
                            ConfigureBossBuff(
                                currentStatus,
                                Configuration.Current.Effects.bonemassBuffHealthRegenMultiplier,
                                Configuration.Current.Effects.bonemassBuffStaminaRegenMultiplier,
                                Configuration.Current.Effects.bonemassBuffJumpStaminaModifier,
                                Configuration.Current.Effects.bonemassBuffRunStaminaModifier,
                                Configuration.Current.Effects.bonemassBuffCooldown,
                                Configuration.Current.Effects.bonemassBuffDuration,
                                Configuration.Current.Effects.bonemassBuffDamageTypesModifiers,
                                Configuration.Current.Effects.bonemassBuffModifyAttackSkill,
                                Configuration.Current.Effects.bonemassBuffDamageModifier,
                                Configuration.Current.Effects.bonemassBuffDescription
                            );
                            break;
                        case YagluthBuff:
                            ConfigureBossBuff(
                                currentStatus,
                                Configuration.Current.Effects.yagluthBuffHealthRegenMultiplier,
                                Configuration.Current.Effects.yagluthBuffStaminaRegenMultiplier,
                                Configuration.Current.Effects.yagluthBuffJumpStaminaModifier,
                                Configuration.Current.Effects.yagluthBuffRunStaminaModifier,
                                Configuration.Current.Effects.yagluthBuffCooldown,
                                Configuration.Current.Effects.yagluthBuffDuration,
                                Configuration.Current.Effects.yagluthBuffDamageTypesModifiers,
                                Configuration.Current.Effects.yagluthBuffModifyAttackSkill,
                                Configuration.Current.Effects.yagluthBuffDamageModifier,
                                Configuration.Current.Effects.yagluthBuffDescription
                            );
                            break;
                        case ModerBuff:
                            ConfigureBossBuff(
                                currentStatus, 
                                Configuration.Current.Effects.moderBuffHealthRegenMultiplier,
                                Configuration.Current.Effects.moderBuffStaminaRegenMultiplier,
                                Configuration.Current.Effects.moderBuffJumpStaminaModifier,
                                Configuration.Current.Effects.moderBuffRunStaminaModifier,
                                Configuration.Current.Effects.moderBuffCooldown,
                                Configuration.Current.Effects.moderBuffDuration,
                                Configuration.Current.Effects.moderBuffDamageTypesModifiers,
                                Configuration.Current.Effects.moderBuffModifyAttackSkill,
                                Configuration.Current.Effects.moderBuffDamageModifier,
                                Configuration.Current.Effects.moderBuffDescription
                            );
                            break;
                        case RestedBuff:
                            currentStatus.m_healthRegenMultiplier = 1 + Configuration.Current.Effects.restedBuffHealthRegenMultiplier / 100;
                            currentStatus.m_staminaRegenMultiplier = 1 + Configuration.Current.Effects.restedBuffStaminaRegenMultiplier / 100;
                            currentStatus.m_jumpStaminaUseModifier = Configuration.Current.Effects.restedBuffJumpStaminaModifier / 100;
                            currentStatus.m_runStaminaDrainModifier = Configuration.Current.Effects.restedBuffRunStaminaModifier / 100;
                            break;

                        case RestingBuff:
                            currentStatus.m_healthRegenMultiplier = 1 + Configuration.Current.Effects.restingBuffHealthRegenMultiplier / 100;
                            currentStatus.m_staminaRegenMultiplier = 1 + Configuration.Current.Effects.restingBuffStaminaRegenMultiplier / 100;
                            break;

                        case WarmBuff:
                            currentStatus.m_healthRegenMultiplier = 1 + Configuration.Current.Effects.warmBuffBuffHealthRegenMultiplier / 100;
                            currentStatus.m_staminaRegenMultiplier = 1 + Configuration.Current.Effects.warmBuffBuffStaminaRegenMultiplier / 100;
                            break;

                        case CorpseRunBuff:
                            currentStatus.m_healthRegenMultiplier = 1 + Configuration.Current.Effects.corpseRunBuffHealthRegenMultiplier / 100;
                            currentStatus.m_staminaRegenMultiplier = 1 + Configuration.Current.Effects.corpseRunBuffStaminaRegenMultiplier / 100;
                            currentStatus.m_jumpStaminaUseModifier = Configuration.Current.Effects.corpseRunBuffJumpStaminaModifier / 100;
                            currentStatus.m_runStaminaDrainModifier = Configuration.Current.Effects.corpseRunBuffRunStaminaModifier / 100;
                            break;

                        case FreezingDebuff:
                            currentStatus.m_healthRegenMultiplier = 1 + Configuration.Current.Effects.freezingDebuffHealthRegenMultiplier / 100;
                            currentStatus.m_staminaRegenMultiplier = 1 + Configuration.Current.Effects.freezingDebuffStaminaRegenMultiplier / 100;
                            currentStatus.m_healthPerTick = Configuration.Current.Effects.freezingDebuffDamagePerTick;
                            break;

                        case ColdDebuff:
                            currentStatus.m_healthRegenMultiplier = Configuration.Current.Effects.coldDebuffHealthRegenMultiplier / 100;
                            currentStatus.m_staminaRegenMultiplier = Configuration.Current.Effects.coldDebuffStaminaRegenMultiplier / 100;
                            break;

                        case WetDebuff:
                            currentStatus.m_healthRegenMultiplier = Configuration.Current.Effects.wetDebuffHealthRegenMultiplier / 100;
                            currentStatus.m_staminaRegenMultiplier = Configuration.Current.Effects.wetDebuffStaminaRegenMultiplier / 100;
                            currentStatus.m_ttl = Configuration.Current.Effects.wetDebuffDuration;
                            break;
                    }
                }
            }
        }
    }
}