using HarmonyLib;
using ValheimPlus.Configurations;
using ValheimPlus.Utility;

namespace ValheimPlus.GameClasses
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
						factor = factor + ((factor / 100) * Configuration.Current.Experience.swords);
						break;
					case SkillType.Knives:
						factor = factor + ((factor / 100) * Configuration.Current.Experience.knives);
						break;
					case SkillType.Clubs:
						factor = factor + ((factor / 100) * Configuration.Current.Experience.clubs);
						break;
					case SkillType.Polearms:
						factor = factor + ((factor / 100) * Configuration.Current.Experience.polearms);
						break;
					case SkillType.Spears:
						factor = factor + ((factor / 100) * Configuration.Current.Experience.spears);
						break;
					case SkillType.Blocking:
						factor = factor + ((factor / 100) * Configuration.Current.Experience.blocking);
						break;
					case SkillType.Axes:
						factor = factor + ((factor / 100) * Configuration.Current.Experience.axes);
						break;
					case SkillType.Bows:
						factor = factor + ((factor / 100) * Configuration.Current.Experience.bows);
						break;
					case SkillType.FireMagic:
						factor = factor + ((factor / 100) * Configuration.Current.Experience.fireMagic);
						break;
					case SkillType.FrostMagic:
						factor = factor + ((factor / 100) * Configuration.Current.Experience.frostMagic);
						break;
					case SkillType.Unarmed:
						factor = factor + ((factor / 100) * Configuration.Current.Experience.unarmed);
						break;
					case SkillType.Pickaxes:
						factor = factor + ((factor / 100) * Configuration.Current.Experience.pickaxes);
						break;
					case SkillType.WoodCutting:
						factor = factor + ((factor / 100) * Configuration.Current.Experience.woodCutting);
						break;
					case SkillType.Jump:
						factor = factor + ((factor / 100) * Configuration.Current.Experience.jump);
						break;
					case SkillType.Sneak:
						factor = factor + ((factor / 100) * Configuration.Current.Experience.sneak);
						break;
					case SkillType.Run:
						factor = factor + ((factor / 100) * Configuration.Current.Experience.run);
						break;
					case SkillType.Swim:
						factor = factor + ((factor / 100) * Configuration.Current.Experience.swim);
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
}
