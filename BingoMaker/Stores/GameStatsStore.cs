using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using BingoMaker.Data.Repositories;
using BingoMaker.Messages;
using BingoMaker.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;

namespace BingoMaker.Stores;

public partial class GameStatsStore : ObservableObject, IGameStatsStore
{
    private readonly IGameRepository _repository;
        
    public Bingo CurrentBingo { get; set; }
    
    private readonly ObservableCollection<Game> _games;
    public ReadOnlyObservableCollection<Game> Games { get; }

    public GameStatsStore(IGameRepository repository)
    {
        _repository = repository;
        _games = new ObservableCollection<Game>();
        Games = new ReadOnlyObservableCollection<Game>(_games);
    }
    
    public async Task LoadAsync()
    {
        try
        {
            var games = _repository.GetAllByBingoId(CurrentBingo.Id)
                .OrderByDescending(game => game.FinishedAt);
            
            _games.Clear();

            foreach (var game in games)
                _games.Add(game);
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
            
            WeakReferenceMessenger.Default.Send(
                new NotificationMessage(Title: "Something goes wrong...", Message: exception.Message, Type: NotificationType.Error));
            
            throw;
        }
    }

    public async Task<bool> DeleteAsync(Game game)
    {
        try
        {
            _repository.Delete(game);
            
            var foundGame = _games.FirstOrDefault(
                item => item.Id == game.Id);

            if (foundGame is not null)
            {
                _games.Remove(foundGame);
            }
            
            WeakReferenceMessenger.Default.Send(
                new NotificationMessage(Title: "Game has been deleted", Type: NotificationType.Success));
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
            
            WeakReferenceMessenger.Default.Send(
                new NotificationMessage(Title: "Something goes wrong...", Message:exception.Message, Type: NotificationType.Error));

            throw;
        }
        
        return true;
    }
}