namespace ValheimPlus.Configurations.Sections
{
    public class CraftFromChestConfiguration : ServerSyncConfig<CraftFromChestConfiguration>
    {
        public float range { get; internal set; } = 20;
        public bool checkFromWorkbench { get; internal set; } = true;
        public bool ignorePrivateAreaCheck { get; internal set; } = true;
        public int lookupInterval { get; internal set; } = 3;
    }
}
