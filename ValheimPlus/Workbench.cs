using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ValheimPlus.Configurations;

namespace ValheimPlus {
    class Workbench {
        [HarmonyPatch(typeof(CraftingStation), "Start")]
        public static class WorkbenchRangeIncrease {
            private static void Prefix(ref float ___m_rangeBuild, GameObject ___m_areaMarker) {
                if (Configuration.Current.Workbench.IsEnabled && Configuration.Current.Workbench.WorkbenchRange > 0) 
                {
                    ___m_rangeBuild = Configuration.Current.Workbench.WorkbenchRange;
                }
            }
        }
    }
}
