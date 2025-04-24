using System.Reflection;
using Anima.Extensions.Console.Command;
using Anima.Extensions.Console.Utilities;
using Microsoft.Extensions.DependencyInjection;

namespace Anima.Extensions.Console.Menu;

/// <inheritdoc />
internal class ConsoleMenu(IServiceProvider provider) : IConsoleMenu
{
    /// <summary>
    /// User-defined <see cref="ExitCommand"/> that will override the default one if present.
    /// </summary>
    internal ExitCommand? CustomExitCommand { get; set; }
    
    /// <inheritdoc />
    public async Task<bool> RunAsync()
    {
        // Initialize menu variables
        IMenuNode? selected = null;
        var current = CreateMenuTree();

        // Run through the menu until the user selects a command
        while (selected == null)
        {
            System.Console.WriteLine();
            
            // Render each item in the current submenu
            for (var i = 1; i <= current.Count; i++)
            {
                // Display the menu item
                var node = current[i - 1];
                var prefix = node is ICommandNode ? "[COMMAND]" : "[SUBMENU]";
                ConsoleWriter.WriteLine($"{i}. {prefix} {node.Name}", 
                    node is ICommandNode ? ConsoleColor.Green : ConsoleColor.Blue);
                
                // Display the description
                if (node is ICommandNode commandNode)
                {
                    // Indent description
                    for (var j = 0; j < i.ToString().Length + 2; j++)
                    {
                        System.Console.Write(' ');
                    }
            
                    ConsoleWriter.WriteLine(commandNode.Command.Description, ConsoleColor.DarkGray);
                }
            }
            
            // Wait for a selection
            System.Console.WriteLine();
            System.Console.Write("Enter your selection: ");
            var response = System.Console.ReadLine();
            System.Console.WriteLine();
            
            // Process the selection
            if (int.TryParse(response, out var result))
            {
                if (result < 1 || result > current.Count)
                {
                    ConsoleWriter.WriteLine($"Please enter a number between 1 and {current.Count}.", 
                        ConsoleColor.Yellow);
                }
                else
                {
                    var selection = current[result - 1];
                    if (selection is ISubmenuNode submenu)
                    {
                        current = submenu.Children;
                    }
                    else
                    {
                        selected = selection;
                    }
                }
            }
            else
            {
                ConsoleWriter.WriteLine(
                    $"{response} is not a valid option. Please enter a number from the menu and try again.",
                    ConsoleColor.Yellow);
            }
        }
        
        // Handle the selected node
        if (selected is ICommandNode command)
        {
            switch (command.Command)
            {
                case ExitCommand:
                    return true; // Signal caller that exit option was selected
                case IConsoleCommand syncCommand:
                    syncCommand.Execute();
                    break;
                case IAsyncConsoleCommand asyncCommand:
                    await asyncCommand.ExecuteAsync();
                    break;
            }
        }
        else
        {
            throw new NotSupportedException($"Unsupported menu node \"{selected.Name}\"" +
                                            $" of type {selected.GetType().FullName}");
        }

        return false; // Signal caller that exit option was not selected
    }
    
    /// <summary>
    /// Creates the menu tree for the console application by reflecting all types across all loaded
    /// assemblies and instantiating those that implement <see cref="IConsoleCommandBase"/>.
    /// </summary>
    /// <returns>List of menu nodes in the root menu of the menu tree.</returns>
    private List<IMenuNode> CreateMenuTree()
    {
        var root = new List<IMenuNode>();
        
        // Iterate each available console command
        foreach (var command in GetAllCommands())
        {
            // Split the path into tokens
            var current = root;
            var tokens = command.Path.Split('/');

            // Iterate each path token
            for (var i = 0; i < tokens.Length; i++)
            {
                // Check if this token represents a submenu
                if (i < tokens.Length - 1)
                {
                    // Create the submenu node for this token if it doesn't exist
                    var node = current.FirstOrDefault(n => n.Name == tokens[i]);
                    if (node == null)
                    {
                        node = new SubmenuNode(tokens[i]);
                        current.Add(node);
                    }

                    // Move to the next submenu node
                    current = ((SubmenuNode)node).Children;
                }
                // Check if this token represents a command
                else
                {
                    current.Add(new CommandNode(command));
                }
            }
        }

        root.Add(new CommandNode(CustomExitCommand ?? new ExitCommand(null, null, null)));
        return root;
    }

    /// <summary>
    /// Creates a new instance of every class in the consuming application and its
    /// dependencies that implements <see cref="IConsoleCommandBase"/>.
    /// </summary>
    /// <returns>List that contains the new instance of each unique command class.</returns>
    private List<IConsoleCommandBase> GetAllCommands()
    {
        var results = new List<IConsoleCommandBase>();
        
        // Iterate each loaded assembly
        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            try
            {
                // Instantiate each class that implements IConsoleCommandBase
                results.AddRange(assembly.GetTypes()
                    .Where(t => !t.IsAbstract
                                && t.IsAssignableTo(typeof(IConsoleCommandBase))
                                && t != typeof(ExitCommand))
                    .Select(type => (IConsoleCommandBase)ActivatorUtilities.CreateInstance(provider, type)));
            }
            catch (ReflectionTypeLoadException)
            {
                // Assembly was seemingly incompatible with reflection in some capacity, so simply skip it
                // by ignoring this exception, as it should not prevent execution and does not need further action
            }
        }

        return results;
    }

    /// <summary>
    /// Default command that allows the user to break out of this menu.
    /// </summary>
    internal class ExitCommand(string? path, string? description, Action? onSelected) : IConsoleCommand
    {
        /// <inheritdoc />
        public string Path => path ?? "Exit";
        
        /// <inheritdoc />
        public string Description => description ?? "Closes the application.";

        /// <inheritdoc />
        public void Execute()
        {
            onSelected?.Invoke();
        }
    }
}