using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BingoMaker.ViewModels.Windows;

public partial class ConfirmDialogViewModel : ObservableObject
{
    private readonly Window _dialog;
    
    public string Title { get; }
    public string? Message { get; }
    
    public bool? HasMessage => Message != null;
    
    public ConfirmDialogViewModel(Window dialog, string title, string? message = null)
    {
        _dialog = dialog;
        Title = title;
        Message = message;
    }
    
    [RelayCommand]
    private void Confirm() => _dialog.Close(true);

    [RelayCommand]
    private void Cancel() => _dialog.Close(false);
}