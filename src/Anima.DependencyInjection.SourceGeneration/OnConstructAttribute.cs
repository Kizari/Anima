using System;
using Anima.Utilities.SourceGeneration;

namespace Anima.DependencyInjection.SourceGeneration;

/// <summary>
/// Marker attribute for methods that are to be executed at the end of the automatically generated constructor.
/// </summary>
public class OnConstructAttribute : StaticSourceDefinition
{
    /// <summary>
    /// The short name for the attribute.
    /// </summary>
    private const string ShortName = "OnConstruct";
    
    /// <summary>
    /// The class name for the attribute.
    /// </summary>
    public const string Name = $"{ShortName}Attribute";
    
    /// <inheritdoc/>
    public override string Source => AttributeHelper.CreateSource(Name,
        "Microsoft.Extensions.DependencyInjection",
        "Methods marked with this attribute will be executed at the end of the generated constructor.",
        AttributeTargets.Method);
}