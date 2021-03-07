using IniParser.Model;
using System.ComponentModel;
using System.Linq;
using UnityEngine;
using ValheimPlus.Utility;

namespace ValheimPlus.Configurations
{
    public interface IConfig
    {
        void LoadSettings(KeyDataCollection data);
        void LoadKeycodes(KeyDataCollection data);
    }

    public abstract class BaseConfig<T> : IConfig where T : IConfig, new()
    {

        public bool HasNeedsServerSync()
        {
            return NeedsServerSync;
        }

        public string ServerSerializeSection()
        {
            if (!IsEnabled || !NeedsServerSync) return "";

            var r = "";

            foreach (var prop in typeof(T).GetProperties())
            {
                r += $"{prop.Name}={prop.GetValue(this, null)}|";
            }
            return r;
        }

        public bool IsEnabled = false;
        public virtual bool NeedsServerSync { get; set;} = false;

        public static IniData iniUpdated = null;

        /// <summary>
        /// Load configuration fron an InitData to a new object 
        /// </summary>
        public static T LoadIni(IniData data, string section)
        {
            var n = new T();
            Debug.Log($"Loading local config section {section}");
            // Load all the KeyCode of that section
            n.LoadKeycodes(data[section]);
            
            // If section empty or disabled we will get the default values already assigned
            if (data[section] == null || data[section]["enabled"] == null || !data[section].GetBool("enabled"))
            {
                Debug.Log(" Section not enabled");
                return n;
            }

            // Load the settings from local ini (except KeyCodes)
            n.LoadSettings(data[section]);

            return n;
        }


        /// <summary>
        /// Load configuration fron an InitData to the current configuration
        /// </summary>
        public void LoadIniFromRemote(IniData data, string section)
        {
            Debug.Log($"Loading remote config section {section}");
            
            // If section empty or disabled from remote
            if (data[section] == null || data[section]["enabled"] == null || !data[section].GetBool("enabled"))
            {
                Debug.Log(" Section not enabled");
                // We disable that config (as already existing and potentially enabled
                IsEnabled = false;
                // We enforce all default settings (except KeyCodes)
                LoadDefaultSettings();
            } 
            else
            {
                // Load the settings from remote ini (except KeyCodes)
                LoadSettings(data[section]);
            }
        }

        /// <summary>
        /// Helper to enforce a keyName first letter to be lowered
        /// i.e MyKeyName will return myKeyName
        /// </summary>
        private string SanatizeKeyName(string keyName)
        {
            if (keyName != string.Empty && char.IsUpper(keyName[0]))
            {
                return char.ToLower(keyName[0]) + keyName.Substring(1);
            }
            return keyName;
        }

        /// <summary>
        /// Set all default values of all current Configuration properties (except KeyCodes)
        /// </summary>
        public void LoadDefaultSettings()
        {
            Debug.Log(" Loading default settings");
            foreach (var prop in typeof(T).GetProperties())
            {
                // If property is a KeyCode we ignore it
                if (prop.PropertyType.Equals(typeof(KeyCode))) continue;

                var keyName = prop.Name;

                // If property is either NeedsServerSync or IsEnabled we ignore them
                if (new[] { "NeedsServerSync", "IsEnabled" }.Contains(keyName)) continue;

                keyName = SanatizeKeyName(keyName);

                Debug.Log($"  Resetting Key {keyName}");
                // Assigned default value of the property
                prop.SetValue(this, prop.PropertyType.ToDefault(), null);
                Debug.Log($"   Key {keyName} set to default value");
            }
        }

        /// <summary>
        /// Set all values of all current Configuration properties (except KeyCodes) from given KeyDataCollection
        /// </summary>
        public void LoadSettings(KeyDataCollection data)
        {
            Debug.Log(" Loading settings");
            
            IsEnabled = true;

            foreach (var prop in typeof(T).GetProperties())
            {
                // If property is a KeyCode we ignore it
                if (prop.PropertyType.Equals(typeof(KeyCode))) continue;

                var keyName = prop.Name;

                // If property is either NeedsServerSync or IsEnabled we ignore them
                if (new[] { "NeedsServerSync", "IsEnabled" }.Contains(keyName)) continue;
                
                keyName = SanatizeKeyName(keyName);

                Debug.Log($"  Loading Key {keyName}");
                // If property is not defined into the given KeyDataCollection we make sure to use the default value
                if (!data.ContainsKey(keyName))
                {
                    prop.SetValue(this, prop.PropertyType.ToDefault(), null);
                    Debug.Log($"   Key {keyName} not defined, using default value");
                    continue;
                }

                // If property is type of float
                if (prop.PropertyType == typeof(float))
                {
                    // Will try to parse the float, if it fails it will use the default value
                    prop.SetValue(this, data.GetFloat(keyName, (float)prop.PropertyType.ToDefault()), null);
                    continue;
                }

                // If property is type of int
                if (prop.PropertyType == typeof(int))
                {
                    // Will try to parse the int, if it fails it will use the default value
                    prop.SetValue(this, data.GetInt(keyName, (int)prop.PropertyType.ToDefault()), null);
                    continue;
                }

                // If property is type of bool
                if (prop.PropertyType == typeof(bool))
                {
                    // Will try to parse the boolean, if it fails it will use the default value
                    prop.SetValue(this, data.GetBool(keyName), null);
                    continue;
                }

                // If property is type of string
                if (prop.PropertyType == typeof(string))
                {
                    // Will try to parse the string, if it fails it will use the default value
                    prop.SetValue(this, data.GetString(keyName, (string)prop.PropertyType.ToDefault()), null);
                    continue;
                }
                Debug.LogWarning($"   Could not load data of type {prop.PropertyType} for Key {keyName}");
            }
        }

        public void LoadKeycodes(KeyDataCollection data)
        {
            Debug.Log(" Loading Keycodes");

            foreach (var prop in typeof(T).GetProperties())
            {
                // If property is not a KeyCode we ignore it
                if (!prop.PropertyType.Equals(typeof(KeyCode))) continue;

                var keyName = prop.Name;
                keyName = SanatizeKeyName(keyName);

                Debug.Log($"  Loading KeyCode {keyName}");
                // If property KeyCode is not defined into the given KeyDataCollection we make sure to use the default value
                if (!data.ContainsKey(keyName))
                {
                    prop.SetValue(this, prop.PropertyType.ToDefault(), null);
                    Debug.Log($"   KeyCode {keyName} not defined, using default value");
                    continue;
                }

                // Will try to parse KeyCode, if it fails it will use the default value
                prop.SetValue(this, data.GetKeyCode(keyName, (KeyCode)prop.PropertyType.ToDefault()), null);
            }
        }

    }

    public abstract class ServerSyncConfig<T>: BaseConfig<T> where T : IConfig, new() {
        public override bool NeedsServerSync { get; set;} = true;
    }
}
