using System;
using Avalonia;
using Avalonia.Data.Converters;

namespace BingoMaker.Converters;

public static class HighlightConverters
{
    public static readonly FuncValueConverter<double, CornerRadius> TopCornerRadius =
        new(height =>
        {
            var scale = double.IsFinite(height) ? Math.Max(0, height) / 16 : 0;
            return new CornerRadius(8 * scale, 8 * scale, 4 * scale, 4 * scale);
        });
    
    public static readonly FuncValueConverter<double, CornerRadius> BottomCornerRadius =
        new(height =>
        {
            var scale = double.IsFinite(height) ? Math.Max(0, height) / 16 : 0;
            return new CornerRadius(8 * scale, 8 * scale, 4 * scale, 4 * scale);
        });
}
