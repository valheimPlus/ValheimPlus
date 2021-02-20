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
                    Debug.Log("Range is here, setting is: " + Settings.getFloat("Workbench", "workbenchRange"));
                    //float defaultValue = 20f;
                    //float editedValue = Settings.getFloat("Workbench", "workbenchRange");
                    
                    //float scaleIncrease = ( editedValue - defaultValue ) / defaultValue * 100f;
                    //___m_areaMarker.gameObject.transform.localScale = new Vector3(scaleIncrease / 100, 1f, scaleIncrease / 100);
                    
                    ___m_rangeBuild = Settings.getFloat("Workbench", "workbenchRange");
                }
            }
        }
    }
}
