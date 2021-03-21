using System;
using UnityEngine;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.IO;

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

        
        /// <summary>
        /// Resize child EffectArea's collision that matches the specified type(s).
        /// </summary>
        public static void ResizeChildEffectArea(MonoBehaviour parent, EffectArea.Type includedTypes, float newRadius)
        {
            if (parent != null)
            {
                EffectArea effectArea = parent.GetComponentInChildren<EffectArea>();
                if (effectArea != null)
                {
                    if ((effectArea.m_type & includedTypes) != 0)
                    {
                        SphereCollider collision = effectArea.GetComponent<SphereCollider>();
                        if (collision != null)
                        {
                            collision.radius = newRadius;
                        }
                    }
                }
            }
        }


        // Clamp value between min and max
        public static int Clamp(int value, int min, int max)
        {
            return Math.Min(max, Math.Max(min, value));
        }

        // Clamp value between min and max
        public static float Clamp(float value, float min, float max)
        {
            return Math.Min(max, Math.Max(min, value));
        }
    }
}
