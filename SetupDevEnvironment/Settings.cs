using SetupDevEnvironment.IO;

namespace SetupDevEnvironment
{
    internal static class Settings
    {
        public static string ValheimInstallDir
        {
            get
            {
                return
                    string.IsNullOrEmpty(Properties.Settings.Default.ValheimInstallDir)
                        ? Links.DefaultValheimInstallFolder
                        : Properties.Settings.Default.ValheimInstallDir;
            }
            set
            {
                Properties.Settings.Default.ValheimInstallDir = value;
            }
        }
        public static string ValheimPlusDevInstallDir
        {
            get
            {
                return
                    string.IsNullOrEmpty(Properties.Settings.Default.ValheimPlusDevInstallDir)
                        ? Links.DefaultValheimPlusDevInstallFolder
                        : Properties.Settings.Default.ValheimPlusDevInstallDir;
            }
            set
            {
                Properties.Settings.Default.ValheimPlusDevInstallDir = value;
            }
        }
    }
}