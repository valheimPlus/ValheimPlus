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

        public static T LoadIni(IniData data, string section)
        {
            var n = new T();
            Debug.Log($"Loading local config section {section}");
            n.LoadKeycodes(data[section]);
            if (data[section] == null || data[section]["enabled"] == null || !data[section].GetBool("enabled"))
            {
                Debug.Log(" Section not enabled");
                return n;
            }

            n.LoadSettings(data[section]);

            return n;
        }

        public void LoadIniFromRemote(IniData data, string section)
        {
            Debug.Log($"Loading remote config section {section}");
            if (data[section] == null || data[section]["enabled"] == null || !data[section].GetBool("enabled"))
            {
                Debug.Log(" Section not enabled");
                IsEnabled = false;
            } 
            else
            {
                LoadSettings(data[section]);
            }
        }

        private string SanatizeKeyName(string keyName)
        {
            if (keyName != string.Empty && char.IsUpper(keyName[0]))
            {
                return char.ToLower(keyName[0]) + keyName.Substring(1);
            }
            return keyName;
        }
        public void LoadDefaultSettings()
        {
            Debug.Log(" Loading default settings");
            foreach (var prop in typeof(T).GetProperties())
            {
                if (prop.PropertyType.Equals(typeof(KeyCode))) continue;

                var keyName = prop.Name;

                if (new[] { "NeedsServerSync", "IsEnabled" }.Contains(keyName)) continue;

                keyName = SanatizeKeyName(keyName);

                Debug.Log($"  Loading Key {keyName}");
                prop.SetValue(this, prop.PropertyType.ToDefault(), null);
                Debug.Log($"   Key {keyName} set to default value");
            }
            }

            public void LoadSettings(KeyDataCollection data)
        {
            Debug.Log(" Loading settings");
            
            IsEnabled = true;

            foreach (var prop in typeof(T).GetProperties())
            {
                if (prop.PropertyType.Equals(typeof(KeyCode))) continue;

                var keyName = prop.Name;

                if (new[] { "NeedsServerSync", "IsEnabled" }.Contains(keyName)) continue;
                
                keyName = SanatizeKeyName(keyName);

                var existingValue = prop.GetValue(this, null);
                Debug.Log($"  Loading Key {keyName}");
                if ((!data.ContainsKey(keyName) || existingValue == null) && !prop.PropertyType.Equals(typeof(KeyCode)))
                {
                    prop.SetValue(this, prop.PropertyType.ToDefault(), null);
                    Debug.Log($"   Key {keyName} not defined, using default value");
                    continue;
                }

                if (prop.PropertyType == typeof(float))
                {
                    prop.SetValue(this, data.GetFloat(keyName, (float)existingValue), null);
                    continue;
                }

                if (prop.PropertyType == typeof(int))
                {
                    prop.SetValue(this, data.GetInt(keyName, (int)existingValue), null);
                    continue;
                }

                if (prop.PropertyType == typeof(bool))
                {
                    prop.SetValue(this, data.GetBool(keyName), null);
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
                if (!prop.PropertyType.Equals(typeof(KeyCode))) continue;

                var keyName = prop.Name;
                keyName = SanatizeKeyName(keyName);


                var existingValue = prop.GetValue(this, null);
                Debug.Log($"  Loading KeyCode {keyName}");
                if (!data.ContainsKey(keyName) || existingValue == null)
                {
                    prop.SetValue(this, prop.PropertyType.ToDefault(), null);
                    Debug.Log($"   KeyCode {keyName} not defined, using default value");
                    continue;
                }

                if (prop.PropertyType == typeof(KeyCode))
                {
                    prop.SetValue(this, data.GetKeyCode(keyName, (KeyCode)existingValue), null);
                    continue;
                }

                Debug.LogWarning($"   Could not load KeyCode for key {keyName}");
            }
        }

    }

    public abstract class ServerSyncConfig<T>: BaseConfig<T> where T : IConfig, new() {
        public override bool NeedsServerSync { get; set;} = true;
    }
}
