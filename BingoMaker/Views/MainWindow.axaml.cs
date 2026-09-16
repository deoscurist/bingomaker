using Avalonia.Controls;
using BingoMaker.Design;

namespace BingoMaker.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        
        if (Avalonia.Controls.Design.IsDesignMode)
        {
            NavigationRoot.Content = new StartupView
            {
                DataContext = DesignData.Startup
            };
        }
    }

    public NavigationPage Navigation => NavigationRoot;
}