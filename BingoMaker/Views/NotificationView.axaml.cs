using System;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media.Transformation;
using Avalonia.Threading;
using BingoMaker.ViewModels;

namespace BingoMaker.Views;

public partial class NotificationView : UserControl
{
    private bool _isClosing;
    private readonly DispatcherTimer _autoCloseTimer = new();
    
    public NotificationView()
    {
        InitializeComponent();
        
        _autoCloseTimer.Tick += OnAutoClose;
        
        Unloaded += (_, _) => _autoCloseTimer.Stop();
    }
    
    private async void OnAutoClose(object? sender, EventArgs e)
    {
        _autoCloseTimer.Stop();
        await CloseAsync();
    }
    
    private async Task CloseAsync()
    {
        _autoCloseTimer.Stop();

        if (_isClosing ||
            DataContext is not NotificationViewModel vm)
            return;

        _isClosing = true;
        NotificationBorder.IsHitTestVisible = false;

        NotificationBorder.RenderTransform =
            TransformOperations.Parse("translate(400px)");

        // Совпадает с Duration="0:0:0.3" в XAML.
        await Task.Delay(300);

        vm.HideCommand.Execute(null);
    }

    private void OnLoaded(object? sender, RoutedEventArgs e)
    {
        NotificationBorder.RenderTransform =
            TransformOperations.Parse("translate(0px)");
        
        _autoCloseTimer.Stop();

        if (DataContext is NotificationViewModel vm
            && vm.Duration is double duration
            && duration > 0)
        {
            _autoCloseTimer.Interval = TimeSpan.FromSeconds(duration);
            _autoCloseTimer.Start();
        }
    }

    private async void OnCloseClick(object? sender, RoutedEventArgs e)
    {
        await CloseAsync();
    }
}