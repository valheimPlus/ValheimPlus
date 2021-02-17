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


namespace ValheimPlus
{
    // COPYRIGHT 2021 KEVIN "nx#8830" J. // http://n-x.xyz
    // GITHUB REPOSITORY https://github.com/nxPublic/ValheimPlus
    

    [BepInPlugin("org.bepinex.plugins.valheim_plus", "Valheim Plus", "0.7")]
    class ValheimPlusPlugin : BaseUnityPlugin
    {
        
        public static string version = "0.7";
        public static string newestVersion = "";
        public static Boolean isUpToDate = false;

        string ConfigPath = Path.GetDirectoryName(Paths.BepInExConfigPath) + "\\valheim_plus.cfg";

        // DO NOT REMOVE MY CREDITS
        public static string Author = "Kevin 'nx' J.";
        public static string Website = "http://n-x.xyz";
        public static string Discord = "nx#8830";
        public static string Repository = "https://github.com/nxPublic/ValheimPlus";
        public static string ApiRepository = "https://api.github.com/repos/nxPublic/valheimPlus/tags";

        // Add your credits here in case you modify the code or make additions, feel free to add as many as you like
        String ModifiedBy = "YourName";

        public static Boolean isDebug = false;

        // Awake is called once when both the game and the plug-in are loaded
        void Awake()
        {



            Logger.LogInfo("Trying to load the configuration file");
            if (File.Exists(ConfigPath))
            {
                Logger.LogInfo("Configuration file found, loading configuration.");
                if (ValheimPlus.Settings.LoadSettings() != true)
                {
                    Logger.LogError("Error while loading configuration file.");
                }
                else
                {

                    Logger.LogInfo("Configuration file loaded succesfully.");

                    var harmony = new Harmony("mod.valheim_plus");
                    harmony.PatchAll();

                    isUpToDate = !Settings.isNewVersionAvailable();
                    if (!isUpToDate)
                    {
                        Logger.LogError("There is a newer version available of ValheimPlus.");
                        Logger.LogWarning("Please visit " + ValheimPlusPlugin.Repository + ".");
                    }
                    else
                    {
                        Logger.LogInfo("ValheimPlus [" + version + "] is up to date.");
                    }

                }
            }
            else
            {
                Logger.LogError("Error: File not found. Plugin not loaded.");
            }
        }


        
    }
}
