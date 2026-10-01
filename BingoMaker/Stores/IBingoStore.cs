using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using BingoMaker.Models;

namespace BingoMaker.Stores;

public interface IBingoStore : INotifyPropertyChanged
{
    bool IsLoaded { get; }
    
    ReadOnlyObservableCollection<Bingo> Bingos { get; }

    Task LoadAsync();
    Task<bool> SaveAsync(Bingo bingo);
    Task<bool> DeleteAsync(Bingo bingo);
    
    public event Action<Bingo>? BingoSaved;
    public event Action<Bingo>? BingoDeleted;
}