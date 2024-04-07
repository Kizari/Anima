using System.Collections.Generic;
using Anima.Generators.Utilities;
using Microsoft.CodeAnalysis;

namespace Anima.Generators.DependencyInjection;

public class ServiceDefinition(INamedTypeSymbol symbol) : ClassDefinition(symbol)
{
    public string? Interface { get; set; }
    public string? Lifetime { get; set; }
    public string? ImplementationType { get; set; }
    public bool IsViewModel { get; set; }
    public List<string> OnConstructMethods { get; set; } = [];
}