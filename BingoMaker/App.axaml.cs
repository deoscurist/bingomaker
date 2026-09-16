using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using BingoMaker.Routing;
using BingoMaker.ViewModels;
using BingoMaker.Views;
using Microsoft.Extensions.DependencyInjection;

namespace BingoMaker;

public partial class App : Application
{
    public static IServiceProvider Services { get; private set; } = null!;
    
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var services = new ServiceCollection();

            services.AddSingleton<NavigationHost>();
            services.AddSingleton<INavigationHost>(sp => sp.GetRequiredService<NavigationHost>());
            services.AddSingleton<ViewLocator>();
            services.AddSingleton<INavigationRouter, NavigationRouter>();
            
            Services = services.BuildServiceProvider();
            
            var window = new MainWindow() { DataContext = new MainViewModel() };
            
            Services.GetRequiredService<NavigationHost>().Attach(window.Navigation);
            Services.GetRequiredService<INavigationRouter>().SetRoot<StartupViewModel>();
            
            desktop.MainWindow = window;
        }

        base.OnFrameworkInitializationCompleted();
    }
}