using System.Collections.ObjectModel;
using BingoMaker.Messages;
using BingoMaker.Models;
using CommunityToolkit.Mvvm.Messaging;

namespace BingoMaker.ViewModels;

public partial class NotificationStackViewModel : ViewModelBase
{
    public ObservableCollection<NotificationViewModel> Notifications { get; set; } = [];
    
    public NotificationStackViewModel()
    {
        WeakReferenceMessenger.Default.Register<NotificationStackViewModel, NotificationMessage>(this, (recipient, message) =>
        {
            var notification = new NotificationViewModel(
                new Notification(message.Title, message.Message, message.Type), message.Duration);
            
            notification.Closed += recipient.OnNotificationClosed;
            
            Notifications.Insert(0, notification);
        });
    }
    
    private void OnNotificationClosed(NotificationViewModel notification)
    {
        notification.Closed -= OnNotificationClosed;
        Notifications.Remove(notification);
    }
}