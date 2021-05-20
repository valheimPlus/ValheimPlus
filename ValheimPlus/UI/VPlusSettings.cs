using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using HarmonyLib;
using IniParser;
using IniParser.Model;
using UnityEngine;
using UnityEngine.SceneManagement;
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
        public static GameObject enableToggle = null;
        public static Font norseFont;
        public static Color32 vplusYellow = new Color32(255, 164, 0, 255);
        public static GameObject settingsContentPanel;
        public static Dictionary<string, List<GameObject>> settingFamillySettings;
        public static Button applyButton;
        public static Button okButton;
        public static Dropdown dropper;
        public static GameObject uiTooltipPrefab = null;
        public static string[] keycodeNames;

        public static List<string> availableSettings { get; private set; }

        public static object GetPropValue(object src, string propName)
        {
            return src.GetType().GetProperty(propName).GetValue(src, null);
        }

        public static GameObject CreateEnableToggle(string settingName, string value, Transform parent, string comments)
        {            
            bool currentVal = bool.Parse(value);
            GameObject enableToggleThis = GameObject.Instantiate(enableToggle);
            enableToggleThis.AddComponent<UITooltip>().m_tooltipPrefab = uiTooltipPrefab;
            enableToggleThis.GetComponent<UITooltip>().Set($"{settingName}", comments);
            enableToggleThis.GetComponentInChildren<Toggle>().isOn = currentVal;
            enableToggleThis.transform.SetParent(parent, false);
            enableToggleThis.SetActive(false);
            return enableToggleThis;
        }

        public static GameObject CreateTextSettingEntry(string name, string value, Transform parent, string comments)
        {            
            GameObject settingName = GameObject.Instantiate(modSettingsBundle.LoadAsset<GameObject>("SettingEntry_Text"));
            settingName.name = name;
            settingName.transform.GetChild(0).GetComponent<Text>().text = $"Setting: {name}\nCurrent value: {value}";
            settingName.transform.SetParent(parent, false);
            settingName.AddComponent<UITooltip>().m_tooltipPrefab = uiTooltipPrefab;
            settingName.GetComponent<UITooltip>().Set($"{name}", comments);
            settingName.SetActive(false);
            return settingName;    
        }

        public static GameObject CreateKeyCodeSettingEntry(string name, string value, Transform parent, string comments)
        {
            GameObject settingName = GameObject.Instantiate(modSettingsBundle.LoadAsset<GameObject>("SettingEntry_KeyCode"));
            settingName.name = name;
            settingName.transform.GetChild(0).GetComponent<Text>().text = $"Setting: {name}";
            settingName.transform.SetParent(parent, false);
            Dropdown dropper = settingName.GetComponentInChildren<Dropdown>();
            dropper.options.Clear();
            foreach(string key in keycodeNames)
            {
                dropper.options.Add(new Dropdown.OptionData(key));
            }
            dropper.value = keycodeNames.ToList().IndexOf(value);
            settingName.AddComponent<UITooltip>().m_tooltipPrefab = uiTooltipPrefab;
            settingName.GetComponent<UITooltip>().Set($"{name}", comments);
            settingName.SetActive(false);
            return settingName;
        }

        public static GameObject CreateBoolSettingEntry(string name, string value, Transform parent, string comments)
        {
            GameObject settingName = GameObject.Instantiate(modSettingsBundle.LoadAsset<GameObject>("SettingEntry_Toggle"));
            settingName.name = name;
            settingName.AddComponent<UITooltip>().m_tooltipPrefab = uiTooltipPrefab;
            settingName.GetComponent<UITooltip>().Set($"{name}", comments);
            settingName.transform.GetChild(0).GetComponent<Text>().text = $"Setting: {name}";
            settingName.GetComponentInChildren<Toggle>().isOn = bool.Parse(value);
            settingName.transform.SetParent(parent, false);
            settingName.GetComponentInChildren<Toggle>().gameObject.GetComponent<RectTransform>().localPosition = new Vector3(0, -12.5f, 0);
            settingName.SetActive(false);
            return settingName;
        }

        public static void Load()
        {
            norseFont = Resources.FindObjectsOfTypeAll<Font>().FirstOrDefault(fnt => fnt.name == "Norse");
            //norseFont = Resources.FindObjectsOfTypeAll<Font>().FirstOrDefault(fnt => fnt.name == "Arial");
            modSettingsBundle = AssetBundle.LoadFromStream(EmbeddedAsset.LoadEmbeddedAsset("Assets.Bundles.settings-ui"));
            modSettingsPanelCloner = modSettingsBundle.LoadAsset<GameObject>("Mod Settings");
            enableToggle = modSettingsBundle.LoadAsset<GameObject>("Toggle_Enable");
            uiTooltipPrefab = modSettingsBundle.LoadAsset<GameObject>("SettingsTooltip");
            keycodeNames = Enum.GetNames(typeof(KeyCode));
            modSettingsPanelCloner.SetActive(false);            
        }

        public static void Apply()
        {
            if (File.Exists(ConfigurationExtra.ConfigIniPath))
            {
                FileIniDataParser parser = new FileIniDataParser();
                IniData configdata = parser.ReadFile(ConfigurationExtra.ConfigIniPath);
                foreach (KeyValuePair<string, List<GameObject>> settingSection in settingFamillySettings)
                {
                    foreach (GameObject settingEntry in settingSection.Value)
                    {
                        PropertyInfo propSection = Configuration.Current.GetType().GetProperty(settingSection.Key);
                        var settingFamilyProp = propSection.GetValue(Configuration.Current, null);
                        Type propType = settingFamilyProp.GetType();
                        if (settingEntry.name.Contains("Toggle_Enable"))
                        {
                            Toggle enableSectionTog = settingEntry.GetComponentInChildren<Toggle>();
                            FieldInfo prop = propType.GetField("IsEnabled");
                            prop.SetValue(settingFamilyProp, enableSectionTog.isOn);
                            configdata[settingSection.Key]["enabled"] = enableSectionTog.isOn.ToString();
                        }
                        else
                        {
                            InputField inputEntry = settingEntry.GetComponentInChildren<InputField>();
                            Toggle enableSectionTog = settingEntry.GetComponentInChildren<Toggle>();
                            Dropdown inputDropdown = settingEntry.GetComponentInChildren<Dropdown>();
                            if (inputEntry != null)
                            {
                                string newVal = inputEntry.text;
                                if (newVal == "")
                                    continue;
                                else
                                {
                                    PropertyInfo prop = propType.GetProperty(settingEntry.name.Replace("(Clone)", ""));
                                    if (prop.PropertyType == typeof(float))
                                    {
                                        prop.SetValue(settingFamilyProp, float.Parse(newVal), null);
                                        configdata[settingSection.Key][settingEntry.name.Replace("(Clone)", "")] = newVal;
                                        continue;
                                    }

                                    if (prop.PropertyType == typeof(int))
                                    {
                                        prop.SetValue(settingFamilyProp, int.Parse(newVal), null);
                                        configdata[settingSection.Key][settingEntry.name.Replace("(Clone)", "")] = newVal;
                                        continue;
                                    }

                                    if (prop.PropertyType == typeof(bool))
                                    {
                                        prop.SetValue(settingFamilyProp, bool.Parse(newVal), null);
                                        configdata[settingSection.Key][settingEntry.name.Replace("(Clone)", "")] = newVal;
                                        continue;
                                    }

                                    if (prop.PropertyType == typeof(KeyCode) && !RPC.VPlusConfigSync.isConnecting)
                                    {
                                        prop.SetValue(settingFamilyProp, Enum.Parse(typeof(KeyCode), newVal), null);
                                        configdata[settingSection.Key][settingEntry.name.Replace("(Clone)", "")] = newVal;
                                        continue;
                                    }
                                    settingFamillySettings[settingSection.Key][settingFamillySettings[settingSection.Key].IndexOf(settingEntry)] =
                                        CreateTextSettingEntry(settingEntry.name.Replace("(Clone)", ""),
                                        newVal, settingEntry.transform.parent,
                                        string.Join("\n", configdata[settingSection.Key].GetKeyData(settingEntry.name.Replace("(Clone)", "")).Comments));
                                }
                            }
                            else if (inputDropdown != null)
                            {
                                PropertyInfo prop = propType.GetProperty(settingEntry.name.Replace("(Clone)", ""));
                                string newVal = keycodeNames[inputDropdown.value];
                                if (newVal == ((KeyCode)prop.GetValue(settingFamilyProp, null)).ToString())
                                    continue;
                                if (prop.PropertyType == typeof(KeyCode) && !RPC.VPlusConfigSync.isConnecting)
                                {
                                    prop.SetValue(settingFamilyProp, Enum.Parse(typeof(KeyCode), newVal), null);
                                    configdata[settingSection.Key][settingEntry.name.Replace("(Clone)", "")] = newVal;
                                    continue;
                                }
                            }
                            else
                            {
                                PropertyInfo prop = propType.GetProperty(settingEntry.name.Replace("(Clone)", ""));
                                prop.SetValue(settingFamilyProp, enableSectionTog.isOn, null);
                                configdata[settingSection.Key][settingEntry.name.Replace("(Clone)", "")] = enableSectionTog.isOn.ToString();
                            }
                        }
                    }
                }
                parser.WriteFile(ConfigurationExtra.ConfigIniPath, configdata);
                ValheimPlusPlugin.harmony.UnpatchSelf();
                ValheimPlusPlugin.harmony.PatchAll();
            }
        }

        public static void Show()
        {
            norseFont = Resources.FindObjectsOfTypeAll<Font>().FirstOrDefault(fnt => fnt.name == "Norse");
            settingFamillySettings = new Dictionary<string, List<GameObject>>();
            if (modSettingsPanelCloner == null)
                Load();
            if (modSettingsPanel == null)
            {
                modSettingsPanel = GameObject.Instantiate(modSettingsPanelCloner);
                modSettingsPanel.transform.SetParent(FejdStartup.instance.m_mainMenu.transform, false);
                modSettingsPanel.transform.localPosition = Vector3.zero;
                applyButton = GameObjectAssistant.GetChildComponentByName<Button>("Apply", modSettingsPanel);
                dropper = modSettingsPanel.GetComponentInChildren<Dropdown>();
                okButton = GameObjectAssistant.GetChildComponentByName<Button>("OK", modSettingsPanel);
                applyButton.onClick.AddListener(delegate {
                    int dropdownval = dropper.value;
                    dropper.value = 0;
                    Apply();
                    Show();
                    dropper.value = dropdownval;
                });
                okButton.onClick.AddListener(delegate { 
                    Apply();
                    modSettingsPanel.SetActive(false);
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                });
                applyButton.gameObject.SetActive(false);
            }            
            settingsContentPanel = GameObjectAssistant.GetChildComponentByName<Transform>("SettingsContent", modSettingsPanel).gameObject;
            settingsContentPanel.transform.parent.parent.gameObject.GetComponentInChildren<Scrollbar>().direction = Scrollbar.Direction.BottomToTop;
            dropper.options.Clear();
            FileIniDataParser parser = new FileIniDataParser();
            IniData configdata = parser.ReadFile(ConfigurationExtra.ConfigIniPath);
            foreach (var prop in typeof(Configuration).GetProperties())
            {
                string keyName = prop.Name;
                if (keyName == "Current" || keyName == "Settings" || keyName == "Time" || keyName == "Deconstruct")
                    continue;
                else
                {
                    settingFamillySettings.Add(keyName, new List<GameObject>());
                    var settingFamillyProp = Configuration.Current.GetType().GetProperty(prop.Name).GetValue(Configuration.Current, null);
                    GameObject enableToggleThis = CreateEnableToggle(keyName, 
                        settingFamillyProp.GetType().GetField("IsEnabled").GetValue(settingFamillyProp).ToString(),
                        settingsContentPanel.transform,
                        string.Join("\n", configdata[keyName].GetKeyData("enabled").Comments));
                    settingFamillySettings[keyName].Add(enableToggleThis);
                    var praeteriCommentarium = "";
                    foreach (var setting in prop.PropertyType.GetProperties())
                    {
                        if (setting.Name == "NeedsServerSync")
                            continue;
                        var keyDatumCommentate = configdata[keyName].GetKeyData(setting.Name);
                        var commentarium = "";
                        if (keyDatumCommentate != null)
                        {
                            commentarium = string.Join("\n", keyDatumCommentate.Comments);
                            praeteriCommentarium = commentarium;
                        }
                        else
                            commentarium = praeteriCommentarium;
                        if (settingFamillyProp.GetType().GetProperty(setting.Name).PropertyType == typeof(bool))
                            settingFamillySettings[keyName].Add(CreateBoolSettingEntry(setting.Name,
                            settingFamillyProp.GetType().GetProperty(setting.Name).GetValue(settingFamillyProp, null).ToString(),
                            settingsContentPanel.transform, commentarium));
                        else if (settingFamillyProp.GetType().GetProperty(setting.Name).PropertyType == typeof(KeyCode))
                            settingFamillySettings[keyName].Add(CreateKeyCodeSettingEntry(setting.Name,
                            settingFamillyProp.GetType().GetProperty(setting.Name).GetValue(settingFamillyProp, null).ToString(),
                            settingsContentPanel.transform, commentarium));
                        else
                            settingFamillySettings[keyName].Add(CreateTextSettingEntry(setting.Name,
                                settingFamillyProp.GetType().GetProperty(setting.Name).GetValue(settingFamillyProp, null).ToString(),
                            settingsContentPanel.transform, commentarium));
                    }
                    dropper.options.Add(new Dropdown.OptionData(keyName));
                }
            }
            availableSettings = dropper.options.Select(option => option.text).ToList();
            dropper.onValueChanged.AddListener(delegate {
                foreach(Transform ting in settingsContentPanel.transform) { ting.gameObject.SetActive(false); }
                foreach(GameObject newTing in settingFamillySettings[availableSettings[dropper.value]]) { newTing.SetActive(true); }
            });
            dropper.value = availableSettings.IndexOf("ValheimPlus");
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
