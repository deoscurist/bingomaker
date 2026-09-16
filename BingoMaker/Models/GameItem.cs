using System;

namespace BingoMaker.Models;

public class GameItem
{
    public DateTime? MarkedAt { get; set; }
    public required BingoItem BingoItem { get; set; }
}