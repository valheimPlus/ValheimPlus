using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;
using ValheimPlus.Configurations;
using ValheimPlus.Utility;

namespace ValheimPlus.UI
{
    public static class VPlusSettings
    {
        public static bool haveAddedModMenu = false;
        public static bool haveLowered = false;
        public static GameObject modSettingsPanel = null;
        public static GameObject modSettingsPanelCloner = null;
        public static AssetBundle modSettingsBundle = null;
        public static Font norseFont;
        public static Color vplusYellow = new Color(255, 164, 0);
        public static GameObject settingsContentPanel;
        public static Dictionary<string, List<GameObject>> settingFamillySettings;

        public static List<string> availableSettings { get; private set; }

        public static object GetPropValue(object src, string propName)
        {
            return src.GetType().GetProperty(propName).GetValue(src, null);
        }

        public static GameObject CreateSettingEntry(string name, string value, Transform parent)
        {
            GameObject settingName = GameObject.Instantiate(new GameObject(name, typeof(RectTransform), typeof(Text), typeof(Outline), typeof(LayoutElement)));
            settingName.GetComponent<Text>().font = norseFont;
            settingName.GetComponent<Text>().color = vplusYellow;
            settingName.GetComponent<Outline>().effectDistance = new Vector2(2, 2);
            settingName.GetComponent<LayoutElement>().minHeight = 250;
            settingName.GetComponent<Text>().fontSize = 30;
            settingName.GetComponent<Text>().text = name;
            settingName.GetComponent<Text>().fontStyle = FontStyle.Bold;
            settingName.GetComponent<RectTransform>().sizeDelta = new Vector2(400, 100);

            settingName.transform.SetParent(parent, false);

            GameObject settingValue = GameObject.Instantiate(new GameObject($"{name}_value", typeof(RectTransform), typeof(Image), typeof(InputField)));
            settingValue.transform.SetParent(settingName.transform, false);
            settingValue.GetComponent<RectTransform>().localPosition = new Vector2(25, -25);
            settingValue.GetComponent<RectTransform>().anchoredPosition = new Vector2(25, -25);
            settingValue.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f);
            settingValue.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);
            settingValue.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
            settingValue.GetComponent<RectTransform>().sizeDelta = new Vector2(250, 50);
            settingValue.GetComponent<Image>().sprite = modSettingsPanel.transform.GetChild(0).gameObject.GetComponent<Image>().sprite;
            

            GameObject settingvaluePlaceholder = GameObject.Instantiate(new GameObject($"{name}_Placeholder", typeof(RectTransform), typeof(Text), typeof(Outline)));
            settingvaluePlaceholder.transform.SetParent(settingValue.transform, false);
            settingvaluePlaceholder.GetComponent<RectTransform>().localPosition = new Vector2(0, -0.5f);
            settingvaluePlaceholder.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -0.5f);
            settingvaluePlaceholder.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
            settingvaluePlaceholder.GetComponent<RectTransform>().anchorMax = new Vector2(1, 1);
            settingvaluePlaceholder.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
            settingvaluePlaceholder.GetComponent<RectTransform>().sizeDelta = new Vector2(-20, -13);
            settingvaluePlaceholder.GetComponent<Outline>().effectDistance = new Vector2(2, 2);
            settingvaluePlaceholder.GetComponent<Text>().font = norseFont;
            settingvaluePlaceholder.GetComponent<Text>().color = vplusYellow;
            settingvaluePlaceholder.GetComponent<Text>().fontSize = 20;
            settingvaluePlaceholder.GetComponent<Text>().fontStyle = FontStyle.BoldAndItalic;
            settingvaluePlaceholder.GetComponent<Text>().text = "";

            GameObject settingvalueText = GameObject.Instantiate(new GameObject($"{name}_value_text", typeof(RectTransform), typeof(Text), typeof(Outline)));
            settingvalueText.transform.SetParent(settingValue.transform, false);
            settingvalueText.GetComponent<RectTransform>().localPosition = new Vector2(0, -0.5f);
            settingvalueText.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -0.5f);
            settingvalueText.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
            settingvalueText.GetComponent<RectTransform>().anchorMax = new Vector2(1, 1);
            settingvalueText.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
            settingvalueText.GetComponent<RectTransform>().sizeDelta = new Vector2(-20, -13);
            settingvalueText.GetComponent<Outline>().effectDistance = new Vector2(2, 2);
            settingvalueText.GetComponent<Text>().font = norseFont;
            settingvalueText.GetComponent<Text>().color = vplusYellow;
            settingvalueText.GetComponent<Text>().fontSize = 20;
            settingvalueText.GetComponent<Text>().fontStyle = FontStyle.BoldAndItalic;
            settingvalueText.GetComponent<Text>().text = value;

            settingName.SetActive(false);
            return settingName;
        }

        public static void Load()
        {
            norseFont = Resources.FindObjectsOfTypeAll<Font>().FirstOrDefault(fnt => fnt.name == "Norse");
            modSettingsBundle = AssetBundle.LoadFromStream(EmbeddedAsset.LoadEmbeddedAsset("Assets.Bundles.settings-ui"));
            modSettingsPanelCloner = modSettingsBundle.LoadAsset<GameObject>("Mod Settings");
            modSettingsPanelCloner.SetActive(false);            
        }

        public static void Show()
        {
            norseFont = Resources.FindObjectsOfTypeAll<Font>().FirstOrDefault(fnt => fnt.name == "Norse");
            settingFamillySettings = new Dictionary<string, List<GameObject>>();
            if (modSettingsPanel == null)
            {
                modSettingsPanel = GameObject.Instantiate(modSettingsPanelCloner);
                modSettingsPanel.transform.SetParent(FejdStartup.instance.m_mainMenu.transform, false);
                modSettingsPanel.transform.localPosition = Vector3.zero;
            }
            Dropdown dropper = modSettingsPanel.GetComponentInChildren<Dropdown>();
            settingsContentPanel = GameObjectAssistant.GetChildComponentByName<Transform>("Content", dropper.gameObject.transform.parent.GetChild(2).gameObject).gameObject;
            dropper.options.Clear();
            foreach (var prop in typeof(Configuration).GetProperties())
            {
                string keyName = prop.Name;
                if (keyName == "Current" || keyName == "Settings")
                    continue;
                else
                {
                    settingFamillySettings.Add(keyName, new List<GameObject>());
                    foreach (var setting in prop.PropertyType.GetProperties())
                    {
                        var test = Configuration.Current.GetType().GetProperty(prop.Name).GetValue(Configuration.Current, null);
                        settingFamillySettings[keyName].Add(CreateSettingEntry(setting.Name,
                            test.GetType().GetProperty(setting.Name).GetValue(test, null).ToString(),
                        settingsContentPanel.transform
                        ));
                    }
                    dropper.options.Add(new Dropdown.OptionData(keyName));
                }
            }
            availableSettings = dropper.options.Select(option => option.text).ToList();
            dropper.onValueChanged.AddListener(delegate {
                foreach(Transform ting in settingsContentPanel.transform) { ting.gameObject.SetActive(false); }
                foreach(GameObject newTing in settingFamillySettings[availableSettings[dropper.value]]) { newTing.SetActive(true); }
            });
            modSettingsPanel.SetActive(true);
        }
    }

    [HarmonyPatch(typeof(FejdStartup), "Update")]
    class PatchUI
    {
        static Transform newStart = null;
        static void Postfix(ref GameObject ___m_mainMenu, ref GameObject ___m_settingsPrefab)
        {
            if (Input.GetKeyDown(KeyCode.F10))
            {
                if (newStart == null && !VPlusSettings.haveAddedModMenu)
                {
                    foreach (Transform child in ___m_mainMenu.transform)
                    {
                        foreach (Transform grandchild in child)
                        {
                            if (grandchild.name == "Start game" && !VPlusSettings.haveAddedModMenu)
                            {
                                newStart = GameObject.Instantiate(grandchild);
                                newStart.name = "V+ Settings";
                                newStart.SetParent(child);
                                newStart.GetComponentInChildren<Text>().text = "V+ Settings";
                                newStart.position = grandchild.position + new Vector3(0, -35, 0);
                                Button FOff = newStart.gameObject.GetComponent<Button>();
                                GameObject.DestroyImmediate(FOff);
                                Button newButton = newStart.gameObject.AddComponent<Button>();
                                newButton.transition = Selectable.Transition.Animation;
                                newButton.onClick.AddListener(() =>
                                {
                                    VPlusSettings.Show();
                                });
                                VPlusSettings.haveAddedModMenu = true;
                            }
                            else if (grandchild.name == "Join game" || grandchild.name == "Settings" || grandchild.name == "Credits" ||
                                     grandchild.name == "Exit")
                            {
                                if (!VPlusSettings.haveLowered)
                                    grandchild.position = grandchild.position + new Vector3(0, -30, 0);
                            }
                        }
                        if (VPlusSettings.haveAddedModMenu)
                            VPlusSettings.haveLowered = true;
                    }
                }
                else
                {
                    VPlusSettings.haveAddedModMenu = false;
                    VPlusSettings.haveLowered = false;
                }
            }
        }
    }
}
