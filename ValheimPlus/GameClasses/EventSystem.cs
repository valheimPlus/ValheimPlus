using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;

namespace ValheimPlus.GameClasses
{
    /// <summary>
    /// Mute all sound when the application loses focus
    /// </summary>
    [HarmonyPatch(typeof(UnityEngine.EventSystems.EventSystem), "OnApplicationFocus")]
    public static class EventSystem_OnApplicationFocus_Patch
    {
        private static void Postfix(bool hasFocus)
        {
            if (PlayerPrefs.GetInt("MuteGameInBackground", 0) == 1)
            {
                if (hasFocus)
                    AudioListener.volume = PlayerPrefs.GetFloat("MasterVolume", 1f);
                else
                    AudioListener.volume = 0f;
            }
        }
    }
}