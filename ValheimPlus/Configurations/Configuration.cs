using ValheimPlus.Configurations.Sections;

namespace ValheimPlus.Configurations
{
    public class Configuration
    {
        public static Configuration Current { get; set; }

        public ItemsConfiguration Items { get; set; }
        public PlayerConfiguration Player { get; set; }
        public BeehiveConfiguration Beehive { get; set; }
        public FermenterConfiguration Fermenter { get; set; }
        public FurnaceConfiguration Furnace { get; set; }
        public KilnConfiguration Kiln { get; set; }
        public FoodConfiguration Food { get; set; }
        public AdvancedBuildingModeConfiguration AdvancedBuildingMode { get; set; }
        public BuildingConfiguration Building { get; set; }
        public ServerConfiguration Server { get; set; }
        public MapConfiguration Map { get; set; }
        public HotkeyConfiguration Hotkeys { get; set; }
        public StaminaConfiguration Stamina { get; set; }
        public AdvancedEditingModeConfiguration AdvancedEditingMode { get; set; }
        public StaminaWeapons WeaponsStamina { get; set; }
        public FireplaceC FireplaceC { get; set; }
        public Unarmed UnarmedScaling { get; set; }
        public WorkbenchC Workbench { get; set; }
    }

}
