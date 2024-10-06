using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace TicTacToe.Views;

public partial class MainWindow : Window
{
    private List<Button> Buttons { get; set; }
    public MainWindow()
    {
        InitializeComponent();
        Buttons = TicTacToeGrid.Children.OfType<Button>().ToList();
    }
    public void PlayButtonClicked(object source, RoutedEventArgs args)
    {
        TitleTextBlock.Text = "Game Started!";
        Buttons.ForEach(button => button.Content = "");
    }

    private void Cell_Clicked(object? sender, RoutedEventArgs e)
    {
        if (sender == null) return;
        var button = (Button)sender;
        button.Content = ReferenceEquals(button.Content, "X") ? "" : "X";
    }
}