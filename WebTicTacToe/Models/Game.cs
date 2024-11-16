using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WebTicTacToe.Models;

/// <summary>
/// The TicTacToe Game Model.
/// </summary>
public class Game
{
    /// <summary>
    /// The Modes with which the TicTacToe Game can be played.
    /// </summary>
    public enum Mode
    {
        [Display(Name = "Multiplayer")]
        Multiplayer,
        [Display(Name = "Single player")]
        SinglePlayer,
        [Display(Name = "Spectator")]
        Spectator,
    }

    /// <summary>
    /// Possible States of the TicTacToe Game.
    /// </summary>
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

    /// <summary>
    /// Private Game constructor.
    /// </summary>
    /// <param name="mode">the Game Mode.</param>
    /// <param name="player1">the first Player.</param>
    /// <param name="player2">the second Player.</param>
    /// <param name="board">the Game Board.</param>
    private Game(Mode mode, Player player1, Player player2, Board board)
    {
        CurrentMode = mode;
        Player1 = player1;
        Player2 = player2;
        Board = board;
        Turn = 0;
        CurrentState = State.Playing;
    }

    /// <summary>
    /// Static small factory to create a new Game.
    /// </summary>
    /// <param name="mode">the Game Mode.</param>
    /// <param name="playerName">the first Player's name.</param>
    /// <param name="playerName2">the second Player's name.</param>
    /// <returns>the new Game.</returns>
    /// <exception cref="ArgumentException">if any of the arguments is invalid.</exception>
    public static Game New(Mode mode, string? playerName, string? playerName2)
    {
        
        if (mode == Mode.Spectator)
            return new Game(Mode.Spectator, Player.NewAi("X"), Player.NewAi("O", "AI Player 2"), Board.New());
        
        if (mode is Mode.SinglePlayer or Mode.Multiplayer && !CheckName(playerName))
            throw new ArgumentException($"'{nameof(playerName)}' cannot be null or must be between 2 and 20 characters long.", nameof(playerName));
            
        if (mode is Mode.Multiplayer && !CheckName(playerName2))
            throw new ArgumentException($"'{nameof(playerName2)}' cannot be null or must be between 2 and 20 characters long.", nameof(playerName2));

        return mode switch
        {
            Mode.Multiplayer => new Game(Mode.Multiplayer, Player.NewHuman("X", playerName),
                Player.NewHuman("O", playerName2!), Board.New()),
            Mode.SinglePlayer => new Game(Mode.SinglePlayer, Player.NewHuman("X", playerName), Player.NewAi("O"),
                Board.New()),
            _ => throw new ArgumentException($"'{nameof(mode)}' is not supported.", nameof(mode))
        };
    }

    /// <summary>
    /// Gets the current Player.
    /// </summary>
    /// <returns>the current Player.</returns>
    public Player CurrentPlayer()
    {
        return Turn % 2 == 0 ? Player1 : Player2;
    }

    /// <summary>
    /// Gets the previous Player.
    /// </summary>
    /// <returns>the previous Player.</returns>
    public Player PreviousPlayer()
    {
        return Turn % 2 == 0 ? Player2 : Player1;
    }

    /// <summary>
    /// Increments the turn's count.
    /// </summary>
    public void NextTurn() => ++Turn;

    /// <summary>
    /// Checks and sets the Game Status.
    /// </summary>
    /// <param name="index">index of the last move.</param>
    /// <returns>the State of the Game.</returns>
    public State CheckEndGame(int index)
    {
        if (Board.CheckWinner(index, CurrentPlayer().Symbol))
        {
            CurrentState = State.Win;
            return State.Win;
        }

        if (!Board.CheckTie())
            return State.Playing;
        
        CurrentState = State.Tie;
        return State.Tie;
    }

    /// <summary>
    /// Checks if the name is valid.
    /// </summary>
    /// <param name="name">the name to check.</param>
    /// <returns>True if it's valid, False otherwise.</returns>
    private static bool CheckName(string? name)
    {
        return name?.Length is >= 2 and <= 20;
    }

    /// <summary>
    /// Checks if the Game is ongoing.
    /// </summary>
    /// <returns>True if it's ongoing, False otherwise.</returns>
    public bool IsPlaying()
    {
        return CurrentState == State.Playing;
    }

    /// <summary>
    /// ToString implementation of the Game.
    /// </summary>
    /// <returns>the string representation of a Game object.</returns>
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