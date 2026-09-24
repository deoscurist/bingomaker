using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using BingoMaker.Models;
using BingoMaker.Routing;
using BingoMaker.Services;
using BingoMaker.Stores;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BingoMaker.ViewModels.Editor;

public partial class BingoFormViewModel : ViewModelBase
{
    private readonly INavigationRouter _router;
    private readonly IBingoStore _bingoStore;
    private readonly IDialogService _dialogService;
    private Bingo _bingo { get;}

    public bool IsExists { get; }

    public string Header => String.IsNullOrEmpty(_bingo.Title) ? "New Bingo" : $"Edit \"{_bingo.Title}\"";

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SaveCommand))] 
    public partial string Title { get; set; }
    
    [ObservableProperty] public partial double Size { get; set; }
    public ObservableCollection<BingoItemViewModel> Items { get; }
    
    public BingoFormViewModel(
        INavigationRouter router, 
        IBingoStore bingoStore, 
        IDialogService dialogService,
        Bingo? bingo = null)
    {
        _router = router;
        _bingoStore = bingoStore;
        _dialogService = dialogService;
        
        IsExists = bingo is not null;
        _bingo = bingo ?? new Bingo();
        
        Title = _bingo.Title;
        Items = new ObservableCollection<BingoItemViewModel>(
            _bingo.Items.Select(item => new BingoItemViewModel(item)));

        if (Items.Count == 0)
            Items.Add(new BingoItemViewModel());
        
        foreach (var item in Items)
            item.PropertyChanged += OnItemPropertyChanged;
        
        Size = Math.Max(2, (int)Math.Ceiling(Math.Sqrt(Items.Count)));
    }
    
    private void OnItemPropertyChanged(
        object? sender,
        PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(BingoItemViewModel.Text))
            SaveCommand.NotifyCanExecuteChanged();
    }

    public bool CanAddItem => Items.Count < 25;

    [RelayCommand(CanExecute = nameof(CanAddItem))]
    public void AddItem()
    {
        var item = new BingoItemViewModel();
        item.PropertyChanged += OnItemPropertyChanged;
        Items.Add(item);
        
        Size = Math.Max(2, (int)Math.Ceiling(Math.Sqrt(Items.Count)));
        
        AddItemCommand.NotifyCanExecuteChanged();
        RemoveItemCommand.NotifyCanExecuteChanged();
        SaveCommand.NotifyCanExecuteChanged();
    }
    
    public bool CanDeleteItem => Items.Count > 1;
    
    [RelayCommand(CanExecute = nameof(CanDeleteItem))]
    public void RemoveItem(BingoItemViewModel item)
    {
        if (!Items.Remove(item))
            return;
        
        item.PropertyChanged -= OnItemPropertyChanged;
        
        Size = Math.Max(2, (int)Math.Ceiling(Math.Sqrt(Items.Count)));
        
        AddItemCommand.NotifyCanExecuteChanged();
        RemoveItemCommand.NotifyCanExecuteChanged();
        SaveCommand.NotifyCanExecuteChanged();
    }
    
    public bool CanSave => !String.IsNullOrWhiteSpace(Title) 
                           && Items.Count >= 1
                           && Items.All(i => !String.IsNullOrWhiteSpace(i.Text));

    [RelayCommand(CanExecute = nameof(CanSave))]
    public async Task Save()
    {
        var previousTitle = _bingo.Title;
        var previousItems = _bingo.Items;
        
        try
        {
            _bingo.Title = Title;
            _bingo.Items = new ObservableCollection<BingoItem>(Items.Select(i => i.ToModel())); 
            await _bingoStore.SaveAsync(_bingo);
        }
        catch (Exception exception)
        {
            _bingo.Title = previousTitle;
            _bingo.Items = previousItems;
            
            return;
        }
        
        await _router.PushAsync<BingoListViewModel>();
        _router.ClearHistory(2);
    }
    
    [RelayCommand]
    public async Task Delete()
    {
        try
        {
            if (await _dialogService.ShowConfirmAsync("Are you sure you want to delete this bingo?"))
            {
                await _bingoStore.DeleteAsync(_bingo);
                await _router.PushAsync<BingoListViewModel>();
                _router.ClearHistory(2);
            }
        }
        catch (Exception exception)
        {
            return;
        }
    }
}
