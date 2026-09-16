using Avalonia.Controls;

namespace BingoMaker.Routing;

public interface INavigationHost
{
    NavigationPage Root { get; }
}