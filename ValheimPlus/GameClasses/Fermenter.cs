using HarmonyLib;
using ValheimPlus.Configurations;

namespace ValheimPlus
{
    /// <summary>
    /// Altering fermenter production speed
    /// </summary>
    [HarmonyPatch(typeof(Fermenter), "Awake")]
    public static class ApplyFermenterChanges
    {
        private static bool Prefix(ref float ___m_fermentationDuration, ref Fermenter __instance)
        {
            if (Configuration.Current.Fermenter.IsEnabled)
            {
                float fermenterDuration = Configuration.Current.Fermenter.fermenterDuration;
                if (fermenterDuration > 0)
                {
                    ___m_fermentationDuration = fermenterDuration;
                }
            }

            return true;
        }
    }

    /// <summary>
    /// Altering fermeter items produced
    /// </summary>
    [HarmonyPatch(typeof(Fermenter), "GetItemConversion")]
    public static class ApplyFermenterItemCountChanges
    {
        private static void Postfix(ref Fermenter.ItemConversion __result)
        {
            if (Configuration.Current.Fermenter.IsEnabled)
            {
                int fermenterItemCount = Configuration.Current.Fermenter.fermenterItemsProduced;
                if (fermenterItemCount > 0)
                {
                    __result.m_producedItems = fermenterItemCount;
                }
            }
        }
    }


    [HarmonyPatch(typeof(Fermenter), "GetHoverText")]
    public static class Fermenter_GetHoverText_Patch
    {
        private static bool Prefix(ref Fermenter __instance, ref string __result)
        {
            if (!Configuration.Current.Fermenter.IsEnabled || !Configuration.Current.Fermenter.showDuration)
                return true;

            if (!PrivateArea.CheckAccess(__instance.transform.position, 0f, false))
            {
                __result = Localization.instance.Localize(__instance.m_name + "\n$piece_noaccess");
                return false;
            }
            switch (__instance.GetStatus())
            {
                case Fermenter.Status.Empty:
                    __result = Localization.instance.Localize(__instance.m_name + " ( $piece_container_empty )\n[<color=yellow><b>$KEY_Use</b></color>] $piece_fermenter_add");
                    return false;
                case Fermenter.Status.Fermenting:
                    {
                        string contentName = __instance.GetContentName();

                        if (__instance.m_exposed)
                        {
                            __result = Localization.instance.Localize(__instance.m_name + " ( " + contentName + ", $piece_fermenter_exposed )");
                            return false;
                        }

                        double durationUntilDone = (double)__instance.m_fermentationDuration - __instance.GetFermentationTime();

                        string info = "";

                        int minutes = (int)durationUntilDone / 60;
                        
                        if (((int)durationUntilDone) >= 120)
                            info = minutes + " minutes";
                        else
                            info = (int)durationUntilDone  + " seconds";

                        __result = Localization.instance.Localize(__instance.m_name + " ( " + contentName + ", $piece_fermenter_fermenting )") + " (" + info + ")";
                        return false;
                    }
                case Fermenter.Status.Ready:
                    {
                        string contentName2 = __instance.GetContentName();
                        __result = Localization.instance.Localize(__instance.m_name + " ( " + contentName2 + ", $piece_fermenter_ready )\n[<color=yellow><b>$KEY_Use</b></color>] $piece_fermenter_tap");
                        return false;
                    }
            }
            __result = __instance.m_name;

            return false;
        }
    }
}
