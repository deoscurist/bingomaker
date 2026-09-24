namespace BingoMaker.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    public static bool IsDesignMode => Avalonia.Controls.Design.IsDesignMode;

    public NotificationStackViewModel Notifications { get; } = new();
}