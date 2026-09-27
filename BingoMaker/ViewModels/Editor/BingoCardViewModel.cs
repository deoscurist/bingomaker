using System.Threading.Tasks;
using BingoMaker.Models;
using CommunityToolkit.Mvvm.Input;

namespace BingoMaker.ViewModels.Editor;

public partial class BingoCardViewModel : ViewModelBase
{
    public Bingo Bingo { get; }

    public BingoCardViewModel(Bingo bingo)
    {
        Bingo = bingo;
    }
}