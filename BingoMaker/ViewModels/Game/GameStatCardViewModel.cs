using System;
using System.Collections.Generic;
using System.Linq;

namespace BingoMaker.ViewModels.Game;

public record StatBar(string Label, int Count, bool HasItems);

public class GameStatCardViewModel : ViewModelBase
{
    public Models.Game Game { get; }

    public IReadOnlyList<StatBar> Bars { get; } = [];
    public bool HasChart => Bars.Any(bar => bar.Count > 0);
    public bool IsEmpty => Game.Items.All(x => x.MarkedAt is null);
    public string TotalDuration { get; } = string.Empty;
    public bool IsLessThanASecond { get;  }
    public bool IsGameFinished { get; } = true;
    
    public GameStatCardViewModel(Models.Game game)
    {
        Game = game;

        if (Game.FinishedAt == null)
        {
            IsGameFinished = false;
            return;
        }

        var finishedAt = Game.FinishedAt.Value;
        var totalDuration = finishedAt - Game.StartedAt;
        
        TotalDuration = totalDuration.ToString(@"hh\:mm\:ss");

        if (totalDuration.TotalSeconds < 1)
            IsLessThanASecond = true;

        var count = (int)Math.Clamp(
            Math.Floor(totalDuration.TotalSeconds), 1, 10);
        
        var bars = new List<StatBar>(count);
        
        for (var i = 0; i < count; i++)
        {
            var isLast = i == count - 1;
            
            var from = Game.StartedAt + totalDuration * ((double)i / count);
            var to = isLast
                ? finishedAt
                :Game.StartedAt + totalDuration * ((double)(i + 1) / count);
            
            var itemCount = Game.Items.Count(
                item => item.MarkedAt >= from 
                        && (isLast ? item.MarkedAt <= to : item.MarkedAt < to));
            
            var elapsedFrom = from - Game.StartedAt;
            var elapsedTo = to - Game.StartedAt;

            var fromLabel = $"{(int)elapsedFrom.TotalMinutes}:{elapsedFrom.Seconds:D2}";
            var toLabel = $"{(int)elapsedTo.TotalMinutes}:{elapsedTo.Seconds:D2}";

            var label = $"{fromLabel}–{toLabel}";
            
            bars.Add(
                new StatBar(label, itemCount, itemCount >0));
        }

        Bars = bars;
    }
}