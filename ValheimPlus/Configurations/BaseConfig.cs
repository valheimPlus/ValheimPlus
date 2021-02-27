using System;
using System.Collections.Generic;
using IniParser.Model;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace ValheimPlus.Configurations
{
    public interface IConfig
    {
        void LoadIniData(KeyDataCollection data);
        bool IsEnabled { get; set; }
    }

    public abstract class BaseConfig
    {
        public static readonly Dictionary<Type, List<PropertyInfo>> propertyCache = new Dictionary<Type, List<PropertyInfo>>();
    }

    public abstract class BaseConfig<T> : BaseConfig, IConfig where T : IConfig, new()
    {
        public bool IsEnabled
        {
            get => _isEnabled;
            set
            {
                if (value != _isEnabled)
                {
                    SectionStatusChangedEvent?.Invoke(this, new SectionStatusChangeEventArgs(value));
                }
                _isEnabled = value;
            }
        }


        public EventHandler<SectionStatusChangeEventArgs> SectionStatusChangedEvent;

        public static IniData iniUpdated = null;
        private bool _isEnabled = false;

        public static T LoadIni(IniData data, string section)
        {
            var n = new T();

            Debug.Log($"Loading config section {section}");
            if (data[section] != null)
            {
                n.LoadIniData(data[section]);
            }
            if (data[section] == null || data[section][nameof(IsEnabled)] == null || !data[section].GetBool(nameof(IsEnabled)))
            {
                n.IsEnabled = false;
                Debug.Log(" Section not enabled");
            }

            return n;
        }


        private static IEnumerable<PropertyInfo> GetProps<T>()
        {
            if (!propertyCache.ContainsKey(typeof(T)))
            {
                // If not cached already, cache it
                propertyCache.Add(typeof(T), typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy).ToList());
            }

            foreach (var property in propertyCache[typeof(T)])
            {
                yield return property;
            }
        }

        public void LoadIniData(KeyDataCollection data)
        {
            IsEnabled = true;

            foreach (var prop in GetProps<T>())
            {
                var keyName = prop.Name;

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

    public abstract class ServerSyncConfig<T> : BaseConfig<T>, ISyncableSection where T : IConfig, new()
    {

    }
}
