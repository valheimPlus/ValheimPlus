namespace ValheimPlus.Configurations.Sections
{
    public class MonsterProjectileConfiguration : ServerSyncConfig<MonsterProjectileConfiguration>
    {
        public float monsterMaxChargeVelocityMultiplier { get; internal set; } = 0;

        public float monsterMaxChargeAccuracyMultiplier { get; internal set; } = 0;
    }
}