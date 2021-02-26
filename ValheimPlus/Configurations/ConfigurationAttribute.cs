using System;
using Steamworks;

namespace ValheimPlus.Configurations
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ConfigurationAttribute : Attribute
    {
        public string Comment { get; set; }
        public string SinceVersion { get; set; }

        public ConfigurationAttribute(string comment, string sinceVersion = "0.8.5")
        {
            Comment = comment;
            SinceVersion = sinceVersion;
        }
    }
}