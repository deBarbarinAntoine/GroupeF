namespace TicTacToe.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
#pragma warning disable CA1822 // Mark members as static
    public string Greeting => "This is a Tic-Tac-Toe Game!";
#pragma warning restore CA1822 // Mark members as static
}