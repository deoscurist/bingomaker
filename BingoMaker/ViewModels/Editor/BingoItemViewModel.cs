using BingoMaker.Models;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BingoMaker.ViewModels.Editor;

public partial class BingoItemViewModel : ViewModelBase
{
    private BingoItem _item;

    [ObservableProperty]
    public partial string Text { get; set; }
    
    [ObservableProperty]
    public partial string? ImageId { get; set; }

    public BingoItemViewModel(BingoItem? item = null)
    {
        _item = item ?? new BingoItem();
        Text = _item.Text;
        ImageId = _item.ImageId;
    }
    
    public BingoItem ToModel() => _item with
    {
        Text = Text,
        ImageId = ImageId
    };
}