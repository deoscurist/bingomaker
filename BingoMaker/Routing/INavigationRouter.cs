using System.Threading.Tasks;
using BingoMaker.ViewModels;

namespace BingoMaker.Routing;

public interface INavigationRouter
{
    void SetRoot<TViewModel>(params object[] args) where TViewModel : ViewModelBase;
    Task PushAsync<TViewModel>(params object[] args) where TViewModel : ViewModelBase;
    Task PopAsync();
}