using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using BingoMaker.Models;
using BingoMaker.Routing;
using BingoMaker.Stores;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BingoMaker.ViewModels.Game;

public partial class GameViewModel : ViewModelBase
{
    private INavigationRouter _router;
    private IGameStore _gameStore;
    
    public Bingo Bingo { get; }
    public int Size { get; }
    [ObservableProperty] public partial TimeSpan GameTime { get; set; }

    public ObservableCollection<GameItemViewModel> Items { get; } = [];
    
    public GameViewModel(INavigationRouter router, IGameStore gameStore, Bingo bingo)
    {
        _router = router;
        _gameStore = gameStore;
        Bingo = bingo;
        
        Size = Math.Max(2, (int)Math.Ceiling(Math.Sqrt(Bingo.Items.Count)));

        _gameStore.PropertyChanged += OnGameStorePropertyChanged;
        _gameStore.ItemMarkChanged += OnStoreItemMarkChanged;

        _ = StartGame();
    }

    private async Task StartGame()
    {
        try
        {
            await _gameStore.StartGame(Bingo);
            
            if (_gameStore.CurrentGame?.Items != null)
                foreach (var item in _gameStore.CurrentGame.Items)
                {
                    var itemVm = new GameItemViewModel(item);
                    Items.Add(itemVm);
                }
        }
        catch (Exception e)
        {
            _gameStore.PropertyChanged -= OnGameStorePropertyChanged;
            _gameStore.ItemMarkChanged -= OnStoreItemMarkChanged;
            
            await _router.PushAsync<GameSetupViewModel>();
            _router.ClearHistory(1);
        }
    }
    
    private void OnGameStorePropertyChanged(object? sender, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == nameof(IGameStore.GameTime))
            GameTime = _gameStore.GameTime;
    }
    
    private void OnStoreItemMarkChanged(GameItem markedItem)
    {
        var itemVm = Items.FirstOrDefault(vm => vm.Model == markedItem);
        
        if (itemVm == null) return;
        
        if (itemVm.MarkedAt != markedItem.MarkedAt)
            itemVm.ToggleMark();
    }

    [RelayCommand]
    public async Task MarkItem(GameItemViewModel item)
    {
        try
        {
            await _gameStore.MarkItem(item.Model);
        }
        catch (Exception e)
        {
            return;
        }
    }

    [RelayCommand]
    public async Task StopGame()
    {
        try
        {
            _gameStore.PropertyChanged -= OnGameStorePropertyChanged;
            _gameStore.ItemMarkChanged -= OnStoreItemMarkChanged;
            
            await _gameStore.FinishGame();
            
            await _router.PushAsync<StartupViewModel>();
            _router.ClearHistory();
        }
        catch (Exception e)
        {
            _gameStore.PropertyChanged += OnGameStorePropertyChanged;
            _gameStore.ItemMarkChanged += OnStoreItemMarkChanged;
            
            return;
        }
    }
}