namespace SetupDevEnvironment.IO
{
    #nullable disable
    internal class ResourceHelper
    {
        public static string GetResource(string name) 
        {
            var assembly = typeof(ResourceHelper).Assembly;
            var resourceName = assembly.GetManifestResourceNames().Single(x => x.Contains(name));

            using (var stream = assembly.GetManifestResourceStream(resourceName))
            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
