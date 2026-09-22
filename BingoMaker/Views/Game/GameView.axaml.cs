using Avalonia.Controls;
using Avalonia.Input;
using BingoMaker.ViewModels.Game;

namespace BingoMaker.Views.Game;

public partial class GameView : ContentPage
{
    public GameView()
    {
        InitializeComponent();
    }

    private void MarkItem_OnTapped(object? sender, TappedEventArgs e)
    {
        if (sender is Control control && DataContext is GameViewModel viewModel)
            viewModel.MarkItemCommand.Execute(control.DataContext);
    }
}