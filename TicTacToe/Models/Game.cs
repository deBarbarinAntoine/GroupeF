using System;
using System.Collections.Generic;

namespace TicTacToe.Models;

public class Game
{
    private Player _player1 { get; set; }
    private Player _player2 { get; set; }
    private List<char> _cells { get; set; }
    public enum Mode
    {
        Solo,
        Multi,
        Spectator
    }

    public static Game NewGame(Mode mode)
    {
        Game game = new Game();
        game._cells = new List<char>(9);
        switch (mode)
        {
            case Mode.Multi:
                game._player1 = Player.New("Player1", 'X');
                game._player2 = Player.New("Player2", 'O');
                break;
            case Mode.Solo:
                game._player1 = Player.New("Player1", 'X');
                game._player2 = IAPlayer.New("Player2", 'O');
                break;
            case Mode.Spectator:
                game._player1 = IAPlayer.New("Player1", 'X');
                game._player2 = IAPlayer.New("Player2", 'O');
                break;
        }
        return game;
    }

    public Player GetPlayer(int i)
    {
        return i % 2 == 0 ? _player1 : _player2;
    }

    public bool MakeMove(int parameter)
    {
        throw new NotImplementedException();    
    }
}