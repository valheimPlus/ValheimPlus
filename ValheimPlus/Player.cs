using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BepInEx;
using Unity;
using UnityEngine;
using System.IO;
using System.Reflection;
using System.Runtime;
using IniParser;
using IniParser.Model;
using HarmonyLib;
using System.Globalization;
using Steamworks;
using ValheimPlus;


namespace ValheimPlus
{
    [HarmonyPatch(typeof(Player), "GetMaxCarryWeight")]
    public static class ModifyMaximumCarryWeight
    {
        private static void Postfix(ref float __result)
        {
            if (Settings.isEnabled("Player"))
            {
                bool Megingjord = false;
                float carryWeight = __result;

                if (carryWeight > 300)
                {
                    Megingjord = true;
                    carryWeight -= 150;
                }

                carryWeight = Settings.getFloat("Player", "baseMaximumWeight");
                if (Megingjord)
                {
                    carryWeight = carryWeight + Settings.getFloat("Player", "baseMegingjordBuff");
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
                        Helper.getPlayerCharacter().Heal(num, true);
                    }
                }
            }
            return false;
        }
        

        private static float getModifiedDeltaTime(ref Player __instance, ref float dt)
        {
            
            float defaultDeltaTimeTarget = 1f;
            float newDetalTimeTarget = 1f;

            if (Settings.isEnabled("Food"))
            {
                float food_multiplier = Settings.getFloat("Food", "foodDuration");
                if (food_multiplier == 50) food_multiplier = 51; // Decimal issue

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
