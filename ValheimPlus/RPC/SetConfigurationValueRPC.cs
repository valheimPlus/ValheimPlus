// ValheimPlus

using UnityEngine;
using ValheimPlus.ConsoleCommands;

namespace ValheimPlus.RPC
{
    public class SetConfigurationValueRPC
    {
        public static void RPC_SetConfigurationValue(long sender, ZPackage inputString)
        {
            if (ZNet.m_isServer) // Server
            {
                var peer = ZNet.instance.GetPeer(sender);
                if (peer == null)
                {
                    return;
                }

                Debug.Log("RPC_SetConfigurationValue SERVER");
                
                // Check if peer is in admin list
                var steamId = peer.m_socket.GetHostName();
                if (ZNet.instance.m_adminList.Contains(steamId))
                {
                    var input = inputString.ReadString();
                    string inputCopy = input;
                    BaseConsoleCommand.TryExecuteCommand(ref input, true);
                    foreach (var peerEntry in ZNet.instance.m_peers)
                    {
                        // Send same back to all clients to actually also set the value on the client
                        ZRoutedRpc.instance.InvokeRoutedRPC(peerEntry.m_uid, "SetConfigurationValue", inputCopy);
                    }
                }
            }
            else // Client
            {
                Debug.Log("RPC_SetConfigurationValue CLIENT");
                var input = inputString.ReadString();
                string inputCopy = input;
                BaseConsoleCommand.TryExecuteCommand(ref input, true);
                Console.instance.AddString($"Command '{inputCopy}' executed");
            }
        }
    }
}