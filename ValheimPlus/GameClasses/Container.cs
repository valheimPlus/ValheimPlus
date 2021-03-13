using HarmonyLib;
using ValheimPlus.Configurations;

namespace ValheimPlus
{
    [HarmonyPatch(typeof(Container), "Awake")]
    public static class Container_Awake_Patch
    {
        private const int woodChestInventoryMaxRows = 10;
        private const int woodChestInventoryMinRows = 2;
        private const int woodChestInventoryMaxCol = 8;
        private const int woodChestInventoryMinCol = 5;

        private const int personalChestInventoryMaxRows = 20;
        private const int personalChestInventoryMinRows = 2;
        private const int personalChestInventoryMaxCol = 8;
        private const int personalChestInventoryMinCol = 3;

        private const int ironChestInventoryMaxRows = 20;
        private const int ironChestInventoryMinRows = 3;
        private const int ironChestInventoryMaxCol = 8;
        private const int ironChestInventoryMinCol = 6;

        private const int karveChestInventoryMaxRows = 30;
        private const int karveChestInventoryMinRows = 3;
        private const int karveChestInventoryMaxCol = 8;
        private const int karveChestInventoryMinCol = 6;

        private const int longboatChestInventoryMaxRows = 30;
        private const int longboatChestInventoryMinRows = 3;
        private const int longboatChestInventoryMaxCol = 8;
        private const int longboatChestInventoryMinCol = 6;

        private const int cartChestInventoryMaxRows = 30;
        private const int cartChestInventoryMinRows = 3;
        private const int cartChestInventoryMaxCol = 8;
        private const int cartChestInventoryMinCol = 6;

        /// <summary>
        /// Configure inventory size for chests, boats, cart
        /// </summary>
        static void Postfix(Container __instance, ref Inventory ___m_inventory)
        {
            if (!Configuration.Current.Inventory.IsEnabled) return;

            string containerName = __instance.transform.parent.name;
            string inventoryName = ___m_inventory.m_name;
            ref int inventoryColumns = ref ___m_inventory.m_width;
            ref int inventoryRows = ref ___m_inventory.m_height;

            // Karve (small boat)
            // Use Contains because the actual name is "Karve (Clone)"
            if (containerName.Contains("Karve"))
            {
                inventoryRows = Helper.Clamp(Configuration.Current.Inventory.karveInventoryRows, karveChestInventoryMinRows, karveChestInventoryMaxRows);
                inventoryColumns = Helper.Clamp(Configuration.Current.Inventory.karveInventoryColumns, karveChestInventoryMinCol, karveChestInventoryMaxCol);
            }
            // Longboat (Large boat)
            else if (containerName.Contains("VikingShip"))
            {
                inventoryRows = Helper.Clamp(Configuration.Current.Inventory.longboatInventoryRows, longboatChestInventoryMinRows, longboatChestInventoryMaxRows);
                inventoryColumns = Helper.Clamp(Configuration.Current.Inventory.longboatInventoryColumns, longboatChestInventoryMinCol, longboatChestInventoryMaxCol);
            }
            // Cart (Wagon)
            else if (containerName.Contains("Cart"))
            {
                inventoryRows = Helper.Clamp(Configuration.Current.Inventory.cartInventoryRows, cartChestInventoryMinRows, cartChestInventoryMaxRows);
                inventoryColumns = Helper.Clamp(Configuration.Current.Inventory.cartInventoryColumns, cartChestInventoryMinCol, cartChestInventoryMaxCol);
            }
            // Chests (containerName is _NetSceneRoot)
            else
            {
                // Personal chest
                if (inventoryName == "$piece_chestprivate")
                {
                    inventoryRows = Helper.Clamp(Configuration.Current.Inventory.personalChestRows, personalChestInventoryMinRows, personalChestInventoryMaxRows);
                    inventoryColumns = Helper.Clamp(Configuration.Current.Inventory.personalChestColumns, personalChestInventoryMinCol, personalChestInventoryMaxCol);
                }
                // Wood chest
                else if (inventoryName == "$piece_chestwood")
                {
                    inventoryRows = Helper.Clamp(Configuration.Current.Inventory.woodChestRows, woodChestInventoryMinRows, woodChestInventoryMaxRows);
                    inventoryColumns = Helper.Clamp(Configuration.Current.Inventory.woodChestColumns, woodChestInventoryMinCol, woodChestInventoryMaxCol);
                }
                // Iron chest
                else if (inventoryName == "$piece_chest")
                {
                    inventoryRows = Helper.Clamp(Configuration.Current.Inventory.ironChestRows, ironChestInventoryMinRows, ironChestInventoryMaxRows);
                    inventoryColumns = Helper.Clamp(Configuration.Current.Inventory.ironChestColumns, ironChestInventoryMinCol, ironChestInventoryMaxCol);
                }
            }

        }
    }
}
