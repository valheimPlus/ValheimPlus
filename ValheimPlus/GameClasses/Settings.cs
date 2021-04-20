using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;

namespace ValheimPlus.GameClasses
{
    public static class MuteGameInBackground
    {
        public static Toggle muteAudioToggle;

        public static void CreateToggle()
        {
            Toggle cmToggle = Settings.instance.m_continousMusic;
            muteAudioToggle = GameObject.Instantiate(cmToggle, cmToggle.transform.parent, false);
            muteAudioToggle.name = "MuteGameInBackground";
            muteAudioToggle.GetComponentInChildren<Text>().text = "Mute Game in Background";

            // scaleFactor is overwritten by GuiScaler::UpdateScale, which is called every frame, but impacted when pressing OK in the settings dialog
            CanvasScaler canvasScalerComponent = muteAudioToggle.transform.root.GetComponentInChildren<CanvasScaler>();
            muteAudioToggle.transform.Translate(new Vector2(0, -40 * canvasScalerComponent.scaleFactor));
        }
    }

    /// <summary>
    /// Read in saved user preference for audio mute toggle
    /// </summary>
    [HarmonyPatch(typeof(Settings), "Awake")]
    public static class Settings_LoadSettings_Patch
    {
        private static void Postfix()
        {
            if (MuteGameInBackground.muteAudioToggle == null)
                MuteGameInBackground.CreateToggle();

            MuteGameInBackground.muteAudioToggle.isOn = (PlayerPrefs.GetInt("MuteGameInBackground", 0) == 1); ;
        }
    }

    /// <summary>
    /// Save out user preference for audio mute toggle
    /// </summary>
    [HarmonyPatch(typeof(Settings), "SaveSettings")]
    public static class Settings_SaveSettings_Patch
    {
        private static void Postfix()
        {
            PlayerPrefs.SetInt("MuteGameInBackground", MuteGameInBackground.muteAudioToggle.isOn ? 1 : 0);
        }
    }
}