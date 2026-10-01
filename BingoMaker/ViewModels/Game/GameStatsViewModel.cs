using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using BingoMaker.Data.Repositories;
using BingoMaker.Models;
using BingoMaker.Routing;
using BingoMaker.Services;
using BingoMaker.Stores;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BingoMaker.ViewModels.Game;

public partial class GameStatsViewModel : ViewModelBase
{
    private readonly INavigationRouter _router;
    private readonly IDialogService _dialogService;
    private readonly IGameStatsStore _gameStatsStore;
    private readonly Bingo _bingo;
    
    public string Header => $"\"{_bingo.Title}\" stats:";
    
    [ObservableProperty]
    public partial bool IsLoaded { get; private set; }

    private readonly ObservableCollection<GameStatCardViewModel> _games;
    public ReadOnlyObservableCollection<GameStatCardViewModel> Games { get; }
    
    public GameStatsViewModel(
        INavigationRouter router,
        IDialogService dialogService,
        IGameStatsStore gameStatsStore, 
        Bingo bingo)
    {
        _router = router;
        _dialogService = dialogService;
        _gameStatsStore = gameStatsStore;
        _bingo = bingo;
        
        _gameStatsStore.CurrentBingo = bingo;

        _ = LoadGames();
        
        _games = new(_gameStatsStore.Games.Select(
            g => new GameStatCardViewModel(g)));
        Games = new ReadOnlyObservableCollection<GameStatCardViewModel>(_games);
        
        ((INotifyCollectionChanged)_gameStatsStore.Games).CollectionChanged
            += OnStoreGamesChanged;
    }
    
    private void OnStoreGamesChanged(
        object? sender,
        NotifyCollectionChangedEventArgs e)
    {
        if (e.Action == NotifyCollectionChangedAction.Remove
            && e.OldItems is not null)
        {
            foreach (Models.Game game in e.OldItems)
            {
                var card = _games.FirstOrDefault(
                    item => item.Game.Id == game.Id);

                if (card is not null)
                    _games.Remove(card);
            }
        }
    }
    
    [RelayCommand]
    public async Task LoadGames()
    {
        try
        {
            await _gameStatsStore.LoadAsync();
        }
        catch (Exception e)
        {
            return;
        }
    }
    
    [RelayCommand]
    public async Task Delete(Models.Game game)
    {
        try
        {
            if (await _dialogService.ShowConfirmAsync(
                    "Delete Game",
                    "Are you sure you want to delete this game?")
               )
                if (await _gameStatsStore.DeleteAsync(game))
                {
                    var foundGame = Games.FirstOrDefault(g => g.Game.Id == game.Id);
                    if (foundGame is not null) 
                        _games.Remove(foundGame);
                }
        }
        catch (Exception e)
        {
            return;
        }
    }

    [RelayCommand]
    public Task OpenGame(Models.Game game) => _router.PushAsync<GameShowViewModel>(game);
}
