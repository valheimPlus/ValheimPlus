namespace ValheimPlus.Configurations.Sections
{
    public class FermenterConfiguration : BaseConfig<FermenterConfiguration>
    {
        public float FermenterDuration { get; set; } = 2400;
        public int FermenterItemsProduced { get; set; } = 4;
    }

}
