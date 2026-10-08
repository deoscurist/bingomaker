using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace BingoMaker.Converters;

public static class NavigationBarConverters
{
    public static readonly FuncValueConverter<Page?, IBrush> TopBackground = new(page => Background(page, "Top"));
    public static readonly FuncValueConverter<Page?, IBrush> BottomBackground = new(page => Background(page, "Bottom"));
    public static readonly FuncValueConverter<Page?, IBrush> TopBorder = new(page => Border(page, "Top"));
    public static readonly FuncValueConverter<Page?, IBrush> BottomBorder = new(page => Border(page, "Bottom"));
    public static readonly FuncValueConverter<Page?, IBrush> TopForeground = new(page => Foreground(page, "Top"));
    public static readonly FuncValueConverter<Page?, IBrush> BottomForeground = new(page => Foreground(page, "Bottom"));
    
    public static readonly FuncValueConverter<Page?, IBrush> TopBaseBrush =
        new(page => new SolidColorBrush(
            Read(page, "Top", "Color", Color.Parse("#AAB4B9"))));

    public static readonly FuncValueConverter<Page?, IBrush> SolidTopBackground =
        new(page => SolidBackground(page, "Top"));
    public static readonly FuncValueConverter<Page?, IBrush> SolidBottomBackground =
        new(page => SolidBackground(page, "Bottom"));

    private static IBrush SolidBackground(Page? page, string bar)
    {
        var color = Read(page, bar, "Color", Color.Parse("#E1E6E9"));
        return new SolidColorBrush(Color.FromRgb(color.R, color.G, color.B));
    }

    private static Color Read(Page? page, string bar, string suffix, Color fallback)
    {
        foreach (var key in new[] { $"{bar}NavigationBar{suffix}", $"NavigationBar{suffix}" })
            if (page is not null && page.Resources.TryGetResource(key, page.ActualThemeVariant, out var local)
                && local is Color localColor)
                return localColor;
        foreach (var key in new[] { $"{bar}NavigationBar{suffix}", $"NavigationBar{suffix}" })
            if (page is not null && page.TryFindResource(key, out var resource) && resource is Color color)
                return color;
        return fallback;
    }

    private static IBrush Foreground(Page? page, string bar) =>
        new SolidColorBrush(Read(page, bar, "TextColor", Color.Parse("#253746")));

    private static LinearGradientBrush Gradient() => new()
    {
        StartPoint = new RelativePoint(0, 0, RelativeUnit.Relative),
        EndPoint = new RelativePoint(0, 1, RelativeUnit.Relative)
    };
    
    private static Color Tint(Color source, string reference)
    {
        var target = Color.Parse(reference);
        static byte Channel(byte value, byte baseline, byte target) => (byte)Math.Round(
            target >= baseline
                ? value + (255 - value) * (target - baseline) / (double)(255 - baseline)
                : value * target / (double)baseline);
        return Color.FromArgb(target.A, Channel(source.R, 170, target.R),
            Channel(source.G, 180, target.G), Channel(source.B, 185, target.B));
    }

    private static IBrush Background(Page? page, string bar)
    {
        var color = Read(page, bar, "Color", Color.Parse("#AAB4B9"));
        var brush = Gradient();
        foreach (var (reference, offset) in new[]
        {
            ("#F0CAD1D5", 0.0),
            ("#E6B5BEC3", 0.46),
            ("#E0AAB4B9", 0.5),
            ("#E08D989F", 0.5),
            ("#E699A5AB", 0.78),
            ("#EBB3BDC3", 1.0)
        })
            brush.GradientStops.Add(new GradientStop(Tint(color, reference), offset));
        return brush;
    }

    private static IBrush Border(Page? page, string bar)
    {
        var color = Read(page, bar, "Color", Color.Parse("#AAB4B9"));
        var brush = Gradient();
        foreach (var (reference, offset) in new[]
        {
            ("#E6F5F8FA", 0.0), ("#B08B979E", 0.48), ("#CC536169", 1.0)
        })
            brush.GradientStops.Add(new GradientStop(Tint(color, reference), offset));
        return brush;
    }
}
