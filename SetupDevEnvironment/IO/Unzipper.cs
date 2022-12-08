using SharpCompress.Common;
using SharpCompress.Readers;
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

            var archive = SharpCompress.Archives.ArchiveFactory.Open(filename);
            using (var reader = archive.ExtractAllEntries())
            {
                reader.WriteAllToDirectory(targetFolder, new ExtractionOptions {  ExtractFullPath = true, Overwrite = true, PreserveFileTime = true });
            }
            return archive.Entries.Select(e => Path.Combine(targetFolder, e.Key)).ToArray();
        }
        //        switch (Path.GetExtension(filename).ToLower())
        //        {
        //            case ".zip":
        //                return UnzipZip(filename, targetFolder);
        //            case ".7z":
        //                return Unzip7z(filename, targetFolder);
        //        }
        //}

        //private static string[] Unzip7z(string filename, string? targetFolder)
        //{
        //    using (var zipFileStream = new SharpCompress.Archives.SevenZip.(filename))
        //    using (var zipfile = new ZipArchive(zipFileStream))
        //    {
        //        zipfile.ExtractToDirectory(targetFolder, true);

        //        return zipfile.Entries
        //            .Select(entry => Path.Combine(targetFolder, entry.FullName))
        //            .ToArray();
        //    }
        //}

        //public static string[] UnzipZip(string filename, string? targetFolder = null)
        //{
        //    using (var zipFileStream = new FileStream(filename, FileMode.Open))
        //    using (var zipfile = new ZipArchive(zipFileStream))
        //    {
        //        zipfile.ExtractToDirectory(targetFolder, true);

        //        return zipfile.Entries
        //            .Select(entry => Path.Combine(targetFolder, entry.FullName))
        //            .ToArray();
        //    }
    }
}
