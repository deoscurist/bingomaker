using System;
using BingoMaker.Models;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BingoMaker.ViewModels.Game;

public partial class GameItemViewModel : ObservableObject
{
    public GameItem Model { get; }
    
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsMarked))]
    public partial DateTime? MarkedAt { get; set; }
    
    public bool IsMarked => MarkedAt.HasValue;

    public GameItemViewModel(GameItem model)
    {
        Model = model;

        MarkedAt = model.MarkedAt;
    }

    public void ToggleMark()
    {
        MarkedAt = IsMarked ? null : DateTime.Now;
    }
}