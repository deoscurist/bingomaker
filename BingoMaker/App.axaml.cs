using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using BingoMaker.Routing;
using BingoMaker.ViewModels;
using BingoMaker.Views;
using BingoMaker.Views.Windows;
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

    private Window GetMainWindow() => (ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)?.MainWindow!;
    
    private void AppAbout_OnClick(object? sender, EventArgs e)
    {
        var aboutWindow = new AboutWindow();
        aboutWindow.ShowDialog(GetMainWindow());
    }
    // private await void GameSetup_OnClick(object? sender, EventArgs e) => router.PushAsync<GameSetupViewModel>();
    // private await void Editor_OnClick(object? sender, EventArgs e) => Environment.Exit(0);
    private void QuitApp_OnClick(object? sender, EventArgs e) => Environment.Exit(0);
}