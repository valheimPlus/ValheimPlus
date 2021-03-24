namespace ValheimPlus.Configurations.Sections
{
    public class ValheimPlusConfiguration : BaseConfig<ValheimPlusConfiguration>
    {
        public bool mainMenuLogo { get; internal set; } = true;
        public bool serverBrowserAdvertisement { get; internal set; } = true;
    }
}
