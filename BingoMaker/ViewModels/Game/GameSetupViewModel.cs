using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using BingoMaker.Data.Repositories;
using BingoMaker.Models;
using BingoMaker.Routing;
using BingoMaker.Services;
using BingoMaker.Stores;
using CommunityToolkit.Mvvm.Input;

namespace BingoMaker.ViewModels.Game;

public record BingoGameStat(Bingo Bingo, int TotalCount, DateTime? LatestFinishedAt);

public partial class GameSetupViewModel : ViewModelBase
{
    private readonly INavigationRouter _router;
    private readonly IBingoRepository _bingoRepository;
    private readonly IGameRepository _gameRepository;
    private readonly IDialogService _dialogService;
    
    public ReadOnlyObservableCollection<BingoGameStat> Bingos { get; set; }

    public GameSetupViewModel(
        INavigationRouter router,
        IBingoRepository bingoRepository,
        IGameRepository gameRepository,
        IDialogService dialogService)
    {
        _router = router;
        _bingoRepository = bingoRepository;
        _gameRepository = gameRepository;
        _dialogService = dialogService;
        
        var bingos = _bingoRepository.GetAll();
        var gamesDict = _gameRepository.GetAll()
            .GroupBy(g => g.BingoId)
            .ToDictionary(
                g => g.Key,
                g => new {
                    TotalCount = g.Count(),
                    LatestFinishedAt = g.Max(x => x.FinishedAt),
                }
            );

        var list = bingos.Select(b => 
        {
            var hasStats = gamesDict.TryGetValue(b.Id, out var stats);
            return new BingoGameStat(
                b, 
                hasStats ? stats!.TotalCount : 0, 
                hasStats ? stats!.LatestFinishedAt : null
            );
        }).ToList();
        
        Bingos = new ReadOnlyObservableCollection<BingoGameStat>(
            new ObservableCollection<BingoGameStat>(list)
        );
    }
    
    [RelayCommand]
    public async Task StartGame(Bingo bingo)
    {
        if (await _dialogService.ShowConfirmAsync(
                "Start a new game",
                "Are you sure you want to start a new game with this bingo?")
           )
            await _router.PushAsync<GameViewModel>(bingo);
    }
    
    [RelayCommand]
    public Task OpenStats(Bingo bingo) => _router.PushAsync<GameStatsViewModel>(bingo);
}