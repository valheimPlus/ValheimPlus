using IniParser.Model;
using System.Linq;
using System.Reflection;
using UnityEngine;
using ValheimPlus.RPC;
using YamlDotNet.Core.Tokens;
using static CharacterDrop;

namespace ValheimPlus.Configurations
{
    public interface IConfig
    {
        void LoadIniData(KeyDataCollection data, string section);
    }

    public abstract class BaseConfig<T> : IConfig where T : class, IConfig, new()
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

        public bool IsEnabled { get; private set; } = false;
        public virtual bool NeedsServerSync { get; set; } = false;

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
            var keyData = data[section];
            n.LoadIniData(keyData, section);

            return n;
        }

        public void LoadIniData(KeyDataCollection data, string section)
        {
            IsEnabled = true;
            var thisConfiguration = GetCurrentConfiguration(section);
            if (thisConfiguration == null)
            {
                Debug.Log("Configuration not set.");
                thisConfiguration = this as T;
                if (thisConfiguration == null) Debug.Log("Error on setting Configuration");
            }

            foreach (var property in typeof(T).GetProperties())
            {
                var keyName = property.Name;
                if (new[] { "NeedsServerSync", "IsEnabled" }.Contains(keyName)) continue;
                var currentValue = property.GetValue(thisConfiguration);
                // Set first char of keyName to lowercase
                if (keyName != string.Empty && char.IsUpper(keyName[0]))
                {
                    keyName = char.ToLower(keyName[0]) + keyName.Substring(1);
                }

                if (!data.ContainsKey(keyName))
                {
                    Debug.Log($" Key {keyName} not defined, using default value");
                    continue;
                }

                Debug.Log($" Loading key {keyName}");

                Debug.Log($"{property.Name} [{keyName}] = {currentValue} ({property.PropertyType})");
                if (property.PropertyType == typeof(float))
                {
                    var value = data.GetFloat(keyName, (float)currentValue);
                    Debug.Log($"{keyName} = {currentValue} => {value}");

                    property.SetValue(this, value, null);
                    continue;
                }

                if (property.PropertyType == typeof(int))
                {
                    var value = data.GetInt(keyName, (int)currentValue);
                    Debug.Log($"{keyName} = {currentValue} => {value}");
                    property.SetValue(this, data.GetInt(keyName, (int)currentValue), null);
                    continue;
                }

                if (property.PropertyType == typeof(bool))
                {
                    var value = data.GetBool(keyName);
                    Debug.Log($"{keyName} = {currentValue} => {value}");
                    property.SetValue(this, value, null);
                    continue;
                }

                if (property.PropertyType == typeof(KeyCode))
                {
                    Debug.Log($"Setting Hotkey is {(ConfigurationExtra.ReadHotKeys ? "enabled" : "disabled")}");
                    var newValue = ConfigurationExtra.ReadHotKeys ? data.GetKeyCode(keyName, (KeyCode)currentValue) : (KeyCode)currentValue;
                    Debug.Log($"{keyName} = {currentValue} => {newValue}");
                    property.SetValue(this, newValue, null);
                    continue;
                }

                Debug.LogWarning($" Could not load data of type {property.PropertyType} for key {keyName}");
            }
        }

        private static object GetCurrentConfiguration(string section)
        {
            if (Configuration.Current == null) return null;
            Debug.Log($"Reading Config '{section}'");
            var properties = Configuration.Current.GetType().GetProperties();
            PropertyInfo property = properties.SingleOrDefault(p => p.Name.Equals(section, System.StringComparison.CurrentCultureIgnoreCase));
            if (property == null)
            {
                Debug.LogWarning($"Property '{section}' not found in Configuration");
                return null;
            }
            var thisConfiguration = property.GetValue(Configuration.Current) as T;
            return thisConfiguration;
        }
    }

    public abstract class ServerSyncConfig<T> : BaseConfig<T> where T : class, IConfig, new()
    {
        public override bool NeedsServerSync { get; set; } = true;
    }
}
