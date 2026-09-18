using Avalonia.Controls;
using Avalonia.Input;
using BingoMaker.ViewModels.Game;

namespace BingoMaker.Views.Game;

public partial class GameSetupView : ContentPage
{
    public GameSetupView()
    {
        InitializeComponent();
    }

    private void StartGame_OnTapped(object? sender, TappedEventArgs e)
    {
        if (sender is Control control && DataContext is GameSetupViewModel viewModel)
            viewModel.StartGameCommand.Execute(control.DataContext);
    }
}