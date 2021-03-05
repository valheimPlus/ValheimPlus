using System;
using System.IO;
using System.Text;
using UnityEngine;

namespace ValheimPlus
{
    static class Helper
    {
		public static Character getPlayerCharacter(Player __instance)
		{
			return (Character)__instance;
		}

        public static float tFloat(this float value, int digits)
        {
            double mult = Math.Pow(10.0, digits);
            double result = Math.Truncate(mult * value) / mult;
            return (float)result;
        }
         
        public static float applyModifierValue(float targetValue, float value)
        {
            if (value == 50) value = 51; // Decimal issue
            if (value == -50) value = -51; // Decimal issue

            if (value <= -100)
                value = -100;

            float newValue = targetValue;

            if (value >= 0)
            {
                newValue = targetValue + ((targetValue / 100) * value);
            }
            else
            {
                newValue = targetValue - ((targetValue / 100) * (value * -1));
            }

            return newValue;
        }

        public static Texture2D LoadPng(Stream fileStream)
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

        public static string CreateMD5(string input)
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }

                return sb.ToString();
            }
        }
    }
}
