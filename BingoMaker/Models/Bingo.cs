using System;
using System.Collections.Generic;

namespace BingoMaker.Models;

public class Bingo
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Title { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    
    public List<BingoItem> Items { get; set; } = [];
}