using System.Text.Json;

namespace WebTicTacToe.Models;

/// <summary>
/// Model providing a static method to dump any object to the Console in JSON format.
/// Used for debugging purposes.
/// </summary>
public class JsonDump
{
    /// <summary>
    /// Dumps any object to the Console in JSON format.
    /// </summary>
    /// <param name="o">the object to dump in JSON.</param>
    public static void Dump(object o)
    {
        var jsonSerialized = JsonSerializer.Serialize(o);
        Console.WriteLine(jsonSerialized);
    }
}