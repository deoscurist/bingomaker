using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;

namespace BingoMaker.Views;

public partial class NotificationStackView : UserControl
{
    public static readonly StyledProperty<double> FadeHeightProperty =
        AvaloniaProperty.Register<NotificationStackView, double>(
            nameof(FadeHeight),
            defaultValue: 32);

    public double FadeHeight
    {
        get => GetValue(FadeHeightProperty);
        set => SetValue(FadeHeightProperty, value);
    }
    
    private readonly LinearGradientBrush _mask = new()
    {
        GradientStops =
        {
            new GradientStop(Colors.Transparent, 0),
            new GradientStop(Colors.Black, 0.05),
            new GradientStop(Colors.Black, 0.95),
            new GradientStop(Colors.Transparent, 1)
        }
    };

    public NotificationStackView()
    {
        InitializeComponent();
        
        UpdateFadeMargins();

        NotificationStackScroll.OpacityMask = _mask;

        NotificationStackScroll.SizeChanged += (_, _) => UpdateMask();
        NotificationStackScroll.Loaded += (_, _) => UpdateMask();
    }
    
    protected override void OnPropertyChanged(
        AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == FadeHeightProperty &&
            NotificationStackScroll is not null &&
            NotificationItems is not null)
        {
            UpdateFadeMargins();
            UpdateMask();
        }
    }
    
    private void UpdateFadeMargins()
    {
        var height = Math.Max(0, FadeHeight);

        NotificationStackScroll.Margin = new Thickness(0, -height, 0, -height);
        NotificationItems.Margin = new Thickness(0, height, 0, height);
    }

    private void UpdateMask()
    {
        var height = NotificationStackScroll.Bounds.Height;

        if (height <= 0)
            return;

        // Не даём двум областям пересечься при высоте меньше 64.
        var fadeHeight = Math.Min(FadeHeight, height / 2);
        var offset = fadeHeight / height;

        _mask.StartPoint =
            new RelativePoint(0, 0, RelativeUnit.Absolute);

        _mask.EndPoint =
            new RelativePoint(0, height, RelativeUnit.Absolute);

        _mask.GradientStops[0].Offset = 0;
        _mask.GradientStops[1].Offset = offset;
        _mask.GradientStops[2].Offset = 1 - offset;
        _mask.GradientStops[3].Offset = 1;

        NotificationStackScroll.OpacityMask = _mask;
    }
}