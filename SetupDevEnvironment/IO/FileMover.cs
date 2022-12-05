using dnlib.DotNet;

namespace SetupDevEnvironment.IO
{
    internal class FileCopier
    {
        public static void CopyFiles(string sourceFolder, string destinationFolder)
        {
            var sourceFiles = Directory.GetFiles(sourceFolder, "*.*", SearchOption.AllDirectories);
            CopyFiles(sourceFolder, sourceFiles, destinationFolder);
        }

        public static void CopyFiles(string sourceFolder, string[] sourceFiles, string destinationFolder)
        {
            Directory.CreateDirectory(destinationFolder);
            foreach (string file in sourceFiles)
            {
                var relativeFile = Path.GetRelativePath(sourceFolder, file);
                var destFile = Path.Combine(destinationFolder, relativeFile);
                
                // we may need new subfolders
                var folder = Path.GetDirectoryName(destFile);
                Directory.CreateDirectory(folder!);

                if (File.GetAttributes(file).HasFlag(System.IO.FileAttributes.Directory)) 
                { 
                    continue; 
                }

                File.Copy(file, destFile, true);
            }
        }
    }
}
