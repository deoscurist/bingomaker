using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

namespace BingoMaker.Controls;

public class BingoPanel : Panel
{
    public static readonly StyledProperty<int> SizeProperty = 
        AvaloniaProperty.Register<BingoPanel, int>("Size", 2, coerce: (_, v) => Math.Max(v, 2) );
    public static readonly StyledProperty<int> SpacingProperty = 
        AvaloniaProperty.Register<BingoPanel, int>("Spacing", 2); 

    public int Size { get => GetValue(SizeProperty); set => SetValue(SizeProperty, value); }
    public int Spacing { get => GetValue(SpacingProperty); set => SetValue(SpacingProperty, value); }
    
    static BingoPanel()
    {
        AffectsMeasure<BingoPanel>(SizeProperty, SpacingProperty);
    }

    private double CalculateItemSize(Size area)
    {
        var constraint = Math.Min(area.Width, area.Height);
        
        var itemSide = constraint / Size;

        return itemSide;
    }
    
    protected override Size MeasureOverride(Size availableSize)
    {
        var itemSide = 0.0;
        var totalSpacing = Spacing * (Size - 1);
        var constraint = Math.Min(availableSize.Width, availableSize.Height) - totalSpacing;

        if (double.IsInfinity(constraint))
        {
            var infinitySize = new Size(double.PositiveInfinity, double.PositiveInfinity);

            foreach (var child in Children)
            {
                child.Measure(infinitySize);
                itemSide = Math.Max(itemSide, Math.Max(child.DesiredSize.Width, child.DesiredSize.Height));
            }
        }
        else
        {
            itemSide = constraint / Size;
        }
        
        return new Size(
            itemSide * Size + totalSpacing,
            itemSide * Size + totalSpacing);
    }
    
    protected override Size ArrangeOverride(Size finalSize) {
        var side = Math.Min(finalSize.Width, finalSize.Height);
        var totalSpacing = Spacing * (Size - 1);
        var itemSide = (side - totalSpacing) / Size;

        var i = 0;
        var z = 0;

        foreach (var child in Children)
        {
            var x = i * itemSide + i * Spacing;
            var y = z * itemSide + z * Spacing;
            
            var destination = new Rect(x, y, 
                itemSide, 
                itemSide);
            
            child.Arrange(destination);

            i++;

            if (i % Size != 0) continue;
            
            i = 0;
            z++;
        }
        
        return finalSize;
    }
}