using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ValheimPlus.Configurations;

namespace ValheimPlus
{
	[HarmonyPatch(typeof(Skills), "RaiseSkill")]
	public static class AddExpGainedDisplay
	{
		/// <summary>
		/// Updates experience modifiers
		/// </summary>
		private static void Prefix(ref Skills __instance, ref Skills.SkillType skillType, ref float factor)
		{
			if (Configuration.Current.Experience.IsEnabled)
			{
				switch ((SkillType)skillType)
				{
					case SkillType.Swords:
						factor = Helper.applyModifierValue(factor, Configuration.Current.Experience.swords);
						break;
					case SkillType.Knives:
						factor = Helper.applyModifierValue(factor, Configuration.Current.Experience.knives);
						break;
					case SkillType.Clubs:
						factor = Helper.applyModifierValue(factor, Configuration.Current.Experience.clubs);
						break;
					case SkillType.Polearms:
						factor = Helper.applyModifierValue(factor, Configuration.Current.Experience.polearms);
						break;
					case SkillType.Spears:
						factor = Helper.applyModifierValue(factor, Configuration.Current.Experience.spears);
						break;
					case SkillType.Blocking:
						factor = Helper.applyModifierValue(factor, Configuration.Current.Experience.blocking);
						break;
					case SkillType.Axes:
						factor = Helper.applyModifierValue(factor, Configuration.Current.Experience.axes);
						break;
					case SkillType.Bows:
						factor = Helper.applyModifierValue(factor, Configuration.Current.Experience.bows);
						break;
					case SkillType.FireMagic:
						factor = Helper.applyModifierValue(factor, Configuration.Current.Experience.fireMagic);
						break;
					case SkillType.FrostMagic:
						factor = Helper.applyModifierValue(factor, Configuration.Current.Experience.frostMagic);
						break;
					case SkillType.Unarmed:
						factor = Helper.applyModifierValue(factor, Configuration.Current.Experience.unarmed);
						break;
					case SkillType.Pickaxes:
						factor = Helper.applyModifierValue(factor, Configuration.Current.Experience.pickaxes);
						break;
					case SkillType.WoodCutting:
						factor = Helper.applyModifierValue(factor, Configuration.Current.Experience.woodCutting);
						break;
					case SkillType.Jump:
						factor = Helper.applyModifierValue(factor, Configuration.Current.Experience.jump);
						break;
					case SkillType.Sneak:
						factor = Helper.applyModifierValue(factor, Configuration.Current.Experience.sneak);
						break;
					case SkillType.Run:
						factor = Helper.applyModifierValue(factor, Configuration.Current.Experience.run);
						break;
					case SkillType.Swim:
						factor = Helper.applyModifierValue(factor, Configuration.Current.Experience.swim);
						break;
					default:
						break;
				}
			}
		}

		/// <summary>
		/// Experience gained notifications
		/// </summary>
		private static void Postfix(Skills __instance, Skills.SkillType skillType, float factor = 1f)
		{
			if (Configuration.Current.Hud.IsEnabled && Configuration.Current.Hud.experienceGainedNotifications)
			{
				Skills.Skill skill = __instance.GetSkill(skillType);
				float percent = skill.m_accumulator / (skill.GetNextLevelRequirement() / 100);
				__instance.m_player.Message(MessageHud.MessageType.TopLeft, skill.m_info.m_skill + " [" + Helper.tFloat(skill.m_accumulator, 2) + "/" + Helper.tFloat(skill.GetNextLevelRequirement(), 2) + "] (" + Helper.tFloat(percent, 0) + "%)", 0, skill.m_info.m_icon);
			}
		}
	}

	public enum SkillType
	{
		None,
		Swords,
		Knives,
		Clubs,
		Polearms,
		Spears,
		Blocking,
		Axes,
		Bows,
		FireMagic,
		FrostMagic,
		Unarmed,
		Pickaxes,
		WoodCutting,
		Jump = 100,
		Sneak,
		Run,
		Swim,
		All = 999
	}

	[HarmonyPatch(typeof(Skills), nameof(Skills.OnDeath))]
	public static class Skills_OnDeath_Transpiler
	{
		private static MethodInfo method_Skills_LowerAllSkills = AccessTools.Method(typeof(Skills), nameof(Skills.LowerAllSkills));
		private static MethodInfo method_LowerAllSkills = AccessTools.Method(typeof(Skills_OnDeath_Transpiler), nameof(Skills_OnDeath_Transpiler.LowerAllSkills));

		/// <summary>
		/// We replace the call to Skills.LowerAllSkills with our own stub, which then applies the death multiplier.
		/// </summary>
		[HarmonyTranspiler]
		public static IEnumerable<CodeInstruction> Transpile(IEnumerable<CodeInstruction> instructions)
		{
			if (!Configuration.Current.Player.IsEnabled) return instructions;

			List<CodeInstruction> il = instructions.ToList();

			for (int i = 0; i < il.Count; ++i)
			{
				if (il[i].Calls(method_Skills_LowerAllSkills))
				{
					il[i].operand = method_LowerAllSkills;
				}
			}

			return il.AsEnumerable();
		}

		public static void LowerAllSkills(Skills instance, float factor)
		{
			if (Configuration.Current.Player.deathPenaltyMultiplier > -100.0f)
			{
				instance.LowerAllSkills(Helper.applyModifierValue(factor, Configuration.Current.Player.deathPenaltyMultiplier));
			}
		}
	}
}
