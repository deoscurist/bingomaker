using System;
using System.Collections.ObjectModel;

namespace BingoMaker.Models;

public class Game
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid BingoId { get; set; }
    public DateTime StartedAt { get; set; } = DateTime.Now;
    public DateTime? FinishedAt { get; set; }
    
    public ObservableCollection<GameItem> Items { get; set; } = [];
}