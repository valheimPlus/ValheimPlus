namespace SetupDevEnvironment.IO
{
    internal class FileMover
    {
        public static void CopyFiles(string sourceFolder, string destinationFolder) 
        {
            Directory.CreateDirectory(destinationFolder);

            var sourceFiles = 
                Directory.GetFiles(sourceFolder, "*.*", SearchOption.AllDirectories);

            foreach (string file in sourceFiles)
            {
                var relativeFile = file.Replace(sourceFolder, "");
                var destFile = Path.Combine(destinationFolder, relativeFile);
                
                // we may need new subfolders
                var folder = Path.GetDirectoryName(destFile);
                Directory.CreateDirectory(folder);

                File.Copy(file, destFile, true);
            }
        }
    }
}
