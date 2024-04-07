using System;
using Anima.Utilities.SourceGeneration;

namespace Anima.EntityFrameworkCore.SourceGeneration;

/// <summary>
/// Static source definition for the [GenerateDbSets] attribute.
/// </summary>
public class GenerateDbSetsAttribute : StaticSourceDefinition
{
    /// <summary>
    /// The short name for the attribute.
    /// </summary>
    public const string ShortName = "GenerateDbSets";
    
    /// <summary>
    /// The class name for the attribute.
    /// </summary>
    public const string Name = $"{ShortName}Attribute";
    
    /// <inheritdoc/>
    public override string Source => AttributeHelper.CreateSource(Name,
        "Microsoft.EntityFrameworkCore",
        "Mark a DbContext implementation with this attribute to have DbSet properties generated " +
        "for each entity in the solution automatically.",
        attributeTargets: AttributeTargets.Class);
}