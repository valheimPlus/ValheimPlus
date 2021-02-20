using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ValheimPlus.Configurations;

namespace ValheimPlus {
    class TimeManipulation {
        [HarmonyPatch(typeof(EnvMan), "FixedUpdate")]
        public static class WorkbenchRangeIncrease {
            private static void Postfix(ref EnvMan __instance) {

                // i am working on this, nx
              
            }
        }
    }
}
