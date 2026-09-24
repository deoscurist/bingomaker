using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using BingoMaker.Models;
using BingoMaker.Routing;
using BingoMaker.Stores;
using BingoMaker.Views.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BingoMaker.ViewModels;

public partial class TrayViewModel : ViewModelBase
{
    private readonly INavigationRouter _router;
    private readonly IGameStore _gameStore;
    
    [ObservableProperty] public partial Models.Bingo? CurrentBingo { get; set; }
    [ObservableProperty] public partial Models.Game? CurrentGame { get; set; }
    [ObservableProperty] public partial TimeSpan GameTime { get; set; }

    public TrayViewModel(INavigationRouter router, IGameStore gameStore)
    {
        _router = router;
        _gameStore = gameStore;
        
        CurrentBingo = _gameStore.CurrentBingo;
        CurrentGame = _gameStore.CurrentGame;
        GameTime = _gameStore.GameTime;
        
        _gameStore.PropertyChanged += OnGameStorePropertyChanged;
        
        _gameStore.ItemMarkChanged += OnStoreItemMarkChanged;
    }
    
    private void OnStoreItemMarkChanged(GameItem item)
    {
        OnPropertyChanged(nameof(CurrentGame));
    }

    private void OnGameStorePropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        switch (e.PropertyName)
        {
            case nameof(IGameStore.CurrentBingo):
                CurrentBingo = _gameStore.CurrentBingo;
                break;
            case nameof(IGameStore.CurrentGame):
                CurrentGame = _gameStore.CurrentGame;
                OnPropertyChanged(nameof(CurrentGame));
                break;
            case nameof(IGameStore.GameTime):
                GameTime = _gameStore.GameTime;
                break;
        }
    }

    private Window? GetMainWindow()
    {
        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            return desktop.MainWindow;
        }
        return null;
    }

    [RelayCommand]
    public void AppAbout()
    {
        var mainWindow = GetMainWindow();
        var aboutWindow = new AboutWindow();
        if (mainWindow != null) aboutWindow.ShowDialog(mainWindow);
    }
    
    [RelayCommand]
    public void QuitApp()
    {
        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            desktop.Shutdown();
    }

    [RelayCommand]
    public async Task FinishGame()
    {
        try
        {
            await _gameStore.FinishGame();
            await _router.PushAsync<StartupViewModel>();
            _router.ClearHistory();
        }
        catch (Exception e)
        {
            return;
        }
    }
    
    [RelayCommand]
    public async Task MarkItem(GameItem item)
    {
        try
        {
            await _gameStore.MarkItem(item);
        }
        catch (Exception e)
        {
            return;
        }
    }
}