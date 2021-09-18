using System;
using UnityEngine;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.IO;
using System.Reflection.Emit;
using HarmonyLib;

namespace ValheimPlus
{
    static class Helper
    {
		public static Character getPlayerCharacter(Player __instance)
		{
			return (Character)__instance;
		}

        /// <summary>
        /// A function to get the current player by network sender id, even in singleplayer
        /// </summary>
        public static Player getPlayerBySenderId(long id)
        {
            // A little more efficient than the Player.GetPlayer function but its miniscule.
            // This one also works in single player and requires no additonal work around.
            List<Player> allPlayers = Player.GetAllPlayers();
            foreach (Player player in allPlayers)
            {
                ZDOID zdoInfo = Helper.getPlayerCharacter(player).GetZDOID();
                if (zdoInfo != new ZDOID(0L, 0U))
                {
                    if (zdoInfo.m_userID == id)
                        return player;
                }
            }
            return null;
        }

        public static List<CodeInstruction> removeForcedCaseFunctionCalls(List<CodeInstruction> il)
        {
            for (int i = 0; i < il.Count; ++i)
            {
                if (il[i].operand != null)
                {
                    string op = il[i].operand.ToString();
                    if (op.Contains(nameof(string.ToUpper)) || op.Contains(nameof(string.ToLower)) || op.Contains(nameof(string.ToLowerInvariant)))
                    {
                        il[i] = new CodeInstruction(OpCodes.Nop);
                        il[i - 1] = new CodeInstruction(OpCodes.Nop);
                        il[i + 1] = new CodeInstruction(OpCodes.Nop);
                    }

                }
            }
            return il;
        }

        public static float tFloat(this float value, int digits)
        {
            double mult = Math.Pow(10.0, digits);
            double result = Math.Truncate(mult * value) / mult;
            return (float)result;
        }
         
        public static float applyModifierValue(float targetValue, float value)
        {
            
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
