using System.IO.Compression;

namespace SetupDevEnvironment.IO
{
    internal static class Unzipper
    {
        /// <summary>
        /// returns all entries relative to the target folder
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="targetFolder"></param>
        /// <returns></returns>
        public static string[] Unzip(string filename, string? targetFolder = null) 
        {
            targetFolder = targetFolder ?? DirectoryHelper.CreateTempFolder();
            Directory.CreateDirectory(targetFolder);

            using (var zipFileStream = new FileStream(filename, FileMode.Open))
            using (var zipfile = new ZipArchive(zipFileStream))
            {
                zipfile.ExtractToDirectory(targetFolder, true);

                return zipfile.Entries
                    .Select(entry => Path.Combine(targetFolder, entry.FullName))
                    .ToArray();
            }
        }
    }
}
