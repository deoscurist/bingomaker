using System.Collections.Generic;
using System.Collections.ObjectModel;
using BingoMaker.Design.Routing;
using BingoMaker.Models;
using BingoMaker.ViewModels;
using BingoMaker.ViewModels.Editor;

namespace BingoMaker.Design;

internal static class DesignData
{
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
    
    public static StartupViewModel Startup { get; } = new(DesignRouter);
    public static BingoListViewModel BingoList { get; } = new(DesignRouter)
    {
        Bingos = new ObservableCollection<Bingo>(Bingos)
    };
    public static BingoFormViewModel BingoForm { get; } = new(DesignRouter, Bingos[0]);
}