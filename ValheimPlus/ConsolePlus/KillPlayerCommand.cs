using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HarmonyLib;
using UnityEngine;

namespace ValheimPlus.ConsolePlus
{
    /// <summary>
    /// Proof of concept command
    /// Kill targetPlayerName
    /// </summary>
    public class KillPlayerCommand : ValheimPlusCommand
    {
        public override List<string> Arguments => new List<string> { "targetPlayer", "<message>"};
        public override bool RequiresAdmin => false;
        public override string CommandName => "kill";

        public override string Description => "Slays the target player with an optional message";

        protected override bool ClientValidArguments(params object[] args)
        {
            return args.Length == Arguments.Count || args.Length == 1;
        }

        protected override void ExecuteClientContext(params object[] args) { }

        protected override void ExecuteServerContext(params object[] args)
        {
            var target = args[0].ToString();
            var message = (args.Length > 1) ? args[1].ToString() : string.Empty;

            var targetPeer = ZNet.instance.GetPeers().Where(p => p.m_playerName.Equals(target)).FirstOrDefault();

            if (targetPeer == null)
            {
                ServerSendReponse("Player not found " + target);
                return;
            }

            ServerExecuteRoutedRPC(targetPeer, "Damage", new HitData()
            {
                m_damage =
                {
                    m_damage = 99999f
                }
            });
            if(!string.IsNullOrEmpty(message))
            {
                //Does not work, requires Localization system
                //Or a mod to the MessageHud to add non-localized messages to the message HUD
                //ServerExecuteRoutedRPC(targetPeer, "Message", MessageHud.MessageType.Center, message, 0);
            }
            ServerSendReponse("Player has been slain!");
        }
    }
}
