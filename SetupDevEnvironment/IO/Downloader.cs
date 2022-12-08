namespace SetupDevEnvironment.IO;

internal partial class Downloader
{

    public static async Task<string> Download(string url, string? fileName = null)
    {
        var tempFile = fileName ?? Path.GetTempFileName();

        using (var client = new HttpClient())
        {
            var data = await client.GetByteArrayAsync(url);
            await File.WriteAllBytesAsync(tempFile, data);
            return tempFile;
        }
    }
}
