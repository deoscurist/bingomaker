using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Avalonia.Threading;
using BingoMaker.Models;
using BingoMaker.Routing;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BingoMaker.ViewModels.Game;

public partial class GameViewModel : ViewModelBase
{
    private INavigationRouter _router;
    private readonly Bingo _bingo;
    private readonly Stopwatch _stopwatch = new();
    private readonly DispatcherTimer _timer;

    public int Size { get; }
    [ObservableProperty] public partial TimeSpan GameTime { get; set; } = TimeSpan.Zero;

    public ObservableCollection<GameItemViewModel> Items { get; } = [];
    
    public GameViewModel(INavigationRouter router, Bingo bingo)
    {
        _router = router;
        _bingo = bingo;
        
        Size = Math.Max(2, (int)Math.Ceiling(Math.Sqrt(_bingo.Items.Count)));

        foreach (var item in _bingo.Items)
        {
            Items.Add(new GameItemViewModel(
                new GameItem { BingoItem = item }));
        }
        
        _timer = new DispatcherTimer
        {
            Interval = TimeSpan.FromMilliseconds(100)
        };

        _timer.Tick += (_, _) => GameTime = _stopwatch.Elapsed;

        StartTimer();
    }
    
    public void StartTimer()
    {
        _stopwatch.Start();
        _timer.Start();
    }
    
    public void StopTimer()
    {
        _timer.Stop();
        _stopwatch.Stop();
        GameTime = _stopwatch.Elapsed;
    }

    [RelayCommand]
    public void MarkItem(GameItemViewModel item)
    {
        item.ToggleMark();
    }

    [RelayCommand]
    public async void StopGame()
    {
        StopTimer();
        await _router.PushAsync<StartupViewModel>();
        _router.ClearHistory();
    }
}