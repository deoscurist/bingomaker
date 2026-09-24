using BingoMaker.Models;

namespace BingoMaker.Messages;

public record NotificationMessage(
    string Title, 
    string Message, 
    NotificationType Type = NotificationType.Info, 
    double? Duration = 5);