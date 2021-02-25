// ---------------------------------------------------------------------------------------
// Solution: ValheimPlus
// Project: ValheimPlus
// Filename: Plant.cs
// 
// Last modified: 2021-02-25 17:34
// Created:       2021-02-25 17:34
// 
// Copyright: 2021 Walter Wissing & Co
// ---------------------------------------------------------------------------------------

using System;
using HarmonyLib;
using UnityEngine.UIElements;
using ValheimPlus.Configurations;

namespace ValheimPlus
{
    [HarmonyPatch(typeof(Plant), "UpdateHealth")]
    public static class PlantBiomeFix
    {

        public static void Postfix(ref Plant __instance)
        {
            if (Configuration.Current.Plant.IsEnabled)
            {
                if (Configuration.Current.Plant.noWrongBiome)
                {
                    if (__instance.m_status == Plant.Status.WrongBiome)
                    {
                        __instance.m_status = Plant.Status.Healthy;
                    }
                }

                if (Configuration.Current.Plant.growWithoutSun)
                {
                    if (__instance.m_status == Plant.Status.NoSun)
                    {
                        __instance.m_status = Plant.Status.Healthy;
                    }
                }
            }
        }
    }
}