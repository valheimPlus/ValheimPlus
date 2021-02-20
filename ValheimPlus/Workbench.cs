using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ValheimPlus {
    class Workbench {
        [HarmonyPatch(typeof(CraftingStation), "Start")]
        public static class WorkbenchRangeIncrease {
            private static void Prefix(ref float ___m_rangeBuild, GameObject ___m_areaMarker) {
                if (Settings.isEnabled("Workbench")) 
                {
                    ___m_rangeBuild = Settings.getFloat("Workbench", "workbenchRange");
                }
            }
        }
    }
}
