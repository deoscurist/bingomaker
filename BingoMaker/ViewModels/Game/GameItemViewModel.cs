using System;
using BingoMaker.Models;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BingoMaker.ViewModels.Game;

public class GameItemViewModel : ObservableObject
{
    public GameItem Model { get; }

    public GameItemViewModel(GameItem model)
    {
        Model = model;
    }

    public DateTime? MarkedAt
    {
        get => Model.MarkedAt;
        set
        {
            if (Model.MarkedAt == value)
                return;

            Model.MarkedAt = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(IsMarked));
        }
    }

    public bool IsMarked => MarkedAt.HasValue;

    public void ToggleMark()
    {
        MarkedAt = IsMarked ? null : DateTime.Now;
    }
}