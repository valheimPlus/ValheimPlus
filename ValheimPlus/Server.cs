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

namespace ValheimPlus
{

    [HarmonyPatch(typeof(ZNet), "Awake")]
    public static class ChangeGameServerVariables
    {
        private static void Postfix(ref ZNet __instance)
        {
            if (Settings.isEnabled("Server"))
            {
                int maxPlayers = Settings.getInt("Server", "maxPlayers");
                if (maxPlayers >= 1)
                {
                    // Set Server Instance Max Players
                    __instance.m_serverPlayerLimit = maxPlayers;
                }
            }

        }

    }
    [HarmonyPatch(typeof(SteamGameServer), "SetMaxPlayerCount")]
    public static class ChangeSteamServerVariables
    {
        private static void Prefix(ref int cPlayersMax)
        {
            if (Settings.isEnabled("Server"))
            {
                int maxPlayers = Settings.getInt("Server", "maxPlayers");
                if (maxPlayers >= 1)
                {
                    cPlayersMax = maxPlayers;
                }
            }

        }

    }
    [HarmonyPatch(typeof(FejdStartup), "IsPublicPasswordValid")]
    public static class ChangeServerPasswordBehavior
    {

        private static void Postfix(ref Boolean __result) // Set after awake function
        {
            if (Settings.isEnabled("Server"))
            {
                if (Settings.getBool("Server", "disableServerPassword"))
                {
                    __result = true;
                }
            }
        }
    }


}
