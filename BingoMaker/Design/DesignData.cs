using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using BingoMaker.Data.Repositories;
using BingoMaker.Design.Data.Repositories;
using BingoMaker.Design.Routing;
using BingoMaker.Design.Services;
using BingoMaker.Design.Stores;
using BingoMaker.Models;
using BingoMaker.Services;
using BingoMaker.ViewModels;
using BingoMaker.ViewModels.Editor;
using BingoMaker.ViewModels.Game;
using BingoMaker.ViewModels.Windows;
using BingoMaker.Views.Windows;

namespace BingoMaker.Design;

internal static class DesignData
{
    private static List<Notification> Notifications { get; } = [
        new Notification("Info Notification", "Some message here"),
        new Notification("Error Notification", "Some message here", NotificationType.Error),
        new Notification("Success Notification", "Some message here", NotificationType.Success),
        new Notification("Warning Notification", "Some message here", NotificationType.Warning),
        new Notification("Notification without message", Type: NotificationType.Info),
    ];
    
    public static List<Game> Games { get; } =
    [
        new Game
        {
            BingoId = Guid.NewGuid(),
            Items =
            [
                new GameItem { BingoItem = new BingoItem { Text = "lol" } },
                new GameItem { BingoItem = new BingoItem { Text = "lol" } },
                new GameItem { BingoItem = new BingoItem { Text = "lol" }, MarkedAt = DateTime.Now },
                new GameItem { BingoItem = new BingoItem { Text = "lol" } },
                new GameItem { BingoItem = new BingoItem { Text = "lol" }, MarkedAt = DateTime.Now },
                new GameItem { BingoItem = new BingoItem { Text = "lol" }, MarkedAt = DateTime.Now },
            ]
        },
        new Game
        {
            BingoId = Guid.NewGuid(),
            Items =
            [
                new GameItem { BingoItem = new BingoItem { Text = "lol" } },
                new GameItem { BingoItem = new BingoItem { Text = "lol" } },
                new GameItem { BingoItem = new BingoItem { Text = "lol" }, MarkedAt = DateTime.Now },
                new GameItem { BingoItem = new BingoItem { Text = "lol" } },
                new GameItem { BingoItem = new BingoItem { Text = "lol" }, MarkedAt = DateTime.Now },
                new GameItem { BingoItem = new BingoItem { Text = "lol" }, MarkedAt = DateTime.Now },
            ]
        },
        new Game
        {
            BingoId = Guid.NewGuid(),
            Items =
            [
                new GameItem { BingoItem = new BingoItem { Text = "lol" } },
                new GameItem { BingoItem = new BingoItem { Text = "lol" } },
                new GameItem { BingoItem = new BingoItem { Text = "lol" }, MarkedAt = DateTime.Now },
                new GameItem { BingoItem = new BingoItem { Text = "lol" } },
                new GameItem { BingoItem = new BingoItem { Text = "lol" }, MarkedAt = DateTime.Now },
                new GameItem { BingoItem = new BingoItem { Text = "lol" }, MarkedAt = DateTime.Now },
            ]
        }
    ];

    public static List<Bingo> Bingos { get; } =
    [
        new Bingo
        {
            Title = "Test Bingo",
            Items =
            [
                new BingoItem { Text = "lol" },
                new BingoItem { Text = "lol 2" },
                new BingoItem { Text = "lol 3" },
            ]
        },
        new Bingo
        {
            Title = "Test Bingo 2",
            Items =
            [
                new BingoItem { Text = "lol" },
                new BingoItem { Text = "lol 2" },
                new BingoItem { Text = "lol 3" },
            ]
        },
        new Bingo
        {
            Title = "Test Bingo 3",
            Items =
            [
                new BingoItem { Text = "lol" },
                new BingoItem { Text = "lol 2" },
                new BingoItem { Text = "lol 3" },
            ]
        },
        new Bingo
        {
            Title = "Test Bingo 4",
            Items =
            [
                new BingoItem { Text = "lol" },
                new BingoItem { Text = "lol 2" },
                new BingoItem { Text = "lol 3" },
            ]
        },
        new Bingo
        {
            Title = "Test Bingo 5",
            Items =
            [
                new BingoItem { Text = "lol" },
                new BingoItem { Text = "lol 2" },
                new BingoItem { Text = "lol 3" },
            ]
        },
        new Bingo
        {
            Title = "Test Bingo 6",
            Items =
            [
                new BingoItem { Text = "lol" },
                new BingoItem { Text = "lol 2" },
                new BingoItem { Text = "lol 3" },
            ]
        },
        new Bingo
        {
            Title = "Test Bingo 7",
            Items =
            [
                new BingoItem { Text = "lol" },
                new BingoItem { Text = "lol 2" },
                new BingoItem { Text = "lol 3" },
            ]
        },
        new Bingo
        {
            Title = "Test Bingo 8",
            Items =
            [
                new BingoItem { Text = "lol" },
                new BingoItem { Text = "lol 2" },
                new BingoItem { Text = "lol 3" },
            ]
        },
        new Bingo
        {
            Title = "Test Bingo 9",
            Items =
            [
                new BingoItem { Text = "lol" },
                new BingoItem { Text = "lol 2" },
                new BingoItem { Text = "lol 3" },
            ]
        }
    ];

    public static DesignRouter DesignRouter { get; } = new();
    public static IDialogService DesignDialog { get; } = new DesignDialogService();
    public static ConfirmDialogViewModel ConfirmDialog { get; } = new(new ConfirmDialog(), "Test Title", "Test Message");

    public static IGameRepository GameRepository { get; } = new DesignGameRepository(Games);
    public static IBingoRepository BingoRepository { get; } = new DesignBingoRepository(Bingos);

    public static DesignBingoStore BingoStore { get; } = new(Bingos);
    public static DesignGameStore GameStore { get; } = new();
    
    public static NotificationStackViewModel NotificationStack { get; } = new()
    {
        Notifications = new ObservableCollection<NotificationViewModel>([
            .. Notifications.Select(n => new NotificationViewModel(n, null))
        ])
    };
    public static NotificationViewModel Notification { get; } = new(Notifications[0], null);
    
    public static StartupViewModel Startup { get; } = new(DesignRouter);
    public static GameSetupViewModel GameSetup { get; } = new(DesignRouter, BingoRepository, GameRepository, DesignDialog);
    public static GameViewModel Game { get; } = new(DesignRouter, GameStore, Bingos[0]);
    public static BingoListViewModel BingoList { get; } = new(DesignRouter, BingoStore, DesignDialog);
    public static BingoFormViewModel BingoForm { get; } = new(DesignRouter, BingoStore, DesignDialog, Bingos[0]);
}