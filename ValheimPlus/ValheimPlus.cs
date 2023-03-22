﻿using BepInEx;
using HarmonyLib;
using System;
using System.IO;
using System.Net;
using ValheimPlus.Configurations;
using ValheimPlus.RPC;
using ValheimPlus.UI;

namespace ValheimPlus
{
    // COPYRIGHT 2021 KEVIN "nx#8830" J. // http://n-x.xyz
    // GITHUB REPOSITORY https://github.com/valheimPlus/ValheimPlus


    [BepInPlugin("org.bepinex.plugins.valheim_plus", "Valheim Plus", version)]
    public class ValheimPlusPlugin : BaseUnityPlugin
    {
        public const string version = "0.9.9.12";
        public static string newestVersion = "";
        public static bool isUpToDate = false;

        public static System.Timers.Timer mapSyncSaveTimer =
            new System.Timers.Timer(TimeSpan.FromMinutes(5).TotalMilliseconds);

        public static readonly string VPlusDataDirectoryPath =
            Paths.BepInExRootPath + Path.DirectorySeparatorChar + "vplus-data";

        public static Harmony harmony = new Harmony("mod.valheim_plus");

        // Project Repository Info
        public static string Repository = "https://github.com/grantapher/ValheimPlus";
        public static string ApiRepository = "https://api.github.com/repos/grantapher/valheimPlus/tags";

        // Website INI for auto update
        public static string iniFile = "https://raw.githubusercontent.com/grantapher/ValheimPlus/" + version + "/valheim_plus.cfg";

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


                harmony.PatchAll();

                isUpToDate = !IsNewVersionAvailable();
                if (!isUpToDate)
                {
                    Logger.LogError("There is a newer version available of ValheimPlus.");
                    Logger.LogWarning("Please visit " + ValheimPlusPlugin.Repository + ".");
                }
                else
                {
                    Logger.LogInfo("ValheimPlus [" + version + "] is up to date.");
                }

                //Create VPlus dir if it does not exist.
                if (!Directory.Exists(VPlusDataDirectoryPath)) Directory.CreateDirectory(VPlusDataDirectoryPath);

                //Logo
                //if (Configuration.Current.ValheimPlus.IsEnabled && Configuration.Current.ValheimPlus.mainMenuLogo)
                // No need to exclude with IF, this only loads the images, causes issues if this config setting is changed
                VPlusMainMenu.Load();

                VPlusSettings.Load();

                //Map Sync Save Timer
                if (ZNet.m_isServer && Configuration.Current.Map.IsEnabled && Configuration.Current.Map.shareMapProgression)
                {
                    mapSyncSaveTimer.AutoReset = true;
                    mapSyncSaveTimer.Elapsed += (sender, args) => VPlusMapSync.SaveMapDataToDisk();
                }
            }
        }

        public static string getCurrentWebIniFile()
        {
            WebClient client = new WebClient();
            client.Headers.Add("User-Agent: V+ Server");
            string reply = null;
            try
            {
                reply = client.DownloadString(iniFile);
            }
            catch (Exception e)
            {
                return null;
            }
            return reply;
        }

        public static bool IsNewVersionAvailable()
        {
            WebClient client = new WebClient();

            client.Headers.Add("User-Agent: V+ Server");

            try
            {
                string reply = client.DownloadString(ApiRepository);
                newestVersion = reply.Split(new[] { "," }, StringSplitOptions.None)[0].Trim().Replace("\"", "").Replace("[{name:", "");
            }
            catch
            {
                ZLog.Log("The newest version could not be determined.");
                newestVersion = "Unknown";
            }

            //Parse versions for proper version check
            if (System.Version.TryParse(newestVersion, out System.Version newVersion))
            {
                if (System.Version.TryParse(version, out System.Version currentVersion))
                {
                    if (currentVersion < newVersion)
                    {
                        return true;
                    }
                }
            }
            else //Fallback version check if the version parsing fails
            {
                if (newestVersion != version)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
