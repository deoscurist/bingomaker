using System.Collections.ObjectModel;
using System.Threading.Tasks;
using BingoMaker.Models;
using BingoMaker.Routing;
using BingoMaker.Stores;
using CommunityToolkit.Mvvm.Input;

namespace BingoMaker.ViewModels.Editor;

public partial class BingoListViewModel : ViewModelBase
{
    private readonly INavigationRouter _router; 
    private readonly IBingoStore _bingoStore;
    
    public ReadOnlyObservableCollection<Bingo> Bingos
        => _bingoStore.Bingos;

    public BingoListViewModel(INavigationRouter router, IBingoStore bingoStore)
    {
        _router = router;
        _bingoStore = bingoStore;
        _bingoStore.LoadAsync();
    }

    [RelayCommand]
    public Task BingoForm(Bingo? bingo) => bingo is null
        ? _router.PushAsync<BingoFormViewModel>()
        : _router.PushAsync<BingoFormViewModel>(bingo);
}