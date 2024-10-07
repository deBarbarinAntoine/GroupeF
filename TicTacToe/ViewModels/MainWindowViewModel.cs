using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using TicTacToe.Models;

namespace TicTacToe.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public string Greeting => "This is a Tic-Tac-Toe Game!";
    
    private Game _game;
    private int _currentPlayerIndex; // Track the current player (0 or 1)

    public Game Game {get => _game; set => SetProperty(ref _game, value); }

    public string CurrentPlayerName => _game.GetPlayer(_currentPlayerIndex).Name;

    public char CurrentPlayerSymbol => Game.GetPlayer(_currentPlayerIndex).Symbol;

    public ICommand MakeMoveCommand { get; }

    public MainWindowViewModel()
    {
        // Create a new game instance (e.g., Solo mode by default)
        _game = Game.NewGame(Game.Mode.Solo);
        _currentPlayerIndex = 0; // Start with Player 1

        MakeMoveCommand = new RelayCommand(MakeMove);
    }

    private void MakeMove()
    {
        // Implement logic to handle the move (update _game, switch players)
        // if (_game.MakeMove((int)parameter)) // Assuming parameter is the cell index
        // {
        //     _currentPlayerIndex = (_currentPlayerIndex + 1) % 2; // Switch players
        //     RaisePropertyChanged(nameof(CurrentPlayerName));
        //     RaisePropertyChanged(nameof(CurrentPlayerSymbol));
        // }
        // Check for win/loss/tie and update UI accordingly
    }
}