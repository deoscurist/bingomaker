using System.Threading.Tasks;
using Avalonia.Controls;
using BingoMaker.ViewModels.Windows;
using BingoMaker.Views.Windows;

namespace BingoMaker.Services;

public class DialogService : IDialogService
{
    private readonly Window _mainWindow;

    public DialogService(Window mainWindow)
    {
        _mainWindow = mainWindow;
    }

    public async Task<bool> ShowConfirmAsync(string title, string? message = null)
    {
        var dialog = new ConfirmDialog();
        dialog.DataContext = new ConfirmDialogViewModel(dialog, title, message);
        var result = await dialog.ShowDialog<bool?>(_mainWindow);
        return result == true;
    }
}