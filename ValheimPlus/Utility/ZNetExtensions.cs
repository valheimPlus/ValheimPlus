public static class ZNetExtensions
{
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

    public static string GetInstanceType(this ZNet znet)
    {
        if (znet.IsLocalInstance())
        {
            return "Local";
        } 
        else if (znet.IsClientInstance())
        {
            return "Client";
        }
        else
        {
            return "Server";
        }
    }
}
