using System.Collections.ObjectModel;
using BingoMaker.Models;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BingoMaker.ViewModels.Game;

public partial class GameSetupViewModel : ViewModelBase
{
    public ObservableCollection<Bingo> Bingos { get; } =
    [
        new Bingo 
        {
            Title = "Test Bingo",
            Items = [
                new BingoItem { Text = "lol"},
                new BingoItem { Text = "lol 2"},
                new BingoItem { Text = "lol 3"},
            ]
        },
        new Bingo 
        {
            Title = "Test Bingo 2",
            Items = [
                new BingoItem { Text = "lol"},
                new BingoItem { Text = "lol 2"},
                new BingoItem { Text = "lol 3"},
            ]
        },
        new Bingo 
        {
            Title = "Test Bingo 3",
            Items = [
                new BingoItem { Text = "lol"},
                new BingoItem { Text = "lol 2"},
                new BingoItem { Text = "lol 3"},
            ]
        },
        new Bingo 
        {
            Title = "Test Bingo 4",
            Items = [
                new BingoItem { Text = "lol"},
                new BingoItem { Text = "lol 2"},
                new BingoItem { Text = "lol 3"},
            ]
        },
        new Bingo 
        {
            Title = "Test Bingo 5",
            Items = [
                new BingoItem { Text = "lol"},
                new BingoItem { Text = "lol 2"},
                new BingoItem { Text = "lol 3"},
            ]
        },
        new Bingo 
        {
            Title = "Test Bingo 6",
            Items = [
                new BingoItem { Text = "lol"},
                new BingoItem { Text = "lol 2"},
                new BingoItem { Text = "lol 3"},
            ]
        },
        new Bingo 
        {
            Title = "Test Bingo 7",
            Items = [
                new BingoItem { Text = "lol"},
                new BingoItem { Text = "lol 2"},
                new BingoItem { Text = "lol 3"},
            ]
        },
        new Bingo 
        {
            Title = "Test Bingo 8",
            Items = [
                new BingoItem { Text = "lol"},
                new BingoItem { Text = "lol 2"},
                new BingoItem { Text = "lol 3"},
            ]
        },
        new Bingo 
        {
            Title = "Test Bingo 9",
            Items = [
                new BingoItem { Text = "lol"},
                new BingoItem { Text = "lol 2"},
                new BingoItem { Text = "lol 3"},
            ]
        },
    ];
}