namespace BingoMaker.ViewModels;

public class MainViewModel : ViewModelBase
{
    public static bool IsDesignMode => Avalonia.Controls.Design.IsDesignMode;

    public NotificationStackViewModel Notifications { get; } = new();
}