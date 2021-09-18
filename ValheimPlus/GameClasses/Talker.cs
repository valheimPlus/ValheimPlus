using HarmonyLib;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;
using ValheimPlus.Configurations;
using HarmonyLib;

namespace ValheimPlus.GameClasses
{

    /// <summary>
    /// Change Ping and global message behavior
    /// </summary>
    [HarmonyPatch(typeof(Talker), nameof(Talker.Awake))]
    public static class Chat_Awake_Patch
    {
        private static bool Prefix(ref Talker __instance)
        {

            if (Configuration.Current.Chat.IsEnabled)
            {
                __instance.m_visperDistance = Configuration.Current.Chat.defaultWhisperDistance;
                __instance.m_normalDistance = Configuration.Current.Chat.defaultNormalDistance;
                __instance.m_shoutDistance = Configuration.Current.Chat.defaultShoutDistance;
            }
            return true;
        }

    }






}
