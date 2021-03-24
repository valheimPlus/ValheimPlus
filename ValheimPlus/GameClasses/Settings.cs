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
            muteAudioToggle = GameObject.Instantiate(cmToggle);
            Text textComponent = muteAudioToggle.GetComponentInChildren<Text>();
            textComponent.text = "Mute Game in Background";
            muteAudioToggle.transform.SetParent(cmToggle.transform.parent, false);
            muteAudioToggle.transform.Translate(new Vector2(0, -50f)); // TODO replace magic number with math
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