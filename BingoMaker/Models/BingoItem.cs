using System;

namespace BingoMaker.Models;

public record BingoItem
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Text { get; set; } = string.Empty;
    public string? ImageId { get; set; }
}