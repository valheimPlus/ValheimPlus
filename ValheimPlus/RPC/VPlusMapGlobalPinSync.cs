using System;
using System.Collections.Generic;
using UnityEngine;
using ValheimPlus.GameClasses;

namespace ValheimPlus.RPC
{
    /// <summary>
    /// Sync map pins between clients via server
    /// </summary>
    internal enum AllowedPinTypes
    {
        Icon0 = Minimap.PinType.Icon0,
        Icon1 = Minimap.PinType.Icon1,
        Icon2 = Minimap.PinType.Icon2,
        Icon3 = Minimap.PinType.Icon3,
        Icon4 = Minimap.PinType.Icon4
    }

    internal class CachedPin
    {
        public Vector3 m_pos;
        public Minimap.PinType m_type;
        public string m_name;
        public bool m_save;
        public bool m_checked;
    }

    internal class NetworkPinPackageInfo
    {
        public long senderName;
        public int count;
    }

    internal class NetworkPinPackageData
    {
        public NetworkPinPackageInfo info;
        public List<CachedPin> pinList;
    }

    internal class VPlusMapGlobalPinSync
    {
        public static List<CachedPin> pinsCache = new List<CachedPin>();

        public static List<Minimap.PinData> cientMapPins
        {
            get
            {
                List<Minimap.PinData> filtered = new List<Minimap.PinData>();

                foreach (var pin in Minimap.instance.m_pins)
                {
                    if (Enum.IsDefined(typeof(AllowedPinTypes), (AllowedPinTypes) pin.m_type))
                    {
                        filtered.Add(pin);
                    }
                }

                return filtered;
            }
        }

        public static void RPC_VPlusMapPinsSync(long sender, ZPackage mapPinPkg)
        {
            if (mapPinPkg == null)
            {
                ZLog.LogWarning("MapPinsSync: Got empty map pin package from server.");
                return;
            }

            if (ZNet.instance.IsServer())
            {
                ServerAction(sender, mapPinPkg);
            }
            else
            {
                ClientAction(sender, mapPinPkg);
            }
        }

        public static void Initialize(bool keepQuiet = false)
        {
            ZLog.Log("--- VPlus Initialize Global Pins Sync");

            if (cientMapPins.Count == 0) return;

            var packedPins = PackRawPins(cientMapPins);

            ZLog.Log($"MapPinsSync: SENDING ({cientMapPins.Count}) Map Pins to server");

            ZRoutedRpc.instance.InvokeRoutedRPC(
                ZRoutedRpc.instance.GetServerPeerID(),
                Game_Start_Patch.ActionNameMapGlobalPinSync,
                new object[] {packedPins}
            );

            ZLog.Log($"MapPinsSync: Sent {cientMapPins.Count} pins to the server");
        }

        private static void ServerAction(long sender, ZPackage clientPinPackage)
        {
            if (sender == ZRoutedRpc.instance.GetServerPeerID()) return; // Don't  process own broadcasted data
            if (clientPinPackage == null) return;

            ZLog.Log("MapPinsSync: Processing PINS from CLIENT");

            NetworkPinPackageData unpackedData = UnpackNetworkPackage(clientPinPackage);

            ZLog.Log($"MapPinsSync: pins sender : {unpackedData.info.senderName}");

            if (unpackedData.info.senderName == ZRoutedRpc.instance.m_id) return;

            MergePinsWithCache(unpackedData);
            BroadcastPins(sender);
        }

        private static void BroadcastPins(long sender)
        {
            var mapPinPkg = PackCachedPins(pinsCache);

            if (mapPinPkg == null)
            {
                ZLog.LogError("MapPinsSync: Error sending map pins to the clients!");
                return;
            }

            foreach (ZNetPeer peer in ZRoutedRpc.instance.m_peers)
            {
                // if (peer.m_uid == sender) continue;

                ZRoutedRpc.instance.InvokeRoutedRPC(
                    peer.m_uid,
                    Game_Start_Patch.ActionNameMapGlobalPinSync,
                    new object[] {mapPinPkg}
                );
            }

            ZLog.Log($"MapPinsSync: Sent {pinsCache.Count} pins to all clients");
        }

        private static void ClientAction(long sender, ZPackage mapPinPkg)
        {
            if (sender != ZRoutedRpc.instance.GetServerPeerID()) return; //Only bother if it's from the server.

            ZLog.Log("MapPinsSync: Processing pins from SERVER");

            NetworkPinPackageData unpackedData = UnpackNetworkPackage(mapPinPkg);

            MergePinsWithCache(unpackedData);
            UpdateClientMinimap(unpackedData, unpackedData.info.senderName);
        }


        private static void UpdateClientMinimap(NetworkPinPackageData data, long pinSender)
        {
            ZLog.Log("MapPinsSync: Updating CLIENT map");

            int count = 0;

            foreach (var serverPin in data.pinList)
            {
                bool inCache = CheckPinExistsInCache(serverPin);
                bool onMap = Minimap.instance.HaveSimilarPin(
                    serverPin.m_pos,
                    serverPin.m_type,
                    serverPin.m_name,
                    serverPin.m_save
                );

                if (inCache && onMap) continue;

                pinsCache.Add(serverPin);

                Minimap.instance.AddPin(
                    serverPin.m_pos,
                    serverPin.m_type,
                    serverPin.m_name,
                    serverPin.m_save,
                    serverPin.m_checked
                );

                count++;
            }

            Minimap.instance.UpdatePins();

            ZLog.LogWarning($"MapPinsSync: Added {count} pins from {pinSender}!");
        }

        private static ZPackage PackRawPins(List<Minimap.PinData> pinsData)
        {
            ZPackage pkg = new ZPackage();

            if (pinsData.Count == 0) return null;

            try
            {
                pkg.Write(ZRoutedRpc.instance.m_id); // Write sender ID


                ZLog.Log($"MapPinsSync: Writing sender ID to package: {ZRoutedRpc.instance.m_id}");

                List<CachedPin> packedPins = new List<CachedPin>();

                ZLog.Log($"MapPinsSync: Writing pins to package...");

                foreach (var pin in pinsData)
                {
                    packedPins.Add(new CachedPin()
                    {
                        m_name = pin.m_name,
                        m_pos = pin.m_pos,
                        m_save = pin.m_save,
                        m_type = pin.m_type,
                        m_checked = pin.m_checked
                    });
                }


                pkg.Write(packedPins.Count); // Write pins count

                foreach (var packedPin in packedPins)
                {
                    pkg.Write(packedPin.m_name);
                    pkg.Write(packedPin.m_pos);
                    pkg.Write(packedPin.m_save);
                    pkg.Write((int) packedPin.m_type);
                    pkg.Write(packedPin.m_checked);
                }

                ZLog.Log($"MapPinsSync: Packed {packedPins.Count} pins");
                return pkg;
            }
            catch
            {
                ZLog.LogError("MapPinsSync: PACKING ERROR");
                return null;
            }
        }


        private static ZPackage PackCachedPins(List<CachedPin> pinsData)
        {
            ZPackage pkg = new ZPackage();

            if (pinsData.Count == 0) return null;

            try
            {
                pkg.Write(ZRoutedRpc.instance.m_id); // Write sender ID
                pkg.Write(pinsData.Count); // Write pins count

                foreach (var packedPin in pinsData)
                {
                    pkg.Write(packedPin.m_name);
                    pkg.Write(packedPin.m_pos);
                    pkg.Write(packedPin.m_save);
                    pkg.Write((int) packedPin.m_type);
                    pkg.Write(packedPin.m_checked);
                }

                ZLog.Log($"MapPinsSync: Packed {pinsData.Count} pins");
                return pkg;
            }
            catch
            {
                ZLog.LogError("MapPinsSync: PACKING ERROR");
                return null;
            }
        }

        private static NetworkPinPackageData UnpackNetworkPackage(ZPackage package)
        {
            List<CachedPin> list = new List<CachedPin>();

            long pinSender = package.ReadLong();
            int pinCount = package.ReadInt();
            int count = 0;

            for (var i = 0; i < pinCount; i++)
            {
                string m_name = package.ReadString();
                Vector3 m_pos = package.ReadVector3();
                bool m_save = package.ReadBool();
                Minimap.PinType m_type = (Minimap.PinType) package.ReadInt();
                bool m_checked = package.ReadBool();

                CachedPin pin = new CachedPin()
                {
                    m_name = m_name,
                    m_pos = m_pos,
                    m_save = m_save,
                    m_type = m_type,
                    m_checked = m_checked
                };

                list.Add(pin);

                if (CheckPinExistsInCache(pin)) continue;

                pinsCache.Add(pin);
                count++;
            }

            package.m_stream.Position = 0;
            package.m_stream.Flush();

            return new NetworkPinPackageData()
            {
                info = new NetworkPinPackageInfo()
                {
                    senderName = pinSender,
                    count = pinCount
                },
                pinList = list
            };
        }

        private static bool CheckPinExistsInCache(CachedPin pin)
        {
            return pinsCache.Exists(
                pinCached => Utils.DistanceXZ(pin.m_pos, pinCached.m_pos) < 5.0
            );
        }

        /*
         * merge acquired pins with local cache 
         */
        private static void MergePinsWithCache(NetworkPinPackageData data)
        {
            int count = 0;

            foreach (var networkPin in data.pinList)
            {
                CachedPin pin = new CachedPin()
                {
                    m_name = networkPin.m_name,
                    m_pos = networkPin.m_pos,
                    m_save = networkPin.m_save,
                    m_type = networkPin.m_type,
                    m_checked = networkPin.m_checked
                };

                if (CheckPinExistsInCache(pin)) continue;

                pinsCache.Add(pin);
                count++;
            }

            ZLog.Log($"MapPinsSync: Merged {count} PINS");
        }
    }
}