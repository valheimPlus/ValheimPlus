using ValheimPlus.Utility;

namespace ValheimPlus.RPC
{
    public class VPlusAck
    {
        public static void RPC_VPlusAck(long sender)
        {
            RpcQueue.GotAck();
        }
    }
}