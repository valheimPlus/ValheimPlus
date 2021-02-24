using RestSharp;
using RestSharp.Serialization.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ValheimPlusManager.Models;

namespace ValheimPlusManager.SupportClasses
{
    public class UpdateManager
    {
        public static ValheimPlusUpdate CheckForValheimPlusUpdates(string valheimPlusVersion)
        {
            ValheimPlusUpdate valheimPlusUpdate = new ValheimPlusUpdate();

            // Calling Github API to fetch versions of ValheimPlus
            var client = new RestClient("https://api.github.com/");
            client.UseSerializer(() => new JsonSerializer { DateFormat = "yyyy-MM-ddTHH:mm:ss.FFFFFFFZ" });
            var request = new RestRequest("repos/valheimPlus/ValheimPlus/releases", DataFormat.Json);
            var response = client.Get(request);

            // Picking out all current versions released
            var versions = client.Execute<List<dynamic>>(request).Data.Select(
              item => item["tag_name"]).ToList(); // list of names

            // Comparing latest release on ValheimPlus Github to currently installed locally
            var latestVersion = new Version(versions[0]);
            var currentVersion = new Version(valheimPlusVersion);
            var result = latestVersion.CompareTo(currentVersion);

            if (result > 0)
            {
                valheimPlusUpdate.NewVersion = true;
                valheimPlusUpdate.Version = versions[0];
                return valheimPlusUpdate;
            }
            else if (result < 0)
            {
                valheimPlusUpdate.NewVersion = false;
                valheimPlusUpdate.Version = versions[0];
                return valheimPlusUpdate;
            }
            else
            {
                valheimPlusUpdate.NewVersion = false;
                valheimPlusUpdate.Version = versions[0];
                return valheimPlusUpdate;
            }
        }
    }
}
