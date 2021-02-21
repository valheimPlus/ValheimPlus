using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ValheimPlus.ConsolePlus
{
    public interface IValheimPlusCommand
    {
        List<string> Arguments { get; }
        string CommandName { get; }
        string Description { get; }
        string Usage { get; }
        bool RequiresAdmin { get; }
        void Execute(params object[] args);
        void Load();
    }

    public abstract class ValheimPlusCommand : IValheimPlusCommand
    {
        public abstract List<string> Arguments { get; }
        public abstract string CommandName { get; }
        public abstract string Description { get; }
        public abstract bool RequiresAdmin { get; }
        public string Usage => string.Join(" ", Arguments);

        protected string InvalidUsage => string.Format("Invalid usage {0}", Usage);

        #region Server
        protected ZNetPeer Sender { get; private set; }
        protected bool ServerCanClientExecuteCommand(string clientHostname)
        {
            if (!ZNet.instance.IsServer())
                return false;
            return ZNet.instance.m_adminList.Contains(clientHostname);
        }
        protected void ServerSendReponse(string message)
        {
            if (!ZNet.instance.IsServer())
                return;
            ZNet.instance.RemotePrint(Sender.m_rpc, message);
        }
        protected void ServerExecuteRoutedRPC(ZNetPeer target, string rpcMethod, params object[] parameters)
        {
            ZRoutedRpc.instance.InvokeRoutedRPC(target.m_uid, target.m_characterID, rpcMethod, parameters);
        }
        private ZNetPeer ServerIdentifySender(string hostName)
        {
            return ZNet.instance.GetPeers()
                .Where(p => p.m_socket.GetHostName()
                .Equals(hostName))
                .FirstOrDefault();
        }
        #endregion

        #region Client
        private ZPackage ClientPackageCommand(params object[] args)
        {
            ZPackage pkg = new ZPackage();
            pkg.Write(CommandName);
            var numArgs = args.Length;
            pkg.Write(numArgs);
            for (int i = 0; i < numArgs; i++)
            {
                pkg.Write(args[i].ToString());
            }
            return pkg;
        }
        protected virtual bool ClientValidArguments(params object[] args)
        {
            //TODO: Determine which arguments are optional and which aren't
            //for now, commands with optional arguments must override this
            return args.Length == Arguments.Count;
        }
        #endregion
        public virtual void Load() { }
        public virtual void Execute(params object[] args)
        {
            if (ZNet.instance.IsServer())
            {
                var client = args[0].ToString();
                Sender = ServerIdentifySender(client);
                object[] commandArgs = args.Skip(1).FirstOrDefault() as object[];
                if (RequiresAdmin && !ServerCanClientExecuteCommand(client))
                {
                    ConsolePlus.LogFormat("Client {0} attempted to execute command {1} without proper permissions", client, CommandName);
                    ServerSendReponse("Invalid permission");
                }
                ExecuteServerContext(commandArgs);
            }
            else
            {
                var serverRpc = ZNet.instance.GetServerRPC();
                if (ClientValidArguments(args))
                {
                    ExecuteClientContext(args);
                    serverRpc.Invoke(ConsolePlus.ExecuteCommandRPC, ClientPackageCommand(args).GetBase64());
                }
                else
                {
                    ConsolePlus.Log(InvalidUsage);
                }
            }
        }

        protected abstract void ExecuteServerContext(params object[] args);
        protected abstract void ExecuteClientContext(params object[] args);
    }
}
