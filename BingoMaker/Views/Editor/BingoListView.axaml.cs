using Avalonia.Controls;
using Avalonia.Input;
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
        if (sender is Control control && DataContext is BingoListViewModel viewModel)                                                                                                                                      
            viewModel.BingoFormCommand.Execute(control.DataContext);
    }
}