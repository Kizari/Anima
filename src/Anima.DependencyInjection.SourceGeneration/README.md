# Anima.DependencyInjection.SourceGeneration

Source generators to reduce boilerplate code when working with Microsoft.Extensions.DependencyInjection.

## RegisterService

This feature eases the burden of manually adding services to the dependency injection container every time
a new service class is created. A constructor is also automatically generated for each service class
that accepts variables corresponding to each unassigned readonly field in said class.

### Usage

Set any service classes as `partial` and mark them with the `[RegisterService]` attribute to add them
to an automatically generated extension method for the current assembly.

## OnConstruct

If you need to run additional code inside the automatically generated constructor, you can simply mark
a method in the class with `[OnConstruct]` and it will run at the end of the constructor.

## Full Example

Given the following classes in a project named **MyClassLibrary**:

```csharp
[RegisterService(ServiceLifetime.Singleton)]
public partial class ServiceCounterService
{
    public int ServiceCount { get; set; }
}
```

```csharp
public interface IRepository<TEntity>;

[RegisterService(ServiceLifetime.Transient)]
public partial class UserRepository : IRepository<User>
{
    private readonly ServiceCounterService _serviceCounter;
    
    [OnConstruct]
    private void OnConstruct()
    {
        _serviceCounter.ServiceCount++;
    }
}
```

This output is generated:

```csharp
public static class MyClassLibraryServiceExtensions
{
    public static IServiceCollection AddMyClassLibrary(this IServiceCollection services)
    {
        services.AddSingleton<ServiceCounterService>();
        services.AddTransient<IRepository<User>, UserRepository>();
    }
}
```

```csharp
public partial class UserRepository
{
    public UserRepository(ServiceCounterService serviceCounter)
    {
        _serviceCounter = serviceCounter;
        
        OnConstruct();
    }
}
```

Then you simply call the extension method from the project that needs to consume your services:

```csharp
public ServiceProvider ConfigureServices()
{
    return new ServiceCollection()
        .AddMyClassLibrary()
        .BuildServiceProvider();
}
```