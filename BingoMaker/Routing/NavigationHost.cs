using System;
using Avalonia.Controls;

namespace BingoMaker.Routing;

public sealed class NavigationHost : INavigationHost
{
    private NavigationPage? _root;
    
    public NavigationPage Root => _root
        ?? throw new InvalidOperationException("NavigationPage is not set.");
    
    public void Attach(NavigationPage root) => _root = root;
}