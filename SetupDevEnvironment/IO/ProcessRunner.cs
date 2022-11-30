using System.Diagnostics;

namespace SetupDevEnvironment.IO
{
    internal class ProcessRunner
    {
        internal static void Run(string path)
        { 
            var process = new Process();
            process.StartInfo = new ProcessStartInfo()
            {
                UseShellExecute = true,
                FileName = path,                
            };

            process.Start();
        }
    }
}
