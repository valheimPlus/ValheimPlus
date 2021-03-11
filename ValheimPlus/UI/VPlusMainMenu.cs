using System.IO;
using UnityEngine;
using UnityEngine.UI;
using ValheimPlus.Utility;

namespace ValheimPlus.UI
{
    public static class VPlusMainMenu
    {
        public static Sprite VPlusLogoSprite;

        public static void Load()
        {
            //Load the logo from embedded asset
            Stream logoStream = EmbeddedAsset.LoadEmbeddedAsset("Assets.logo.png");
            Texture2D logoTexture = Helper.LoadPng(logoStream);
            VPlusLogoSprite = Sprite.Create(logoTexture, new Rect(0, 0, logoTexture.width, logoTexture.height), new Vector2(0.5f, 0.5f));
            logoStream.Dispose();
        }
    }
}