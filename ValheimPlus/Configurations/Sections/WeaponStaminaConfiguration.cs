namespace ValheimPlus.Configurations.Sections
{
    public class WeaponStaminaConfiguration : ServerSyncConfig<WeaponStaminaConfiguration>
    {
        public float Axes { get; internal set; } = 0; 
        public float Bows { get; internal set; } = 0;
        public float Clubs { get; internal set; } = 0;
        public float Knives { get; internal set; } = 0;
        public float Pickaxes { get; internal set; } = 0;
        public float Polearms { get; internal set; } = 0;
        public float Spears { get; internal set; } = 0; 
        public float Swords { get; internal set; } = 0;
        public float Unarmed { get; internal set; } = 0;
    }
}
