using System;
using System.Collections.Generic;
using System.Threading;
using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using UnityEngine;


namespace CustomSeed
{
    [BepInPlugin("ValheimPlus.CustomSeed", "Custom Seed", "1.0.0")]

    public class CustomSeed : BaseUnityPlugin
    {
        public static ConfigEntry<string> ValheimPlusCustomSeed;
        private readonly Harmony harmony = new Harmony("ValheimPlus.CustomSeed");

        void Awake()
        {
            ValheimPlusCustomSeed = Config.Bind("CustomSeed",   // The section under which the option is shown
                                     "custom_seed",  // The key of the configuration option in the configuration file
                                     "0", // The default value
                                     "[ValheimPlus]Config file loaded for custom seed"); // Description of the option to show in the config file


            harmony.PatchAll();

        }
        [HarmonyPatch(typeof(World),MethodType.Constructor,new Type[] { typeof(string), typeof(string) })]
        class CustomSeed_Patch
        {
            static void Postfix(ref string ___m_seedName, ref int ___m_seed)
            {
                if (CustomSeed.ValheimPlusCustomSeed.Value == "0")
                {
                    Debug.Log($"[ValheimPlus]Custom Seed not set, kicking to random seed generation)");

                }
                else
                {
                    ___m_seedName = CustomSeed.ValheimPlusCustomSeed.Value;
                    Debug.Log($"[ValheimPlus]Dedicated Server Custom Seed:{___m_seedName}");
                    ___m_seed = HashHelper.GetStableHashCode(___m_seedName);
                    Debug.Log($"[ValheimPlus]Dedicated Server Custom Seed Hash:{___m_seed}");
                }

            }

        }
       
    }

}
