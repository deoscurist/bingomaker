using System.Collections.ObjectModel;
using System.Threading.Tasks;
using BingoMaker.Models;
using BingoMaker.Routing;
using CommunityToolkit.Mvvm.Input;

namespace BingoMaker.ViewModels.Game;

public partial class GameSetupViewModel(INavigationRouter router) : ViewModelBase
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
    
    [RelayCommand]
    public Task StartGame(Bingo bingo) => router.PushAsync<GameViewModel>(bingo);
}