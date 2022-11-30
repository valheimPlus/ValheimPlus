namespace SetupDevEnvironment.IO
{
    public static class DirectoryHelper
    {
        public static string CreateTempFolder()
        {
            var tempFolder = Path.GetTempPath();
            var folder = Path.Combine(tempFolder, Path.GetRandomFileName());
            Directory.CreateDirectory(folder);
            return folder;
        }
    }
}
