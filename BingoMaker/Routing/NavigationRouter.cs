using System;
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
}