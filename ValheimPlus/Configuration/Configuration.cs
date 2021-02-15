using IniParser.Model;
using UnityEngine;

namespace ValheimPlus
{
    public class Configuration
    {
        public ItemsConfiguration Items { get; set; }
        public PlayerConfiguration Player { get; set; }
        public BeehiveConfiguration Beehive { get; set; }
        public FermenterConfiguration Fermenter { get; set; }
        public FurnaceConfiguration Furnace { get; set; }
        public KilnConfiguration Kiln { get; set; }
        public FoodConfiguration Food { get; set; }
        public AdvancedBuildingModeConfiguration AdvancedBuildingMode { get; set; }
        public BuildingConfiguration Building { get; set;}
        public ServerConfiguration Server { get; set; }
        public MapConfiguration Map { get; set; }
        public HotkeyConfiguration Hotkeys { get; set; } = null;
    }

    public class ItemsConfiguration: BaseConfig<ItemsConfiguration> {
        public bool NoTeleportPrevention { get; set; } = false;
        public float BaseItemWeightReduction { get; set; } = 0;

    }

    public class PlayerConfiguration: BaseConfig<PlayerConfiguration> {
        public float BaseMaximumWeight { get; set; } = 300;
        public float BaseMegingjordBuff { get; set; } = 150;
        public float BaseAutoPickUpRange { get; set; } =2;
    }

    public class BeehiveConfiguration: BaseConfig<BeehiveConfiguration> {
        public float HoneyProductionSpeed { get; set; } = 10;
        public int MaximumHoneyPerBeehive { get; set; } = 4;
    }

    public class FermenterConfiguration: BaseConfig<FermenterConfiguration> {
        public float FermenterDuration { get; set; } = 2400;
        public int FermenterItemsProduced { get; set; } = 4;
    }

    public class FurnaceConfiguration: BaseConfig<FurnaceConfiguration> {
        public int MaximumOre { get; set; } = 10;
        public int MaximumCoal { get; set; } = 10;
        public int CoalUsedPerProduct { get; set; } = 4;
        public float ProductionSpeed { get; set; } = 10;
    }

    public class KilnConfiguration: BaseConfig<KilnConfiguration> {
        public float ProductionSpeed { get; set; } = 10;
    }

    public class FoodConfiguration: BaseConfig<FoodConfiguration> {
        public float FoodDurationMultiplier { get; set; } = 0;
    }

    public class BuildingConfiguration: BaseConfig<BuildingConfiguration> {
        public bool NoInvalidPlacementRestriction { get; set; } = false;
        public bool NoWeatherDamage { get; set; } = false;
    }

    public class ServerConfiguration: BaseConfig<ServerConfiguration> { 
        public int MaxPlayers { get; set; } = 10;
        public bool DisableServerPassword { get; set; } = false;
    }

    public class AdvancedBuildingModeConfiguration: BaseConfig<AdvancedBuildingModeConfiguration> {
        public KeyCode EnterAdvancedBuildingMode { get; set; } = KeyCode.F1;
        public KeyCode ExitAdvancedBuildingMode { get; set; } = KeyCode.F3;
    }


    public class MapConfiguration: BaseConfig<MapConfiguration> {
        public bool ShareMapProgression { get; set; } = true;
        public float ExploreRadius { get; set; } = 100;
    }

    public class HotkeyConfiguration: BaseConfig<HotkeyConfiguration> {
        public KeyCode RollForwards { get; set; } = KeyCode.F9;
        public KeyCode RollBackwards { get; set; } = KeyCode.F10;
    }

}
