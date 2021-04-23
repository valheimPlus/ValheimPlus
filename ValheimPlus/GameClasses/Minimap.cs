using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using ValheimPlus.Configurations;
using ValheimPlus.RPC;
using ValheimPlus.Utility;
using Random = UnityEngine.Random;

// ToDo add packet system to convey map markers
namespace ValheimPlus.GameClasses
{
    /// <summary>
    /// Hooks base explore method
    /// </summary>
    [HarmonyPatch(typeof(Minimap))]
    public class HookExplore
    {
        [HarmonyReversePatch]
        [HarmonyPatch(typeof(Minimap), "Explore", new Type[] { typeof(Vector3), typeof(float) })]
        public static void call_Explore(object instance, Vector3 p, float radius) => throw new NotImplementedException();
    }

    /// <summary>
    /// Update exploration for all players
    /// </summary>
    [HarmonyPatch(typeof(Minimap), "UpdateExplore")]
    public static class ChangeMapBehavior
    {
        private static void Prefix(ref float dt, ref Player player, ref Minimap __instance, ref float ___m_exploreTimer, ref float ___m_exploreInterval)
        {
            if (Configuration.Current.Map.exploreRadius > 10000) Configuration.Current.Map.exploreRadius = 10000;

            if (!Configuration.Current.Map.IsEnabled) return;

            if (Configuration.Current.Map.shareMapProgression)
            {
                float explorerTime = ___m_exploreTimer;
                explorerTime += Time.deltaTime;
                if (explorerTime > ___m_exploreInterval)
                {
                    if (ZNet.instance.m_players.Any())
                    {
                        foreach (ZNet.PlayerInfo m_Player in ZNet.instance.m_players)
                        {
                            HookExplore.call_Explore(__instance, m_Player.m_position, Configuration.Current.Map.exploreRadius);
                        }
                    }
                }
            }

            // Always reveal for your own, we do this non the less to apply the potentially bigger exploreRadius
            HookExplore.call_Explore(__instance, player.transform.position, Configuration.Current.Map.exploreRadius);
        }
    }

    [HarmonyPatch(typeof(Minimap), "Awake")]
    public static class MinimapAwake
    {
        private static void Postfix()
        {
            if (ZNet.m_isServer && Configuration.Current.Map.IsEnabled && Configuration.Current.Map.shareMapProgression)
            {
                //Init map array
                VPlusMapSync.ServerMapData = new bool[Minimap.instance.m_textureSize * Minimap.instance.m_textureSize];

                //Load map data from disk
                VPlusMapSync.LoadMapDataFromDisk();

                //Start map data save timer
                ValheimPlusPlugin.mapSyncSaveTimer.Start();
            }
        }
    }

    public static class MapPinEditor_Patches
    {
        public static GameObject pinEditorPanel;
        public static AssetBundle mapPinBundle = null;
        public static Dropdown iconSelected;
        public static InputField pinName;
        public static Toggle sharePin;
        public static Vector3 pinPos;

        [HarmonyPatch(typeof(Minimap), nameof(Minimap.AddPin))]
        public static class Minimap_AddPin_Patch
        {
            private static void Postfix(ref Minimap __instance, ref Minimap.PinData __result)
            {
                if (Configuration.Current.Map.IsEnabled && Configuration.Current.Map.shareAllPins)
                    VPlusMapPinSync.SendMapPinToServer(__result);
            }
        }

        [HarmonyPatch(typeof(Minimap), "Awake")]
        public static class MapPinEditor_Patches_Awake
        {
            private static void AddPin(ref Minimap __instance)
            {
                Minimap.PinType pintype = iconSelected.value == 4 ? Minimap.PinType.Icon4 : (Minimap.PinType)iconSelected.value;
                Minimap.PinData addedPin = __instance.AddPin(pinPos, pintype, pinName.text, true, false);
                if (Configuration.Current.Map.shareablePins && sharePin.isOn && !Configuration.Current.Map.shareAllPins)
                    VPlusMapPinSync.SendMapPinToServer(addedPin);
                pinEditorPanel.SetActive(false);
                __instance.m_wasFocused = false;
            }

            private static void Postfix(ref Minimap __instance)
            {
                if (Configuration.Current.Map.IsEnabled)
                {
                    GameObject iconPanelOld = GameObjectAssistant.GetChildComponentByName<Transform>("IconPanel", __instance.m_largeRoot).gameObject;
                    for (int i = 0; i < 5; i++)
                    {
                        GameObjectAssistant.GetChildComponentByName<Transform>("Icon" + i.ToString(), iconPanelOld).gameObject.SetActive(false);
                    }
                    GameObjectAssistant.GetChildComponentByName<Transform>("Bkg", iconPanelOld).gameObject.SetActive(false);
                    iconPanelOld.SetActive(false);
                    __instance.m_nameInput.gameObject.SetActive(false);
                    if (mapPinBundle == null)
                    {
                        mapPinBundle = AssetBundle.LoadFromStream(EmbeddedAsset.LoadEmbeddedAsset("Assets.Bundles.map-pin-ui"));
                    }
                    GameObject pinEditorPanelParent = mapPinBundle.LoadAsset<GameObject>("MapPinEditor");
                    pinEditorPanel = GameObject.Instantiate(pinEditorPanelParent.transform.GetChild(0).gameObject);
                    pinEditorPanel.transform.SetParent(__instance.m_largeRoot.transform, false);
                    pinEditorPanel.SetActive(false);

                    pinName = pinEditorPanel.GetComponentInChildren<InputField>();
                    if (pinName != null)
                        Debug.Log("Pin Name loaded properly");
                    Minimap theInstance = __instance;
                    GameObjectAssistant.GetChildComponentByName<Transform>("OK", pinEditorPanel).gameObject.GetComponent<Button>().onClick.AddListener(delegate { AddPin(ref theInstance); });
                    GameObjectAssistant.GetChildComponentByName<Transform>("Cancel", pinEditorPanel).gameObject.GetComponent<Button>().onClick.AddListener(delegate { Minimap.instance.m_wasFocused = false; pinEditorPanel.SetActive(false); });
                    iconSelected = pinEditorPanel.GetComponentInChildren<Dropdown>();
                    iconSelected.options.Clear();
                    int ind = 0;
                    List<string> list = new List<string> { "Fire", "Home", "Hammer", "Circle", "Rune" };
                    foreach (string option in list)
                    {
                        iconSelected.options.Add(new Dropdown.OptionData(option, Minimap.instance.m_icons[ind].m_icon));
                        ind++;
                    }
                    if (iconSelected != null)
                        Debug.Log("Dropdown loaded properly");
                    sharePin = pinEditorPanel.GetComponentInChildren<Toggle>();
                    if (sharePin != null)
                        Debug.Log("Share pin loaded properly");
                    if (!Configuration.Current.Map.shareablePins || Configuration.Current.Map.shareAllPins)
                        sharePin.gameObject.SetActive(false);
                }
            }
        }

        [HarmonyPatch(typeof(Minimap), "OnMapDblClick")]
        public static class MapPinEditor_Patches_OnMapDblClick
        {
            private static bool Prefix(ref Minimap __instance)
            {
                if (Configuration.Current.Map.IsEnabled)
                {
                    if (!pinEditorPanel.activeSelf)
                    {
                        pinEditorPanel.SetActive(true);
                    }
                    if (!pinName.isFocused)
                    {
                        EventSystem.current.SetSelectedGameObject(pinName.gameObject);
                    }
                    pinPos = __instance.ScreenToWorldPoint(Input.mousePosition);
                    __instance.m_wasFocused = true;
                    //iconPanel.transform.localPosition = pinPos;
                    return false;
                }
                return true;
            }
        }

        [HarmonyPatch(typeof(Minimap), "UpdateNameInput")]
        public static class MapPinEditor_Patches_UpdateNameInput
        {
            private static bool Prefix(ref Minimap __instance)
            {
                if (Configuration.Current.Map.IsEnabled)
                {
                    // Break out of this unnecessary thing
                    return false;
                }
                return true;
            }
        }
    }
}
