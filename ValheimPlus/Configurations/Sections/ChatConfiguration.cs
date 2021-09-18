namespace ValheimPlus.Configurations.Sections
{
    public class ChatConfiguration : BaseConfig<ChatConfiguration>
    {
        public float shoutDistance { get; internal set; } = 125;
        public float pingDistance { get; internal set; } = 125;
        public bool forcedCase { get; internal set; } = true;
        public bool outOfRangeShoutsDisplayInChatWindow { get; internal set; } = true;
        public float defaultWhisperDistance { get; internal set; } = 4;
        public float defaultNormalDistance { get; internal set; } = 15;
        public float defaultShoutDistance { get; internal set; } = 70;
    }
}
