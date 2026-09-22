using System;
using System.Collections.ObjectModel;

namespace BingoMaker.Models;

public class Bingo
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Title { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    
    public ObservableCollection<BingoItem> Items { get; set; } = [];
}