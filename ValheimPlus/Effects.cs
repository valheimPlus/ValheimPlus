using HarmonyLib;
using System;
using ValheimPlus.Configurations;

namespace ValheimPlus
{ 
    class Effects
    {
        const string EikthyrBuff = "$se_eikthyr_name";
        const string TheElderBuff = "$se_theelder_name";
        const string BonemassBuff = "$se_bonemass_name";
        const string YagluthBuff = "$se_yagluth_name";

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

        private static void ConfigureEffects(ObjectDB __instance)
        {
            foreach (StatusEffect statusEffect in __instance.m_StatusEffects)
            {
                if (statusEffect is SE_Stats currentStatus)
                {
                    /* Debug
                    Debug.Log(">>> Effects:" + statusEffect.m_name);
                    Debug.Log(">> m_activationAnimation:" + currentStatus.m_activationAnimation);
                    Debug.Log(">> m_addMaxCarryWeight:" + currentStatus.m_addMaxCarryWeight);
                    Debug.Log(">> m_cooldown:" + currentStatus.m_cooldown);
                    Debug.Log(">> m_damageModifier:" + currentStatus.m_damageModifier);
                    Debug.Log(">> m_healthOverTime:" + currentStatus.m_healthOverTime);
                    Debug.Log(">> m_healthOverTimeDuration:" + currentStatus.m_healthOverTimeDuration);
                    Debug.Log(">> m_healthOverTimeInterval:" + currentStatus.m_healthOverTimeInterval);
                    Debug.Log(">> m_healthOverTimeTickHP:" + currentStatus.m_healthOverTimeTickHP);
                    Debug.Log(">> m_healthOverTimeTicks:" + currentStatus.m_healthOverTimeTicks);
                    Debug.Log(">> m_healthOverTimeTimer:" + currentStatus.m_healthOverTimeTimer);
                    Debug.Log(">> m_healthPerTick:" + currentStatus.m_healthPerTick);
                    Debug.Log(">> m_healthPerTickMinHealthPercentage:" + currentStatus.m_healthPerTickMinHealthPercentage);
                    Debug.Log(">> m_healthRegenMultiplier:" + currentStatus.m_healthRegenMultiplier);
                    Debug.Log(">> m_jumpStaminaUseModifier:" + currentStatus.m_jumpStaminaUseModifier);
                    Debug.Log(">> m_runStaminaDrainModifier:" + currentStatus.m_runStaminaDrainModifier);
                    Debug.Log(">> m_staminaDrainPerSec:" + currentStatus.m_staminaDrainPerSec);
                    Debug.Log(">> m_staminaOverTime:" + currentStatus.m_staminaOverTime);
                    Debug.Log(">> m_staminaOverTimeDuration:" + currentStatus.m_staminaOverTimeDuration);
                    Debug.Log(">> m_staminaRegenMultiplier:" + currentStatus.m_staminaRegenMultiplier);
                    Debug.Log(">> m_stealthModifier:" + currentStatus.m_stealthModifier);
                    Debug.Log(">> m_ttl:" + currentStatus.m_ttl);
                    */

                    switch (statusEffect.m_name)
                    {
                        case RestedBuff:
                            currentStatus.m_healthRegenMultiplier = Configuration.Current.Effects.restedBuffHealthRegenMultiplier / 100;
                            currentStatus.m_staminaRegenMultiplier = Configuration.Current.Effects.restedBuffStaminaRegenMultiplier / 100;
                            currentStatus.m_jumpStaminaUseModifier = Configuration.Current.Effects.restedBuffJumpStaminaModifier / 100;
                            currentStatus.m_runStaminaDrainModifier = Configuration.Current.Effects.restedBuffRunStaminaModifier / 100;
                            break;

                        case RestingBuff:
                            currentStatus.m_healthRegenMultiplier = Configuration.Current.Effects.restingBuffHealthRegenMultiplier / 100;
                            currentStatus.m_staminaRegenMultiplier = Configuration.Current.Effects.restingBuffStaminaRegenMultiplier / 100;
                            break;

                        case WarmBuff:
                            currentStatus.m_healthRegenMultiplier = Configuration.Current.Effects.warmBuffBuffHealthRegenMultiplier / 100;
                            currentStatus.m_staminaRegenMultiplier = Configuration.Current.Effects.warmBuffBuffStaminaRegenMultiplier / 100;
                            break;

                        case CorpseRunBuff:
                            currentStatus.m_healthRegenMultiplier = Configuration.Current.Effects.corpseRunBuffHealthRegenMultiplier / 100;
                            currentStatus.m_staminaRegenMultiplier = Configuration.Current.Effects.corpseRunBuffStaminaRegenMultiplier / 100;
                            currentStatus.m_jumpStaminaUseModifier = Configuration.Current.Effects.corpseRunBuffJumpStaminaModifier / 100;
                            currentStatus.m_runStaminaDrainModifier = Configuration.Current.Effects.corpseRunBuffRunStaminaModifier / 100;
                            break;

                        case FreezingDebuff:
                            currentStatus.m_healthRegenMultiplier = Configuration.Current.Effects.freezingDebuffHealthRegenMultiplier / 100;
                            currentStatus.m_staminaRegenMultiplier = Configuration.Current.Effects.freezingDebuffStaminaRegenMultiplier / 100;
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