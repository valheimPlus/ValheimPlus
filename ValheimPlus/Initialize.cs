using BepInEx;
using HarmonyLib;
using System;
using System.IO;
using ValheimPlus.Configurations;

namespace ValheimPlus
{
    // COPYRIGHT 2021 KEVIN "nx#8830" J. // http://n-x.xyz
    // GITHUB REPOSITORY https://github.com/nxPublic/ValheimPlus


    [BepInPlugin("org.bepinex.plugins.valheim_plus", "Valheim Plus", "0.6")]
    class ValheimPlusPlugin : BaseUnityPlugin
    {

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

            if (ConfigurationExtra.LoadSettings() != true)
            {
                Logger.LogError("Error while loading configuration file.");
            }
            else
            {

                Logger.LogInfo("Configuration file loaded succesfully.");

                var harmony = new Harmony("mod.valheim_plus");
                harmony.PatchAll();

                if (Settings.isNewVersionAvailable("0.6"))
                {
                    Logger.LogError("There is a newer version available of ValheimPlus.");
                    Logger.LogWarning("Please visit " + ValheimPlusPlugin.Repository + ".");
                }
                else
                {
                    Logger.LogInfo("ValheimPlus is up to date.");
                }

            }
        }
    }
}
