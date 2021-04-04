using Steamworks;
using System.Collections.Generic;
using ValheimPlus.Utility;

namespace ValheimPlus.RPC
{

    public class VPlusNetworkStatus
    {
        public bool CompressionEnabled { get; set; } = false;
        public int EstimatedUpstreamCapacity { get; set; } = 0;

        public VPlusNetworkStatus(bool CompressionEnabled, int EstimatedUpstreamCapacity)
        {
            this.CompressionEnabled = CompressionEnabled;
            this.EstimatedUpstreamCapacity = EstimatedUpstreamCapacity;
        }

        public VPlusNetworkStatus(ZPackage pkg)
        {
            this.Deserialize(pkg);
            ZLog.Log("Desierialized networkstatus to " + this.ToString());
        }

        public ZPackage Serialize()
        {
            ZPackage pkg = new ZPackage();
            pkg.Write(CompressionEnabled);
            pkg.Write(EstimatedUpstreamCapacity);
            return pkg;
        }
        public void Deserialize(ZPackage pkg)
        {
            this.CompressionEnabled = pkg.ReadBool();
            this.EstimatedUpstreamCapacity = pkg.ReadInt();
        }
        override
        public string ToString()
        {
            return "CompressionEnabled: " + this.CompressionEnabled + ", EstimatedUpstreamCapacity: " + this.EstimatedUpstreamCapacity;
        }
    }
    public class VPlusNetworkStatusManager
    {
        public static Dictionary<long, VPlusNetworkStatus> peerNetworkCapabilities = new Dictionary<long, VPlusNetworkStatus>();
        public static void RPC_VPlusNetworkStatusSync(long sender, ZPackage configPkg)
        {
            ZLog.Log("-------------------------- Got NetworkSyncStatus from " + sender);
            if(!peerNetworkCapabilities.ContainsKey(sender))
            {
                peerNetworkCapabilities.Add(sender, new VPlusNetworkStatus(configPkg));
            }
            else
            {
                peerNetworkCapabilities[sender] = new VPlusNetworkStatus(configPkg);
            }
        }

        public static void SendNetworkStatus(long target, VPlusNetworkStatus status)
        {
            ZRoutedRpc.instance.InvokeRoutedRPC(target, "VPlusNetworkStatusSync", status.Serialize());
        }

        public static bool SteamPeerSupportsCompression(CSteamID id)
        {
            long znetID = ZNet.instance.GetPeerBySteamID(id).m_uid;
            if ( ! peerNetworkCapabilities.ContainsKey(znetID))
            {
                return false;
            }
            return peerNetworkCapabilities[znetID].CompressionEnabled;
        }
    }
}