using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using BingoMaker.Data.Repositories;
using BingoMaker.Messages;
using BingoMaker.Models;
using CommunityToolkit.Mvvm.Messaging;

namespace BingoMaker.Stores;

public class BingoStore : IBingoStore
{
    private IBingoRepository _repository;
    private ObservableCollection<Bingo> _bingos;
    public ReadOnlyObservableCollection<Bingo> Bingos { get; }
    
    private bool _isLoaded;

    public BingoStore(IBingoRepository repository)
    {
        _repository = repository;
        _bingos = new ObservableCollection<Bingo>();
        Bingos = new ReadOnlyObservableCollection<Bingo>(_bingos);
    }
    
    public async Task LoadAsync()
    {
        if (_isLoaded)
            return;
        
        try
        {
            var bingos = _repository.GetAll();
            _bingos.Clear();
        
            foreach (var bingo in bingos)
                _bingos.Add(bingo);
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
            
            WeakReferenceMessenger.Default.Send(
                new NotificationMessage(Title: "Something goes wrong...", Message:exception.Message, Type: NotificationType.Error));

            return;
        }
        
        _isLoaded = true;
    }

    public async Task<bool> SaveAsync(Bingo bingo)
    {
        try
        {
            _repository.Save(bingo);
            
            var foundBingo = _bingos.FirstOrDefault(
                item => item.Id == bingo.Id);

            if (foundBingo is null)
            {
                _bingos.Add(bingo);
            }
            else
            {
                var index = _bingos.IndexOf(foundBingo);
                _bingos[index] = bingo;
            }
            
            WeakReferenceMessenger.Default.Send(
                new NotificationMessage(Title: "Bingo has been saved", Type: NotificationType.Success));
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
            
            WeakReferenceMessenger.Default.Send(
                new NotificationMessage(Title: "Something goes wrong...", Message:exception.Message, Type: NotificationType.Error));

            return false;
        }
        
        return true;
    }

    public async Task<bool> DeleteAsync(Bingo bingo)
    {
        try
        {
            _repository.Delete(bingo);
            
            var foundBingo = _bingos.FirstOrDefault(
                item => item.Id == bingo.Id);

            if (foundBingo is not null)
            {
                _bingos.Remove(foundBingo);
            }
            
            WeakReferenceMessenger.Default.Send(
                new NotificationMessage(Title: "Bingo has been deleted", Type: NotificationType.Success));
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
            
            WeakReferenceMessenger.Default.Send(
                new NotificationMessage(Title: "Something goes wrong...", Message:exception.Message, Type: NotificationType.Error));
            
            return false;
        }
        
        return true;
    }
}