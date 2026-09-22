using System.Reflection;
using Avalonia.Controls;

namespace BingoMaker.Views.Windows;

public partial class AboutWindow : Window
{
    public string Version { get; } =
        "v" + (Assembly.GetExecutingAssembly()
            .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion ?? "0.0.0");
    
    public AboutWindow()
    {
        InitializeComponent();
    }
}