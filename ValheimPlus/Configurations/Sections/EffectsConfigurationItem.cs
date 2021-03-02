namespace ValheimPlus.Configurations.Sections
{
    public class EffectsConfigurationItem : ServerSyncConfig<EffectsConfigurationItem>
    {
        public float cooldown { get; internal set; } = float.NaN;
        public float damageModifier { get; internal set; } = float.NaN;
        public float duration { get; internal set; } = float.NaN;
        public float healthPerTick { get; internal set; } = float.NaN;
        public float healthRegenModifier { get; internal set; } = float.NaN;
        public float jumpStaminaModifier { get; internal set; } = float.NaN;
        public float runStaminaModifier { get; internal set; } = float.NaN;
        public float staminaRegenModifier { get; internal set; } = float.NaN;
        public float stealthModifier { get; internal set; } = float.NaN;
        public string damageTypesModifiers { get; internal set; } = "";
        public string description { get; internal set; } = "";
        public string modifyAttackSkill { get; internal set; } = "";
    }
}
