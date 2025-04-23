namespace Anima.Extensions.Console.Menu;

/// <inheritdoc />
internal class SubmenuNode(string name) : ISubmenuNode
{
    /// <inheritdoc />
    public string Name { get; } = name;

    /// <inheritdoc />
    public List<IMenuNode> Children { get; } = [];
}