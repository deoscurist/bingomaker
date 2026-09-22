using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.VisualTree;
using BingoMaker.ViewModels.Editor;

namespace BingoMaker.Views.Editor;

public partial class BingoListView : ContentPage
{
    public BingoListView()
    {
        InitializeComponent();
    }

    private async void BingoForm_OnTapped(object? sender, TappedEventArgs e)
    {
        if (e.Source is Button || (e.Source as Visual)?.FindAncestorOfType<Button>() != null)
            return;
        
        if (sender is Control control && DataContext is BingoListViewModel viewModel)                                                                                                                                      
            viewModel.BingoFormCommand.Execute(control.DataContext);
    }
}