//elliotttate/Bepinex-Tools

using dnlib.DotNet;
using System.Reflection;
using FieldAttributes = dnlib.DotNet.FieldAttributes;
using MethodAttributes = dnlib.DotNet.MethodAttributes;
using TypeAttributes = dnlib.DotNet.TypeAttributes;

namespace SetupDevEnvironment.IO;

internal class AssemblyPublicizer
{
    public AssemblyPublicizer()
    {
        Logger.Start();
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
                Logger.Log(ex.Message);
            }
        }
    }

    static void ProcessAssembly(Assembly assembly)
    {
        string assemblyPath = assembly.Location;
        string filename = assembly.GetName().Name!;
        string outputPath = Path.Combine(Path.GetDirectoryName(assemblyPath)!, "publicized_assemblies");
        string outputSuffix = "_publicized";

        Directory.CreateDirectory(outputPath);

        string curHash = ComputeHash(assembly);

        string hashPath = Path.Combine(outputPath, $"{filename}{outputSuffix}.hash");
        string lastHash = string.Empty;

        if (File.Exists(hashPath))
            lastHash = File.ReadAllText(hashPath);

        if (curHash == lastHash)
            return;

        Logger.Log($"Making a public assembly from {filename}");
        RewriteAssembly(assemblyPath).Write($"{Path.Combine(outputPath, filename)}{outputSuffix}.dll");
        File.WriteAllText(hashPath, curHash);
    }

    static string ComputeHash(Assembly assembly)
    {
        return assembly.ManifestModule.ModuleVersionId.ToString();
    }

    static ModuleDef RewriteAssembly(string assemblyPath)
    {
        var assembly = ModuleDefMD.Load(assemblyPath);
        var types = assembly.GetTypes();
        Logger.Log($"{assembly.Name}: {types.Count()} types");
        foreach (var type in types)
        {
            MakeTypePublic(assembly, type);
            MakeMethodsPublic(assembly, type);
            MakeFieldsPublic(assembly, type);
        }

        return assembly;
    }

    private static void MakeFieldsPublic(ModuleDef assembly, TypeDef type)
    {
        var eventNames = type.Events
            .Select(ev => ev.Name.ToString()).ToList();

        var fields = type.Fields
            .Where(x => !eventNames.Contains(x.Name)).ToArray();

        Logger.Log($"{assembly.Name}\\{type.Name}: {fields.Length} fields");

        foreach (FieldDef field in fields)
        {
            field.Attributes &= ~FieldAttributes.FieldAccessMask;
            field.Attributes |= FieldAttributes.Public;
        }
    }

    private static void MakeTypePublic(ModuleDef assembly, TypeDef type)
    {
        Logger.Log($"{assembly.Name}\\{type.Name}");

        type.Attributes &= ~TypeAttributes.VisibilityMask;
        if (type.IsNested)
            type.Attributes |= TypeAttributes.NestedPublic;
        else
            type.Attributes |= TypeAttributes.Public;
    }

    private static void MakeMethodsPublic(ModuleDef assembly, TypeDef type)
    {
        Logger.Log($"{assembly.Name}\\{type.Name}: {type.Methods.Count} Methods");
        foreach (MethodDef method in type.Methods)
        {
            method.Attributes &= ~MethodAttributes.MemberAccessMask;
            method.Attributes |= MethodAttributes.Public;
        }
    }
}
