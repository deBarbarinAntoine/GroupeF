using System.Text.Json.Serialization;

namespace WebTicTacToe.Models;

/// <summary>
/// TicTacToe Player Model.
/// </summary>
public class Player
{
    
    public string Name { get; init; }
    public string Symbol { get; init; }
    public bool IsHuman { get; init; }
    public int LastMove { get; set; }

    /// <summary>
    /// Empty constructor for JSON Deserialization.
    /// </summary>
    [JsonConstructor]
    public Player() {}

    /// <summary>
    /// Basic private Player constructor.
    /// </summary>
    /// <param name="name">the Player's name.</param>
    /// <param name="symbol">the Player's symbol.</param>
    /// <param name="isHuman">whether the player is Human or not.</param>
    private Player(string name, string symbol, bool isHuman)
    {
        Name = name;
        Symbol = symbol;
        IsHuman = isHuman;
    }

    /// <summary>
    /// Static small factory to create a Human Player.
    /// </summary>
    /// <param name="symbol">the Player's symbol.</param>
    /// <param name="name">the Player's name.</param>
    /// <returns>the new Human Player's instance.</returns>
    /// <exception cref="ArgumentException">if the Player's name is invalid.</exception>
    public static Player NewHuman(string symbol, string name)
    {
        if (name.Length is < 2 or > 20)
            throw new ArgumentException("Player name must be between 2 and 20 characters");
        
        return new Player(name, symbol, true);
    }

    /// <summary>
    /// Static small factory to create an AI Player.
    /// </summary>
    /// <param name="symbol">the Player's symbol.</param>
    /// <param name="name">the Player's name.</param>
    /// <returns>the new AI Player's instance.</returns>
    /// <exception cref="ArgumentException">if the Player's name is invalid.</exception>
    public static Player NewAi(string symbol, string name = "AI Player")
    {
        if (name.Length is < 2 or > 20)
            throw new ArgumentException("Player name must be between 2 and 20 characters");
        
        return new Player(name, symbol, false);
    }

    /// <summary>
    /// Evaluates all empty cells in the board and sort the results by score.
    /// </summary>
    /// <param name="board">the TicTacToe Game's Board.</param>
    /// <returns>the list of index/score pairs sorted by score.</returns>
    private List<Tuple<int, int>> EvaluateBoard(Board board)
    {
        var results = new List<Tuple<int, int>>();
        for (int i = 0; i < board.Count; i++)
        {
            if (board.CheckCell(i))
                results.Add(new Tuple<int, int>(i, board.EvaluateCell(i, Symbol)));
        }
        results.Sort((it1, it2) => it2.Item2 - it1.Item2);
        
        return results;
    }

    /// <summary>
    /// Play method for a Human Player.
    /// </summary>
    /// <param name="board">the TicTacToe Game's Board.</param>
    /// <param name="index">index of the cell to play in.</param>
    /// <returns>True if it worked, False otherwise.</returns>
    public bool Play(Board board, int index)
    {
        if (!board.CheckCell(index))
            return false;
        
        board.SetCell(index, Symbol);
        LastMove = index;
        
        return true;
    }

    /// <summary>
    /// Play method for an AI Player.
    /// </summary>
    /// <param name="board">the TicTacToe Game's Board.</param>
    /// <returns>True if it worked, False otherwise.</returns>
    public bool AiPlay(Board board)
    {
        var evaluatedBoard = EvaluateBoard(board);
        var randomInt = ThrowDice();

        int move = randomInt switch
        {
            > 200 => evaluatedBoard[0].Item1,
            > 50 => evaluatedBoard[randomInt % evaluatedBoard.Count].Item1,
            _ => evaluatedBoard[^1].Item1
        };

        return Play(board, move);
    }

    /// <summary>
    /// Method to generate a random integer between 1 and 1000.
    /// </summary>
    /// <returns>the random number generated.</returns>
    private int ThrowDice()
    {
        return Random.Shared.Next(1, 1_000);
    }

    /// <summary>
    /// ToString implementation for the Player Model.
    /// </summary>
    /// <returns>the string representation of a Player instance.</returns>
    public override string ToString()
    {
        string buffer = string.Empty;
        
        buffer += $"Name: {Name}\n";
        buffer += $"Symbol: {Symbol}\n";
        buffer += $"IsHuman: {IsHuman}\n";
        
        return buffer;
    }
}