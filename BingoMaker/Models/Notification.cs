namespace BingoMaker.Models;

public enum NotificationType
{
    Info,
    Warning,
    Success,
    Error,
}

public record Notification(string Title, string? Message = null, NotificationType Type = NotificationType.Info);