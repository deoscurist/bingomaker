using System.Collections.ObjectModel;
using System.Threading.Tasks;
using BingoMaker.Models;
using BingoMaker.Routing;
using BingoMaker.Stores;
using CommunityToolkit.Mvvm.Input;

namespace BingoMaker.ViewModels.Game;

public partial class GameSetupViewModel : ViewModelBase
{
    private readonly INavigationRouter _router;
    private readonly IBingoStore _bingoStore;
    
    public ReadOnlyObservableCollection<Bingo> Bingos
        => _bingoStore.Bingos;

    public GameSetupViewModel(
        INavigationRouter router,
        IBingoStore bingoStore)
    {
        _router = router;
        _bingoStore = bingoStore;
        _bingoStore.LoadAsync();
    }
    
    [RelayCommand]
    public Task StartGame(Bingo bingo) => _router.PushAsync<GameViewModel>(bingo);
}