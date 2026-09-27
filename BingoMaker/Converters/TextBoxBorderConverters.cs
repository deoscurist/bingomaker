using System;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace BingoMaker.Converters;

public static class TextBoxBorderConverters
{
    public static readonly FuncValueConverter<IBrush, IBrush?> Hover = new(brush =>
    {
        if (brush is not ISolidColorBrush solid)
            return brush;

        var color = solid.Color;
        return new SolidColorBrush(Color.FromArgb(color.A,
            Lighten(color.R), Lighten(color.G), Lighten(color.B)), solid.Opacity);
    });

    private static byte Lighten(byte channel) =>
        (byte)Math.Round(channel + (255 - channel) * 0.5);
}
