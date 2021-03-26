using HarmonyLib;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using ValheimPlus.Configurations;
using ValheimPlus.UI;

namespace ValheimPlus.GameClasses
{
    /// <summary>
    /// Set max player limit and disable server password 
    /// </summary>
    [HarmonyPatch(typeof(FejdStartup), "Awake")]
    public static class HookServerStart
    {
        private static void Postfix(ref FejdStartup __instance)
        {
            if (Configuration.Current.Server.IsEnabled && Configuration.Current.Server.disableServerPassword)
            {
                __instance.m_minimumPasswordLength = 0;
            }
            if (Configuration.Current.Server.IsEnabled)
            {
                __instance.m_serverPlayerLimit = Configuration.Current.Server.maxPlayers;
            }
        }
    }

    /// <summary>
    /// Adding V+ logo and version text
    /// </summary>
    [HarmonyPatch(typeof(FejdStartup), "SetupGui")]
    public static class FejdStartup_SetupGui_Patch
    {
        private static void Postfix(ref FejdStartup __instance)
        {
            // logo
            if(Configuration.Current.ValheimPlus.IsEnabled && Configuration.Current.ValheimPlus.mainMenuLogo)
            {
                GameObject logo = GameObject.Find("LOGO");
                logo.GetComponent<Image>().sprite = VPlusMainMenu.VPlusLogoSprite;
            }

            // version text for bottom right of startup
            __instance.m_versionLabel.fontSize = 14;
            string gameVersion = Version.CombineVersion(global::Version.m_major, global::Version.m_minor, global::Version.m_patch);
            __instance.m_versionLabel.text = "version " + gameVersion + "\n" + "ValheimPlus " + ValheimPlusPlugin.version;

            
            if(Configuration.Current.ValheimPlus.IsEnabled && Configuration.Current.ValheimPlus.serverBrowserAdvertisement)
            {
                DefaultControls.Resources uiResources = new DefaultControls.Resources();
                GameObject joinpanel = GameObjectAssistant.GetChildComponentByName<Transform>("JoinPanel", __instance.m_startGamePanel).gameObject;
                if (joinpanel)
                {
                    foreach (Transform panel in joinpanel.transform)
                    {
                        if (panel.gameObject.name == "Server help")
                        {
                            GameObject serverHelpObj = panel.gameObject;
                            GameObject banner = GameObject.Instantiate(serverHelpObj);
                            banner.transform.SetParent(joinpanel.transform, false);
                            banner.transform.localPosition = banner.transform.localPosition + new Vector3(0, -300);
                            banner.GetComponent<RectTransform>().sizeDelta = new Vector2(315, 315);
                            banner.transform.GetChild(0).GetComponent<Text>().text = "";
                            banner.transform.GetChild(1).gameObject.SetActive(false);
                            GameObject bannerButton = DefaultControls.CreateButton(uiResources);
                            bannerButton.transform.SetParent(banner.transform, false);
                            bannerButton.GetComponentInChildren<Text>().text = "";
                            bannerButton.GetComponent<RectTransform>().sizeDelta = new Vector2(315, 315);
                            bannerButton.GetComponent<Button>().image.sprite = VPlusMainMenu.VPlusBannerSprite;
                            bannerButton.GetComponent<Button>().transition = Selectable.Transition.SpriteSwap;
                            SpriteState ss = new SpriteState();
                            ss.highlightedSprite = VPlusMainMenu.VPlusBannerHoverSprite;
                            bannerButton.GetComponent<Button>().spriteState = ss;
                            bannerButton.GetComponent<Button>().onClick.AddListener(delegate { Application.OpenURL("http://zap-hosting-banner.valheim.plus"); });
                        }
                    }
                }

            }

        }
    }

    /// <summary>
    /// Alters public password requirements
    /// </summary>
    [HarmonyPatch(typeof(FejdStartup), "IsPublicPasswordValid")]
    public static class ChangeServerPasswordBehavior
    {
        private static bool Prefix(ref bool __result)
        {
            if (Configuration.Current.Server.IsEnabled && Configuration.Current.Server.disableServerPassword)
            {
                // return always true
                __result = true;
                return false;
            }

            // continue with default function
            return true;
        }
    }

    /// <summary>
    /// Override password error
    /// </summary>
    [HarmonyPatch(typeof(FejdStartup), "GetPublicPasswordError")]
    public static class RemovePublicPasswordError
    {
        private static bool Prefix(ref string __result)
        {
            if (Configuration.Current.Server.IsEnabled && Configuration.Current.Server.disableServerPassword)
            {
                __result = "";
                return false;
            }

            return true;
        }
    }
}