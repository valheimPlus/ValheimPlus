using System;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

// Todo, better error handling

namespace ValheimPlus
{
    class Settings
    {
        public static Boolean isNewVersionAvailable(string version)
        {

            WebClient client = new WebClient();
            client.Headers.Add("User-Agent: V+ Server");

            string reply = client.DownloadString(ValheimPlusPlugin.ApiRepository);

            string newestVersion = reply.Split(new[] { "," }, StringSplitOptions.None)[0].Trim().Replace("\"", "").Replace("[{name:", "");
            try
            {
                if (newestVersion != version)
                {
                    return true;
                }
            }
            catch (Exception e)
            {
                Debug.Log("The newest version could not be determined.");
                return false;
            }
            return false;
        }

        public class GithubTagInfo
        {
            public string name { get; set; }
            public DateTimeOffset Date { get; set; }
            public string zipball_url { get; set; }
            public string tarball_url { get; set; }
            public Dictionary<string, commitHashes> Hashes { get; set; }
            public string node_id { get; set; }
        }

        public class commitHashes
        {
            public int sha { get; set; }
            public int url { get; set; }
        }

    }
}
