using System;
using System.Collections.Generic;
using BingoMaker.Data.Repositories;
using BingoMaker.Design.Data.Repositories;
using BingoMaker.Design.Routing;
using BingoMaker.Design.Stores;
using BingoMaker.Models;
using BingoMaker.ViewModels;
using BingoMaker.ViewModels.Editor;
using BingoMaker.ViewModels.Game;

namespace BingoMaker.Design;

internal static class DesignData
{
        public static List<Game> Games { get; } = [
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
        
    public static List<Bingo> Bingos { get; } = [
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
    
    public static IGameRepository GameRepository { get; } = new DesignGameRepository(Games);
    public static IBingoRepository BingoRepository { get; } = new DesignBingoRepository(Bingos);
    
    public static DesignBingoStore BingoStore { get; } = new(Bingos);
    public static DesignGameStore GameStore { get; } = new();
    
    public static StartupViewModel Startup { get; } = new(DesignRouter);
    public static GameSetupViewModel GameSetup { get; } = new(DesignRouter, BingoRepository, GameRepository);
    public static GameViewModel Game { get; } = new(DesignRouter, GameStore, Bingos[0]);
    public static BingoListViewModel BingoList { get; } = new(DesignRouter, BingoStore);
    public static BingoFormViewModel BingoForm { get; } = new(DesignRouter, BingoStore, Bingos[0]);
}