using HarmonyLib;


namespace ValheimPlus
{
    class CheatModification
    {
        [HarmonyPatch(typeof(Console), "IsCheatsEnabled")]
        public static class EnableCheats
        {
            private static void Postfix(Console __instance, ref bool __result)
            {
                __result = true;
            }
        }
    }
}
