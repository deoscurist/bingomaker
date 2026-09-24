using System;
using BingoMaker.Models;
using CommunityToolkit.Mvvm.Input;

namespace BingoMaker.ViewModels;

public partial class NotificationViewModel : ViewModelBase
{
    public string Title { get; set; }
    public string Message { get; set; }
    public NotificationType Type { get; set; }
    public bool IsInfo => Type == NotificationType.Info;
    public bool IsWarning => Type == NotificationType.Warning;
    public bool IsSuccess => Type == NotificationType.Success;
    public bool IsError => Type == NotificationType.Error;
    
    public double? Duration { get; }
    public event Action<NotificationViewModel>? Closed;
    
    public NotificationViewModel(Notification notification, double? duration)
    {
        Title = notification.Title;
        Message = notification.Message;
        Type = notification.Type;
        Duration = duration;
    }
    
    [RelayCommand]
    public void Hide()
    {
        Closed?.Invoke(this);
    }
}