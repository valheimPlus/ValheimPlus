using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ValheimPlus.GameClasses;
using ValheimPlus.Utility;

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

    internal class NetworkPinPackageData
    {
        public class Info
        {
            public long senderName;
            public int totalChunks;
            public int chunkPinsCount;
            public bool isLast;
        }

        public Info info;
        public List<CachedPin> pinsList;
    }


    internal class VPlusMapGlobalPinSync
    {
        public static List<CachedPin> pinsCache = new List<CachedPin>();
        private static int chunkSize = 50;

        public static List<CachedPin> cientMapPins
        {
            get
            {
                List<CachedPin> filtered = new List<CachedPin>();

                foreach (var pin in Minimap.instance.m_pins)
                {
                    if (Enum.IsDefined(typeof(AllowedPinTypes), (AllowedPinTypes) pin.m_type))
                    {
                        CachedPin cachedPin = ConvertRawToCachedPin(pin);

                        filtered.Add(cachedPin);
                    }
                }

                return filtered;
            }
        }

        public static void RPC_VPlusMapGlobalPinSyncServer(long sender, ZPackage mapPinPkg)
        {
            if (mapPinPkg == null)
            {
                ZLog.LogWarning("MapPinsSync: Got empty map pin package from client.");
                return;
            }

            ServerHandler(sender, mapPinPkg);
        }

        public static void RPC_VPlusMapGlobalPinSyncClient(long sender, ZPackage mapPinPkg)
        {
            if (mapPinPkg == null)
            {
                ZLog.LogWarning("MapPinsSync: Got empty map pin package from server.");
                return;
            }

            ClientHandler(sender, mapPinPkg);
        }

        public static void RPC_VPlusMapGlobalPinSyncRemovePin(long sender, ZPackage mapPinPkg)
        {
            if (mapPinPkg == null)
            {
                ZLog.LogWarning("MapPinsSync: Got empty map pin package from server.");
                return;
            }
            
            var pos = mapPinPkg.ReadVector3();
            var radius = mapPinPkg.ReadSingle();
            var pin = Minimap.instance.GetClosestPin(pos, radius);

            CachedPin cachedPin = ConvertRawToCachedPin(pin);

            RemovePinFromCache(cachedPin);
        }

        public static void Initialize(bool keepQuiet = false)
        {
            ZLog.Log("--- VPlus Initialize Global Pins Sync");

            if (cientMapPins.Count == 0) return;

            var packageList = ChunkPayload(cientMapPins);

            if (packageList == null)
            {
                ZLog.LogError("MapPinsSync: Error sending map pins to the server!");
                return;
            }

            foreach (ZPackage package in packageList)
            {
                var rpcData = new RpcData()
                {
                    Name = Game_Start_Patch.ActionNameMapGlobalPinSyncServer,
                    Payload = new object[] {package},
                    Target = ZRoutedRpc.instance.GetServerPeerID()
                };

                RpcQueue.Enqueue(rpcData);
            }
        }

        public static void SendPinRemoveRequest(Vector3 pos, float radius)
        {
            ZPackage package = new ZPackage();
            long senderId = ZRoutedRpc.instance.m_id;

            package.Write(pos);
            package.Write(radius);

            ZRoutedRpc.instance.InvokeRoutedRPC(
                senderId,
                Game_Start_Patch.ActionNameMapGlobalPinSyncRemovePin,
                new object[] {package}
            );
        }

        private static void ServerHandler(long sender, ZPackage clientNetPackage)
        {
            if (sender == ZRoutedRpc.instance.GetServerPeerID()) return; // Don't  process own broadcasted data
            if (clientNetPackage == null) return;


            ZLog.Log("MapPinsSync: Processing PINS from CLIENT");


            NetworkPinPackageData unpackedData = UnpackNetworkPackage(clientNetPackage);

            if (unpackedData.info.senderName == ZRoutedRpc.instance.m_id) return;

            MergePinsWithCache(unpackedData);

            // Don't proceed at the final logic until get all chunks.
            if (!unpackedData.info.isLast) return;

            // Tell RPC Queue we got all packages (?)
            // VPlusAck.SendAck(sender);

            BroadcastPins(sender);
        }

        private static void ClientHandler(long sender, ZPackage mapPinPkg)
        {
            if (sender != ZRoutedRpc.instance.GetServerPeerID()) return; //Only bother if it's from the server.

            ZLog.Log("MapPinsSync: Processing pins from SERVER");

            NetworkPinPackageData unpackedData = UnpackNetworkPackage(mapPinPkg);

            MergePinsWithCache(unpackedData);

            // Don't proceed at the final logic until get all chunks.
            if (!unpackedData.info.isLast) return;

            // Tell RPC Queue we got all packages (?)
            // VPlusAck.SendAck(sender);

            UpdateClientMinimap();
        }

        private static void BroadcastPins(long sender)
        {
            var packageList = ChunkPayload(pinsCache);

            if (packageList == null)
            {
                ZLog.LogError("MapPinsSync: Error sending map pins to the clients!");
                return;
            }

            //Send the updated server map to all clients
            foreach (ZPackage package in packageList)
            {
                var rpcData = new RpcData()
                {
                    Name = Game_Start_Patch.ActionNameMapGlobalPinSyncClient,
                    Payload = new object[] {package},
                    Target = ZRoutedRpc.Everybody
                };

                RpcQueue.Enqueue(rpcData);
            }

            ZLog.Log($"-------------------------- Packages: {packageList.Count}");
            ZLog.Log($"MapPinsSync: Sent {packageList.Count * chunkSize} pins to all clients");
        }


        private static void UpdateClientMinimap()
        {
            ZLog.Log("MapPinsSync: Updating CLIENT map");

            int count = 0;

            foreach (var serverPin in pinsCache)
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

            ZLog.LogWarning($"MapPinsSync: Added {count} pins!");
        }

        private static List<ZPackage> ChunkPayload(List<CachedPin> pinsData)
        {
            List<ZPackage> packageList = new List<ZPackage>();
            List<List<CachedPin>> chunkedPinsData = pinsData.ChunkBy(chunkSize);

            foreach (var chunk in chunkedPinsData)
            {
                ZPackage package = PackPins(
                    chunk,
                    chunkedPinsData.Count,
                    chunk == chunkedPinsData.Last()
                );

                packageList.Add(package);
            }

            return packageList;
        }

        private static ZPackage PackPins(List<CachedPin> pinsChunk, int chunksCount, bool isLast)
        {
            ZPackage pkg = new ZPackage();

            if (pinsChunk.Count == 0) return null;

            try
            {
                pkg.Write(ZRoutedRpc.instance.m_id); // Write sender ID
                pkg.Write(chunksCount); // Write total chunks count
                pkg.Write(pinsChunk.Count); // Write pins count in this chunk

                foreach (var pin in pinsChunk)
                {
                    pkg.Write(pin.m_name);
                    pkg.Write(pin.m_pos);
                    pkg.Write(pin.m_save);
                    pkg.Write((int) pin.m_type);
                    pkg.Write(pin.m_checked);
                }

                // boolean dictating if this is the last chunk in the ZPackage sequence
                pkg.Write(isLast);

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

            long senderId = package.ReadLong();
            int chunksCount = package.ReadInt();
            int pinsCount = package.ReadInt();

            int count = 0;

            for (var i = 0; i < pinsCount; i++)
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

            bool isLast = package.ReadBool();

            return new NetworkPinPackageData()
            {
                info = new NetworkPinPackageData.Info()
                {
                    senderName = senderId,
                    totalChunks = chunksCount,
                    chunkPinsCount = pinsCount,
                    isLast = isLast
                },
                pinsList = list
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

            foreach (var networkPin in data.pinsList)
            {
                if (CheckPinExistsInCache(networkPin)) continue;

                pinsCache.Add(networkPin);
                count++;
            }

            ZLog.Log($"MapPinsSync: Merged {count} PINS");
        }

        private static void RemovePinFromCache(CachedPin pin)
        {
            ZLog.Log($"globalPinSync: removed pin '{pin.m_name}'");
            
            if (pinsCache.Contains(pin))
            {
                pinsCache.Remove(pin);
            }
        }

        private static CachedPin ConvertRawToCachedPin(Minimap.PinData rawPin)
        {
            return new CachedPin()
            {
                m_name = rawPin.m_name,
                m_pos = rawPin.m_pos,
                m_save = rawPin.m_save,
                m_type = rawPin.m_type,
                m_checked = rawPin.m_checked
            };
        }
    }
}