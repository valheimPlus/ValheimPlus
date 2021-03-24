using System.IO;
using UnityEngine;
using UnityEngine.UI;
using ValheimPlus.Utility;

namespace ValheimPlus.UI
{
    public static class VPlusMainMenu
    {
        public static Sprite VPlusLogoSprite;
        public static Sprite VPlusBannerSprite;
        public static Sprite VPlusBannerHoverSprite;
        public static void Load()
        {
            //Load the logo from embedded asset
            Stream logoStream = EmbeddedAsset.LoadEmbeddedAsset("Assets.logo.png");
            Texture2D logoTexture = Helper.LoadPng(logoStream);
            VPlusLogoSprite = Sprite.Create(logoTexture, new Rect(0, 0, logoTexture.width, logoTexture.height), new Vector2(0.5f, 0.5f));
            logoStream.Dispose();

            //Load the banner from embedded asset
            Stream bannerStream = EmbeddedAsset.LoadEmbeddedAsset("Assets.ZapHosting.png");
            Texture2D bannerTexture = Helper.LoadPng(bannerStream);
            VPlusBannerSprite = Sprite.Create(bannerTexture, new Rect(0, 0, bannerTexture.width, bannerTexture.height), new Vector2(0.5f, 0.5f));
            bannerStream.Dispose();

            //Load the bannerfrom embedded asset
            Stream bannerHoverStream = EmbeddedAsset.LoadEmbeddedAsset("Assets.ZapHosting_hover.png");
            Texture2D bannerHoverTexture = Helper.LoadPng(bannerHoverStream);
            VPlusBannerHoverSprite = Sprite.Create(bannerHoverTexture, new Rect(0, 0, bannerHoverTexture.width, bannerHoverTexture.height), new Vector2(0.5f, 0.5f));
            bannerStream.Dispose();
        }
    }
}