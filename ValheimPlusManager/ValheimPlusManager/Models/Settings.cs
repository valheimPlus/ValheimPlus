using System.Xml.Serialization;

namespace ValheimPlusManager.Models
{
    [XmlRoot("Settings")]
    public class Settings
    {
        public string ClientInstallationPath { get; set; }
        public string ServerInstallationPath { get; set; }
        public string ClientPath { get; set; }
        public string ServerPath { get; set; }
        public string ValheimPlusVersion { get; set; }
    }
}
