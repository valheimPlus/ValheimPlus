namespace SetupDevEnvironment.IO;

internal class LogEvent : EventArgs
{
    public string Message { get; private set; }

    public LogEvent(string message)
    {
        Message = message;
    }
}
