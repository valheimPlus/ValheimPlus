namespace ValheimPlus.Configurations.Sections
{
    public class PickableItemsConfiguration : ServerSyncConfig<PickableItemsConfiguration>
    {
        public int dandelionAmount { get; set; } = 1;
        public int mushroomAmount { get; set; } = 1;
        public int mushroomBlueAmount { get; set; } = 1;
        public int mushroomYellowAmount { get; set; } = 1;
        public int thistleAmount { get; set; } = 1;
        public int raspberryAmount { get; set; } = 1;
        public int blueberryAmount { get; set; } = 1;
    }
}