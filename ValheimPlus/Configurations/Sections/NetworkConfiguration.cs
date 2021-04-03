namespace ValheimPlus.Configurations.Sections
{
    public class NetworkConfiguration : BaseConfig<NetworkConfiguration>
    {
        public int maxSendRatePerConnection_kbps { get; internal set; } = 128;
        public int maxSendRateOverall_kbps { get; internal set; } = 1280;
        public bool enableCompression { get; internal set; } = false;
    }

}
