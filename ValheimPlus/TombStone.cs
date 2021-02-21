using HarmonyLib;
using UnityEngine;
using ValheimPlus.Configurations;

namespace ValheimPlus
{
    [HarmonyPatch(typeof(TombStone), "OnTakeAllSuccess")]
    public static class RemoveDeathPinOnTombStoneEmpty
    {
        private static void Postfix()
        {
            if (Configuration.Current.Map.IsEnabled && Configuration.Current.Map.RemoveDeathPinOnTombstoneEmpty)
            {
                PlayerProfile playerProfile = Game.instance.m_playerProfile;
                long worldUID = ZNet.instance.GetWorldUID();

                if(!playerProfile.m_worldData.TryGetValue(worldUID, out PlayerProfile.WorldPlayerData worldData))
                {
                    return;
                }

                // resets death point data to remove death pin when removing all things from tombstone
                worldData.m_deathPoint = Vector3.zero;
                worldData.m_haveDeathPoint = false;

                playerProfile.m_worldData[worldUID] = worldData;
            }
        }
    }
}
