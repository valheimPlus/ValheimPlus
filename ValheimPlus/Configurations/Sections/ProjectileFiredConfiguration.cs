namespace ValheimPlus.Configurations.Sections
{
    public class ProjectileFiredConfiguration : ServerSyncConfig<ProjectileFiredConfiguration>
    {
        public float projectileVelMinCharge { get; internal set; } = 0;
        public float projectileVelMaxCharge { get; internal set; } = 0;

        public float projectileVarMinCharge { get; internal set; } = 0;
        public float projectileVarMaxCharge { get; internal set; } = 0;
    }
}
