using HarmonyLib;
using System;
using ValheimPlus.Configurations;
using UnityEngine;

namespace ValheimPlus
{
    
    [HarmonyPatch(typeof(Player), "OnSpawned")]
    public static class ModifyOnSpawned
    {
        private static void Prefix()
        {

            Tutorial.TutorialText introTutorial = new Tutorial.TutorialText()
            {
                m_label = "ValheimPlus Intro",
                m_name = "vplus",
                m_text = "We hope you enjoy the mod, please support our Patreon so we can continue to provide new updates!",
                m_topic = "Welcome to Valheim+"
            };

            if (!Tutorial.instance.m_texts.Contains(introTutorial))
            {
                Tutorial.instance.m_texts.Add(introTutorial);
            }

            Player.m_localPlayer.ShowTutorial("vplus");
        }
    }
    

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

                carryWeight = Configuration.Current.Player.baseMaximumWeight;
                if (Megingjord)
                {
                    carryWeight = carryWeight + Configuration.Current.Player.baseMegingjordBuff;
                }

                __result = carryWeight;
            }
        }
    }

    // ToDo have item tooltips be affected.
    [HarmonyPatch(typeof(Player), "UpdateFood")]
    public static class ApplyFoodChanges
    {
        private static Boolean Prefix(ref Player __instance, ref float dt, ref bool forceUpdate)
        {
            __instance.m_foodUpdateTimer += dt;
            if (__instance.m_foodUpdateTimer >= getModifiedDeltaTime(ref __instance, ref dt) || forceUpdate)
            {
                __instance.m_foodUpdateTimer = 0f;
                foreach (Player.Food food in __instance.m_foods)
                {
                    food.m_health -= food.m_item.m_shared.m_food / food.m_item.m_shared.m_foodBurnTime;
                    food.m_stamina -= food.m_item.m_shared.m_foodStamina / food.m_item.m_shared.m_foodBurnTime;
                    if (food.m_health < 0f)
                    {
                        food.m_health = 0f;
                    }
                    if (food.m_stamina < 0f)
                    {
                        food.m_stamina = 0f;
                    }
                    if (food.m_health <= 0f)
                    {
                        __instance.Message(MessageHud.MessageType.Center, "$msg_food_done", 0, null);
                        __instance.m_foods.Remove(food);
                        break;
                    }
                }
                float health;
                float stamina;
                __instance.GetTotalFoodValue(out health, out stamina);
                __instance.SetMaxHealth(health, true);
                __instance.SetMaxStamina(stamina, true);
            }
            if (!forceUpdate)
            {
                __instance.m_foodRegenTimer += dt;
                if (__instance.m_foodRegenTimer >= 10f)
                {
                    __instance.m_foodRegenTimer = 0f;
                    float num = 0f;
                    foreach (Player.Food food2 in __instance.m_foods)
                    {
                        num += food2.m_item.m_shared.m_foodRegen;
                    }
                    if (num > 0f)
                    {
                        float num2 = 1f;
                        __instance.m_seman.ModifyHealthRegen(ref num2);
                        num *= num2;
                        Helper.getPlayerCharacter(__instance).Heal(num, true);
                    }
                }
            }
            return false;
        }
        

        private static float getModifiedDeltaTime(ref Player __instance, ref float dt)
        {
            
            float defaultDeltaTimeTarget = 1f;
            float newDetalTimeTarget = 1f;

            if (Configuration.Current.Food.IsEnabled)
            {
                float food_multiplier = Configuration.Current.Food.foodDurationMultiplier;
                if (food_multiplier == 50) food_multiplier = 51; // Decimal issue
                if (food_multiplier == -50) food_multiplier = -51; // Decimal issue

                if (food_multiplier >= 0)
                {
                    newDetalTimeTarget = defaultDeltaTimeTarget + ((defaultDeltaTimeTarget / 100) * food_multiplier);
                }
                else
                {
                    newDetalTimeTarget = defaultDeltaTimeTarget - ((defaultDeltaTimeTarget / 100) * (food_multiplier * -1));
                }

                return newDetalTimeTarget;
            }

            return defaultDeltaTimeTarget;
        }

    }

}
