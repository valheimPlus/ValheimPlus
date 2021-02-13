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

namespace ValheimPlus
{
    // COPYRIGHT 2021 KEVIN "nx#8830" J. // http://n-x.xyz
    // GITHUB REPOSITORY https://github.com/nxPublic/ValheimPlus


    [BepInPlugin("org.bepinex.plugins.valheim_multipliers", "Valheim Multipliers", "0.5.0.0")]
    class ValheimMultipliersPlugin : BaseUnityPlugin
    {
        string ConfigPath = Path.GetDirectoryName(Paths.BepInExConfigPath) + "\\valheim_multipliers.cfg";

        // DO NOT REMOVE MY CREDITS
        string Author = "nx";
        string Website = "http://n-x.xyz";
        string Discord = "nx#8830";
        string Repository = "https://github.com/nxPublic/ValheimPlus";

        // Add your credits here
        String ModifiedBy = "YourName";

        public static Boolean isDebug = true;

        public static IniData Config { get; set; }

        // Awake is called once when both the game and the plug-in are loaded
        void Awake()
        {
            Logger.LogInfo("Trying to load the configuration file");
            if (File.Exists(ConfigPath))
            {
                Logger.LogInfo("Configuration file found, loading configuration.");
                if (LoadSettings() != true)
                {
                    Logger.LogError("Error while loading configuration file.");
                }
                else
                {
                    Logger.LogInfo("Configuration file loaded succesfully.");
                    // apply hooks
                    Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly());
                }
            }
            else
            {
                Logger.LogError("Error: File not found. Plugin not loaded.");
            }
        }
        private bool LoadSettings()
        {
            try
            {
                var parser = new FileIniDataParser();
                Config = parser.ReadFile(ConfigPath);
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        // ##################################################### SECTION = PLAYER
        [HarmonyPatch(typeof(Player), "GetMaxCarryWeight")]
        public static class ModifyMaximumCarryWeight
        {
            private static void Postfix(ref float __result)
            {
                bool Megingjord = false;
                float carryWeight = __result;

                if (carryWeight > 300)
                {
                    Megingjord = true;
                    carryWeight -= 150;
                }

                carryWeight = toFloat(Config["Player"]["baseMaximumWeight"]);
                if (Megingjord)
                {
                    carryWeight = carryWeight + toFloat(Config["Player"]["baseMegingjordBuff"]);
                }

                __result = carryWeight;
            }
        }
        [HarmonyPatch(typeof(Player), "AutoPickup")]
        public static class ModifyAutoPickUpRange
        {
            private static bool Prefix(ref float ___m_autoPickupRange)
            {
                ___m_autoPickupRange = toFloat(Config["Player"]["baseAutoPickUpRange"]);
                return true;
            }
        }

        // ##################################################### SECTION = BEEHIVES
        [HarmonyPatch(typeof(Beehive), "Awake")]
        public static class ApplyBeehiveChanges
        {
            private static bool Prefix(ref float ___m_secPerUnit, ref int ___m_maxHoney)
            {
                if (Config["Beehive"]["enabled"] == "true")
                {
                    ___m_secPerUnit = float.Parse(Config["Beehive"]["honeyProductionSpeed"]);
                    ___m_maxHoney = int.Parse(Config["Beehive"]["maximumHoneyPerBeehive"]);
                    if (isDebug)
                    {
                        Debug.Log("Beehive Production :" + toFloat(Config["Beehive"]["honeyProductionSpeed"]));
                        Debug.Log("Beehive Maximum :" + int.Parse(Config["Beehive"]["maximumHoneyPerBeehive"]));
                    }
                }
                return true;
            }

        }

        // ##################################################### SECTION = Fermenter
        [HarmonyPatch(typeof(Fermenter), "Awake")]
        public static class ApplyFermenterChanges
        {
            private static bool Prefix(ref float ___m_fermentationDuration, ref Fermenter __instance)
            {
                float fermenterDuration = toFloat(Config["Fermenter"]["fermenterDuration"]);
                if (fermenterDuration > 0)
                {
                    ___m_fermentationDuration = fermenterDuration;
                }
                return true;
            }

        }
        [HarmonyPatch(typeof(Fermenter), "GetItemConversion")]
        public static class ApplyFermenterItemCountChanges
        {
            private static void Postfix(ref Fermenter.ItemConversion __result)
            {
                int fermenterItemCount = int.Parse(Config["Fermenter"]["fermenterItemsProduced"]);
                if (fermenterItemCount > 0)
                {
                    __result.m_producedItems = fermenterItemCount;
                }
            }

        }


        // ##################################################### SECTION = Items
        [HarmonyPatch(typeof(Inventory), "IsTeleportable")]
        public static class noItemTeleportPrevention
        {
            private static void Postfix(ref Boolean __result)
            {
                if (Config["Items"]["noTeleportPrevention"] == "true")
                    __result = true;
            }
        }

        // ##################################################### SECTION = Furnace
        [HarmonyPatch(typeof(Smelter), "Awake")]
        public static class ApplyFurnaceChanges
        {
            private static void Prefix(ref Smelter __instance)
            {
                int MaximumOre = int.Parse(Config["Furnace"]["maximumOre"]); 
                int MaximumFuel = int.Parse(Config["Furnace"]["maximumCoal"]);
                float ProductionSpeed = toFloat(Config["Furnace"]["productionSpeed"]);
                int CoalPerProduct = int.Parse(Config["Furnace"]["coalUsedPerProduct"]);
                __instance.m_maxOre = MaximumOre;
                __instance.m_maxFuel = MaximumFuel;
                __instance.m_secPerProduct = ProductionSpeed;
                __instance.m_fuelPerProduct = CoalPerProduct;
            }
        }

        // ##################################################### SECTION = Tooltip & Item Modification
        [HarmonyPatch(typeof(ItemDrop), "Awake")]
        public static class ChangeTooltip
        {
            private static void Prefix(ref ItemDrop __instance)
            {
                if (isDebug)
                    Debug.Log(__instance.m_itemData.m_shared.m_name + ", type:" + __instance.m_itemData.m_shared.m_itemType.ToString());

                if (Config["Items"]["noTeleportPrevention"] == "true")
                {
                    __instance.m_itemData.m_shared.m_teleportable = true;
                }

                float food_multiplier = toFloat(Config["Food"]["foodDurationMultiplier"]);
                if (food_multiplier > 0.1)
                {
                    if (Convert.ToInt32(__instance.m_itemData.m_shared.m_itemType) == 2)
                        __instance.m_itemData.m_shared.m_foodBurnTime = __instance.m_itemData.m_shared.m_foodBurnTime + (__instance.m_itemData.m_shared.m_foodBurnTime * toFloat(Config["Food"]["foodDurationMultiplier"]));
                }

                float itemWeigthReduction = toFloat(Config["Items"]["baseItemWeightReduction"]);
                if (itemWeigthReduction > 0)
                {
                    __instance.m_itemData.m_shared.m_weight = __instance.m_itemData.m_shared.m_weight - (__instance.m_itemData.m_shared.m_weight * itemWeigthReduction);
                }


            }
        }



        // ##################################################### SECTION = BUILDING
        [HarmonyPatch(typeof(Player), "UpdatePlacementGhost")]
        public static class ModifyPlacingRestrictionOfGhost
        {
            private static void Postfix(ref Int32 ___m_placementStatus, ref GameObject ___m_placementGhost)
            {
                if (Config["Building"]["noInvalidPlacementRestriction"] == "true")
                {
                    if (___m_placementStatus == 1)
                    {
                        ___m_placementStatus = 0;
                        ___m_placementGhost.GetComponent<Piece>().SetInvalidPlacementHeightlight(false);
                    }
                }
            }
        }


        // Helper Functions
        private static float toFloat(string value)
        {
            return float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
        }

        public enum ItemType
        {
            None,
            Material,
            Consumable,
            OneHandedWeapon,
            Bow,
            Shield,
            Helmet,
            Chest,
            Ammo = 9,
            Customization,
            Legs,
            Hands,
            Trophie,
            TwoHandedWeapon,
            Torch,
            Misc,
            Shoulder,
            Utility,
            Tool,
            Attach_Atgeir
        }

    }
}
