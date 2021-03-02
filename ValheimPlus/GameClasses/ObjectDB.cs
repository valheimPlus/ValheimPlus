using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using ValheimPlus.Configurations;
using ValheimPlus.Configurations.Sections;

namespace ValheimPlus
{
    class Effects
    {
        private static Dictionary<string, string> effectsDictionary = new Dictionary<string, string>()
        {
            { "$se_eikthyr_name", "eikthyr" },
            { "$se_theelder_name", "theElder" },
            { "$se_bonemass_name", "bonemass" },
            { "$se_yagluth_name", "yagluth" },
            { "$se_moder_name", "moder" },

            { "$se_cold_name", "cold" },
            { "$se_corpserun_name", "corpseRun" },
            { "$se_freezing_name", "freezing" },
            { "$se_rested_name", "rested" },
            { "$se_resting_name", "resting" },
            { "$se_warm_name", "warm" },
            { "$se_wet_name", "wet" },

            { "$item_mead_frostres", "frostResistanceMead" },
            { "$item_mead_poisonres", "poisonResistanceMead" },
            { "$se_trollseteffect_name", "trollSetBonus" },
            { "$se_frostres_name", "wolfItemBonus" }

            // Items acting on other component for now let's just not enable them for now
            // { "$se_mead_name", "tastyMead" }, 
            // { "$item_mead_hp_medium", "mediumHealingMead" },
            // { "$item_mead_hp_minor", "minorHealingMead" },
            // { "$item_mead_stamina_medium", "mediumStaminaMead" },
            // { "$item_mead_stamina_minor", "minorStaminaMead" },
        };

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

        private static void DisplayEffectStats(SE_Stats currentStatus, bool isBefore)
        {
            if (!Configuration.Current.Effects.debug)
            {
                return;
            }

            Debug.Log(string.Concat(Enumerable.Repeat("=", 86)));
            if (isBefore)
            {
                Debug.Log(String.Format("{0,-84} |", $"Effect: {effectsDictionary[currentStatus.m_name]}"));
            }
            else
            {
                Debug.Log(String.Format("{0,-84} |", $"After patching effect: {effectsDictionary[currentStatus.m_name]}"));
            }
            
            Debug.Log(string.Concat(Enumerable.Repeat(string.Concat(Enumerable.Repeat("-", 27)) + "|-", 3)));
            Debug.Log(String.Format("{0,-26} | ", "Attribute") + String.Format("{0,-26} | ", "Value") + String.Format("{0,-26} |", "Units"));
            Debug.Log(string.Concat(Enumerable.Repeat(string.Concat(Enumerable.Repeat("-", 27)) + "|-", 3)));
            Debug.Log(String.Format("{0,-26} | ", "cooldown") + String.Format("{0,-26} | ", currentStatus.m_cooldown) + String.Format("{0,-26} |", "Seconds"));
            Debug.Log(String.Format("{0,-26} | ", "damageModifier") + String.Format("{0,-26} | ", (currentStatus.m_damageModifier - 1) * 100) + String.Format("{0,-26} |", "Percentage"));
            Debug.Log(String.Format("{0,-26} | ", "duration") + String.Format("{0,-26} | ", currentStatus.m_ttl) + String.Format("{0,-26} |", "Seconds"));
            Debug.Log(String.Format("{0,-26} | ", "healthPerTick") + String.Format("{0,-26} | ", currentStatus.m_healthPerTick) + String.Format("{0,-26} |", "HP Per Tick"));
            Debug.Log(String.Format("{0,-26} | ", "healthRegenMultiplier") + String.Format("{0,-26} | ", (currentStatus.m_healthRegenMultiplier - 1) * 100) + String.Format("{0,-26} |", "Percentage"));
            Debug.Log(String.Format("{0,-26} | ", "jumpStaminaModifier") + String.Format("{0,-26} | ", currentStatus.m_jumpStaminaUseModifier * 100) + String.Format("{0,-26} |", "Percentage"));
            Debug.Log(String.Format("{0,-26} | ", "runStaminaModifier") + String.Format("{0,-26} | ", currentStatus.m_runStaminaDrainModifier * 100) + String.Format("{0,-26} |", "Percentage"));
            Debug.Log(String.Format("{0,-26} | ", "staminaRegenMultiplier") + String.Format("{0,-26} | ", (currentStatus.m_staminaRegenMultiplier - 1) * 100) + String.Format("{0,-26} |", "Percentage"));
            Debug.Log(String.Format("{0,-26} | ", "stealthModifier") + String.Format("{0,-26} | ", currentStatus.m_stealthModifier * -100) + String.Format("{0,-26} |", "Percentage"));
            Debug.Log(String.Format("{0,-26} | ", "damageTypesModifiers") + String.Format("{0,-26} | ", currentStatus.m_mods.Count) + String.Format("{0,-26} |", "Pairs"));
            foreach (HitData.DamageModPair mod in currentStatus.m_mods)
            {
                Debug.Log(String.Format("{0,-26} | ", $" > {mod.m_type}") + String.Format("{0,-26} | ", mod.m_modifier) + String.Format("{0,-26} |", "Combination"));
            }
            Debug.Log(String.Format("{0,-26} | ", "description") + String.Format("{0,-26} | ", currentStatus.m_tooltip.Trim()) + String.Format("{0,-26} |", "Text"));
            Debug.Log(String.Format("{0,-26} | ", "modifyAttackSkill") + String.Format("{0,-26} | ", currentStatus.m_modifyAttackSkill) + String.Format("{0,-26} |", "Attack Skill"));
            Debug.Log(string.Concat(Enumerable.Repeat("=", 86)));
        }

        private static void ConfigureEffect(SE_Stats currentStatus, EffectsConfigurationItem configuration)
        {
            //DisplayEffectStats(currentStatus, true);
           
            bool isModified = false;
            if (!float.IsNaN(configuration.cooldown))
            {
                currentStatus.m_cooldown = configuration.cooldown;
                isModified = true;
            }
            if (!float.IsNaN(configuration.damageModifier))
            {
                currentStatus.m_damageModifier = 1 + configuration.damageModifier / 100;
                isModified = true;
            }
            if (!float.IsNaN(configuration.duration))
            {
                currentStatus.m_ttl = configuration.duration;
                isModified = true;
            }
            if (!float.IsNaN(configuration.healthPerTick))
            {
                currentStatus.m_healthPerTick = configuration.healthPerTick;
                isModified = true;
            }
            if (!float.IsNaN(configuration.healthRegenModifier))
            {
                currentStatus.m_healthRegenMultiplier = 1 + configuration.healthRegenModifier / 100;
                isModified = true;
            }
            if (!float.IsNaN(configuration.jumpStaminaModifier))
            {
                currentStatus.m_jumpStaminaUseModifier = configuration.jumpStaminaModifier / 100;
                isModified = true;
            }
            if (!float.IsNaN(configuration.runStaminaModifier))
            {
                currentStatus.m_runStaminaDrainModifier = configuration.runStaminaModifier / 100;
                isModified = true;
            }
            if (!float.IsNaN(configuration.staminaRegenModifier))
            {
                currentStatus.m_staminaRegenMultiplier = 1 + configuration.staminaRegenModifier / 100;
                isModified = true;
            }
            if (!float.IsNaN(configuration.stealthModifier))
            {
                currentStatus.m_stealthModifier = (configuration.stealthModifier / 100) * -1;
                isModified = true;
            }
            if (!string.IsNullOrEmpty(configuration.damageTypesModifiers))
            {
                currentStatus.m_mods = ParseDamageTypesModifiers(configuration.damageTypesModifiers);
                isModified = true;
            }
            if (!string.IsNullOrEmpty(configuration.description.Trim()))
            {
                currentStatus.m_tooltip = configuration.description.Trim();
                isModified = true;
            }
            if (!string.IsNullOrEmpty(configuration.modifyAttackSkill))
            {
                currentStatus.m_modifyAttackSkill = ParseAttackSkill(configuration.modifyAttackSkill);
                isModified = true;
            }
            
            if (isModified)
            {
                DisplayEffectStats(currentStatus, false);
            }
        }

        private static void ConfigureEffects(ObjectDB __instance)
        {
            foreach (StatusEffect statusEffect in __instance.m_StatusEffects)
            {
                if (statusEffect is SE_Stats currentStatus)
                {
                    DisplayEffectStats(currentStatus, true);
                    PropertyInfo[] configurationsPropertyInfo = typeof(EffectsConfiguration).GetProperties();
                    Dictionary<string, PropertyInfo> properties = new Dictionary<string, PropertyInfo>();
                    foreach(PropertyInfo configurationPropertyInfo in configurationsPropertyInfo)
                    {
                        properties.Add(configurationPropertyInfo.Name, configurationPropertyInfo);
                    }
                    
                    if (effectsDictionary.ContainsKey(currentStatus.m_name) && properties.ContainsKey(effectsDictionary[currentStatus.m_name])) {
                        MethodInfo[] methInfos = properties[effectsDictionary[currentStatus.m_name]].GetAccessors();
                        for (int i = 0; i < methInfos.Length; i++)
                        {
                            MethodInfo accessor = methInfos[i];
                            if (accessor.ReturnType != typeof(void))
                            {
                                EffectsConfigurationItem configuration = (EffectsConfigurationItem)accessor.Invoke(Configuration.Current.Effects, new object[] { });
                                ConfigureEffect(currentStatus, configuration);
                            }
                        }
                    }
                }
            }
        }
    }
}