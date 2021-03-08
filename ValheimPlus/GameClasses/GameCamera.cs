using System.Runtime.InteropServices;
using HarmonyLib;
using UnityEngine;
using ValheimPlus.Configurations;

namespace ValheimPlus
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

    [HarmonyPatch(typeof(GameCamera), nameof(GameCamera.UpdateMouseCapture))]
    public static class GameCamera_UpdateMouseCapture_RememberMousePosition
    {
        private static Vector2 savedMousePosition;

        private static void Prefix(out State __state)
        {
            __state = new State()
            {
                PreviousLockState = Cursor.lockState,
                CurrentMousePosition = new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y - 1)
            };
        }

        private static void Postfix(State __state)
        {
            if (!(Configuration.Current.Hud.IsEnabled && Configuration.Current.Hud.rememberMousePosition))
            {
                return;
            }

            var currentLockState = Cursor.lockState;

            if (__state.PreviousLockState == currentLockState)
            {
                return;
            }

            if (__state.PreviousLockState == CursorLockMode.None && currentLockState == CursorLockMode.Locked)
            {
                savedMousePosition = __state.CurrentMousePosition;
            }

            if (__state.PreviousLockState == CursorLockMode.Locked)
            {
                if (Application.platform == RuntimePlatform.WindowsPlayer)
                {
                    User32.SetCursorPosition(savedMousePosition);
                }
            }
        }

        public class State
        {
            public CursorLockMode PreviousLockState { get; set; }
            public Vector2 CurrentMousePosition { get; set; }
        }

        private static class User32
        {
            public static void SetCursorPosition(Vector2 position)
            {
                SetCursorPos((int)position.x, (int)position.y);
            }

            [DllImport("user32.dll")]
            private static extern bool SetCursorPos(int X, int Y);
        }
    }
}
