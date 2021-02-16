using BepInEx;
using IniParser;
using IniParser.Model;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using UnityEngine;
using ValheimPlus.Configurations.Sections;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace ValheimPlus.Configurations
{
    public class ConfigurationExtra
    {
        static string ConfigYamlPath = Path.GetDirectoryName(Paths.BepInExConfigPath) + "\\valheim_plus.yml";
        static string ConfigIniPath = Path.GetDirectoryName(Paths.BepInExConfigPath) + "\\valheim_plus.cfg";

        public static bool LoadSettings()
        {
            try
            {

                if (File.Exists(ConfigYamlPath))
                    Configuration.Current = LoadFromYaml(ConfigYamlPath);
                else if (File.Exists(ConfigIniPath))
                    Configuration.Current = LoadFromIni(ConfigIniPath);
                else
                {
                    Debug.LogError("Error: Configuration not found. Plugin not loaded.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Could not load config file: {ex}");
                return false;
            }
            return true;
        }

        public static Configuration LoadFromYaml(string filename)
        {
            var deserializer = new DeserializerBuilder()
            .WithNamingConvention(PascalCaseNamingConvention.Instance)  // see height_in_inches in sample yml 
            .Build();

            return deserializer.Deserialize<Configuration>(File.ReadAllText(filename));
        }

        public static Configuration LoadFromIni(string filename)
        {
            var parser = new FileIniDataParser();
            var configdata = parser.ReadFile(filename);
            var conf = new Configuration()
            {
                AdvancedBuildingMode = AdvancedBuildingModeConfiguration.LoadIni(configdata, "AdvancedBuildingMode"),
                Items = ItemsConfiguration.LoadIni(configdata, "Items"),
                Beehive = BeehiveConfiguration.LoadIni(configdata, "Beehive"),
                Building = BuildingConfiguration.LoadIni(configdata, "Building"),
                Fermenter = FermenterConfiguration.LoadIni(configdata, "Fermenter"),
                Food = FoodConfiguration.LoadIni(configdata, "Food"),
                Furnace = FurnaceConfiguration.LoadIni(configdata, "Furnace"),
                Hotkeys = HotkeyConfiguration.LoadIni(configdata, "Hotkeys"),
                Kiln = KilnConfiguration.LoadIni(configdata, "Kiln"),
                Map = MapConfiguration.LoadIni(configdata, "Map"),
                Player = PlayerConfiguration.LoadIni(configdata, "Player"),
                Server = ServerConfiguration.LoadIni(configdata, "Server"),
                Stamina = StaminaConfiguration.LoadIni(configdata, "Stamina"),
                AdvancedEditingMode = AdvancedEditingModeConfiguration.LoadIni(configdata, "AdvancedEditingMode")
            };

            return conf;
        }

    }

    public interface IConfig
    {
        void LoadIniData(KeyDataCollection data);
    }

    public abstract class BaseConfig<T> : IConfig where T : IConfig, new()
    {
        public bool IsEnabled = false;

        public static T LoadIni(IniData data, string section)
        {
            var n = new T();

            Debug.Log($"Loading config section {section}");
            if (data[section] == null || data[section]["enabled"] == null || !data[section].GetBool("enabled")) return n;

            n.LoadIniData(data[section]);
            return n;
        }

        public void LoadIniData(KeyDataCollection data)
        {
            IsEnabled = true;

            foreach (var prop in typeof(T).GetProperties())
            {
                var keyName = prop.Name;

                // Set first char of keyName to lowercase
                if (keyName != string.Empty && char.IsUpper(keyName[0]))
                {
                    keyName = char.ToLower(keyName[0]) + keyName.Substring(1);
                }

                if (ValheimPlusPlugin.isDebug)
                {
                    if (data.ContainsKey(keyName))
                        Debug.Log($"Loading key {keyName}");
                    else
                        Debug.Log($"Key {keyName} not defined, using default value");
                }

                if (!data.ContainsKey(keyName)) continue;

                if (prop.PropertyType == typeof(float))
                {
                    prop.SetValue(this, data.GetFloat(keyName), null);
                    continue;
                }

                if (prop.PropertyType == typeof(int))
                {
                    prop.SetValue(this, data.GetInt(keyName), null);
                    continue;
                }

                if (prop.PropertyType == typeof(bool))
                {
                    prop.SetValue(this, data.GetBool(keyName), null);
                    continue;
                }

                if (prop.PropertyType == typeof(KeyCode))
                {
                    prop.SetValue(this, data.GetKeyCode(keyName), null);
                    continue;
                }

                Debug.LogWarning($"Could not load data of type {prop.PropertyType} for key {keyName}");
            }
        }

    }

    public static class IniDataExtensions
    {
        public static float GetFloat(this KeyDataCollection data, string key)
        {
            return float.Parse(data[key], CultureInfo.InvariantCulture.NumberFormat);
        }
        public static bool GetBool(this KeyDataCollection data, string key)
        {
            var truevals = new[] { "y", "yes", "true" };
            return truevals.Contains(data[key].ToLower());
        }
        public static int GetInt(this KeyDataCollection data, string key)
        {
            return int.Parse(data[key]);
        }

        public static KeyCode GetKeyCode(this KeyDataCollection data, string key)
        {
            return (KeyCode)System.Enum.Parse(typeof(KeyCode), data[key]);
        }

        public static T LoadConfiguration<T>(this IniData data, string key) where T : BaseConfig<T>, new()
        {
            // this function gives null reference error
            var idata = data[key];
            return (T)typeof(T).GetMethod("LoadIni").Invoke(null, new[] { idata });
        }

    }
}
