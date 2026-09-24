using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Avalonia.Threading;
using BingoMaker.Data.Repositories;
using BingoMaker.Messages;
using BingoMaker.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;

namespace BingoMaker.Stores;

public partial class GameStore : ObservableObject, IGameStore
{
    private readonly IGameRepository _repository;

    [ObservableProperty]
    public partial Bingo? CurrentBingo { get; private set; }

    [ObservableProperty]
    public partial Game? CurrentGame { get; private set; }
    
    [ObservableProperty]
    public partial TimeSpan GameTime { get; private set; }
    
    public event Action<GameItem>? ItemMarkChanged;
    
    private readonly Stopwatch _stopwatch = new();
    private readonly DispatcherTimer _timer;

    public GameStore(IGameRepository repository)
    {
        _repository = repository;

        _timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
        
        _timer.Tick += (_, _) => GameTime = _stopwatch.Elapsed;
    }
    
    public async Task<bool> StartGame(Bingo bingo)
    {
        try
        {
            GameTime = TimeSpan.Zero;
            _stopwatch.Restart();
            _timer.Start();
            
            var game = new Game { BingoId = bingo.Id };

            foreach (var item in bingo.Items)
                game.Items.Add(new GameItem { BingoItem = item });

            _repository.Save(game);

            CurrentGame = game;
            CurrentBingo = bingo;
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
            
            WeakReferenceMessenger.Default.Send(
                new NotificationMessage(
                    Title: "Cannot start a game", 
                    Message: exception.Message, 
                    Type: NotificationType.Error));
            
            CurrentGame = null;
            CurrentBingo = null;
            
            throw;
        }
        
        return true;
    }

    public async Task<bool> MarkItem(GameItem item)
    {
        var previousMarkedAt = item.MarkedAt;

        try
        {
            item.MarkedAt = item.MarkedAt == null ? DateTime.Now : null;
            _repository.Save(CurrentGame!);
            ItemMarkChanged?.Invoke(item);
        }
        catch (Exception exception)
        {
            item.MarkedAt = previousMarkedAt;
            
            Console.WriteLine(exception);
            
            WeakReferenceMessenger.Default.Send(
                new NotificationMessage(
                    Title: "Cannot mark an item", 
                    Message: exception.Message, 
                    Type: NotificationType.Error));
            
            throw;
        }

        return true;
    }

    public async Task<bool> FinishGame()
    {
        if (CurrentGame == null)
            return false;

        try
        {
            _stopwatch.Stop();
            _timer.Stop();
            GameTime = _stopwatch.Elapsed;
            
            CurrentGame!.FinishedAt = DateTime.Now;
            
            _repository.Save(CurrentGame!);
            
            CurrentGame = null;
            
            WeakReferenceMessenger.Default.Send(
                new NotificationMessage(
                    Title: "Your results have been saved", 
                    Message: "You can find them in bingo stats", 
                    Type: NotificationType.Info));
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
            
            WeakReferenceMessenger.Default.Send(
                new NotificationMessage(
                    Title: "Cannot finish game", 
                    Message: exception.Message, 
                    Type: NotificationType.Error));
            
            _stopwatch.Start();
            _timer.Start();
            
            throw;
        }

        return true;
    }
}