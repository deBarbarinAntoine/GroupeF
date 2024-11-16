namespace WebTicTacToe.Models;

/// <summary>
/// TicTacToe Game Model for the /Game/Index view (creation form).
/// </summary>
public class GameConstructor
{
    public string PlayerName1 { get; set; }
    public string PlayerName2 { get; set; }
    public Game.Mode SelectedMode { get; set; }

    /// <summary>
    /// Empty constructor for JSON Deserialization.
    /// </summary>
    public GameConstructor() {}

    /// <summary>
    /// Basic GameConstructor constructor.
    /// </summary>
    /// <param name="playerName1">the first Player's name.</param>
    /// <param name="playerName2">the second Player's name.</param>
    /// <param name="mode">the Game Mode.</param>
    public GameConstructor(string playerName1, string playerName2, Game.Mode mode)
    {
        PlayerName1 = playerName1;
        PlayerName2 = playerName2;
        SelectedMode = mode;
    }

    /// <summary>
    /// ToString implementation for the GameConstructor Model.
    /// </summary>
    /// <returns>the string representation of a GameConstructor object.</returns>
    public override string ToString()
    {
        string buffer = string.Empty;
        
        buffer += $"PlayerName1: {PlayerName1}\n";
        buffer += $"PlayerName2: {PlayerName2}\n";
        buffer += $"SelectedMode: {SelectedMode}\n";
        
        return buffer;
    }
}