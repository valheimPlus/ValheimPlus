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
    [HarmonyPatch(typeof(Chat), nameof(Chat.OnNewChatMessage))]
    public static class Chat_AddInworldText_Patch
    {
        private static bool Prefix(ref Chat __instance, GameObject go, long senderID, Vector3 pos, Talker.Type type, string user, string text)
        {

            if (Configuration.Current.Chat.IsEnabled)
            {
                Player Author = Helper.getPlayerBySenderId(senderID);

                // Handle Ping Changes
                if (type == Talker.Type.Ping && Configuration.Current.Chat.pingDistance > 1)
                {
                    text = text.Replace('<', ' ');
                    text = text.Replace('>', ' ');

                    // Restrict the ping display to players only within a certain radius of the creator of the ping
                    if (Vector3.Distance(Author.transform.position, Player.m_localPlayer.transform.position) >= Configuration.Current.Chat.pingDistance)
                    {
                        // exit
                        return false;
                    }

                    __instance.AddInworldText(go, senderID, pos, type, user, text);
                    return false;
                }

                // Handle Shout distances and types
                if (type == Talker.Type.Shout && Configuration.Current.Chat.shoutDistance > 1)
                {
                    // Restrict the shout display to players only within a certain radius 
                    if (Vector3.Distance(Author.transform.position, Player.m_localPlayer.transform.position) <= Configuration.Current.Chat.shoutDistance)
                    {
                        // rerturn to default behavior
                        return true;
                    }
                    else
                    {
                        // Only add string to chat window and show no ping
                        if (Configuration.Current.Chat.outOfRangeShoutsDisplayInChatWindow)
                            __instance.AddString(user, text, Talker.Type.Shout);
                        return false;
                    }
                }


            }
            return true;
        }

    }



    [HarmonyPatch(typeof(Chat), nameof(Chat.AddInworldText))]
    public static class Chat_AddInworldText_Transpiler
    {
        /// <summary>
        //  Replaces all enforced case conversions for Shouts and Whispters
        /// </summary>
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            if (!Configuration.Current.Chat.IsEnabled || Configuration.Current.Chat.forcedCase)
                return instructions;

            List<CodeInstruction> il = instructions.ToList();
            il = Helper.removeForcedCaseFunctionCalls(il);
            return il.AsEnumerable();
        }


    }

    [HarmonyPatch(typeof(Chat), nameof(Chat.AddString))]
    public static class Chat_AddString_Transpiler
    {
        /// <summary>
        //  Replaces all enforced case conversions for Shouts and Whispters
        /// </summary>
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            if (!Configuration.Current.Chat.IsEnabled || Configuration.Current.Chat.forcedCase)
               return instructions;

            List<CodeInstruction> il = instructions.ToList();
            il = Helper.removeForcedCaseFunctionCalls(il);
            return il.AsEnumerable();
        }


    }




}
