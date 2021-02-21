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

            /*
            private static void Prefix(Skills __instance, Skills.SkillType skillType, float factor = 1f)
            {
               

            }*/

            private static void Postfix(Skills __instance, Skills.SkillType skillType, float factor = 1f)
            {
                if (Configuration.Current.Player.IsEnabled && Configuration.Current.Player.ExperienceGainedNotifications)
                {
                    Skills.Skill skill = __instance.GetSkill(skillType);
                    float percent = skill.m_accumulator / (skill.GetNextLevelRequirement() / 100);
                    __instance.m_player.Message(MessageHud.MessageType.TopLeft, skill.m_info.m_skill + " [" + Helper.tFloat(skill.m_accumulator, 2) + "/" + Helper.tFloat(skill.GetNextLevelRequirement(), 2) + "] (" + Helper.tFloat(percent, 0) + "%)", 0, skill.m_info.m_icon);
                }
            }

        }
        
    }
}
