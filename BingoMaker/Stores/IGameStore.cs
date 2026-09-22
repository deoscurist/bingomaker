using System;
using System.ComponentModel;
using System.Threading.Tasks;
using BingoMaker.Models;

namespace BingoMaker.Stores;

public interface IGameStore : INotifyPropertyChanged
{
    Bingo? CurrentBingo { get; }
    Game? CurrentGame { get; }
    TimeSpan GameTime { get; }
    
    event Action<GameItem> ItemMarkChanged;
    
    Task<bool> StartGame(Bingo bingo);
    Task<bool> MarkItem(GameItem item);
    Task<bool> FinishGame();
}