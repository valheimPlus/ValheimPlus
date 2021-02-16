using HarmonyLib;
using Steamworks;
using System;
using ValheimPlus.Configurations;

namespace ValheimPlus
{

    [HarmonyPatch(typeof(ZNet), "Awake")]
    public static class ChangeGameServerVariables
    {
        private static void Postfix(ref ZNet __instance)
        {
            if (Configuration.Current.Server.IsEnabled)
            {
                int maxPlayers = Configuration.Current.Server.MaxPlayers;
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
            if (Configuration.Current.Server.IsEnabled)
            {
                int maxPlayers = Configuration.Current.Server.MaxPlayers;
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
            if (Configuration.Current.Server.IsEnabled)
            {
                if (Configuration.Current.Server.DisableServerPassword)
                {
                    __result = true;
                }
            }
        }
    }

}
