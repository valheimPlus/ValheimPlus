using HarmonyLib;
using ValheimPlus.Configurations;
using UnityEngine;

namespace ValheimPlus
{

    /// <summary>
    /// Disable portals
    /// </summary>
    [HarmonyPatch(typeof(TeleportWorld), "Teleport", new System.Type[] { typeof(Player) })]
    public static class TeleportWorld_Teleport_Patch
    {
        private static bool Prefix(ref TeleportWorld __instance, ref Player player)
        {
            if (Configuration.Current.Game.IsEnabled && Configuration.Current.Game.disablePortals)
            {
                MessageHud.instance.ShowMessage(MessageHud.MessageType.Center, "Portals have been disabled on this Server.");
                return false;
            }

            return true;
        }
    }
}
