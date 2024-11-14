using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace YellowDirectory.Models;

/// <summary>
/// YamlDump is a simple model used to dump objects in the console in the YAML format.
/// (used for debugging).
/// </summary>
public class YamlDump
{
    /// <summary>
    /// Statically takes an object and dump it in the console in YAML format.
    /// </summary>
    /// <param name="o">the object to dump in the console.</param>
    public static void DumpAsYaml(object o)
    {
        ISerializer serializer = new SerializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();

        Console.WriteLine(serializer.Serialize(o));
    }
}