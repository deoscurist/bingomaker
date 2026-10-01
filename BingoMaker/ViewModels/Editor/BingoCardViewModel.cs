using System.Threading.Tasks;
using BingoMaker.Models;
using CommunityToolkit.Mvvm.Input;

namespace BingoMaker.ViewModels.Editor;

public partial class BingoCardViewModel : ViewModelBase
{
    private Bingo _bingo;

    public Bingo Bingo
    {
        get => _bingo;
        internal set
        {
            _bingo = value;
            OnPropertyChanged(nameof(Bingo));
        }
    }

    public BingoCardViewModel(Bingo bingo)
    {
        _bingo = bingo;
    }
}