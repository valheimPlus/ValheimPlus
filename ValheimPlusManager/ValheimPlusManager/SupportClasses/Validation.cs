using System;

namespace ValheimPlusManager.SupportClasses
{
    public sealed class Validation
    {
        public static bool CheckInstallationStatus(string installationPath)
        {
            // Checking if ValheimPlus is already installed for the client or server
            return System.IO.File.Exists(String.Format("{0}{1}", installationPath, "BepInEx/plugins/ValheimPlus.dll"));
        }

        private Validation()
        {
        }
        private static Validation instance = null;
        public static Validation Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Validation();
                }
                return instance;
            }
        }
    }
}
