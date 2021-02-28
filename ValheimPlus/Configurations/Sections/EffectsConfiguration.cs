namespace ValheimPlus.Configurations.Sections
{
    public class EffectsConfiguration : ServerSyncConfig<EffectsConfiguration>
    {
        public float restedBuffHealthRegenMultiplier { get; internal set; } = 50;
        public float restedBuffStaminaRegenMultiplier { get; internal set; } = 100;
        public float restedBuffJumpStaminaModifier { get; internal set; } = 0;
        public float restedBuffRunStaminaModifier { get; internal set; } = 0;

        public float restingBuffHealthRegenMultiplier { get; internal set; } = 100;
        public float restingBuffStaminaRegenMultiplier { get; internal set; } = 200;

        public float warmBuffBuffHealthRegenMultiplier { get; internal set; } = 0;
        public float warmBuffBuffStaminaRegenMultiplier { get; internal set; } = 100;


        public float corpseRunBuffHealthRegenMultiplier { get; internal set; } = 0;
        public float corpseRunBuffStaminaRegenMultiplier { get; internal set; } = 0; 
        public float corpseRunBuffJumpStaminaModifier { get; internal set; } = -75;
        public float corpseRunBuffRunStaminaModifier { get; internal set; } = -75;

        public float freezingDebuffHealthRegenMultiplier { get; internal set; } = 0;
        public float freezingDebuffStaminaRegenMultiplier { get; internal set; } = -60;
        public float freezingDebuffDamagePerTick { get; internal set; } = -1;

        public float coldDebuffHealthRegenMultiplier { get; internal set; } = -50;
        public float coldDebuffStaminaRegenMultiplier { get; internal set; } = -25;

        public float wetDebuffHealthRegenMultiplier { get; internal set; } = -25;
        public float wetDebuffStaminaRegenMultiplier { get; internal set; } = -15;
        public float wetDebuffDuration { get; internal set; } = 120;

        public float eikthyrBuffHealthRegenMultiplier { get; internal set; } = 0;
        public float eikthyrBuffStaminaRegenMultiplier { get; internal set; } = 0;
        public float eikthyrBuffJumpStaminaModifier { get; internal set; } = -60;
        public float eikthyrBuffRunStaminaModifier { get; internal set; } = -60;
        public float eikthyrBuffCooldown { get; internal set; } = 1200;
        public float eikthyrBuffDuration { get; internal set; } = 300;
        public string eikthyrBuffModifyAttackSkill { get; internal set; } = "";
        public float eikthyrBuffDamageModifier { get; internal set; } = 0;
        public string eikthyrBuffDamageTypesModifiers { get; internal set; } = "";
        public string eikthyrBuffDescription { get; internal set; } = "";

        public float theElderBuffHealthRegenMultiplier { get; internal set; } = 0;
        public float theElderBuffStaminaRegenMultiplier { get; internal set; } = 0;
        public float theElderBuffJumpStaminaModifier { get; internal set; } = 0;
        public float theElderBuffRunStaminaModifier { get; internal set; } = 0;
        public float theElderBuffCooldown { get; internal set; } = 1200;
        public float theElderBuffDuration { get; internal set; } = 300;
        public string theElderBuffModifyAttackSkill { get; internal set; } = "WoodCutting";
        public float theElderBuffDamageModifier { get; internal set; } = 60;
        public string theElderBuffDamageTypesModifiers { get; internal set; } = "";
        public string theElderBuffDescription { get; internal set; } = "";

        public float bonemassBuffHealthRegenMultiplier { get; internal set; } = 0;
        public float bonemassBuffStaminaRegenMultiplier { get; internal set; } = 0;
        public float bonemassBuffJumpStaminaModifier { get; internal set; } = 0;
        public float bonemassBuffRunStaminaModifier { get; internal set; } = 0;
        public float bonemassBuffCooldown { get; internal set; } = 1200;
        public float bonemassBuffDuration { get; internal set; } = 300;
        public string bonemassBuffModifyAttackSkill { get; internal set; } = "";
        public float bonemassBuffDamageModifier { get; internal set; } = 0;
        public string bonemassBuffDamageTypesModifiers { get; internal set; } = "Blunt:Resistant|Slash:Resistant|Pierce:Resistant";
        public string bonemassBuffDescription { get; internal set; } = "";

        public float yagluthBuffHealthRegenMultiplier { get; internal set; } = 0;
        public float yagluthBuffStaminaRegenMultiplier { get; internal set; } = 0;
        public float yagluthBuffJumpStaminaModifier { get; internal set; } = 0;
        public float yagluthBuffRunStaminaModifier { get; internal set; } = 0;
        public float yagluthBuffCooldown { get; internal set; } = 1200;
        public float yagluthBuffDuration { get; internal set; } = 300;
        public string yagluthBuffModifyAttackSkill { get; internal set; } = "";
        public float yagluthBuffDamageModifier { get; internal set; } = 0;
        public string yagluthBuffDamageTypesModifiers { get; internal set; } = "Fire:Resistant|Frost:Resistant|Lightning:Resistant";
        public string yagluthBuffDescription { get; internal set; } = "";

        public float moderBuffHealthRegenMultiplier { get; internal set; } = 0;
        public float moderBuffStaminaRegenMultiplier { get; internal set; } = 0;
        public float moderBuffJumpStaminaModifier { get; internal set; } = 0;
        public float moderBuffRunStaminaModifier { get; internal set; } = 0;
        public float moderBuffCooldown { get; internal set; } = 1200;
        public float moderBuffDuration { get; internal set; } = 300;
        public string moderBuffModifyAttackSkill { get; internal set; } = "";
        public float moderBuffDamageModifier { get; internal set; } = 0;
        public string moderBuffDamageTypesModifiers { get; internal set; } = "";
        public string moderBuffDescription { get; internal set; } = "";
    }
}
