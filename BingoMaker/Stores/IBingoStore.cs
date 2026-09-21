using System.Collections.ObjectModel;
using System.Threading.Tasks;
using BingoMaker.Models;

namespace BingoMaker.Stores;

public interface IBingoStore
{
    ReadOnlyObservableCollection<Bingo> Bingos { get; }

    Task LoadAsync();
    Task<bool> SaveAsync(Bingo bingo);
    Task<bool> DeleteAsync(Bingo bingo);
}