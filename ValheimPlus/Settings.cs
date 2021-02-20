using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BepInEx;
using Unity;
using UnityEngine;
using System.IO;
using System.Reflection;
using System.Runtime;
using IniParser;
using IniParser.Model;
using HarmonyLib;
using System.Globalization;
using Steamworks;
using System.Net;

// Todo, better error handling

namespace ValheimPlus
{
    class Settings
    {

        public static IniData Config { get; set; }
        public static IniData Defaults { get; set; }

        static string ConfigPath = Path.GetDirectoryName(Paths.BepInExConfigPath) + Path.DirectorySeparatorChar + "valheim_plus.cfg";
        static string ConfigDefaultsPath = Path.GetDirectoryName(Paths.BepInExConfigPath) + Path.DirectorySeparatorChar + "valheim_plus_defaults.cfg";

        public static bool LoadSettings()
        {
            try
            {
                IniData userConfig;
                var parser = new FileIniDataParser();

                userConfig = parser.ReadFile(ConfigPath);
                Defaults = parser.ReadFile(ConfigDefaultsPath);

                Defaults.Merge(userConfig);

                Config = Defaults;
            }
            catch (Exception ex)
            {
                Debug.Log(ex.Message);
                return false;
            }
            return true;
        }

        public static bool isEnabled(string section)
        {
            return Boolean.Parse(Config[section]["enabled"]);
        }
        public static bool getBool(string section, string name)
        {
            return (Config[section][name].ToLower() == "true");
        }
        public static string getString(string section, string name)
        {
            return Config[section][name];
        }
        public static int getInt(string section, string name)
        {
            return int.Parse(Config[section][name]);
        }
        public static float getFloat(string section, string name)
        {
            
            return float.Parse(Config[section][name], CultureInfo.InvariantCulture.NumberFormat);
        }
        public static KeyCode getHotkey(string name)
        {
            KeyCode HotKey = KeyCode.None;
            try
            {
                HotKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), Config["Hotkeys"][name]);
            }
            catch(Exception e){
                HotKey = KeyCode.None;
            }
            
            return HotKey;
        }

        public static Boolean isNewVersionAvailable ()
        {
            WebClient client = new WebClient();
            client.Headers.Add("User-Agent: V+ Server");
            string reply;
            try
            {
                reply = client.DownloadString(ValheimPlusPlugin.ApiRepository);
                ValheimPlusPlugin.newestVersion = reply.Split(new[] { "," }, StringSplitOptions.None)[0].Trim().Replace("\"", "").Replace("[{name:", "");
            }
            catch(Exception e)
            {
                Debug.Log("The newest version could not be determined.");
                ValheimPlusPlugin.newestVersion = "Unknown";
            }

            if (ValheimPlusPlugin.newestVersion != ValheimPlusPlugin.version)
            {
                return true;
            }
            return false;
        }

        public static string getHash()
        {
            string toHash = "";

            string[] importantSections = { "Player", "UnarmedScaling", "Food", "Fermenter", "Furnace", "Kiln", "Items", "Building", "Beehive", "AdvancedBuildingMode", "Stamina" };

            foreach (SectionData Section in Config.Sections)
            {
                if (importantSections.Contains(Section.SectionName))
                {
                    foreach (KeyData lines in Config[Section.SectionName])
                    {
                        toHash += "[" + lines.KeyName +"]" + "[" + lines.Value + "]";
                    }
                }
            }

            return CreateMD5(toHash);
        }

        private static string CreateMD5(string input)
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }

       

    }
}
