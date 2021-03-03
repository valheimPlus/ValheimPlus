public static class ZNetExtensions
{
    public enum ZNetInstanceType
    {
        Local,
        Client,
        Server
    }
     
    public static bool IsLocalInstance(this ZNet znet)
    {
        return !znet.IsDedicated() && znet.IsServer() && (znet.GetNrOfPlayers() > znet.GetPeers().Count);
    }

    public static bool IsClientInstance(this ZNet znet)
    {
        return !znet.IsDedicated() && !znet.IsServer();
    }

    public static bool IsServerInstance(this ZNet znet)
    {
        return !znet.IsLocalInstance() && !znet.IsClientInstance();
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
