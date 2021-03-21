namespace ValheimPlus.Configurations.Sections
{
    public class ProjectileFiredConfiguration : ServerSyncConfig<ProjectileFiredConfiguration>
    {
        public float playerProjectileVelMinCharge { get; internal set; } = 0;
        public float playerProjectileVelMaxCharge { get; internal set; } = 0;

        public float playerProjectileVarMinCharge { get; internal set; } = 0;
        public float playerProjectileVarMaxCharge { get; internal set; } = 0;

        public float projectileVelMinCharge { get; internal set; } = 0;
        public float projectileVelMaxCharge { get; internal set; } = 0;

        public float projectileVarMinCharge { get; internal set; } = 0;
        public float projectileVarMaxCharge { get; internal set; } = 0;
    }
}
