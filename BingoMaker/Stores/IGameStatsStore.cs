using System.Collections.ObjectModel;
using System.Threading.Tasks;
using BingoMaker.Models;

namespace BingoMaker.Stores;

public interface IGameStatsStore
{
    Bingo CurrentBingo { get; set; }

    ReadOnlyObservableCollection<Game> Games { get; }
    
    Task LoadAsync();
    Task<bool> DeleteAsync(Game game);
}