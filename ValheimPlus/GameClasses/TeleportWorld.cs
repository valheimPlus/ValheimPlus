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
    /// <summary>
    /// Show's portal names in big text
    /// </summary>
    [HarmonyPatch(typeof(TeleportWorld), "GetHoverText")]
    public static class TeleportWorld_bigPortalText_Patch
    {
        private static bool Prefix(ref TeleportWorld __instance, ref string __result)
        {
            string portalName = __instance.GetText();
            string ConnectionStatus = __instance.HaveTarget() ? "$piece_portal_connected" : "$piece_portal_unconnected";

            if (Configuration.Current.Game.IsEnabled && Configuration.Current.Game.bigPortalNames)
            {
                __result = Localization.instance.Localize(string.Concat(new string[]
                    {
                    "$piece_portal $piece_portal_tag:\"",
                    portalName,
                    "\"  [",
                    ConnectionStatus,
                    "]\n[<color=yellow><b>$KEY_Use</b></color>] $piece_portal_settag"
                    }));

                string BigPortalName = Localization.instance.Localize(string.Concat(new string[]
                    {
                    "$piece_portal $piece_portal_tag:",
                    " ",
                    "[",portalName,"]"
                    }));

                MessageHud.instance.ShowMessage(MessageHud.MessageType.Center, BigPortalName, 0, null);
                return true;
            }
            __result = Localization.instance.Localize(string.Concat(new string[]
                    {
                    "$piece_portal $piece_portal_tag:\"",
                    portalName,
                    "\"  [",
                    ConnectionStatus,
                    "]\n[<color=yellow><b>$KEY_Use</b></color>] $piece_portal_settag"
                    }));
            return false;
        }
    }
}
