using System.Text.Json.Serialization;

namespace WebTicTacToe.Models;

/// <summary>
/// The TicTacToe Board Model.
/// </summary>
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

    /// <summary>
    /// Static small factory.
    /// </summary>
    /// <returns>A new Board instance.</returns>
    public static Board New()
    {
        return new Board();
    }

    /// <summary>
    /// Checks if the cell is empty.
    /// </summary>
    /// <param name="index">index of the cell to check.</param>
    /// <returns>True if it's empty, False otherwise.</returns>
    public bool CheckCell(int index)
    {
        return Grid[index].Equals(string.Empty);
    }

    /// <summary>
    /// Gets the cell's value.
    /// </summary>
    /// <param name="index">index of the cell to get.</param>
    /// <returns>the cell's value.</returns>
    public string GetCell(int index)
    {
        return Grid[index];
    }

    /// <summary>
    /// Sets a cell with a value.
    /// </summary>
    /// <param name="index">index of the cell to set.</param>
    /// <param name="value">value to set in the cell.</param>
    public void SetCell(int index, string value)
    {
        // DEBUG
        Console.WriteLine($"index: {index}, value: {value}");
        
        Grid[index] = value;
    }

    /// <summary>
    /// Gets all alignments available for a particular cell.
    /// </summary>
    /// <param name="index">the index of the cell.</param>
    /// <returns>the list of alignments (each alignment is a list of indexes)</returns>
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

    /// <summary>
    /// Evaluates a cell according to the status of its alignments.
    /// </summary>
    /// <param name="index">index of the cell to evaluate (must be an empty cell for it to be useful).</param>
    /// <param name="symbol">symbol of the current player.</param>
    /// <returns>the evaluation (positive integer).</returns>
    public int EvaluateCell(int index, string symbol)
    {
        // Get the alignments of the cell
        var alignments = GetAlignments(index);

        // Initialize a list to store the evaluation of each alignment
        List<int> alignEvals = new (alignments.Count);

        // Evaluate each alignment
        foreach (var alignment in alignments)
        {
            int value = 0;
            foreach (var i in alignment)
            {
                // Give 1 point for each symbol placed by the current player
                if (Grid[i] == symbol)
                    ++value;
                // Give 3 points for each symbol placed by the opponent
                else if (Grid[i] != string.Empty)
                    value += 3;
            }
            // Add the alignment's evaluation to the list
            alignEvals.Add(value);
        }

        // Evaluate crucial options (any alignment with two equal symbols)
        if (alignEvals.Any(eval => eval == 2))
            return 70;
        if (alignEvals.Any(eval => eval == 6))
            return 50;

        // Sum of evaluations if there's no crucial move to perform
        for (var i = 0; i < alignEvals.Count; i++)
        {
            alignEvals[i] = CountAlign(alignEvals[i]);
        }

        return alignEvals.Sum();
    }

    // Give an evaluation to an alignment
    private int CountAlign(int align)
    {
        switch (align) {
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

    /// <summary>
    /// Checks if a cell is on a diagonal.
    /// </summary>
    /// <param name="index">index of the cell to check.</param>
    /// <returns>the diagonals in which the cell is placed.</returns>
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

    /// <summary>
    /// Gets the row of the cell.
    /// </summary>
    /// <param name="index">index of the cell.</param>
    /// <returns>the row number.</returns>
    private int GetRow(int index)
    {
        return index / 3;
    }

    /// <summary>
    /// Gets the column of the cell.
    /// </summary>
    /// <param name="index">index of the cell.</param>
    /// <returns>the column number.</returns>
    private int GetColumn(int index)
    {
        return index % 3;
    }

    /// <summary>
    /// Gets the index of a cell from its location in row and column.
    /// </summary>
    /// <param name="row">row of the cell.</param>
    /// <param name="col">column of the cell.</param>
    /// <returns>the index number of the cell.</returns>
    private int GetIndex(int row, int col)
    {
        return row * 3 + col;
    }

    /// <summary>
    /// Checks if the player won.
    /// </summary>
    /// <param name="index">index of the last move.</param>
    /// <param name="value">symbol of the last player who made a move.</param>
    /// <returns>True if the player won, False otherwise.</returns>
    public bool CheckWinner(int index, string value)
    {
        var alignments = GetAlignments(index);
        return alignments.Any(alignment => alignment.All(i => GetCell(i) == value));
    }

    /// <summary>
    /// Checks if the Board is full (Tie).
    /// </summary>
    /// <returns>True if the Board is full, False otherwise.</returns>
    public bool CheckTie()
    {
        return !Grid.Any(cell => cell.Equals(string.Empty));
    }

    /// <summary>
    /// ToString method to display the Board in the Console.
    /// </summary>
    /// <returns>The string representation of the Board object.</returns>
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