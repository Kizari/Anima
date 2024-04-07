using System.Collections.Generic;
using Anima.Utilities.SourceGeneration;
using Microsoft.CodeAnalysis;

namespace Anima.DependencyInjection.SourceGeneration;

/// <summary>
/// Extended class definition specifically for service classes used with dependency injection.
/// </summary>
public class ServiceDefinition(INamedTypeSymbol symbol) : ClassDefinition(symbol)
{
    /// <summary>
    /// The fully qualified name of the service interface if one is implemented.
    /// </summary>
    public string? Interface { get; set; }
    
    /// <summary>
    /// The name of the ServiceLifetime enum member associated with this service.
    /// </summary>
    public string? Lifetime { get; set; }
    
    /// <summary>
    /// The fully qualified name of the concrete type of the service.
    /// </summary>
    public string? ImplementationType { get; set; }
    
    /// <summary>
    /// Names of all methods in the service class marked with [OnConstruct].
    /// </summary>
    public List<string> OnConstructMethods { get; set; } = [];
}