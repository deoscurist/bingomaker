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

public partial class GameShowViewModel : ViewModelBase
{
    private readonly INavigationRouter _router;
    private readonly IDialogService _dialogService;
    private readonly IGameStatsStore _gameStatsStore;
    
    public Models.Game Game { get; }
    public Bingo Bingo { get; }

    public string Title => $"Show \"{Bingo.Title}\" game on {Game.StartedAt:dd\\.MM\\.yyyy hh\\:mm}";
    public int Size { get; }
    public ObservableCollection<GameItemViewModel> Items { get; } = [];
    
    public GameShowViewModel(
        INavigationRouter router,
        IDialogService dialogService,
        IGameStatsStore gameStatsStore,
        IBingoRepository bingoRepository, 
        Models.Game game)
    {
        _router = router;
        _dialogService = dialogService;
        _gameStatsStore = gameStatsStore;
        
        Game = game;
        Bingo = bingoRepository.GetById(game.BingoId)!;
        
        Size = Math.Max(2, (int)Math.Ceiling(Math.Sqrt(Game.Items.Count)));
        
        Items = new ObservableCollection<GameItemViewModel>(
            Game.Items.Select(x => new GameItemViewModel(x)));
    }

    [RelayCommand]
    public async Task Delete()
    {
        try
        {
            if (await _dialogService.ShowConfirmAsync(
                    "Delete Game",
                    "Are you sure you want to delete this game?")
               )
            {
                await _gameStatsStore.DeleteAsync(Game);
                await _router.PopAsync();
            }
        }
        catch (Exception e)
        {
            return;
        }
    }
}