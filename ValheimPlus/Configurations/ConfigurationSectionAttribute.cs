using System;

namespace ValheimPlus.Configurations
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ConfigurationSectionAttribute : Attribute
    {
        public string Comment { get; set; }
        public string SinceVersion { get; set; }

        public ConfigurationSectionAttribute(string comment, string sinceVersion = "0.9.0")
        {
            Comment = comment;
            SinceVersion = sinceVersion;
        }
    }
}