using BepInEx;
using HarmonyLib;
using System;
using System.IO;
using ValheimPlus.Configurations;

namespace ValheimPlus
{
    // COPYRIGHT 2021 KEVIN "nx#8830" J. // http://n-x.xyz
    // GITHUB REPOSITORY https://github.com/nxPublic/ValheimPlus
    

    [BepInPlugin("org.bepinex.plugins.valheim_plus", "Valheim Plus", "0.8.5")]
    class ValheimPlusPlugin : BaseUnityPlugin
    {
        
        public static string version = "0.8.5";
        public static string newestVersion = "";
        public static Boolean isUpToDate = false;

        string ConfigPath = Path.GetDirectoryName(Paths.BepInExConfigPath) + Path.DirectorySeparatorChar + "valheim_plus.cfg";

        // Project Repository Info
        public static string Repository = "https://github.com/nxPublic/ValheimPlus";
        public static string ApiRepository = "https://api.github.com/repos/nxPublic/valheimPlus/tags";

        // Awake is called once when both the game and the plug-in are loaded
        void Awake()
        {
            Logger.LogInfo("Trying to load the configuration file");


            if (ConfigurationExtra.LoadSettings() != true)
            {
                Logger.LogError("Error while loading configuration file.");
            }else{

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
    }
}
