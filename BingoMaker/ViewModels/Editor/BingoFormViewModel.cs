using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using BingoMaker.Models;
using BingoMaker.Routing;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BingoMaker.ViewModels.Editor;

public partial class BingoFormViewModel : ViewModelBase
{
    private INavigationRouter _router;
    private Bingo _bingo { get; set; }

    public bool IsExists { get; } = false;

    public string Header => String.IsNullOrEmpty(_bingo.Title) ? "New Bingo" : $"Edit \"{_bingo.Title}\"";

    [ObservableProperty] public partial string Title { get; set; }
    [ObservableProperty] public partial double Size { get; set; }
    public ObservableCollection<BingoItem> Items { get; }
    
    public BingoFormViewModel(INavigationRouter router, Bingo? bingo = null)
    {
        _router = router;
        
        IsExists = bingo is not null;
        _bingo = bingo ?? new Bingo();

        Size = Math.Max(2, (int)Math.Ceiling(Math.Sqrt(_bingo.Items.Count)));
        
        Title = _bingo.Title;
        Items = new ObservableCollection<BingoItem>(_bingo.Items);

        if (Items.Count == 0)
        {
            Items.Add(new BingoItem());
        }
        
        Items.CollectionChanged += OnItemsCollectionChanged;
    }
    
    private void OnItemsCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        AddItemCommand.NotifyCanExecuteChanged();
        RemoveItemCommand.NotifyCanExecuteChanged();
    }

    public bool CanAddItem => Items.Count < 25;

    public bool CanDeleteItem => Items.Count > 1;

    [RelayCommand(CanExecute = nameof(CanAddItem))]
    public void AddItem()
    {
        Items.Add(new BingoItem());
        
        Size = Math.Max(2, (int)Math.Ceiling(Math.Sqrt(Items.Count)));
        RemoveItemCommand.NotifyCanExecuteChanged();
    }
    
    [RelayCommand(CanExecute = nameof(CanDeleteItem))]
    public void RemoveItem(BingoItem item)
    {
        Items.Remove(item);
        
        Size = Math.Max(2, (int)Math.Ceiling(Math.Sqrt(Items.Count)));
        AddItemCommand.NotifyCanExecuteChanged();
    }

    [RelayCommand]
    public void Save()
    {
        _router.PushAsync<BingoListViewModel>();
    }
    
    [RelayCommand]
    public async void Delete()
    {
        await _router.PushAsync<BingoListViewModel>();
        _router.ClearHistory(2);
    }
}
