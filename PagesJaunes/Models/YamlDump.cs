using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace PagesJaunes.Models;

public class YamlDump
{
    public static void DumpAsYaml(object o)
    {
        ISerializer serializer = new SerializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();

        Console.WriteLine(serializer.Serialize(o));
    }
}