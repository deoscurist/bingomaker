using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Avalonia.Threading;
using BingoMaker.Data.Repositories;
using BingoMaker.Models;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BingoMaker.Stores;

public partial class GameStore : ObservableObject, IGameStore
{
    private readonly IGameRepository _repository;

    [ObservableProperty]
    public partial Bingo? CurrentBingo { get; private set; }

    [ObservableProperty]
    public partial Game? CurrentGame { get; private set; }
    
    [ObservableProperty]
    public partial TimeSpan GameTime { get; private set; }
    
    public event Action<GameItem>? ItemMarkChanged;
    
    private readonly Stopwatch _stopwatch = new();
    private readonly DispatcherTimer _timer;

    public GameStore(IGameRepository repository)
    {
        _repository = repository;

        _timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
        
        _timer.Tick += (_, _) => GameTime = _stopwatch.Elapsed;
    }
    
    public async Task<bool> StartGame(Bingo bingo)
    {
        GameTime = TimeSpan.Zero;
        _stopwatch.Restart();
        _timer.Start();
        
        var game = new Game { BingoId = bingo.Id };

        foreach (var item in bingo.Items)
            game.Items.Add(new GameItem { BingoItem = item });

        _repository.Save(game);

        CurrentGame = game;
        CurrentBingo = bingo;
        
        return true;
    }

    public async Task<bool> MarkItem(GameItem item)
    {
        item.MarkedAt = item.MarkedAt == null ? DateTime.Now : null;
        
        _repository.Save(CurrentGame!);
        
        ItemMarkChanged?.Invoke(item);

        return true;
    }

    public async Task<bool> FinishGame()
    {
        if (CurrentGame == null)
            return false;

        _stopwatch.Stop();
        _timer.Stop();
        GameTime = _stopwatch.Elapsed;
        
        CurrentGame!.FinishedAt = DateTime.Now;
        
        _repository.Save(CurrentGame!);
        
        CurrentGame = null;

        return true;
    }
}