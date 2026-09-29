using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Layout;

namespace BingoMaker.Views.Templates;

public class ListCard : TemplatedControl
{
    public static readonly StyledProperty<string> TitleProperty =
        AvaloniaProperty.Register<ListCard, string>(nameof(Title));
    
    public static readonly StyledProperty<HorizontalAlignment> TitleAlignmentProperty =
        AvaloniaProperty.Register<ListCard, HorizontalAlignment>(nameof(TitleAlignment), HorizontalAlignment.Center);
    
    public static readonly StyledProperty<object?> ContentProperty =
        AvaloniaProperty.Register<ListCard, object?>(nameof(Content));
    
    public static readonly StyledProperty<object?> ActionsProperty =
        AvaloniaProperty.Register<ListCard, object?>(nameof(Actions));

    public string Title
    {
        get => GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }
    
    public HorizontalAlignment TitleAlignment
    {
        get => GetValue(TitleAlignmentProperty);
        set => SetValue(TitleAlignmentProperty, value);
    }
    
    public object? Content
    {
        get => GetValue(ContentProperty);
        set => SetValue(ContentProperty, value);
    }
    
    public object? Actions
    {
        get => GetValue(ActionsProperty);
        set => SetValue(ActionsProperty, value);
    }
}