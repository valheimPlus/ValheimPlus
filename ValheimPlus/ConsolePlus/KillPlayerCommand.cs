using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ValheimPlus.ConsolePlus
{
    /// <summary>
    /// Proof of concept command
    /// Kill targetPlayerName
    /// </summary>
    class KillPlayerCommand : BaseValheimPlusCommand
    {
        public override List<string> Arguments => new List<string>{ "targetPlayer" };
        public override bool RequiresAdmin => true;

        protected override void ExecuteClientContext()
        {
        }

        protected override void ExecuteServerContext(params object[] args)
        {
            var allPlayers = ZNet.instance.GetPlayerList()
                .Select(pi => Player.GetPlayer(pi.m_characterID.userID));

            foreach(var player in allPlayers)
            {
                player.ApplyDamage(new HitData()
                { 
                    m_damage = 
                    { 
                        m_damage = 99999f 
                    } 
                }, true, false);
            }

        }
    }
}
