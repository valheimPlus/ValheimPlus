using BepInEx;
using IniParser;
using IniParser.Model;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace ValheimPlus.Configurations
{
    public class ConfigurationExtra
    {
        public static string GetServerHashFor(Configuration config)
        {
            var serialized = "";
            foreach (var prop in typeof(Configuration).GetProperties())
            {
                var keyName = prop.Name;
                var method = prop.PropertyType.GetMethod("ServerSerializeSection", BindingFlags.Public | BindingFlags.FlattenHierarchy | BindingFlags.Instance);
                
                if (method != null)
                {
                    var instance = prop.GetValue(config, null);
                    string result = (string)method.Invoke(instance, new object[] { });
                    serialized += result;
                }
            }

            return Settings.CreateMD5(serialized);
        }

        public static string ConfigIniPath = Path.GetDirectoryName(Paths.BepInExConfigPath) + Path.DirectorySeparatorChar + "valheim_plus.cfg";

        public static bool LoadSettings()
        {
            try
            {
                if (File.Exists(ConfigIniPath))
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

        /// <summary>
        /// Return a new local configuration fron an IniData
        /// </summary>
        public static Configuration LoadFromIni(IniData configdata)
        {
            Configuration conf = new Configuration();
            // Store the IniData; will be used in a case of a server down sync
            conf.ConfigData = configdata;
            conf.ConfigData.ClearAllComments();
            foreach (var prop in typeof(Configuration).GetProperties())
            {
                string keyName = prop.Name;
                // Try to get Method LoadIni from current config property 
                MethodInfo method = prop.PropertyType.GetMethod("LoadIni", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
                // Check if exist
                if (method != null)
                {
                    // Invoke the property which will result by creating an object of the Type of current property
                    var result = method.Invoke(null, new object[] { configdata, keyName });
                    // Assigned that new object into current config property
                    prop.SetValue(conf, result, null);
                }
            }
            return conf;
        }

        /// <summary>
        /// Return a new local configuration given by a filepath
        /// </summary>
        public static Configuration LoadFromIni(string filename)
        {
            FileIniDataParser parser = new FileIniDataParser();
            return LoadFromIni(parser.ReadFile(filename));
        }

        /// <summary>
        /// Return a new local configuration given by a stream
        /// </summary>
        public static Configuration LoadFromIni(Stream iniStream)
        {
            using (StreamReader iniReader = new StreamReader(iniStream))
            {
                FileIniDataParser parser = new FileIniDataParser();
                return LoadFromIni(parser.ReadData(iniReader));
            }
        }

        /// <summary>
        /// Load a remote configuration given by a stream into current configuration
        /// </summary>
        public static void LoadConfigurationFromStream(Stream iniStream)
        {
            using (StreamReader iniReader = new StreamReader(iniStream))
            {
                FileIniDataParser parser = new FileIniDataParser();
                IniData configdata = parser.ReadData(iniReader);
                configdata.ClearAllComments();
                foreach (var prop in typeof(Configuration).GetProperties())
                {
                    string keyName = prop.Name;

                    // If current Configuration properties is a ServerSyncConfig, HasNeedsServerSync through TryGetBoolMethod will return 1
                    int hasNeedsServerSync = Helper.TryGetBoolMethod(prop, Configuration.Current, "HasNeedsServerSync");
                    if (hasNeedsServerSync > 0)
                    {
                        // Try to get Method LoadIniFromRemote from current config property assembly
                        MethodInfo method = prop.PropertyType.GetMethod("LoadIniFromRemote", BindingFlags.Public | BindingFlags.FlattenHierarchy | BindingFlags.Instance);
                        // Get current instance of the current config property
                        var instance = prop.GetValue(Configuration.Current, null);
                        // Check if Method exist on the current property
                        if (method != null)
                        {
                            // Call instance method LoadIniFromRemote which will overload current config base on received data
                            method.Invoke(instance, new object[] { configdata, keyName });
                        }
                    }
                }
            }
        }
    }
    public static class IniDataExtensions
    {
        public static float GetFloat(this KeyDataCollection data, string key, float defaultVal)
        {
            if (float.TryParse(data[key], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat, out var result)) { 
                return result;
            }

            Debug.LogWarning($"   [Float] Could not read {key}, using default value of {defaultVal}");
            return defaultVal;
        }

        public static bool GetBool(this KeyDataCollection data, string key)
        {
            var truevals = new[] { "y", "yes", "true" };
            return truevals.Contains($"{data[key]}".ToLower());
        }

        public static int GetInt(this KeyDataCollection data, string key, int defaultVal)
        {
            if (int.TryParse(data[key], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat, out var result)) { 
                return result;
            }

            Debug.LogWarning($"   [Int] Could not read {key}, using default value of {defaultVal}");
            return defaultVal;
        }

        public static string GetString(this KeyDataCollection data, string key, string defaultVal)
        {
            string value = $"{data[key]}";
            if (value.Length > 1)
            {
                return value;
            }
            Debug.LogWarning($"   [String] Could not read {key}, using default value of {defaultVal}");
            return defaultVal;
        }

        public static KeyCode GetKeyCode(this KeyDataCollection data, string key, KeyCode defaultVal)
        {
            if (Enum.TryParse<KeyCode>(data[key], out var result)) {
                return result;
            }

            Debug.LogWarning($"   [KeyCode] Could not read {key}, using default value of {defaultVal}");
            return defaultVal;
        }

        public static T LoadConfiguration<T>(this IniData data, string key) where T : BaseConfig<T>, new()
        {
            // this function gives null reference error
            KeyDataCollection idata = data[key];
            return (T)typeof(T).GetMethod("LoadIni").Invoke(null, new[] { idata });
        }
    }
}
