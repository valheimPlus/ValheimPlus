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
                    ___m_areaMarker.GetComponent<CircleProjector>().m_radius = ___m_rangeBuild;
                    float scaleIncrease = ( Configuration.Current.Workbench.WorkbenchRange - 20f ) / 20f * 100f;
                    ___m_areaMarker.gameObject.transform.localScale = new Vector3(scaleIncrease / 100, 1f, scaleIncrease / 100);
                }
            }
        }
    }
}
