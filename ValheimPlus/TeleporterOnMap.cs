using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HarmonyLib;
using UnityEngine;

namespace ValheimPlus
{
    // React to setting tag on portal
    // Send special Chatmessage to server
    [HarmonyPatch(typeof(TeleportWorld), "RPC_SetTag", new Type[] { typeof(long), typeof(string) })]
    public static class SyncTeleporterOnMap1
    {
        public static void Postfix(TeleportWorld __instance, long sender, string tag)
        {
            if (ZNet.instance.IsServer())
            {
                return;
            }
            ZDO temp = __instance.m_nview.GetZDO();

            ZDOMan.instance.GetZDO(temp.m_uid);

            foreach (ZDOMan.ZDOPeer peer in ZDOMan.instance.m_peers)
            {
                peer.ForceSendZDO(temp.m_uid);
            }

            // I used the ChatMessage RPC since this is one of the few calls which are received/processed on the server and can be issued on client
            ZRoutedRpc.instance.InvokeRoutedRPC(ZRoutedRpc.Everybody, "ChatMessage", new object[] { Player.m_localPlayer.GetHeadPoint(), 2, "SERVER", "PORTALUPDATE" });
        }
    }


    // Receive special chat message from client
    // Force sending new LocationIcons to all clients for map update
    //
    // /!\ /!\ /!\ /!\ /!\ /!\ /!\ /!\ /!\ /!\ /!\ /!\ /!\ /!\ /!\ /!\ /!\ /!\ /!\ /!\ /!\ /!\ /!\ /!\
    // Prevent further execution, if it is a special message (name == "SERVER", text == "PORTALUPDATE")
    // /!\ /!\ /!\ /!\ /!\ /!\ /!\ /!\ /!\ /!\ /!\ /!\ /!\ /!\ /!\ /!\ /!\ /!\ /!\ /!\ /!\ /!\ /!\ /!\
    [HarmonyPatch(typeof(Chat), "RPC_ChatMessage", new Type[] { typeof(long), typeof(Vector3), typeof(int), typeof(string), typeof(string) })]
    public static class SyncTeleporterOnMap2
    {
        public static bool Prefix(Chat __instance, long sender, string name, string text)
        {
            if (text == "PORTALUPDATE" && name == "SERVER")
            {
                if (ZNet.instance.IsServer())
                {
                    Task.Factory.StartNew(() =>
                    {
                        Thread.Sleep(5000);
                        foreach (var peer in ZNet.instance.m_peers)
                        {
                            if (!peer.m_server)
                            {
                                ZoneSystem.instance.SendLocationIcons(peer.m_uid);
                            }
                        }
                    });
                }
                return false;
            }

            return true;
        }
    }

    // Hook GetLocationIcons and add all portal locations with prefix "PORTAL"
    [HarmonyPatch(typeof(ZoneSystem), "GetLocationIcons", new Type[] { typeof(Dictionary<Vector3, string>) })]
    public static class SyncTeleporterOnMap3
    {
        // Only serverside
        public static void Prefix(ref ZoneSystem __instance, ref Dictionary<Vector3, string> icons)
        {
            if (!ZNet.instance.IsServer())
            {
                return;
            }

            List<ZDO> connected = new List<ZDO>();
            Dictionary<string, ZDO> unconnected = new Dictionary<string, ZDO>();

            foreach (List<ZDO> zdoarray in ZDOMan.instance.m_objectsBySector)
            {
                if (zdoarray != null)
                {
                    foreach (ZDO zdo in zdoarray.Where(x => x.m_prefab == -661882940))
                    {
                        string tag = zdo.GetString("tag");

                        if (!unconnected.ContainsKey(tag))
                        {
                            unconnected.Add(tag, zdo);
                        }
                        else
                        {
                            connected.Add(zdo);
                            connected.Add(unconnected[tag]);
                            unconnected.Remove(tag);
                        }

                    }
                }
            }

            // Add icons for connected portals
            // prefix connected names with a *
            foreach (var zdo in connected)
            {
                icons[zdo.m_position] = "PORTAL* " + zdo.GetString("tag");
            }

            // Add icons for unconnected portals
            // no prefix
            foreach (var zdo in unconnected.Values)
            {
                icons[zdo.m_position] = "PORTAL" + zdo.GetString("tag");
            }
        }

        // Holder for our pins
        private static List<Minimap.PinData> addedPins;

        // only client-side
        public static void Postfix(ref ZoneSystem __instance, ref Dictionary<Vector3, string> icons)
        {
            if (!ZNet.instance.IsServer())
            {
                if (addedPins == null)
                {
                    addedPins = new List<Minimap.PinData>();
                }

                Dictionary<Vector3, string> checkList = new Dictionary<Vector3, string>();
                
                // Add all portal 'icons' to our checklist
                foreach (var key in icons.Keys.ToArray())
                {
                    if (icons[key].StartsWith("PORTAL"))
                    {
                        checkList.Add(key, icons[key].Substring(6));
                        icons.Remove(key);
                    }
                }

                foreach (var kv in checkList)
                {
                    // Was pin already added?
                    Minimap.PinData foundPin = addedPins.FirstOrDefault(x => x.m_pos == kv.Key);
                    if (foundPin != null)
                    {
                        // Did the pin's name change?
                        if (foundPin.m_name != kv.Value)
                        {
                            // Remove pin at location and readd with new name
                            Minimap.instance.RemovePin(foundPin);
                            addedPins.Remove(foundPin);
                            addedPins.Add(Minimap.instance.AddPin(kv.Key, Minimap.PinType.Icon4, kv.Value, false, false));
                        }
                    }
                    else
                    {
                        // Add new pin
                        addedPins.Add(Minimap.instance.AddPin(kv.Key, Minimap.PinType.Icon4, kv.Value, false, false));
                    }
                }

                // Remove destroyed portals from map
                // doesn't really react on portal destruction, only works if after a portal was destroyed, someone set the name on another portal
                foreach (var kv in addedPins.ToList())
                {
                    if (checkList.All(x => x.Key != kv.m_pos))
                    {
                        Minimap.instance.RemovePin(kv);
                        addedPins.Remove(kv);
                    }
                }
            }
        }
    }
}