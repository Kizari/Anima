using System;
using Anima.Utilities.SourceGeneration;

namespace Anima.DependencyInjection.SourceGeneration;

/// <summary>
/// Marks an abstract class for automatic constructor creation of private and protected readonly fields.
/// </summary>
public class ServiceBaseAttribute : StaticSourceDefinition
{
    /// <summary>
    /// The short name for the attribute.
    /// </summary>
    public const string ShortName = "ServiceBase";
    
    /// <summary>
    /// The class name for the attribute.
    /// </summary>
    public const string Name = $"{ShortName}Attribute";

    /// <inheritdoc/>
    public override string Source => AttributeHelper.CreateSource(Name,
        "Microsoft.Extensions.DependencyInjection",
        "Abstract classes marked with this attribute will have a constructor generated automatically.",
        AttributeTargets.Class);
}