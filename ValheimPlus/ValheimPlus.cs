using BepInEx;
using HarmonyLib;
using ValheimPlus.Configurations;
using ValheimPlus.UI;

namespace ValheimPlus
{
    // COPYRIGHT 2021 KEVIN "nx#8830" J. // http://n-x.xyz
    // GITHUB REPOSITORY https://github.com/valheimPlus/ValheimPlus
    

    [BepInPlugin("org.bepinex.plugins.valheim_plus", "Valheim Plus", version)]
    public class ValheimPlusPlugin : BaseUnityPlugin
    {
        public const string version = "0.9.4";
        public static string newestVersion = "";
        public static bool isUpToDate = false;

        // Project Repository Info
        public static string Repository = "https://github.com/valheimPlus/ValheimPlus";
        public static string ApiRepository = "https://api.github.com/repos/valheimPlus/valheimPlus/tags";

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

                Harmony harmony = new Harmony("mod.valheim_plus");
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

                //Logo
                VPlusMainMenu.Load();
            }
        }
    }
}
