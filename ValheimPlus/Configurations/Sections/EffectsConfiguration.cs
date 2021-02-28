namespace ValheimPlus.Configurations.Sections
{
    public class EffectsConfiguration : ServerSyncConfig<EffectsConfiguration>
    {
        public float restedBuffHealthRegenMultiplier { get; internal set; } = 150;
        public float restedBuffStaminaRegenMultiplier { get; internal set; } = 200;
        public float restedBuffJumpStaminaModifier { get; internal set; } = 0;
        public float restedBuffRunStaminaModifier { get; internal set; } = 0;

        public float restingBuffHealthRegenMultiplier { get; internal set; } = 300;
        public float restingBuffStaminaRegenMultiplier { get; internal set; } = 400;

        public float warmBuffBuffHealthRegenMultiplier { get; internal set; } = 100;
        public float warmBuffBuffStaminaRegenMultiplier { get; internal set; } = 200;


        public float corpseRunBuffHealthRegenMultiplier { get; internal set; } = 100;
        public float corpseRunBuffStaminaRegenMultiplier { get; internal set; } = 100; 
        public float corpseRunBuffJumpStaminaModifier { get; internal set; } = -75;
        public float corpseRunBuffRunStaminaModifier { get; internal set; } = -75;

        public float freezingDebuffHealthRegenMultiplier { get; internal set; } = 0;
        public float freezingDebuffStaminaRegenMultiplier { get; internal set; } = 40;
        public float freezingDebuffDamagePerTick { get; internal set; } = -1;

        public float coldDebuffHealthRegenMultiplier { get; internal set; } = 50;
        public float coldDebuffStaminaRegenMultiplier { get; internal set; } = 75;

        public float wetDebuffHealthRegenMultiplier { get; internal set; } = 75;
        public float wetDebuffStaminaRegenMultiplier { get; internal set; } = 85;
        public float wetDebuffDuration { get; internal set; } = 120;
    }
}
