using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;
using ValheimPlus.UI;

namespace ValheimPlus
{
    [HarmonyPatch(typeof(FejdStartup), "SetupGui")]
    class PatchUI
    {
        static void Postfix()
        {
            GameObject logo = GameObject.Find("LOGO");
            logo.GetComponent<Image>().sprite = VPlusMainMenu.VPlusLogoSprite;
        }
    }
}