using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using BingoMaker.Models;
using BingoMaker.Routing;
using BingoMaker.Services;
using BingoMaker.Stores;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BingoMaker.ViewModels.Editor;

public partial class BingoListViewModel : ViewModelBase
{
    private readonly INavigationRouter _router; 
    private readonly IBingoStore _bingoStore;
    private readonly IDialogService _dialogService;
    
    [ObservableProperty]
    public partial bool IsLoaded { get; private set; }
    
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
        
        _bingoStore.PropertyChanged += (_, _) => IsLoaded = _bingoStore.IsLoaded;

        _ = LoadBingos();
    }

    [RelayCommand]
    public async Task LoadBingos()
    {
        try
        {
            await _bingoStore.LoadAsync();
        }
        catch (Exception e)
        {
            return;
        }
    }

    [RelayCommand]
    public async Task Delete(Bingo bingo)
    {
        try
        {
            if (await _dialogService.ShowConfirmAsync(
                    "Delete Bingo",
                    "Are you sure you want to delete this bingo?")
               )
                await _bingoStore.DeleteAsync(bingo);
        }
        catch (Exception e)
        {
            return;
        }
    }

    [RelayCommand]
    public Task BingoForm(Bingo? bingo) => bingo is null
        ? _router.PushAsync<BingoFormViewModel>()
        : _router.PushAsync<BingoFormViewModel>(bingo);
}