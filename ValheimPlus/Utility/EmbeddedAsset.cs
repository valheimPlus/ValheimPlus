using System.IO;
using System.Reflection;

namespace ValheimPlus.Utility
{
    public static class EmbeddedAsset
    {
        public static Stream LoadEmbeddedAsset(string assetPath)
        {
            Assembly objAsm = Assembly.GetExecutingAssembly();
            string[] embeddedResources = objAsm.GetManifestResourceNames(); //This now contains the files that are embedded, we set 'embeddedFile' to the one we want to load

            if (objAsm.GetManifestResourceInfo(objAsm.GetName().Name + "." + assetPath) != null)
            {
                return objAsm.GetManifestResourceStream(objAsm.GetName().Name + "." + assetPath);
            }

            return null;
        }
    }
}