//elliotttate/Bepinex-Tools

using dnlib.DotNet;
using System.ComponentModel;
using System.Reflection;
using FieldAttributes = dnlib.DotNet.FieldAttributes;
using MethodAttributes = dnlib.DotNet.MethodAttributes;
using TypeAttributes = dnlib.DotNet.TypeAttributes;

namespace SetupDevEnvironment.IO;

internal class AssemblyPublicizer
{
    public const string
        MODNAME = "Bepinex_Publicizer",
        AUTHOR = "MrPurple6411",
        GUID = AUTHOR + "_" + MODNAME,
        VERSION = "1.0.0.0";

    public event ProgressChangedEventHandler? OnLogMessage;

    private void Log(string msg)
    {
        if (OnLogMessage != null)
        {
            OnLogMessage(this, new ProgressChangedEventArgs(0, msg));
        }
    }

    public void ProcessAssemblies(string valheimPlusInstallDir)
    {
        var managedFolder = Path.Combine(valheimPlusInstallDir, "valheim_Data\\Managed\\");
        var files = Directory.GetFiles(
            managedFolder, "assembly*.dll", SearchOption.TopDirectoryOnly);

        foreach (var file in files)
        {
            try
            {
                var assembly = Assembly.LoadFile(file);
                ProcessAssembly(assembly);
            } catch(Exception ex)
            {
                Log(ex.Message);
            }
        }
    }

    void ProcessAssembly(Assembly assembly)
    {
        string assemblyPath = assembly.Location;
        string filename = assembly.GetName().Name;
        string outputPath = Path.Combine(Path.GetDirectoryName(assemblyPath), "publicized_assemblies");
        string outputSuffix = "_publicized";

        Directory.CreateDirectory(outputPath);

        string curHash = ComputeHash(assembly);

        string hashPath = Path.Combine(outputPath, $"{filename}{outputSuffix}.hash");
        string lastHash = null;

        if (File.Exists(hashPath))
            lastHash = File.ReadAllText(hashPath);

        if (curHash == lastHash)
            return;

        Log($"Making a public assembly from {filename}");
        RewriteAssembly(assemblyPath).Write($"{Path.Combine(outputPath, filename)}{outputSuffix}.dll");
        File.WriteAllText(hashPath, curHash);
    }

    static string ComputeHash(Assembly assembly)
    {
        return assembly.ManifestModule.ModuleVersionId.ToString();
    }

    static ModuleDef RewriteAssembly(string assemblyPath)
    {
        ModuleDef assembly = ModuleDefMD.Load(assemblyPath);

        foreach (var type in assembly.GetTypes())
        {
            type.Attributes &= ~TypeAttributes.VisibilityMask;

            if (type.IsNested)
                type.Attributes |= TypeAttributes.NestedPublic;
            else
                type.Attributes |= TypeAttributes.Public;

            foreach (MethodDef method in type.Methods)
            {
                method.Attributes &= ~MethodAttributes.MemberAccessMask;
                method.Attributes |= MethodAttributes.Public;
            }

            List<string> eventNames = new List<string>();
            foreach (EventDef ev in type.Events)
                eventNames.Add(ev.Name);

            foreach (FieldDef field in type.Fields)
            {
                if (!eventNames.Contains(field.Name))
                {
                    field.Attributes &= ~FieldAttributes.FieldAccessMask;
                    field.Attributes |= FieldAttributes.Public;
                }
            }
        }
        return assembly;
    }
}
