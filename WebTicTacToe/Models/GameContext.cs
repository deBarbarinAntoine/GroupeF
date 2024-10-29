using System.Text.Json;
using System.Text.Json.Serialization;

namespace WebTicTacToe.Models;

public class GameContext
{
    public string GameSerialized { get; init; }

    public bool Empty { get; init; }

    /// <summary>
    /// Empty constructor for JSON deserialization
    /// and setting empty HttpContext.Session environment.
    /// </summary>
    [JsonConstructor]
    public GameContext()
    {
        GameSerialized = string.Empty;
        Empty = true;
    }
    
    public GameContext(Game game)
    {
        // DEBUG
        Console.WriteLine($"Board:\n{game.Board}");
        
        GameSerialized = JsonSerializer.Serialize(game);
        Empty = false;
    }

    public Game? GetGame()
    {
        return JsonSerializer.Deserialize<Game>(GameSerialized);
    }

    public override string ToString()
    {
        string buffer = string.Empty;
        buffer += $"Empty: {Empty}\n";
        buffer += $"Game Serialized: {GameSerialized}\n";
        return buffer;
    }
}