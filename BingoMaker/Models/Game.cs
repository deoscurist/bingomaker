using System;
using System.Collections.Generic;

namespace BingoMaker.Models;

public class Game
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid BingoId { get; set; }
    public DateTime StartedAt { get; set; } = DateTime.Now;
    public DateTime? FinishedAt { get; set; }
    
    public List<GameItem> Items { get; set; } = [];
}