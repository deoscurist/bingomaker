using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using BingoMaker.Models;
using BingoMaker.Stores;

namespace BingoMaker.Design.Stores;

public class DesignBingoStore : IBingoStore
{
    public ReadOnlyObservableCollection<Bingo> Bingos { get; }

    public DesignBingoStore(IEnumerable<Bingo> bingos)
    {
        Bingos = new ReadOnlyObservableCollection<Bingo>(
            new ObservableCollection<Bingo>(bingos));
    }
    
    public Task LoadAsync() => Task.CompletedTask;

    public Task<bool> SaveAsync(Bingo bingo)
        => throw new NotSupportedException();

    public Task<bool> DeleteAsync(Bingo bingo)
        => throw new NotSupportedException();
}