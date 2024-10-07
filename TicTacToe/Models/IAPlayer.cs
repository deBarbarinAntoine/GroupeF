namespace TicTacToe.Models;

public class IAPlayer : Player
{
    protected IAPlayer(string name, char symbol) : base(name, symbol) {}

    public static IAPlayer New(string name, char playerSymbol)
    {
        return new IAPlayer(name, playerSymbol);
    }
}