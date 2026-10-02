using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using BingoMaker.Models;
using BingoMaker.Routing;
using BingoMaker.Services;
using BingoMaker.Stores;
using BingoMaker.ViewModels.Game;
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

    private readonly ObservableCollection<BingoCardViewModel> _bingos;
    public ReadOnlyObservableCollection<BingoCardViewModel> Bingos { get; }

    public BingoListViewModel(
        INavigationRouter router, 
        IBingoStore bingoStore,
        IDialogService dialogService)
    {
        _router = router;
        _bingoStore = bingoStore;
        _dialogService = dialogService;
        
        _bingos = new ObservableCollection<BingoCardViewModel>();
        Bingos = new ReadOnlyObservableCollection<BingoCardViewModel>(_bingos);
        
        _bingoStore.BingoSaved += OnBingoSaved;
        _bingoStore.BingoDeleted += OnBingoDeleted;

        _ = LoadBingos();
    }

    [RelayCommand]
    public async Task LoadBingos()
    {
        try
        {
            await _bingoStore.LoadAsync();
            
            _bingos.Clear();
            
            foreach (var bingo in _bingoStore.Bingos)
                _bingos.Add(new BingoCardViewModel(bingo));

            IsLoaded = true;
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
                if (await _bingoStore.DeleteAsync(bingo))
                {
                    var foundBingo = _bingos.FirstOrDefault(b => b.Bingo.Id == bingo.Id);
                    if (foundBingo is not null) 
                        _bingos.Remove(foundBingo);
                }
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
    
    private void OnBingoSaved(Bingo bingo)
    {
        var card = _bingos.FirstOrDefault(
            item => item.Bingo.Id == bingo.Id);

        if (card is null)
            _bingos.Insert(0, new BingoCardViewModel(bingo));
        else
            card.Bingo = bingo;
    }

    private void OnBingoDeleted(Bingo bingo)
    {
        var card = _bingos.FirstOrDefault(
            item => item.Bingo.Id == bingo.Id);

        if (card is not null)
            _bingos.Remove(card);
    }
}