using System.Text.Json;
using System.Text.Json.Serialization;

namespace WebTicTacToe.Models;

/// <summary>
/// Game Model used in the Session Context.
/// </summary>
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

    /// <summary>
    /// Simple constructor used to Serialize a Game object into a GameContext object.
    /// </summary>
    /// <param name="game">the Game to Serialize.</param>
    public GameContext(Game game)
    {
        // DEBUG
        // Console.WriteLine($"Board:\n{game.Board}");
        
        GameSerialized = JsonSerializer.Serialize(game);
        Empty = false;
    }

    /// <summary>
    /// Method used to Deserialize a GameContext object into a Game object.
    /// </summary>
    /// <returns>the Game object.</returns>
    public Game? GetGame()
    {
        return JsonSerializer.Deserialize<Game>(GameSerialized);
    }

    /// <summary>
    /// ToString implementation for the GameContext Model.
    /// </summary>
    /// <returns>the string representation of a GameContext object.</returns>
    public override string ToString()
    {
        string buffer = string.Empty;
        buffer += $"Empty: {Empty}\n";
        buffer += $"Game Serialized: {GameSerialized}\n";
        return buffer;
    }
}