using System.Threading.Tasks;
using BingoMaker.Routing;
using BingoMaker.ViewModels.Editor;
using BingoMaker.ViewModels.Game;
using CommunityToolkit.Mvvm.Input;

namespace BingoMaker.ViewModels;

public partial class StartupViewModel(INavigationRouter router) : ViewModelBase
{
    [RelayCommand]
    public Task GameSetup() => router.PushAsync<GameSetupViewModel>();
    
    [RelayCommand]
    public Task BingoList() => router.PushAsync<BingoListViewModel>();
}