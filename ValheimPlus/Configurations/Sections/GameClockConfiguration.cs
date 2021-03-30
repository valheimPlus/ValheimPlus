namespace ValheimPlus.Configurations.Sections
{
    public class GameClockConfiguration : ServerSyncConfig<GameClockConfiguration>
    {
        public bool useAMPM { get; set; } = false;

        public int textFontSize { get; set; } = 34;

        public int textRedChannel { get; set; } = 248;
        public int textGreenChannel { get; set; } = 105;
        public int textBlueChannel { get; set; } = 0;
        public int textTransparencyChannel { get; set; } = 255;
    }
}