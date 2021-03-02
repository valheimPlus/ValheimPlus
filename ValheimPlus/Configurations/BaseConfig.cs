using IniParser.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

        public static T LoadIni(IniData data, string section, bool nested)
        {
            var n = new T();

            
            Debug.Log($"Loading config section {section}");
            if (!nested && (data[section] == null || data[section]["enabled"] == null || !data[section].GetBool("enabled")))
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

            IEnumerator<KeyData> iterator = data.GetEnumerator();
            Dictionary<string, IniData> nestedDictionary = new Dictionary<string, IniData>();
            while (iterator.MoveNext())
            {
                KeyData keyData = iterator.Current;
                Debug.Log($" >> Key {keyData.KeyName}: {keyData.Value}");
                string[] nestedData = keyData.KeyName.Split('.');
                if (nestedData.Length > 1)
                {
                    string nestedIndexKey = nestedData[0];
                    string nestednKey = nestedData[1];
                    if (!nestedDictionary.ContainsKey(nestedIndexKey))
                    {
                        nestedDictionary.Add(nestedIndexKey, new IniData());
                    }

                    IniData iniData = nestedDictionary[nestedIndexKey];
                    iniData[nestedIndexKey].AddKey(nestednKey, keyData.Value);
                }
            }

            foreach (var prop in typeof(T).GetProperties())
            {
                var keyName = prop.Name;
                if (new[] { "NeedsServerSync", "IsEnabled" }.Contains(keyName)) continue;
                // Set first char of keyName to lowercase
                if (keyName != string.Empty && char.IsUpper(keyName[0]))
                {
                    keyName = char.ToLower(keyName[0]) + keyName.Substring(1);
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

                if (prop.PropertyType == typeof(string))
                {
                    prop.SetValue(this, data.GetString(keyName, (string)existingValue), null);
                    continue;
                }

                MethodInfo method = prop.PropertyType.GetMethod("LoadIni", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
                if (method != null) //Is a nested configuration
                {
                    string nestedKey = keyName.Split('.')[0];
                    if (nestedDictionary.ContainsKey(nestedKey))
                    {
                        var result = method.Invoke(null, new object[] { nestedDictionary[nestedKey], nestedKey, true });
                        prop.SetValue(this, result, null);
                        continue;
                    }
                    prop.SetValue(this, Activator.CreateInstance(prop.PropertyType), null);
                    continue;
                }              

                if (!data.ContainsKey(keyName))
                {
                    Debug.Log($" Key {keyName} not defined, using default value");
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
