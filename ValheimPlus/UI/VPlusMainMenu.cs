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
            Texture2D logoTexture = LoadPng(logoStream);
            VPlusLogoSprite = Sprite.Create(logoTexture, new Rect(0, 0, logoTexture.width, logoTexture.height), new Vector2(0.5f, 0.5f));
            logoStream.Dispose();
        }

        private static Texture2D LoadPng(Stream fileStream)
        {
            Texture2D texture = null;

            if (fileStream != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    fileStream.CopyTo(memoryStream);

                    texture = new Texture2D(2, 2);
                    texture.LoadImage(memoryStream.ToArray()); //This will auto-resize the texture dimensions.
                }
            }

            return texture;
        }
    }
}