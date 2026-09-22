using System;
using Avalonia;
using Avalonia.Controls;

namespace BingoMaker.Controls;

public class BingoPanel : Panel
{
    public static readonly StyledProperty<int> SizeProperty = 
        AvaloniaProperty.Register<BingoPanel, int>("Size", 2, coerce: (_, v) => Math.Max(v, 2) );
    public static readonly StyledProperty<int> SpacingProperty = 
        AvaloniaProperty.Register<BingoPanel, int>("Spacing", 2); 

    public int Size { get => GetValue(SizeProperty); set => SetValue(SizeProperty, value); }
    public int Spacing { get => GetValue(SpacingProperty); set => SetValue(SpacingProperty, value); }
    
    private double TotalSpacing => Spacing * (Size - 1);
    
    static BingoPanel()
    {
        AffectsMeasure<BingoPanel>(SizeProperty, SpacingProperty);
    }
    
    protected override Size MeasureOverride(Size availableSize)
    {
        var itemSide = 0.0;
        var constraint = Math.Min(availableSize.Width, availableSize.Height) - TotalSpacing;

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
            itemSide * Size + TotalSpacing,
            itemSide * Size + TotalSpacing);
    }
    
    protected override Size ArrangeOverride(Size finalSize) {
        var side = Math.Min(finalSize.Width, finalSize.Height);
        var itemSide = (side - TotalSpacing) / Size;

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