using System;
using System.Collections.Generic;
using System.Text;
using IniParser;
using IniParser.Model;
using ValheimPlusManager.Models;

namespace ValheimPlusManager.SupportClasses
{
    public class ConfigManager
    {
        public static ValheimPlusConf ReadConfigFile()
        {
            ValheimPlusConf valheimPlusConfiguration = new ValheimPlusConf();

            var parser = new FileIniDataParser();
            IniData data = parser.ReadFile("C:/Users/msn/Desktop/ServerTest/BepInEx/config/valheim_plus.cfg");

            // Advanced building mode settings
            valheimPlusConfiguration.advancedBuildingModeEnabled = bool.Parse(data["AdvancedBuildingMode"]["enabled"]);
            valheimPlusConfiguration.enterAdvancedBuildingMode = data["AdvancedBuildingMode"]["enterAdvancedBuildingMode"];
            valheimPlusConfiguration.exitAdvancedBuildingMode = data["AdvancedBuildingMode"]["exitAdvancedBuildingMode"];

            // Advanced editing mode settings


            // Player settings
            valheimPlusConfiguration.playerSettingsEnabled = bool.Parse(data["Player"]["enabled"]);
            valheimPlusConfiguration.baseMaximumWeight = float.Parse(data["Player"]["baseMaximumWeight"]);
            valheimPlusConfiguration.baseMegingjordBuff = float.Parse(data["Player"]["baseMegingjordBuff"]);
            valheimPlusConfiguration.baseAutoPickUpRange = float.Parse(data["Player"]["baseAutoPickUpRange"]);
            valheimPlusConfiguration.disableCameraShake = bool.Parse(data["Player"]["disableCameraShake"]);
            valheimPlusConfiguration.baseUnarmedDamage = float.Parse(data["Player"]["baseUnarmedDamage"]);

            return valheimPlusConfiguration;
        }
    }
}
