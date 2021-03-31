using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;
using ValheimPlus;
using ValheimPlus.Configurations;
using ValheimPlus.Utility;

namespace ValheimPlus.GameClasses
{
    /// <summary>
    /// Forces a tamed creature to stay asleep if it's recovering from being stunned.
    /// </summary>
    [HarmonyPatch(typeof(MonsterAI), "UpdateSleep")]
    public static class MonsterAI_UpdateSleep_Patch
    {
        public static void Prefix(MonsterAI __instance, ref float dt)
        {
            if (Configuration.Current.Tameable.IsEnabled)
            {
                MonsterAI monsterAI = __instance;
                ZDO zdo = monsterAI.m_nview.GetZDO();

                if ((TameableMortalityTypes)Configuration.Current.Tameable.mortality != TameableMortalityTypes.Essential || zdo == null || !zdo.GetBool("isRecoveringFromStun"))
                    return;

                if (monsterAI.m_character.m_moveDir != Vector3.zero)
                    monsterAI.StopMoving();

                if (monsterAI.m_sleepTimer != 0f)
                    monsterAI.m_sleepTimer = 0f;

                float timeSinceStun = zdo.GetFloat("timeSinceStun") + dt;
                zdo.Set("timeSinceStun", timeSinceStun);

                if (timeSinceStun >= Configuration.Current.Tameable.stunRecoveryTime)
                {
                    zdo.Set("timeSinceStun", 0f);
                    monsterAI.m_sleepTimer = 0.5f;
                    monsterAI.m_character.m_animator.SetBool("sleeping", false);
                    zdo.Set("sleeping", false);
                    zdo.Set("isRecoveringFromStun", false);
                }

                dt = 0f;
            }
        }
    }
}
