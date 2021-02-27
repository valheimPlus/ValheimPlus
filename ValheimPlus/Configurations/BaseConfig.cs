using System;
using System.Collections.Generic;
using System.ComponentModel;
using IniParser.Model;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace ValheimPlus.Configurations
{
    public interface IConfig
    {
        void LoadIniData(KeyDataCollection data);
        bool IsEnabled { get; set; }
    }

    public abstract class BaseConfig : INotifyPropertyChanged
    {
        public static readonly Dictionary<Type, List<PropertyInfo>> propertyCache = new Dictionary<Type, List<PropertyInfo>>();
        public static readonly Dictionary<Type, Dictionary<string, object>> defaultValueCache = new Dictionary<Type, Dictionary<string, object>>();

        internal static IEnumerable<PropertyInfo> GetProps<T>()
        {
            return GetProps(typeof(T));
        }

        internal static IEnumerable<PropertyInfo> GetProps(Type t)
        {
            if (!propertyCache.ContainsKey(t))
            {
                propertyCache.Add(t,t.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy).ToList());
            }

            foreach (var property in propertyCache[t])
            {
                yield return property;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void SetValue<T>(string propertyName, object value)
        {
            PropertyInfo p = GetProps(typeof(T)).FirstOrDefault(x => x.Name == propertyName);
            if (p == null)
            {
                throw new ArgumentException($"Property {propertyName} does not exist in {typeof(T).Name}");
            }

            object oldValue = p.GetValue(this, null);
            p.SetValue(this, value, null);
            if (oldValue != value)
            {
                PropertyChanged?.Invoke(this,new PropertyChangedEventArgs(p.Name));
            }
        }

        public object GetDefault(Type sectionType, string propertyName)
        {
            return defaultValueCache[sectionType][propertyName];
        }

        public U GetDefault<U>(string propertyName)
        {
            return (U) defaultValueCache[GetType()][propertyName];
        }

        public void CacheDefaults()
        {
            if (defaultValueCache.ContainsKey(GetType()))
            {
                return;
            }

            Dictionary<string, object> temp = new Dictionary<string, object>();
            foreach (var p in GetProps(GetType()))
            {
                temp.Add(p.Name, p.GetValue(this, null));
            }

            defaultValueCache.Add(GetType(), temp);
        }
    }

    public abstract class BaseConfig<T> : BaseConfig, IConfig where T : IConfig, INotifyPropertyChanged, new()
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

        public void LoadIniData(KeyDataCollection data)
        {
            IsEnabled = true;

            foreach (var prop in GetProps<T>())
            {
                var keyName = prop.Name;

                if (!data.ContainsKey(keyName))
                {
                    Debug.Log($" Key {keyName} not defined, using default value");
                    continue;
                }

                Debug.Log($" Loading key {keyName}");
                var existingValue = prop.GetValue(this, null);

                if (prop.PropertyType == typeof(float))
                {
                    this.SetValue<T>(keyName, data.GetFloat(keyName, (float) existingValue));
                    continue;
                }

                if (prop.PropertyType == typeof(int))
                {
                    SetValue<T>(keyName, data.GetInt(keyName, (int) existingValue));
                    continue;
                }

                if (prop.PropertyType == typeof(bool))
                {
                    SetValue<T>(keyName, data.GetBool(keyName));
                    continue;
                }

                if (prop.PropertyType == typeof(KeyCode))
                {
                    SetValue<T>(keyName, data.GetKeyCode(keyName, (KeyCode)existingValue));
                    continue;
                }

                Debug.LogWarning($" Could not load data of type {prop.PropertyType} for key {keyName}");
            }
        }
    }

    public abstract class ServerSyncConfig<T> : BaseConfig<T>, ISyncableSection where T : class, IConfig, INotifyPropertyChanged, new()
    {

    }
}
