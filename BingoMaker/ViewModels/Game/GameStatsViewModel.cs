using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using BingoMaker.Data.Repositories;
using BingoMaker.Models;
using BingoMaker.Routing;
using BingoMaker.Services;
using CommunityToolkit.Mvvm.Input;

namespace BingoMaker.ViewModels.Game;

public partial class GameStatsViewModel : ViewModelBase
{
    private readonly INavigationRouter _router;
    private readonly IDialogService _dialogService;
    private readonly IGameRepository _gameRepository;
    private readonly Bingo _bingo;
    
    public string Header => $"\"{_bingo.Title}\" stats:";
    
    public ObservableCollection<GameStatCardViewModel> Games { get; }
    
    public GameStatsViewModel(
        INavigationRouter router,
        IDialogService dialogService,
        IGameRepository gameRepository, 
        Bingo bingo)
    {
        _router = router;
        _dialogService = dialogService;
        _gameRepository = gameRepository;
        _bingo = bingo;
        
        Games = new ObservableCollection<GameStatCardViewModel>(
            [.. _gameRepository.GetAllByBingoId()
                .OrderByDescending(game => game.StartedAt)
                .Select(game => new GameStatCardViewModel(game))]);
    }
    
    [RelayCommand]
    public async Task Delete(Models.Game game)
    {
        try
        {
            if (await _dialogService.ShowConfirmAsync(
                    "Delete Game",
                    "Are you sure you want to delete this game?")
               )
                if (_gameRepository.Delete(game))
                {
                    var foundGame = Games.FirstOrDefault(g => g.Game.Id == game.Id);
                    if (foundGame is not null) 
                        Games.Remove(foundGame);
                }
        }
        catch (Exception e)
        {
            return;
        }
    }

    [RelayCommand]
    public Task OpenGame(Models.Game game) => Task.CompletedTask;
}
