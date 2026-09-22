using System.Collections.ObjectModel;
using System.Threading.Tasks;
using BingoMaker.Models;
using BingoMaker.Routing;
using BingoMaker.Services;
using BingoMaker.Stores;
using BingoMaker.ViewModels.Windows;
using BingoMaker.Views.Windows;
using CommunityToolkit.Mvvm.Input;

namespace BingoMaker.ViewModels.Editor;

public partial class BingoListViewModel : ViewModelBase
{
    private readonly INavigationRouter _router; 
    private readonly IBingoStore _bingoStore;
    private readonly IDialogService _dialogService;
    
    public ReadOnlyObservableCollection<Bingo> Bingos
        => _bingoStore.Bingos;

    public BingoListViewModel(
        INavigationRouter router, 
        IBingoStore bingoStore,
        IDialogService dialogService)
    {
        _router = router;
        _bingoStore = bingoStore;
        _dialogService = dialogService;
        _bingoStore.LoadAsync();
    }

    [RelayCommand]
    public async Task Delete(Bingo bingo)
    {
        if (await _dialogService.ShowConfirmAsync("Are you sure you want to delete this bingo?"))
            await _bingoStore.DeleteAsync(bingo);
    }

    [RelayCommand]
    public Task BingoForm(Bingo? bingo) => bingo is null
        ? _router.PushAsync<BingoFormViewModel>()
        : _router.PushAsync<BingoFormViewModel>(bingo);
}