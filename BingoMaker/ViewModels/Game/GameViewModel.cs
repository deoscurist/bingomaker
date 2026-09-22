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

        _gameStore.StartGame(bingo);
        
        if (_gameStore.CurrentGame?.Items != null)
            foreach (var item in _gameStore.CurrentGame.Items)
            {
                var itemVm = new GameItemViewModel(item);
                Items.Add(itemVm);
            }
    }
    
    private void OnGameStorePropertyChanged(object? sender, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == nameof(IGameStore.GameTime))
            GameTime = _gameStore.GameTime;
    }
    
    private void OnStoreItemMarkChanged(GameItem updatedModel)
    {
        var targetVm = Items.FirstOrDefault(vm => vm.Model == updatedModel);
        
        if (targetVm == null) return;
        
        if (targetVm.MarkedAt != updatedModel.MarkedAt)
        {
            targetVm.MarkedAt = updatedModel.MarkedAt;
        }
    }

    [RelayCommand]
    public void MarkItem(GameItemViewModel item)
    {
        item.ToggleMark();
        _gameStore.MarkItem(item.Model);
    }

    [RelayCommand]
    public async Task StopGame()
    {
        _gameStore.PropertyChanged -= OnGameStorePropertyChanged;
        _gameStore.ItemMarkChanged -= OnStoreItemMarkChanged;
        
        _gameStore.FinishGame();
        
        await _router.PushAsync<StartupViewModel>();
        _router.ClearHistory();
    }
}