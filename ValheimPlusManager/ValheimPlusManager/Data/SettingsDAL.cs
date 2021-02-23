using System.IO;
using System.Xml.Serialization;
using ValheimPlusManager.Models;

namespace ValheimPlusManager.Data
{
    public static class SettingsDAL
    {
        public static Settings GetSettings()
        {
            using (var fileStream = File.Open("Data/Settings.xml", FileMode.Open))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Settings));
                return (Settings)serializer.Deserialize(fileStream);
            }
        }
    }
}
