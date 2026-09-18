using System;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using BingoMaker.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace BingoMaker.Routing;

public sealed class NavigationRouter(
    INavigationHost host,
    ViewLocator locator,
    IServiceProvider services) : INavigationRouter
{
    public void SetRoot<TViewModel>(params object[] args) where TViewModel : ViewModelBase
        => host.Root.Content = GetPage<TViewModel>(args);
    
    public Task PushAsync<TViewModel>(params object[] args) where TViewModel : ViewModelBase
        => host.Root.PushAsync(GetPage<TViewModel>(args));

    private Page GetPage<TViewModel>(params object[] args) where TViewModel : ViewModelBase
    {
        var viewModel = ActivatorUtilities.CreateInstance<TViewModel>(services, args);
        
        if (locator.Build(viewModel) is not Page page)
            throw new InvalidOperationException($"View for {typeof(TViewModel).Name} not a Page.");
        
        page.DataContext = viewModel;
        
        return page;
    }

    public Task PopAsync() => host.Root.PopAsync();
    public void ClearHistory(int? count = null)
    {
        if (count is < 0)
            throw new ArgumentOutOfRangeException(nameof(count));
        
        var navigation = host.Root;

        if (navigation.IsNavigating)
            throw new InvalidOperationException("Waiting for transition");

        var pages = navigation.NavigationStack.ToArray();

        var historyCount = Math.Max(0, pages.Length - 1);
        var removeCount = Math.Min(count ?? historyCount, historyCount);

        for (var i = 0; i < removeCount; i++)
        {
            navigation.RemovePage(pages[pages.Length - 2 - i]);
        }
    }
}