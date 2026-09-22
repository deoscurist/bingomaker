using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using BingoMaker.Data;
using BingoMaker.Routing;
using BingoMaker.Stores;
using BingoMaker.ViewModels;
using BingoMaker.Views;
using BingoMaker.Views.Windows;
using LiteDB;
using Microsoft.Extensions.DependencyInjection;

namespace BingoMaker;

public partial class App : Application
{
    public static ServiceProvider Services { get; private set; } = null!;
    
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var services = new ServiceCollection();

            services.AddDBConnection();

            services.AddSingleton<IBingoStore, BingoStore>();
            services.AddSingleton<IGameStore, GameStore>();
            
            services.AddSingleton<NavigationHost>();
            services.AddSingleton<INavigationHost>(sp => sp.GetRequiredService<NavigationHost>());
            services.AddSingleton<ViewLocator>();
            services.AddSingleton<INavigationRouter, NavigationRouter>();

            Services = services.BuildServiceProvider();
            
            try
            {
                
                var window = new MainWindow() { DataContext = new MainViewModel() };
                
                _ = Services.GetRequiredService<ILiteDatabase>(); 
                
                Services.GetRequiredService<NavigationHost>().Attach(window.Navigation);
                Services.GetRequiredService<INavigationRouter>().SetRoot<StartupViewModel>();
                
                desktop.MainWindow = window;
            }
            catch
            {
                Services.Dispose();
                throw;
            }
            
            desktop.Exit += (_, _) => Services.Dispose();
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
    private void QuitApp_OnClick(object? sender, EventArgs e)
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            desktop.Shutdown();
    }
}