using HarmonyLib;
using System;
using System.Collections.Generic;
using UnityEngine;
using ValheimPlus.Configurations;
using ValheimPlus.Configurations.Sections;

namespace ValheimPlus
{
    class Effects
    {
        const string Eikthyr = "$se_eikthyr_name";
        const string TheElder = "$se_theelder_name";
        const string Bonemass = "$se_bonemass_name";
        const string Yagluth = "$se_yagluth_name";
        const string Moder = "$se_moder_name";

        const string Cold = "$se_cold_name";
        const string CorpseRun = "$se_corpserun_name";
        const string Freezing = "$se_freezing_name";
        const string Rested = "$se_rested_name";
        const string Resting = "$se_resting_name";
        const string Warm = "$se_warm_name";
        const string Wet = "$se_wet_name";

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
            List<HitData.DamageModPair> damageTypesModifiersListing = new List<HitData.DamageModPair>();
            damageTypeModifiers = damageTypesModifiers.Split('|');
            foreach (string damageTypeModifier in damageTypeModifiers)
            {
                string[] damageTypeModifierConfiguration = damageTypeModifier.Split(':');
                if (damageTypeModifierConfiguration.Length == 2)
                {
                    HitData.DamageModPair damageModPair;
                    damageModPair.m_type = ParseDamageType(damageTypeModifierConfiguration[0]);
                    damageModPair.m_modifier = ParseDamageModifier(damageTypeModifierConfiguration[1]);
                    damageTypesModifiersListing.Add(damageModPair);
                }
            }
            return damageTypesModifiersListing;
        }
        private static void ConfigureEffect(SE_Stats currentStatus, EffectsConfigurationItem configuration)
        {
            if (!float.IsNaN(configuration.cooldown))
            {
                currentStatus.m_cooldown = configuration.cooldown;
            }
            if (!float.IsNaN(configuration.damageModifier))
            {
                currentStatus.m_damageModifier = 1 + configuration.damageModifier / 100;
            }
            if (!float.IsNaN(configuration.duration))
            {
                currentStatus.m_ttl = configuration.duration;
            }
            if (!float.IsNaN(configuration.healthPerTick))
            {
                currentStatus.m_healthPerTick = configuration.healthPerTick;
            }
            if (!float.IsNaN(configuration.healthRegenMultiplier))
            {
                currentStatus.m_healthRegenMultiplier = 1 + configuration.healthRegenMultiplier / 100;
            }
            if (!float.IsNaN(configuration.jumpStaminaModifier))
            {
                currentStatus.m_jumpStaminaUseModifier = configuration.jumpStaminaModifier / 100;
            }
            if (!float.IsNaN(configuration.runStaminaModifier))
            {
                currentStatus.m_runStaminaDrainModifier = configuration.runStaminaModifier / 100;
            }
            if (!float.IsNaN(configuration.staminaRegenMultiplier))
            {
                currentStatus.m_staminaRegenMultiplier = 1 + configuration.staminaRegenMultiplier / 100;
            }
            if (!string.IsNullOrEmpty(configuration.damageTypesModifiers))
            {
                currentStatus.m_mods = ParseDamageTypesModifiers(configuration.damageTypesModifiers);
            }
            if (!string.IsNullOrEmpty(configuration.description))
            {
                currentStatus.m_tooltip = configuration.description;
            }
            if (!string.IsNullOrEmpty(configuration.modifyAttackSkill))
            {
                currentStatus.m_modifyAttackSkill = ParseAttackSkill(configuration.modifyAttackSkill);
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
                        case Eikthyr:
                            ConfigureEffect(currentStatus, Configuration.Current.Effects.eikthyr);
                            break;
                        case TheElder:
                            ConfigureEffect(currentStatus, Configuration.Current.Effects.theElder);
                            break;
                        case Bonemass:
                            ConfigureEffect(currentStatus, Configuration.Current.Effects.bonemass);
                            break;
                        case Yagluth:
                            ConfigureEffect(currentStatus, Configuration.Current.Effects.yagluth);
                            break;
                        case Moder:
                            ConfigureEffect(currentStatus, Configuration.Current.Effects.moder);
                            break;


                        case Cold:
                            ConfigureEffect(currentStatus, Configuration.Current.Effects.cold);
                            break;
                        case CorpseRun:
                            ConfigureEffect(currentStatus, Configuration.Current.Effects.corpseRun);
                            break;
                        case Freezing:
                            ConfigureEffect(currentStatus, Configuration.Current.Effects.freezing);
                            break;
                        case Rested:
                            ConfigureEffect(currentStatus, Configuration.Current.Effects.rested);
                            break;
                        case Resting:
                            ConfigureEffect(currentStatus, Configuration.Current.Effects.resting);
                            break;
                        case Warm:
                            ConfigureEffect(currentStatus, Configuration.Current.Effects.warm);
                            break;
                        case Wet:
                            ConfigureEffect(currentStatus, Configuration.Current.Effects.wet);
                            break;
                    }
                }
            }
        }
    }
}