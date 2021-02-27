using System;
using System.Collections.Generic;
using System.IO;
using BepInEx;
using UnityEngine;

namespace ValheimPlus.RPC
{
    public class VPlusMapSync
    {
        public static bool[] ServerMapData;

        public static readonly string mapSyncFilePath = Paths.BepInExRootPath + Path.DirectorySeparatorChar + "mapSync.dat";

        public static void RPC_VPlusMapSync(long sender, ZPackage mapPkg)
        {
            if (ZNet.m_isServer) //Server
            {
                if (sender == ZRoutedRpc.instance.GetServerPeerID()) return;

                if (mapPkg == null) return;

                //Get number of explored areas
                int exploredAreaCount = mapPkg.ReadInt();

                //Iterate and add them to server's combined map data.
                for (int i = 0; i < exploredAreaCount; i++)
                {
                    Vector2i exploredArea = mapPkg.ReadVector2i();

                    ServerMapData[exploredArea.y * Minimap.instance.m_textureSize + exploredArea.x] = true;
                }

                ZLog.Log($"Received {exploredAreaCount} map points from peer {sender}.");

                ZPackage serverMapPkg = new ZPackage();

                List<Vector2i> serverExploredAreas = new List<Vector2i>();

                for (int y = 0; y < Minimap.instance.m_textureSize; ++y)
                {
                    for (int x = 0; x < Minimap.instance.m_textureSize; ++x)
                    {
                        if (ServerMapData[y * Minimap.instance.m_textureSize + x])
                        {
                            serverExploredAreas.Add(new Vector2i(x, y));
                        }
                    }
                }

                //Write number of explored areas
                serverMapPkg.Write(serverExploredAreas.Count);

                //Iterate and write all explored areas to package.
                foreach (Vector2i exploredArea in serverExploredAreas)
                {
                    serverMapPkg.Write(exploredArea);
                }

                //Send the updated server map to all clients
                ZRoutedRpc.instance.InvokeRoutedRPC(ZRoutedRpc.Everybody, "VPlusMapSync", new object[] { serverMapPkg });

                ZLog.Log($"Sent map updates to all clients ({serverExploredAreas.Count} map points)");
            }
            else //Client
            {
                if (sender != ZRoutedRpc.instance.GetServerPeerID()) return; //Only bother if it's from the server.

                if (mapPkg == null)
                {
                    ZLog.LogWarning("Warning: Got empty map sync package from server.");
                    return;
                }

                //Get number of explored areas
                int exploredAreaCount = mapPkg.ReadInt();

                //Iterate and add them to explored map
                for (int i = 0; i < exploredAreaCount; i++)
                {
                    Vector2i exploredArea = mapPkg.ReadVector2i();

                    Minimap.instance.m_explored[exploredArea.y * Minimap.instance.m_textureSize + exploredArea.x] = true;

                    Minimap.instance.m_fogTexture.SetPixel(exploredArea.x, exploredArea.y, (Color)new Color32((byte)0, (byte)0, (byte)0, (byte)0));
                }

                //Update map fog texture
                Minimap.instance.m_fogTexture.Apply();

                ZLog.Log($"I got {exploredAreaCount} map points from the server!");
            }
        }

        public static void SendMapToServer()
        {
            ZLog.Log("-------------------- SENDING VPLUSMAPSYNC DATA");

            //Iterate the explored map and prepare data for transmission
            List<Vector2i> exploredAreas = new List<Vector2i>();

            for (int y = 0; y < Minimap.instance.m_textureSize; ++y)
            {
                for (int x = 0; x < Minimap.instance.m_textureSize; ++x)
                {
                    if (Minimap.instance.m_explored[y * Minimap.instance.m_textureSize + x])
                    {
                        exploredAreas.Add(new Vector2i(x, y));
                    }
                }
            }

            if (exploredAreas.Count == 0) return;

            ZPackage mapPackage = new ZPackage();

            //Write number of explored areas
            mapPackage.Write(exploredAreas.Count);

            foreach (Vector2i exploredArea in exploredAreas)
            {
                mapPackage.Write(exploredArea);
            }

            ZRoutedRpc.instance.InvokeRoutedRPC(ZRoutedRpc.instance.GetServerPeerID(), "VPlusMapSync",
                new object[] { mapPackage });

            ZLog.Log($"Sent my map data to the server ({exploredAreas.Count} map points)");
        }

        public static void LoadMapDataFromDisk()
        {
            if (ServerMapData == null) return;

            //Load map data
            if (File.Exists(mapSyncFilePath))
            {
                try
                {
                    string mapData = File.ReadAllText(mapSyncFilePath);

                    string[] dataPoints = mapData.Split(',');

                    foreach (string dataPoint in dataPoints)
                    {
                        if (int.TryParse(dataPoint, out int result))
                        {
                            VPlusMapSync.ServerMapData[result] = true;
                        }
                    }

                    ZLog.Log($"Loaded {dataPoints.Length} map points from disk.");
                }
                catch (Exception ex)
                {
                    ZLog.LogError("Failed to load synchronized map data.");
                    ZLog.LogError(ex);
                }
            }
        }

        public static void SaveMapDataToDisk()
        {
            if (ServerMapData == null) return;

            List<int> mapDataToDisk = new List<int>();

            for (int y = 0; y < Minimap.instance.m_textureSize; ++y)
            {
                for (int x = 0; x < Minimap.instance.m_textureSize; ++x)
                {
                    if (ServerMapData[y * Minimap.instance.m_textureSize + x])
                    {
                        mapDataToDisk.Add(y * Minimap.instance.m_textureSize + x);
                    }
                }
            }

            if (mapDataToDisk.Count > 0)
            {
                File.Delete(mapSyncFilePath);
                File.WriteAllText(mapSyncFilePath, string.Join(",", mapDataToDisk));

                ZLog.Log($"Saved {mapDataToDisk.Count} map points to disk.");
            }
        }
    }
}