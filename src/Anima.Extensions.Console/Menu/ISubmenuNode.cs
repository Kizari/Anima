namespace Anima.Extensions.Console.Menu;

/// <summary>
/// Represents a menu/submenu in an <see cref="IConsoleMenu"/> tree.
/// </summary>
internal interface ISubmenuNode : IMenuNode
{
    /// <summary>
    /// Menu items that make up this submenu.
    /// </summary>
    List<IMenuNode> Children { get; }
}