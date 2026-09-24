using System;
using System.ComponentModel;
using System.Threading.Tasks;
using BingoMaker.Models;
using BingoMaker.Stores;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BingoMaker.Design.Stores;

public class DesignGameStore : ObservableObject, IGameStore
{
    public Bingo CurrentBingo { get; }
    public Game? CurrentGame { get; }
    public TimeSpan GameTime { get; }
    
    public event Action<GameItem>? ItemMarkChanged;
    public Task<bool> StartGame(Bingo bingo) => Task.FromResult(true);

    public Task<bool> MarkItem(GameItem item) => Task.FromResult(true);

    public Task<bool> FinishGame() => Task.FromResult(true);
}