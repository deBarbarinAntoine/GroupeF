using System.Text.Json.Serialization;

namespace WebTicTacToe.Models;

public class Player
{
    
    public string Name { get; init; }
    public string Symbol { get; init; }
    public bool IsHuman { get; init; }
    public int LastMove { get; set; }

    /// <summary>
    /// Empty constructor for JSON deserialization
    /// </summary>
    [JsonConstructor]
    public Player() {}
    
    private Player(string name, string symbol, bool isHuman)
    {
        Name = name;
        Symbol = symbol;
        IsHuman = isHuman;
    }

    public static Player NewHuman(string symbol, string name)
    {
        if (name.Length is < 2 or > 20)
            throw new ArgumentException("Player name must be between 2 and 20 characters");
        
        return new Player(name, symbol, true);
    }
    
    public static Player NewIa(string symbol, string name = "IA Player")
    {
        if (name.Length is < 2 or > 20)
            throw new ArgumentException("Player name must be between 2 and 20 characters");
        
        return new Player(name, symbol, false);
    }

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

    public bool Play(Board board, int index)
    {
        if (!board.CheckCell(index))
            return false;
        
        board.SetCell(index, Symbol);
        LastMove = index;
        
        return true;
    }
    
    public bool IaPlay(Board board)
    {
        var evaluatedBoard = EvaluateBoard(board);
        var randomInt = ThrowDice();
        int move;
        if (randomInt > 200)
            move = evaluatedBoard[0].Item1;
        if (randomInt > 50)
            move = evaluatedBoard[randomInt % evaluatedBoard.Count].Item1;
        else move = evaluatedBoard[^1].Item1;
        
        board.SetCell(move, Symbol);
        LastMove = move;
        
        return true;
    }
    private int ThrowDice()
    {
        return Random.Shared.Next(1, 1_000);
    }

    public override string ToString()
    {
        string buffer = string.Empty;
        
        buffer += $"Name: {Name}\n";
        buffer += $"Symbol: {Symbol}\n";
        buffer += $"IsHuman: {IsHuman}\n";
        
        return buffer;
    }
}