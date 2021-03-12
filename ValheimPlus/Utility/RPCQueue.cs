using System.Collections.Generic;
using BepInEx;

namespace ValheimPlus.Utility
{
    public class RpcData
    {
        public string Name;
        public long Target = ZRoutedRpc.Everybody;
        public object[] Payload;
    }

    public static class RpcQueue
    {
        private static Queue<RpcData> _rpcQueue = new Queue<RpcData>();
        private static bool _ack = true;

        public static void Enqueue(RpcData rpc)
        {
            _rpcQueue.Enqueue(rpc);
        }

        public static bool SendNextRpc()
        {
            if (_rpcQueue.Count == 0 || !_ack) return false;

            RpcData rpc = _rpcQueue.Dequeue();

            if (rpc.Name.IsNullOrWhiteSpace() ||
                rpc.Payload == null)
            {
                return false;
            }

            ZRoutedRpc.instance.InvokeRoutedRPC(rpc.Target, rpc.Name, rpc.Payload);
            _ack = false;

            return true;
        }

        public static void GotAck()
        {
            _ack = true;
        }
    }
}