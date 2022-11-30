namespace SetupDevEnvironment.IO
{
    internal class FileCopier
    {
        public static void CopyFiles(string sourceFolder, string destinationFolder) 
        {
            Directory.CreateDirectory(destinationFolder);

            var sourceFiles = Directory.GetFiles(sourceFolder, "*.*", SearchOption.AllDirectories);

            foreach (string file in sourceFiles)
            {
                var relativeFile = Path.GetRelativePath(sourceFolder, file);//.Substring(sourceFolder.Length + 1);
                var destFile = Path.Combine(destinationFolder, relativeFile);
                
                // we may need new subfolders
                var folder = Path.GetDirectoryName(destFile);
                Directory.CreateDirectory(folder);

                File.Copy(file, destFile, true);
            }
        }
    }
}
