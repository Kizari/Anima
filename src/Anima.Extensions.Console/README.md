# Anima.Extensions.Console

Utilities for quickly creating console applications with `Microsoft.Extensions.DependencyInjection`.

## Usage

The following sections explain how to create a basic console application using this library.

### 1. Create an application menu

Add console commands to your application by implementing `IConsoleCommand` or `IAsyncConsoleCommand` into
any classes you like.

```csharp
public class HelloWorldCommand : IConsoleCommand
{
    public string Path => "Print Commands/Print Hello World";
    
    public string Description => "Displays the words \"Hello World\" in the console.";
    
    public void Execute()
    {
        Console.WriteLine("Hello, World!");
    }
}
```

The above example will add the `Print Hello World` command to a menu named `Print Commands`, which when selected,
will write `Hello, World!` to the console.

### 2. Set up the application

The following example uses a top-level statements `Program.cs`, but this will work with any setup of
console application that has a configurable `IServiceCollection`.

First, call the `AddAnimaConsole` extension method to add necessary services to the service container.

```csharp
var services = new ServiceCollection()
    .AddAnimaConsole()
    .BuildServiceProvider();
```

Resolve `IConsoleApplicationBuilderFactory` from the service provider and create a new builder instance.

```csharp
var factory = services.GetRequiredService<IConsoleApplicationBuilderFactory>();
var builder = factory.Create();
```

Build the application. This example adds a red application name title when the app starts, then builds the app instance.

```csharp
var application = builder
    .WithTitle("My Console Application", ConsoleColor.Red)
    .Build();
```

### 3. Run the application

There are two ways to do this.

```csharp
await application.RunAsync();
```

This above will run the application indefinitely, until the user selects the `Exit` option from the root application
menu. The exit command is added to the end of the root menu automatically by this library.

Alternatively, the menu can be run through just once before returning control back to the application.

```csharp
await application.RunOnceAsync();
```

Once a command is selected, it will be executed, then the application will close if there is nothing after this line.