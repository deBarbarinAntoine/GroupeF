using System.Text.Json.Serialization;

namespace WebTicTacToe.Models;

public class Board
{
    public List<string> Grid { get; set; } = new(9);
    private enum Diagonals { None, Upwards, Downwards, Both };

    public int Count { get; set; } = 9;

    
    /// <summary>
    /// Empty constructor for JSON deserialization
    /// </summary>
    [JsonConstructor]
    public Board()
    {
        for (var row = 0; row < Count; row++)
        {
            Grid.Add(string.Empty);
        }
    }

    public static Board New()
    {
        return new Board();
    }

    public bool CheckCell(int index)
    {
        return Grid[index].Equals(string.Empty);
    }

    public string GetCell(int index)
    {
        return Grid[index];
    }

    public void SetCell(int index, string value)
    {
        // DEBUG
        Console.WriteLine($"index: {index}, value: {value}");
        
        Grid[index] = value;
    }

    private List<List<int>> GetAlignments(int index)
    {
        var alignments = new List<List<int>>();

        // Get row and column based on index
        var row = GetRow(index);
        var col = GetColumn(index);

        // Add all cells in the same row and column
        var alignX = new List<int>(3);
        var alignY = new List<int>(3);
        for (int i = 0; i < 3; i++)
        {
            alignX.Add(GetIndex(row, i));
            alignY.Add(GetIndex(i, col));
        }
        alignments.Add(alignX);
        alignments.Add(alignY);

        // Check for diagonals
        List<int>? alignDiagonal;
        switch (IsOnDiagonal(index))
        {
            case Diagonals.None:
                break;
            
            case Diagonals.Upwards:
                alignDiagonal = new List<int>(3);
                for (int i = 0; i < 3; i++)
                {
                    alignDiagonal.Add(GetIndex(i, 2 - i));
                }
                alignments.Add(alignDiagonal);
                break;
            
            case Diagonals.Downwards:
                alignDiagonal = new List<int>(3);
                for (int i = 0; i < 3; i++)
                {
                    alignDiagonal.Add(GetIndex(i, i));
                }
                alignments.Add(alignDiagonal);
                break;
            
            case Diagonals.Both:
                var alignDiagonal1 = new List<int>(3);
                var alignDiagonal2 = new List<int>(3);
                for (int i = 0; i < 3; i++)
                {
                    alignDiagonal1.Add(GetIndex(i, i));
                    alignDiagonal2.Add(GetIndex(i, 2 - i));
                }
                alignments.Add(alignDiagonal1);
                alignments.Add(alignDiagonal2);
                break;
        }

        return alignments;
    }

    public int EvaluateCell(int index, string symbol)
    {
        var alignments = GetAlignments(index);

        List<int> alignEvals = new (alignments.Count);

        foreach (var alignment in alignments)
        {
            int value = 0;
            foreach (var i in alignment)
            {
                if (Grid[i] == symbol)
                    ++value;
                else if (Grid[i] != string.Empty)
                    value += 3;
            }
            alignEvals.Add(value);
        }
        
        if (alignEvals.Any(eval => eval == 2))
            return 70;
        if (alignEvals.Any(eval => eval == 6))
            return 50;
        
        switch (alignEvals.Sum()) {
            case 1:
                return 5;
            case 0:
                return 3;
            case 3:
                return 1;
            default: // included 4
                return 0;
        }
    }
    
    private Diagonals IsOnDiagonal(int index)
    {
        Diagonals diagonals = Diagonals.None;
        
        var row = GetRow(index);
        var col = GetColumn(index);
        while (row - 1 >= 0 && col + 1 <= 2)
        {
            --row;
            ++col;
        }
        if (row == 0 && col == 2)
            diagonals = Diagonals.Upwards;
        
        row = GetRow(index);
        col = GetColumn(index);
        while (row -1 >= 0 && col -1 >= 0)
        {
            --row;
            --col;
        }
        if (row == 0 && col == 0)
            diagonals = diagonals == Diagonals.Upwards ? Diagonals.Both : Diagonals.Downwards;

        return diagonals;
    }

    private int GetRow(int index)
    {
        return index / 3;
    }

    private int GetColumn(int index)
    {
        return index % 3;
    }

    private int GetIndex(int row, int col)
    {
        return row * 3 + col;
    }

    public bool CheckWinner(int index, string value)
    {
        var alignments = GetAlignments(index);
        return alignments.Any(alignment => alignment.All(i => GetCell(i) == value));
    }

    public bool CheckTie()
    {
        return !Grid.Any(cell => cell.Equals(string.Empty));
    }

    public override string ToString()
    {
        string buffer = "-------------\n";
        
        for (int i = 0; i < Count; i++)
        {
            string cell = GetCell(i) == string.Empty ? " " : GetCell(i);
            buffer += $"| {cell} ";
            if (i % 3 == 2)
                buffer += "|\n-------------\n";
        }
        
        return buffer;
    }
}