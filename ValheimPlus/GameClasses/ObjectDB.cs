using HarmonyLib;
using UnityEngine;
using ValheimPlus.Configurations;

namespace ValheimPlus
{
    [HarmonyPatch(typeof(ObjectDB), "Awake")]
    public static class ModifyModerEffects
    {
        private static void Postfix(ObjectDB __instance)
        {
            if (Configuration.Current.ModerEffect.IsEnabled)
            {
                Debug.Log("ModerEffect enabled");
                ConfigureEffect(__instance);
            }
        }

        private static void ConfigureEffect(ObjectDB __instance)
        {
            foreach (StatusEffect statusEffect in __instance.m_StatusEffects)
            {
                if (statusEffect is SE_Stats currentStatus)
                {
                    Debug.Log(statusEffect.m_name);
                    if (statusEffect.m_name.Equals("$se_moder_name"))
                    {
                        currentStatus.m_cooldown = Configuration.Current.ModerEffect.cooldown;
                        currentStatus.m_ttl = Configuration.Current.ModerEffect.duration;
                    }
                }
            }
        }
    }
}
