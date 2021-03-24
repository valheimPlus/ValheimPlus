using HarmonyLib;
using ValheimPlus.Configurations;

namespace ValheimPlus.GameClasses
{
    /// <summary>
    /// Advanced Editing Mode Game Camera changes
    /// </summary>
    [HarmonyPatch(typeof(GameCamera), "UpdateCamera")]
    public static class BlockCameraScrollInAEM
    {
        private static void Prefix(GameCamera __instance)
        {
            if (AEM.isActive)
            {
                __instance.m_maxDistance = __instance.m_distance;
                __instance.m_minDistance = __instance.m_distance;
            }
            else
            {
                if (Configuration.Current.Camera.IsEnabled)
                {
                    if (Configuration.Current.Camera.cameraMaximumZoomDistance >= 1 && Configuration.Current.Camera.cameraMaximumZoomDistance <= 100)
                        __instance.m_maxDistance = Configuration.Current.Camera.cameraMaximumZoomDistance;
                    if (Configuration.Current.Camera.cameraBoatMaximumZoomDistance >= 1 && Configuration.Current.Camera.cameraBoatMaximumZoomDistance <= 100)
                        __instance.m_maxDistanceBoat = Configuration.Current.Camera.cameraBoatMaximumZoomDistance;
                    if (Configuration.Current.Camera.cameraFOV >= 1 && Configuration.Current.Camera.cameraFOV <= 140)
                        __instance.m_fov = Configuration.Current.Camera.cameraFOV;

                    __instance.m_minDistance = 1;
                }
                else
                {
                    __instance.m_maxDistance = 6;
                    __instance.m_minDistance = 1;
                }
            }
        }
    }
}
