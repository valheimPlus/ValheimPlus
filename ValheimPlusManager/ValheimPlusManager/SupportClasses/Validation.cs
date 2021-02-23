using System;
using System.Collections.Generic;
using System.Text;

namespace ValheimPlusManager.SupportClasses
{
    public static class Validation
    {
        public static bool CheckInstallationStatus(string installationPath)
        {
            // Checking if ValheimPlus is already installed for the client or server
            return System.IO.File.Exists(String.Format("{0}{1}", installationPath, "BepInEx/plugins/ValheimPlus.dll"));
        }
    }
}
