using System.ComponentModel;

namespace SetupDevEnvironment
{
    internal class Logger
    {
        public event ProgressChangedEventHandler? OnLogMessage;
        public static Logger Instance { get; private set; }

        public static void Log(string msg, int progressPct = 0)
        {
            if (Instance.OnLogMessage != null)
            {
                Instance.OnLogMessage(Instance, new ProgressChangedEventArgs(progressPct, msg));
            }
        }
        
        private Logger() { }

        public static Logger Start()
        {
            if (Instance == null)
            {
                Instance = new Logger();
            }
            return Instance;
        }
    }
}
