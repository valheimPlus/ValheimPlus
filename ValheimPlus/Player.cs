using HarmonyLib;
using System;
using ValheimPlus.Configurations;

namespace ValheimPlus
{
    [HarmonyPatch(typeof(Player), "GetMaxCarryWeight")]
    public static class ModifyMaximumCarryWeight
    {
        private static void Postfix(ref float __result)
        {
            if (Configuration.Current.Player.IsEnabled)
            {
                bool Megingjord = false;
                float carryWeight = __result;

                if (carryWeight > 300)
                {
                    Megingjord = true;
                    carryWeight -= 150;
                }

                carryWeight = Configuration.Current.Player.BaseMaximumWeight;
                if (Megingjord)
                {
                    carryWeight = carryWeight + Configuration.Current.Player.BaseMegingjordBuff;
                }

                __result = carryWeight;
            }
        }
    }

    // ToDo have item tooltips be affected.
    [HarmonyPatch(typeof(Player), "UpdateFood")]
    public static class ApplyFoodChanges
    {
        private static Boolean Prefix(ref Player __instance, float dt, ref bool forceUpdate)
        {
            __instance.m_foodUpdateTimer += dt;

            float defaultDeltaTimeTarget = 1f;
            float newDetalTimeTarget = 1f;

            if (Configuration.Current.Food.IsEnabled)
            {
                float food_multiplier = Configuration.Current.Food.FoodDurationMultiplier;

                if (food_multiplier >= 0)
                {
                    newDetalTimeTarget = defaultDeltaTimeTarget + ((defaultDeltaTimeTarget / 100) * food_multiplier);
                }
                else
                {
                    newDetalTimeTarget = defaultDeltaTimeTarget - ((defaultDeltaTimeTarget / 100) * (food_multiplier * -1));
                }

                if (__instance.m_foodUpdateTimer >= newDetalTimeTarget || forceUpdate)
                {
                    __instance.m_foodUpdateTimer = defaultDeltaTimeTarget;
                    return true;
                }

                return false;
            }
            return true;
        }

    }

}
