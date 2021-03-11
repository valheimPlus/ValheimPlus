using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ValheimPlus.Utility;
using static ValheimPlus.VPlusDataObjects;

namespace ValheimPlus.RPC
{
    public class VPlusMapSync
    {
        public static bool[] ServerMapData;

        public static bool ShouldSyncOnSpawn = true;

        public static void RPC_VPlusMapSync(long sender, ZPackage mapPkg)
        {
            if (ZNet.m_isServer) //Server
            {
                if (sender == ZRoutedRpc.instance.GetServerPeerID()) return;

                if (mapPkg == null) return;

                //Get number of explored areas
                int exploredAreaCount = mapPkg.ReadInt();

                if (exploredAreaCount > 0)
                {
                    //Iterate and add them to server's combined map data.
                    for (int i = 0; i < exploredAreaCount; i++)
                    {
                        MapRange exploredArea = mapPkg.ReadVPlusMapRange();

                        for (int x = exploredArea.StartingX; x < exploredArea.EndingX; x++)
                        {
                            ServerMapData[exploredArea.Y * Minimap.instance.m_textureSize + x] = true;
                        }
                    }

                    ZLog.Log($"Received {exploredAreaCount} map ranges from peer #{sender}.");

                    //Send Ack
                    VPlusAck.SendAck(sender);
                }

                //Check if this is the last chunk from the client.
                bool lastMapPackage = mapPkg.ReadBool();

                if (!lastMapPackage) return; //This package is one of many chunks, so don't update clients until we get all of them.
                
                //Convert map data into ranges
                List<MapRange> serverExploredAreas = ExplorationDataToMapRanges(ServerMapData);

                //Chunk up the map data
                List<ZPackage> packages = ChunkMapData(serverExploredAreas);

                //Send the updated server map to all clients
                foreach(ZPackage pkg in packages)
                {
                    RpcQueue.Enqueue(new RpcData()
                    {
                        Name = "VPlusMapSync",
                        Payload = new object[] { pkg },
                        Target = ZRoutedRpc.Everybody
                    });
                }

                ZLog.Log($"-------------------------- Packages: {packages.Count}");

                ZLog.Log($"Sent map updates to all clients ({serverExploredAreas.Count} map ranges, {packages.Count} chunks)");
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

                if (exploredAreaCount > 0)
                {
                    //Iterate and add them to explored map
                    for (int i = 0; i < exploredAreaCount; i++)
                    {
                        MapRange exploredArea = mapPkg.ReadVPlusMapRange();

                        for (int x = exploredArea.StartingX; x < exploredArea.EndingX; x++)
                        {
                            Minimap.instance.Explore(x, exploredArea.Y);
                        }
                    }

                    //Update fog texture
                    Minimap.instance.m_fogTexture.Apply();

                    ZLog.Log($"I got {exploredAreaCount} map ranges from the server!");

                    //Send Ack
                    VPlusAck.SendAck(sender);
                }
                else
                {
                    ZLog.Log("Server has no explored areas to sync, continuing.");
                }
            }
        }

        public static void SendMapToServer()
        {
            ZLog.Log("-------------------- SENDING VPLUSMAPSYNC DATA");

            //Convert exploration data to ranges
            List<MapRange> exploredAreas = ExplorationDataToMapRanges(Minimap.instance.m_explored);

            //If we have no data to send, just send an empty RPC to trigger the server end to sync.
            if (exploredAreas.Count == 0)
            {
                ZPackage pkg = new ZPackage();

                pkg.Write(0); //Number of explored areas we're sending (zero in this case)
                pkg.Write(true); //Trigger server sync by telling the server this is the last package we'll be sending.

                ZRoutedRpc.instance.InvokeRoutedRPC(ZRoutedRpc.instance.GetServerPeerID(), "VPlusMapSync",
                    new object[] { pkg });
            }
            else //We have data to send. Prep it and send it.
            {
                //Chunk map data
                List<ZPackage> packages = ChunkMapData(exploredAreas);

                //Route all chunks to the server
                foreach (ZPackage pkg in packages)
                {
                    RpcQueue.Enqueue(new RpcData()
                    {
                        Name = "VPlusMapSync",
                        Payload = new object[] { pkg },
                        Target = ZRoutedRpc.instance.GetServerPeerID()
                    });
                }

                ZLog.Log($"Sent my map data to the server ({exploredAreas.Count} map ranges, {packages.Count} chunks)");
            }
        }

        public static void LoadMapDataFromDisk()
        {
            //TODO: Optimize / Improve on disk format for exploration data. (JSON?)

            if (ServerMapData == null) return;

            //Load map data
            if (File.Exists(ValheimPlusPlugin.VPlusDataDirectoryPath +
                            Path.DirectorySeparatorChar +
                            ZNet.instance.GetWorldName() + "_mapSync.dat"))
            {
                try
                {
                    string mapData = File.ReadAllText(ValheimPlusPlugin.VPlusDataDirectoryPath +
                                                      Path.DirectorySeparatorChar +
                                                      ZNet.instance.GetWorldName() + "_mapSync.dat");

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
            //TODO: Optimize / Improve on disk format for exploration data. (JSON?)

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
                File.Delete(ValheimPlusPlugin.VPlusDataDirectoryPath +
                            Path.DirectorySeparatorChar +
                            ZNet.instance.GetWorldName() + "_mapSync.dat");
                File.WriteAllText(ValheimPlusPlugin.VPlusDataDirectoryPath +
                                  Path.DirectorySeparatorChar +
                                  ZNet.instance.GetWorldName() + "_mapSync.dat", string.Join(",", mapDataToDisk));

                ZLog.Log($"Saved {mapDataToDisk.Count} map points to disk.");
            }
        }

        private static List<MapRange> ExplorationDataToMapRanges(bool[] explorationData)
        {
            //Iterate the explored map and convert to ranges
            List<MapRange> exploredAreas = new List<MapRange>();

            for (int y = 0; y < Minimap.instance.m_textureSize; ++y)
            {
                int startX = -1, endX = -1;

                for (int x = 0; x < Minimap.instance.m_textureSize; ++x)
                {
                    //Find the first X value that is true
                    if (explorationData[y * Minimap.instance.m_textureSize + x] && startX == -1 && endX == -1)
                    {
                        startX = x;
                        continue;
                    }

                    //Find the last X value that is true
                    if (!explorationData[y * Minimap.instance.m_textureSize + x] && startX > -1 && endX == -1)
                    {
                        endX = x - 1;
                        continue;
                    }

                    //If we have both X values in the range, save it for this Y value.
                    if (startX > -1 && endX > -1)
                    {
                        exploredAreas.Add(new MapRange()
                        {
                            StartingX = startX,
                            EndingX = endX,
                            Y = y
                        });

                        startX = -1;
                        endX = -1;
                    }
                }

                //If we got a starting X coordinate but never got an end coordinate, this range is completely explored.
                if (startX > -1 && endX == -1)
                {
                    //The row is true til the end, create a range for it.
                    exploredAreas.Add(new MapRange()
                    {
                        StartingX = startX,
                        EndingX = Minimap.instance.m_textureSize,
                        Y = y
                    });
                }
            }

            return exploredAreas;
        }

        private static List<ZPackage> ChunkMapData(List<MapRange> mapData, int chunkSize = 10000)
        {
            if (mapData == null || mapData.Count == 0) return null;

            //Chunk the map data into pieces based on the maximum possible map data
            List<List<MapRange>> chunkedData = mapData.ChunkBy(chunkSize);

            List<ZPackage> packageList = new List<ZPackage>();

            //Iterate the chunks
            foreach(List<MapRange> thisChunk in chunkedData)
            {
                ZPackage pkg = new ZPackage();

                //Write number of MapRanges in this package
                pkg.Write(thisChunk.Count);

                //Write each MapRange in this chunk to this package.
                foreach(MapRange mapRange in thisChunk)
                {
                    pkg.WriteVPlusMapRange(mapRange);
                }

                //Write boolean dictating if this is the last chunk in the ZPackage sequence
                if (thisChunk == chunkedData.Last())
                {
                    pkg.Write(true);
                }
                else
                {
                    pkg.Write(false);
                }

                //Add the package to the package list
                packageList.Add(pkg);
            }

            return packageList;
        }
    }
}