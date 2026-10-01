using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using BingoMaker.Models;
using BingoMaker.Stores;

namespace BingoMaker.Design.Stores;

public class DesignGameStats : IGameStatsStore
{
    public bool IsLoaded { get; }
    
    public Bingo CurrentBingo { get; set; }
    public ReadOnlyObservableCollection<Game> Games { get; }

    public DesignGameStats(IEnumerable<Game> games)
    {
        Games = new ReadOnlyObservableCollection<Game>(new ObservableCollection<Game>(games));
    }
    
    public Task LoadAsync() => Task.CompletedTask;
    public Task<bool> DeleteAsync(Game game) => Task.FromResult(false);
}