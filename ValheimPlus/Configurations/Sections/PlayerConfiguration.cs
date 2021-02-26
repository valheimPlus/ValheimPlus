// ValheimPlus

namespace ValheimPlus.Configurations.Sections
{
    [ConfigurationSection("Player settings")]
    public class PlayerConfiguration : ServerSyncConfig<PlayerConfiguration>
    {
        [Configuration("The base amount of carry weight of your character")]
        public float baseMaximumWeight { get; internal set; } = 300;

        [Configuration("Increase the buff you receive to your carry weight from Megingjord's girdle")]
        public float baseMegingjordBuff { get; internal set; } = 150;

        [Configuration("Increase auto pickup range of all items")]
        public float baseAutoPickUpRange { get; internal set; } = 2;

        [Configuration("Disable all types of camera shake")]
        public bool disableCameraShake { get; internal set; } = false;

        [Configuration("The base unarmed damage multiplied by your skill level")]
        public float baseUnarmedDamage { get; internal set; } = 0;
    }
}