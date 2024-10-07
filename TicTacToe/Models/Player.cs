namespace TicTacToe.Models;

public class Player
{
    public string Name { get; }
    public char Symbol { get; }

    protected Player(string name, char symbol)   
    {
        Name = name;
        Symbol = symbol;
    }

    public static Player New(string name, char playerSymbol)
    {
        return new Player(name, playerSymbol);
    }
    
}