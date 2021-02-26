using System;

namespace ValheimPlusManagerWPF.SupportClasses
{
    public sealed class ValidationManager
    {
        public static bool CheckInstallationStatus(string installationPath)
        {
            // Checking if ValheimPlus is already installed for the client or server
            return System.IO.File.Exists(String.Format("{0}{1}", installationPath, "BepInEx/plugins/ValheimPlus.dll"));
        }

        private ValidationManager()
        {
        }
        private static ValidationManager instance = null;
        public static ValidationManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ValidationManager();
                }
                return instance;
            }
        }
    }
}
