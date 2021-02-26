using HarmonyLib;
using ValheimPlus.Configurations;
using UnityEngine;

namespace ValheimPlus
{
    class SkillsModification
    {
        [HarmonyPatch(typeof(Skills), "LowerAllSkills")]
        public static class ChangeSkillLossOnDeath
        {
            private static void Prefix(ref float factor)
            {
                if (Configuration.Current.Skills.IsEnabled)
                {
                    float decreaseFactor = Configuration.Current.Skills.skillDeathLowerFactor;
                    factor = decreaseFactor;
                }
            }
        }
    }
}
