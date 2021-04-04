using Steamworks;

public static class ZNetExtensions
{
    public enum ZNetInstanceType
    {
        Local,
        Client,
        Server
    }

    public static ZNetPeer GetPeerBySteamID(this ZNet znet, CSteamID id)
    {
        foreach (ZNetPeer znetPeer in znet.m_peers)
        {
            if(znetPeer.m_socket is ZSteamSocket)
            {
                ZSteamSocket zsteamsocket = (ZSteamSocket)znetPeer.m_socket;
                if(zsteamsocket.GetPeerID() == id)
                {
                    return znetPeer;
                }
            }
        }
        return null;
    }

    public static bool IsLocalInstance(this ZNet znet)
    {
        return znet.IsServer() && !znet.IsDedicated();
    }

    public static bool IsClientInstance(this ZNet znet)
    {
        return !znet.IsServer() && !znet.IsDedicated();
    }

    public static bool IsServerInstance(this ZNet znet)
    {
        return znet.IsServer() && znet.IsDedicated();
    }

    public static ZNetInstanceType GetInstanceType(this ZNet znet)
    {
        if (znet.IsLocalInstance())
        {
            return ZNetInstanceType.Local;
        }
        else if (znet.IsClientInstance())
        {
            return ZNetInstanceType.Client;
        }
        else
        {
            return ZNetInstanceType.Server;
        }
    }
}
