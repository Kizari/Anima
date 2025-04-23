namespace Anima.Extensions.Console.Menu;

/// <summary>
/// Represents a menu item in a console application's menu/submenu.
/// </summary>
internal interface IMenuNode
{
    /// <summary>
    /// Display text for this menu item.
    /// </summary>
    string Name { get; }
}