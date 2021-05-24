using System.Collections.Generic;
using UnityEngine;
using ValheimPlus.GameClasses;

namespace ValheimPlus.RPC
{
    /// <summary>
    /// Sync map pins between clients via server
    /// </summary>
    public class VPlusMapPinsSync
    {
        public static List<Minimap.PinData> pinsCache = new List<Minimap.PinData>();

        public static void RPC_VPlusMapPinsSync(long sender, ZPackage mapPinPkg)
        {
            if (mapPinPkg == null)
            {
                ZLog.LogWarning("Warning: Got empty map pin package from server.");
                return;
            }

            if (ZNet.m_isServer)
            {
                ServerAction(sender, mapPinPkg);
            }
            else
            {
                ClientAction(sender, mapPinPkg);
            }
        }


        private static void ServerAction(long sender, ZPackage clientPinPackage)
        {
            if (sender == ZRoutedRpc.instance.GetServerPeerID()) return;
            if (clientPinPackage == null) return;
            
            ZLog.Log("-------------------- VPLUS: PROCESSING CLIENT PINS");
            
            MergePinsWithCache(clientPinPackage);
            BroadcastPins(sender);
        }

        private static void BroadcastPins(long sender)
        {
            var mapPinPkg = PackPins(pinsCache);

            foreach (ZNetPeer peer in ZRoutedRpc.instance.m_peers)
            {
                if (peer.m_uid == sender) continue;

                ZRoutedRpc.instance.InvokeRoutedRPC(
                    peer.m_uid,
                    Game_Start_Patch.ActionNameMapPinsSync,
                    new object[] {mapPinPkg}
                );
            }

            ZLog.Log($"Sent {pinsCache.Count} pins to all clients");
        }

        private static void ClientAction(long sender, ZPackage mapPinPkg)
        {
            if (sender != ZRoutedRpc.instance.GetServerPeerID()) return;

            ZLog.Log("-------------------- VPLUS: PROCESSING SERVER PINS");

            MergePinsWithCache(mapPinPkg);
            UpdateClientMinimap(mapPinPkg);
        }

        public static void SendMapPinsToServer(List<Minimap.PinData> pinsData, bool keepQuiet = false)
        {
            ZLog.Log("-------------------- SENDING VPLUS MapPins DATA");

            var pkg = PackPins(pinsData, keepQuiet);

            ZRoutedRpc.instance.InvokeRoutedRPC(
                ZRoutedRpc.instance.GetServerPeerID(),
                Game_Start_Patch.ActionNameMapPinsSync,
                new object[] {pkg}
            );

            ZLog.Log($"Sent map pin {pinsData.Count} to the server");
        }


        private static void UpdateClientMinimap(ZPackage mapPinPkg)
        {
            long pinSender = mapPinPkg.ReadLong();
            string senderName = mapPinPkg.ReadString();


            if (!(
                senderName != Player.m_localPlayer.GetPlayerName()
                && pinSender != ZRoutedRpc.instance.m_id
            )) return;


            string pinsSerialized = mapPinPkg.ReadString();
            List<Minimap.PinData> serverPins = JsonUtility.FromJson<List<Minimap.PinData>>(pinsSerialized);
            int count = 0;

            foreach (var serverPin in serverPins)
            {
                if (CheckPinExistsInCache(serverPin)) continue;

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

            ZLog.Log($"Added {count} pins from {senderName}!");
        }

        private static ZPackage PackPins(List<Minimap.PinData> pinsData, bool keepQuiet = false)
        {
            ZPackage pkg = new ZPackage();

            pkg.Write(ZRoutedRpc.instance.m_id); // Sender ID

            if (keepQuiet)
            {
                pkg.Write(""); // Loaded in
            }
            else
            {
                pkg.Write(Player.m_localPlayer.GetPlayerName()); // Sender Name
            }

            string serializedPins = JsonUtility.ToJson(pinsData);

            pkg.Write(serializedPins);
            pkg.Write(keepQuiet); // Don't shout
            return pkg;
        }


        private static bool CheckPinExistsInCache(Minimap.PinData pin)
        {
            Minimap.PinData result = pinsCache.Find(pinCached =>
                {
                    return (
                        pin.m_name == pinCached.m_name
                        && pin.m_type == pinCached.m_type
                        && (
                            pin.m_save == pinCached.m_save
                            && (double) Utils.DistanceXZ(pin.m_pos, pinCached.m_pos) < 1.0
                        )
                    );
                }
            );

            return result != null;
        }

        /*
         * merge acquired pins with local cache 
         */
        private static void MergePinsWithCache(ZPackage package)
        {
            long pinSender = package.ReadLong();
            string senderName = package.ReadString();


            if (!(
                senderName != Player.m_localPlayer.GetPlayerName()
                && pinSender != ZRoutedRpc.instance.m_id
            )) return;


            string pinsSerialized = package.ReadString();
            List<Minimap.PinData> packagePins = JsonUtility.FromJson<List<Minimap.PinData>>(pinsSerialized);


            foreach (var pin in packagePins)
            {
                if (CheckPinExistsInCache(pin)) continue;

                pinsCache.Add(pin);
            }
        }
    }
}