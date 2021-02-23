using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BepInEx;
using Unity;
using UnityEngine;
using System.IO;
using System.Reflection;
using System.Runtime;
using IniParser;
using IniParser.Model;
using HarmonyLib;
using System.Globalization;
using Steamworks;
using ValheimPlus;
using UnityEngine.Rendering;
using UnityEngine.UI;
using ValheimPlus.Configurations;

namespace ValheimPlus
{
    class Experience
    {

		[HarmonyPatch(typeof(Skills), "RaiseSkill")]
		public static class AddExpGainedDisplay
		{
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
}
