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
