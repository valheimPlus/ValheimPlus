using IniParser.Model;
using System.Linq;
using UnityEngine;

namespace ValheimPlus.Configurations
{
    public interface IConfig
    {
        void LoadIniData(KeyDataCollection data);
    }

    public abstract class BaseConfig<T> : IConfig where T : IConfig, new()
    {

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

            
            Debug.Log($"Loading config section {section}");
            if (data[section] == null || data[section]["enabled"] == null || !data[section].GetBool("enabled"))
            {
                Debug.Log(" Section not enabled");
                return n;
            }

            n.LoadIniData(data[section]);

            return n;
        }

        public void LoadIniData(KeyDataCollection data)
        {
            IsEnabled = true;

            foreach (var prop in typeof(T).GetProperties())
            {
                var keyName = prop.Name;
                if (new[] { "NeedsServerSync", "IsEnabled" }.Contains(keyName)) continue;
                // Set first char of keyName to lowercase
                if (keyName != string.Empty && char.IsUpper(keyName[0]))
                {
                    keyName = char.ToLower(keyName[0]) + keyName.Substring(1);
                }

                if (!data.ContainsKey(keyName)) {
                    Debug.Log($" Key {keyName} not defined, using default value");
                    continue;
                }

                Debug.Log($" Loading key {keyName}");
                var existingValue = prop.GetValue(this, null);

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

                if (prop.PropertyType == typeof(KeyCode))
                {
                    prop.SetValue(this, data.GetKeyCode(keyName, (KeyCode)existingValue), null);
                    continue;
                }

                Debug.LogWarning($" Could not load data of type {prop.PropertyType} for key {keyName}");
            }
        }

    }

    public abstract class ServerSyncConfig<T>: BaseConfig<T> where T : IConfig, new() {
        public override bool NeedsServerSync { get; set;} = true;
    }
}
