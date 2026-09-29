using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using BingoMaker.ViewModels.Game;
using ScottPlot;
using ScottPlot.TickGenerators;

namespace BingoMaker.Views.Game;

public partial class GameStatCardView : UserControl
{
    public GameStatCardView()
    {
        InitializeComponent();
        StatsPlot.UserInputProcessor.Disable();
        
        DataContextChanged += OnDataContextChanged;
        UpdatePlot();
    }
    
    private void OnDataContextChanged(object? sender, EventArgs e)
    {
        UpdatePlot();
    }
    
    private void UpdatePlot()
    {
        var plot = StatsPlot.Plot;
        plot.Clear();
        
        plot.FigureBackground.Color = ScottPlot.Colors.Transparent;
        plot.HideGrid();

        var vm = DataContext as GameStatCardViewModel;

        if (vm != null)
        {
            var ticks = new Tick[vm.Bars.Count];

            for (var i = 0; i < vm.Bars.Count; i++)
            {
                var bar = vm.Bars[i];

                plot.Add.Bar(position: i, value: bar.Count);
                ticks[i] = new Tick(i, bar.Label);
            }

            plot.Axes.Bottom.TickGenerator = new NumericManual(ticks);
            plot.Axes.Margins(bottom: 0);
            plot.Axes.AutoScale();
            
            plot.Axes.Left.TickGenerator = new NumericAutomatic
            {
                IntegerTicksOnly = true
            };

            plot.Axes.Left.MinorTickStyle.Length = 0;
        }

        StatsPlot.Refresh();
    }
}