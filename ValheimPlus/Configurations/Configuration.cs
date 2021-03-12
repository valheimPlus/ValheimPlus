using ValheimPlus.Configurations.Sections;

namespace ValheimPlus.Configurations
{
    public class Configuration
    {
        public static Configuration Current { get; set; }

        public AdvancedBuildingModeConfiguration AdvancedBuildingMode { get; set; }
        public AdvancedEditingModeConfiguration AdvancedEditingMode { get; set; }
        public BeehiveConfiguration Beehive { get; set; }
        public BuildingConfiguration Building { get; set; }
        public DeconstructConfiguration Deconstruct { get; set; }
        public InventoryConfiguration Inventory { get; set; }
        public ItemsConfiguration Items { get; set; }
        public FermenterConfiguration Fermenter { get; set; }
        public FireSourceConfiguration FireSource { get; set; }
        public FoodConfiguration Food { get; set; }
        public SmelterConfiguration Smelter { get; set; }
        public FurnaceConfiguration Furnace { get; set; }
        public HotkeyConfiguration Hotkeys { get; set; }
        public KilnConfiguration Kiln { get; set; }
        public MapConfiguration Map { get; set; }
        public PlayerConfiguration Player { get; set; }
        public ServerConfiguration Server { get; set; }
        public StaminaConfiguration Stamina { get; set; }
        public StaminaUsageConfiguration StaminaUsage { get; set; }
        public WorkbenchConfiguration Workbench { get; set; }
        public TimeConfiguration Time { get; set; }
        public WardConfiguration Ward { get; set; }
        public StructuralIntegrityConfiguration StructuralIntegrity { get; set; }
        public HudConfiguration Hud { get; set; }
        public ExperienceConfiguration Experience { get; set; }
        public CameraConfiguration Camera { get; set; }
        public GameConfiguration Game { get; set; }
        public WagonConfiguration Wagon { get; set; }
        public GatherConfiguration Gathering { get; set; }
        public DurabilityConfiguration Durability { get; set; }
        public ArmorConfiguration Armor { get; set; }
		public FreePlacementRotationConfiguration FreePlacementRotation { get; set; }
        public ShieldConfiguration Shields { get; set; }
        public FirstPersonConfiguration FirstPerson { get; internal set; }
    }
}
