using BepInEx;
using IniParser;
using IniParser.Model;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
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

            var conf = new Configuration();
            foreach (var prop in typeof(Configuration).GetProperties())
            {
                var keyName = prop.Name;
                var method = prop.PropertyType.GetMethod("LoadIni", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);

                if (method != null)
                {
                    var result = method.Invoke(null, new object[] { configdata, keyName });
                    prop.SetValue(conf, result, null);
                }
            }
            //conf = new Configuration()
            //{
            //    AdvancedBuildingMode = AdvancedBuildingModeConfiguration.LoadIni(configdata, "AdvancedBuildingMode"),
            //    Items = ItemsConfiguration.LoadIni(configdata, "Items"),
            //    Beehive = BeehiveConfiguration.LoadIni(configdata, "Beehive"),
            //    Building = BuildingConfiguration.LoadIni(configdata, "Building"),
            //    Fermenter = FermenterConfiguration.LoadIni(configdata, "Fermenter"),
            //    Food = FoodConfiguration.LoadIni(configdata, "Food"),
            //    Furnace = FurnaceConfiguration.LoadIni(configdata, "Furnace"),
            //    Hotkeys = HotkeyConfiguration.LoadIni(configdata, "Hotkeys"),
            //    Kiln = KilnConfiguration.LoadIni(configdata, "Kiln"),
            //    Map = MapConfiguration.LoadIni(configdata, "Map"),
            //    Player = PlayerConfiguration.LoadIni(configdata, "Player"),
            //    Server = ServerConfiguration.LoadIni(configdata, "Server"),
            //    Stamina = StaminaConfiguration.LoadIni(configdata, "Stamina"),
            //    AdvancedEditingMode = AdvancedEditingModeConfiguration.LoadIni(configdata, "AdvancedEditingMode")
            //};

            return conf;
        }
    }

    public static class IniDataExtensions
    {
        public static float GetFloat(this KeyDataCollection data, string key, float defaultVal)
        {
            if (float.TryParse(data[key], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat, out var result)) { 
                return result;
            }
            Debug.LogWarning($"[Float] Could not read {key}, using default value of {defaultVal}");
            return defaultVal;
        }
        public static bool GetBool(this KeyDataCollection data, string key)
        {
            var truevals = new[] { "y", "yes", "true" };
            return truevals.Contains(data[key].ToLower());
        }
        public static int GetInt(this KeyDataCollection data, string key, int defaultVal)
        {
            if (int.TryParse(data[key], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat, out var result)) { 
                return result;
            }
            Debug.LogWarning($"[Int] Could not read {key}, using default value of {defaultVal}");
            return defaultVal;
        }

        public static KeyCode GetKeyCode(this KeyDataCollection data, string key, KeyCode defaultVal)
        {
            if (System.Enum.TryParse<KeyCode>(data[key], out var result)) {
                return result;
            }
            Debug.LogWarning($"[KeyCode] Could not read {key}, using default value of {defaultVal}");
            return defaultVal;
        }

        public static T LoadConfiguration<T>(this IniData data, string key) where T : BaseConfig<T>, new()
        {
            // this function gives null reference error
            var idata = data[key];
            return (T)typeof(T).GetMethod("LoadIni").Invoke(null, new[] { idata });
        }

    }
}
