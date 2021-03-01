using System;
using System.Net;
using System.Text;
using UnityEngine;

// Todo, better error handling

namespace ValheimPlus
{
    class Settings
    {
        public static bool isNewVersionAvailable ()
        {
            WebClient client = new WebClient();
            client.Headers.Add("User-Agent: V+ Server");
            string reply;
            try
            {
                reply = client.DownloadString(ValheimPlusPlugin.ApiRepository);
                ValheimPlusPlugin.newestVersion = reply.Split(new[] { "," }, StringSplitOptions.None)[0].Trim().Replace("\"", "").Replace("[{name:", "");
            }
            catch
            {
                Debug.Log("The newest version could not be determined.");
                ValheimPlusPlugin.newestVersion = "Unknown";
            }

            if (ValheimPlusPlugin.newestVersion != ValheimPlusPlugin.version)
            {
                return true;
            }

            return false;
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
