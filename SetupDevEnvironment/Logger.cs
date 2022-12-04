using System.ComponentModel;

namespace SetupDevEnvironment
{
    internal class Logger
    {
        public event ProgressChangedEventHandler? OnLogMessage;
        public static Logger? Instance { get; private set; }
        private static StreamWriter? _logFile = null;

        public static void Log(string msg, int progressPct = 0)
        {
            if (Instance?.OnLogMessage != null)
            {
                Instance.OnLogMessage(Instance, new ProgressChangedEventArgs(progressPct, msg));
            }
            if (_logFile != null)
            {
                _logFile.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " " + msg);
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

        public static void ToDisk(string path)
        {
            Start();
            _logFile = new StreamWriter(File.Create(path));
            _logFile.AutoFlush = true;
        }

        internal static void Stop()
        {
            if (_logFile is null) return;

            _logFile.Flush();
            _logFile.Close();
            _logFile.Dispose();
        }
    }
}
