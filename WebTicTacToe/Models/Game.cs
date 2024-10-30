using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WebTicTacToe.Models;

public class Game
{
    public enum Mode
    {
        [Display(Name = "Multiplayer")]
        Multiplayer,
        [Display(Name = "Single player")]
        SinglePlayer,
        [Display(Name = "Spectator")]
        Spectator,
    }

    public enum State
    {
        Playing,
        Tie,
        Win,
    }
    public Mode CurrentMode { get; init; }
    public Player Player1 { get; init; }
    public Player Player2 { get; init; }
    public Board Board { get; init; }
    public int Turn { get; set; }
    public State CurrentState { get; set; }
    
    public State GetState() => CurrentState;

    /// <summary>
    /// Empty constructor for JSON deserialization
    /// </summary>
    [JsonConstructor]
    public Game() {}
    private Game(Mode mode, Player player1, Player player2, Board board)
    {
        CurrentMode = mode;
        Player1 = player1;
        Player2 = player2;
        Board = board;
        Turn = 0;
        CurrentState = State.Playing;
    }

    public static Game New(Mode mode, string? playerName, string? playerName2)
    {
        
        if (mode == Mode.Spectator)
            return new Game(Mode.Spectator, Player.NewIa("X"), Player.NewIa("O", "IA Player 2"), Board.New());
        
        if (mode is Mode.SinglePlayer or Mode.Multiplayer && !CheckName(playerName))
            throw new ArgumentException($"'{nameof(playerName)}' cannot be null or must be between 2 and 20 characters long.", nameof(playerName));
            
        if (mode is Mode.Multiplayer && !CheckName(playerName2))
            throw new ArgumentException($"'{nameof(playerName2)}' cannot be null or must be between 2 and 20 characters long.", nameof(playerName2));

        return mode switch
        {
            Mode.Multiplayer => new Game(Mode.Multiplayer, Player.NewHuman("X", playerName),
                Player.NewHuman("O", playerName2!), Board.New()),
            Mode.SinglePlayer => new Game(Mode.SinglePlayer, Player.NewHuman("X", playerName), Player.NewIa("O"),
                Board.New()),
            _ => throw new ArgumentException($"'{nameof(mode)}' is not supported.", nameof(mode))
        };
    }

    public Player CurrentPlayer()
    {
        return Turn % 2 == 0 ? Player1 : Player2;
    }

    public Player PreviousPlayer()
    {
        return Turn % 2 == 0 ? Player2 : Player1;
    }
    
    public void NextTurn() => ++Turn;

    public State CheckEndGame(int index, Player currentPlayer)
    {
        if (Board.CheckWinner(index, currentPlayer.Symbol))
        {
            CurrentState = State.Win;
            return State.Win;
        }

        if (!Board.CheckTie())
            return State.Playing;
        
        CurrentState = State.Tie;
        return State.Tie;
    }

    private static bool CheckName(string? name)
    {
        return name?.Length is >= 2 and <= 20;
    }

    public bool IsPlaying()
    {
        return CurrentState == State.Playing;
    }

    public override string ToString()
    {
        string buffer = string.Empty;
        
        buffer += $"State: {CurrentState.ToString()}\n";
        buffer += $"Turn: {Turn}\n";
        buffer += $"Player1:\n{Player1}\n";
        buffer += $"Player2:\n{Player2}\n";
        buffer += $"Board:\n{Board}";
        
        return buffer;
    }
}