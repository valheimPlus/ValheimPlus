using HarmonyLib;
using ValheimPlus.Configurations;

namespace ValheimPlus
{
    [HarmonyPatch(typeof(Player), "Awake")]
    public static class ModifyPlayerValues
    {
        private static void Postfix(Player __instance)
        {
            if (Configuration.Current.Stamina.IsEnabled)
            {
                __instance.m_dodgeStaminaUsage = Configuration.Current.Stamina.DodgeStaminaUsage;;
                __instance.m_encumberedStaminaDrain = Configuration.Current.Stamina.EncumberedStaminaDrain;
                __instance.m_sneakStaminaDrain = Configuration.Current.Stamina.SneakStaminaDrain;
                __instance.m_runStaminaDrain = Configuration.Current.Stamina.RunStaminaDrain;
                __instance.m_staminaRegenDelay = Configuration.Current.Stamina.StaminaRegenDelay;
                __instance.m_staminaRegen = Configuration.Current.Stamina.StaminaRegen;
                __instance.m_swimStaminaDrainMinSkill = Configuration.Current.Stamina.SwimStaminaDrain;
                __instance.m_jumpStaminaUsage = Configuration.Current.Stamina.JumpStaminaUsage;
            }
            if (Configuration.Current.Player.IsEnabled)
            {
                __instance.m_autoPickupRange = Configuration.Current.Player.BaseAutoPickUpRange;
                __instance.m_baseCameraShake = Configuration.Current.Player.DisableCameraShake ? 0f : 4f;
            }
            if (Configuration.Current.Building.IsEnabled)
            {
                __instance.m_maxPlaceDistance = Configuration.Current.Building.MaximumPlacementDistance;
            }
        }
    }


}
