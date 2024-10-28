namespace WebTicTacToe.Models;

public class GameConstructor
{
    public string PlayerName1 { get; set; }
    public string PlayerName2 { get; set; }
    public Game.Mode SelectedMode { get; set; }
    
    public GameConstructor() {}

    public GameConstructor(string playerName1, string playerName2, Game.Mode mode)
    {
        PlayerName1 = playerName1;
        PlayerName2 = playerName2;
        SelectedMode = mode;
    }

    public override string ToString()
    {
        string buffer = string.Empty;
        
        buffer += $"PlayerName1: {PlayerName1}\n";
        buffer += $"PlayerName2: {PlayerName2}\n";
        buffer += $"SelectedMode: {SelectedMode}\n";
        
        return buffer;
    }
}