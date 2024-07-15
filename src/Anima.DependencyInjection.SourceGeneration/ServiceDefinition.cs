using System.Collections.Generic;
using System.Linq;
using Anima.Utilities.SourceGeneration;
using Microsoft.CodeAnalysis;

namespace Anima.DependencyInjection.SourceGeneration;

/// <summary>
/// Extended class definition specifically for service classes used with dependency injection.
/// </summary>
public class ServiceDefinition(INamedTypeSymbol symbol) : ClassDefinition(symbol)
{
    /// <summary>
    /// The fully qualified name of the service interfaces if implemented.
    /// </summary>
    public string[]? Interfaces { get; set; }
    
    /// <summary>
    /// Fully qualified name of the abstract base class if one exists.
    /// </summary>
    public string? AbstractBase { get; set; }
    
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
    public List<string>? OnConstructMethods { get; set; }

    /// <summary>
    /// True if the class is to be injected as an implementation of an abstract class.
    /// </summary>
    public bool HasAbstractBase => Interfaces?.Any() != true && AbstractBase != null;
    
    /// <summary>
    /// Fields in the type that represent injected dependencies.
    /// </summary>
    public IFieldSymbol[]? ServiceFields { get; set; }
    
    /// <summary>
    /// Fields in the base type that represent injected dependencies.
    /// </summary>
    public IFieldSymbol[]? BaseServiceFields { get; set; }
}