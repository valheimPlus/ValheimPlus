using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HarmonyLib;
using UnityEngine;

namespace ValheimPlus
{
    [HarmonyPatch(typeof(Game), "Start")]
    public static class GameStartPatchSyncTeleporterOnMap
    {
        private static void Prefix()
        {
            //Config Sync
            ZRoutedRpc.instance.Register(nameof(VPlusTeleporterSync.RPC_VPlusTeleporterSync).Substring(4), new Action<long, ZPackage>(VPlusTeleporterSync.RPC_VPlusTeleporterSync));
        }
    }


    public class VPlusTeleporterSync
    {
        // Holder for our pins
        public static List<Minimap.PinData> addedPins = new List<Minimap.PinData>();

        public static Minimap.PinData AddPinHelper(
            Vector3 pos,
            Minimap.PinType type,
            string name,
            bool save,
            bool isChecked)
        {
            Minimap.PinData pinData = new Minimap.PinData();
            pinData.m_type = type;
            pinData.m_name = name;
            pinData.m_pos = pos;
            pinData.m_icon = Minimap.instance.GetSprite(type);
            pinData.m_save = save;
            pinData.m_checked = isChecked;
            return pinData;
        }


        public static void RPC_VPlusTeleporterSync(long sender, ZPackage teleporterZPackage)
        {
            // SERVER SIDE
            if (ZNet.instance.IsServer())
            {
                // Create package and send it to all clients
                ZPackage package = new ZPackage();

                // Collect all portal locations/names
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

                package.Write(connected.Count);
                foreach (var connectedPortal in connected)
                {
                    package.Write(connectedPortal.m_position);
                    package.Write("*" + connectedPortal.GetString("tag"));
                }

                package.Write(unconnected.Count);
                foreach (var unconnectedPortal in unconnected)
                {
                    package.Write(unconnectedPortal.Value.m_position);
                    package.Write(unconnectedPortal.Value.GetString("tag"));
                }

                // Send only to single client (new peer) if zPackage is null
                if (teleporterZPackage == null)
                {
                    ZLog.Log("Sending portal information to client (new peer)");
                    // Send to single client (on new connections only)
                    ZRoutedRpc.instance.InvokeRoutedRPC(sender, nameof(RPC_VPlusTeleporterSync).Substring(4), new object[] { package });
                }
                else
                {
                    ZLog.Log("Sending portal information to all clients");
                    foreach (var peer in ZNet.instance.m_peers)
                    {
                        if (!peer.m_server)
                        {
                            // Send to all clients
                            ZRoutedRpc.instance.InvokeRoutedRPC(peer.m_uid, nameof(RPC_VPlusTeleporterSync).Substring(4), new object[] { package });
                        }
                    }
                }
            }

            // CLIENT SIDE
            else
            {
                if (teleporterZPackage != null && teleporterZPackage.Size() > 0 && sender == ZRoutedRpc.instance.GetServerPeerID())
                {
                    int numConnectedPortals = teleporterZPackage.ReadInt();

                    Dictionary<Vector3, string> checkList = new Dictionary<Vector3, string>();

                    // prevent MT crashing
                    lock (addedPins)
                    {
                        while (numConnectedPortals > 0)
                        {
                            Vector3 portalPosition = teleporterZPackage.ReadVector3();
                            string portalName = teleporterZPackage.ReadString();

                            checkList.Add(portalPosition, portalName);

                            // Was pin already added?
                            Minimap.PinData foundPin = addedPins.FirstOrDefault(x => x.m_pos == portalPosition);
                            if (foundPin != null)
                            {
                                // Did the pin's name change?
                                if (foundPin.m_name != portalName)
                                {
                                    // Remove pin at location and readd with new name
                                    Minimap.instance.RemovePin(foundPin);
                                    addedPins.Remove(foundPin);
                                    addedPins.Add(AddPinHelper(portalPosition, Minimap.PinType.Icon4, portalName, false, false));
                                }
                            }
                            else
                            {
                                // Add new pin
                                addedPins.Add(AddPinHelper(portalPosition, Minimap.PinType.Icon4, portalName, false, false));
                            }

                            numConnectedPortals--;
                        }

                        int numUnconnectedPortals = teleporterZPackage.ReadInt();

                        while (numUnconnectedPortals > 0)
                        {
                            Vector3 portalPosition = teleporterZPackage.ReadVector3();
                            string portalName = teleporterZPackage.ReadString();

                            checkList.Add(portalPosition, portalName);

                            // Was pin already added?
                            Minimap.PinData foundPin = addedPins.FirstOrDefault(x => x.m_pos == portalPosition);
                            if (foundPin != null)
                            {
                                // Did the pin's name change?
                                if (foundPin.m_name != portalName)
                                {
                                    // Remove pin at location and readd with new name
                                    addedPins.Remove(foundPin);
                                    addedPins.Add(AddPinHelper(portalPosition, Minimap.PinType.Icon4, portalName, false, false));
                                }
                            }
                            else
                            {
                                // Add new pin
                                addedPins.Add(AddPinHelper(portalPosition, Minimap.PinType.Icon4, portalName, false, false));
                            }

                            numUnconnectedPortals--;
                        }

                        // Remove destroyed portals from map
                        // doesn't really react on portal destruction, only works if after a portal was destroyed, someone set the name on another portal
                        foreach (var kv in addedPins.ToList())
                        {
                            if (checkList.All(x => x.Key != kv.m_pos))
                            {
                                addedPins.Remove(kv);
                            }
                        }
                    }
                }
            }
        }
    }




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

            // Force sending ZDO to server
            ZDO temp = __instance.m_nview.GetZDO();

            ZDOMan.instance.GetZDO(temp.m_uid);

            ZDOMan.instance.GetPeer(ZRoutedRpc.instance.GetServerPeerID()).ForceSendZDO(temp.m_uid);
            Task.Factory.StartNew(() =>
            {
                // Wait for ZDO to be sent else server won't have accurate information to send back
                Thread.Sleep(5000);
                // Send trigger to server
                ZLog.Log("Sending message to server to trigger delivery of map icons after renaming portal");
                ZRoutedRpc.instance.InvokeRoutedRPC(ZRoutedRpc.instance.GetServerPeerID(), nameof(VPlusTeleporterSync.RPC_VPlusTeleporterSync).Substring(4),
                    new object[] { new ZPackage() });
            });
        }
    }


    // CLIENT SIDE
    [HarmonyPatch(typeof(Minimap), "SetMapData")]
    public static class SyncTeleporterOnMap2
    {
        public static void Postfix()
        {
            if (ZNet.m_isServer)
            {
                return;
            }

            ZLog.Log("Sending message to server to trigger delivery of map icons");
            ZRoutedRpc.instance.InvokeRoutedRPC(ZRoutedRpc.instance.GetServerPeerID(), nameof(VPlusTeleporterSync.RPC_VPlusTeleporterSync).Substring(4),
                new object[] { new ZPackage() });

        }
    }

    // CLIENT SIDE Set map pins after UpdateLocationPins
    [HarmonyPatch(typeof(Minimap), "UpdateLocationPins")]
    public static class SyncTeleporterOnMap3
    {
        public static void Postfix()
        {
            if (ZNet.m_isServer)
            {
                return;
            }

            List<Minimap.PinData> copy;

            lock (VPlusTeleporterSync.addedPins)
            {
                copy = VPlusTeleporterSync.addedPins.ToList();
            }

            foreach (var pin in copy)
            {
                Minimap.PinData foundPin = Minimap.instance.m_pins.FirstOrDefault(x => x.m_pos == pin.m_pos);

                if (foundPin == null)
                {
                    // Pin not on map, add
                    Minimap.instance.AddPin(pin.m_pos, pin.m_type, pin.m_name, pin.m_save, pin.m_checked);
                }
                else if (foundPin.m_name != pin.m_name)
                {
                    // Pin name change, remove and add new
                    Minimap.instance.RemovePin(foundPin);
                    Minimap.instance.AddPin(pin.m_pos, pin.m_type, pin.m_name, pin.m_save, pin.m_checked);
                }
            }


            foreach (var pin in Minimap.instance.m_pins.Where(x => !x.m_save).ToList())
            {
                // Do nothing if it is a location pin
                if (Minimap.instance.m_locationPins.Values.Any(x => x.m_pos == pin.m_pos))
                {
                    continue;
                }

                if (copy.All(x => x.m_pos != pin.m_pos))
                {
                    Minimap.instance.RemovePin(pin);
                }
            }
        }
    }
}