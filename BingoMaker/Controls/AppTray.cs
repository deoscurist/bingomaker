using System;
using System.ComponentModel;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Platform;
using Avalonia.Threading;
using BingoMaker.ViewModels;

namespace BingoMaker.Controls;

public class AppTray : IDisposable
{
    private readonly TrayIcon _trayIcon;
    private readonly TrayViewModel _viewModel;
    
    private NativeMenuItem? _currentBingoItem;

    public AppTray(Application app, TrayViewModel viewModel)
    {
        _viewModel = viewModel;
        
        _trayIcon = new TrayIcon
        {
            Icon = new WindowIcon(
                AssetLoader.Open(new Uri("avares://BingoMaker/Assets/bingo-logo.ico"))),
            ToolTipText = "BingoMaker",
            Menu = [],
            IsVisible = true,
        };
        
        TrayIcon.SetIcons(app, [_trayIcon]);
        
        RebuildMenu();
        
        _viewModel.PropertyChanged += OnViewModelPropertyChanged;
    }
    
    private void RebuildMenu()
    {
        Dispatcher.UIThread.Post(() =>
        {
            var nativeMenu = _trayIcon.Menu;
            nativeMenu.Items.Clear();
            
            _currentBingoItem = new NativeMenuItem
            {
                Header = "No active bingo",
            };

            if (_viewModel.CurrentGame != null)
            {
                var currentBingoSubMenu = new NativeMenu();
                
                currentBingoSubMenu.Add(new NativeMenuItem 
                { 
                    Header = "Stop", 
                    Command = _viewModel.FinishGameCommand
                });
                
                currentBingoSubMenu.Add(
                    new NativeMenuItemSeparator());

                foreach (var item in _viewModel.CurrentGame.Items.Where(x => x.MarkedAt == null))
                {
                    currentBingoSubMenu.Add(new NativeMenuItem
                    {
                        Header = $"Select \"{item.BingoItem.Text}\"",
                        Command = _viewModel.MarkItemCommand,
                        CommandParameter = item
                    });
                }
                
                _currentBingoItem = new NativeMenuItem
                {
                    Header = $"Active Game ({_viewModel.GameTime:hh\\:mm\\:ss})",
                    Menu = currentBingoSubMenu
                };
            }

            nativeMenu.Items.Add(_currentBingoItem);
            nativeMenu.Items.Add(new NativeMenuItemSeparator());
            nativeMenu.Items.Add(new NativeMenuItem { Header = "About", Command = _viewModel.AppAboutCommand });
            nativeMenu.Items.Add(new NativeMenuItem { Header = "Quit", Command = _viewModel.QuitAppCommand });
        });
    }

    public void Dispose()
    {
        _viewModel.PropertyChanged -= OnViewModelPropertyChanged;
        _trayIcon.Dispose();
    }

    private void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        switch (e.PropertyName)
        {
            case nameof(_viewModel.GameTime):
                Dispatcher.UIThread.Post(() =>
                {
                    if (_currentBingoItem != null && _viewModel.CurrentGame != null)
                    {
                        _currentBingoItem.Header = 
                            $"\"{_viewModel.CurrentBingo!.Title}\": ({_viewModel.GameTime:hh\\:mm\\:ss})";
                    }
                });
                return;
            case nameof(_viewModel.CurrentGame):
                RebuildMenu();
                break;
        }
    }
}