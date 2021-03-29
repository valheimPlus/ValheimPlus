using HarmonyLib;
using ValheimPlus.Configurations;
using UnityEngine;

namespace ValheimPlus.GameClasses
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
        private static void Postfix(TeleportWorld __instance, string __result)
        {
            string portalName = __instance.GetText();

            if (Configuration.Current.Game.IsEnabled && Configuration.Current.Game.bigPortalNames)
            {
                __result = Localization.instance.Localize(string.Concat(new string[]
                    {
                    "$piece_portal $piece_portal_tag:",
                    " ",
                    "[",portalName,"]"
                    }));

                MessageHud.instance.ShowMessage(MessageHud.MessageType.Center, __result, 0, null);
                return;
            }
            return;
        }
    }
}
