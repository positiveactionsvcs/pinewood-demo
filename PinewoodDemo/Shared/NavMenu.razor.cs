namespace PinewoodDemo.Shared;

/// <summary>
/// Navigation menu.
/// </summary>
public partial class NavMenu
{
    private bool _collapseNavMenu = true;

    private string? NavMenuCssClass => _collapseNavMenu ? "collapse" : null;

    private void ToggleNavMenu()
    {
        _collapseNavMenu = !_collapseNavMenu;
    }
}