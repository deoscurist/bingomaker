using System.Threading.Tasks;
using BingoMaker.Routing;
using BingoMaker.ViewModels;

namespace BingoMaker.Design.Routing;

internal sealed class DesignRouter : INavigationRouter
{
    public void SetRoot<TViewModel>(params object[] args) where TViewModel : ViewModelBase
    {
        return;
    }

    public Task PushAsync<TViewModel>(params object[] args) where TViewModel : ViewModelBase => Task.CompletedTask;

    public Task PopAsync() => Task.CompletedTask;
    public void ClearHistory(int? count = null)
    {
        return;
    }
}