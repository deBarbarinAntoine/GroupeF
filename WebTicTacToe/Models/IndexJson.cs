namespace WebTicTacToe.Models;

/// <summary>
/// Index Model for the POST /Game/Play route JSON payload.
/// </summary>
public class IndexJson
{
    public int Index { get; init; }

    /// <summary>
    /// Empty constructor for JSON deserialization.
    /// </summary>
    public IndexJson() {}

    /// <summary>
    /// Basic IndexJson constructor.
    /// </summary>
    /// <param name="index">the index value.</param>
    public IndexJson(int index) => Index = index;
}