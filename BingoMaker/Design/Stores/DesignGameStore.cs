using System;
using System.Threading.Tasks;
using BingoMaker.Models;
using BingoMaker.Stores;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BingoMaker.Design.Stores;

public class DesignGameStore : ObservableObject, IGameStore
{
    public Bingo? CurrentBingo { get; private set; }
    public Game? CurrentGame { get; private set; }
    public TimeSpan GameTime { get; private set; }

    public event Action<GameItem>? ItemMarkChanged;

    public Task<bool> StartGame(Bingo bingo)
    {
        CurrentBingo = bingo;
        GameTime = TimeSpan.FromMinutes(3) + TimeSpan.FromSeconds(25);

        var game = new Game
        {
            BingoId = bingo.Id,
            StartedAt = DateTime.Now - GameTime
        };

        for (var i = 0; i < bingo.Items.Count; i++)
        {
            game.Items.Add(new GameItem
            {
                BingoItem = bingo.Items[i],
                MarkedAt = i % 2 == 0
                    ? game.StartedAt + TimeSpan.FromSeconds(30)
                    : null
            });
        }

        CurrentGame = game;

        OnPropertyChanged(nameof(CurrentBingo));
        OnPropertyChanged(nameof(CurrentGame));
        OnPropertyChanged(nameof(GameTime));

        return Task.FromResult(true);
    }

    public Task<bool> MarkItem(GameItem item)
    {
        item.MarkedAt = item.MarkedAt == null ? DateTime.Now : null;
        ItemMarkChanged?.Invoke(item);

        return Task.FromResult(true);
    }

    public Task<bool> FinishGame()
    {
        if (CurrentGame != null)
            CurrentGame.FinishedAt = DateTime.Now;

        return Task.FromResult(true);
    }
}