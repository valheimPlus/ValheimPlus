namespace SetupDevEnvironment.IO
{
    internal class ResourceHelper
    {
        public static string GetResource(string name) 
        {
            var assembly = typeof(ResourceHelper).Assembly;
            var resourceName = assembly.GetManifestResourceNames().Single(x => x.Contains(name));

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
