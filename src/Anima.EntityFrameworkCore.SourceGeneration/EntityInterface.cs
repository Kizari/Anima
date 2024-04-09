using Anima.Utilities.SourceGeneration;

namespace Anima.EntityFrameworkCore.SourceGeneration;

/// <summary>
/// Static source definition for the IEntity interface.
/// This is a marker interface for entity classes that don't need to implement IEntityTypeConfiguration.
/// </summary>
public class EntityInterface : StaticSourceDefinition
{
    /// <summary>
    /// The type name for the interface.
    /// </summary>
    public const string Name = "IEntity";

    /// <summary>
    /// The containing namespace for the interface.
    /// </summary>
    public const string Namespace = "Anima.EntityFrameworkCore";

    /// <inheritdoc/>
    public override string Source => new SourceBuilder()
        .AppendLine("using System;")
        .AppendLine()
        .AppendLine($"namespace {Namespace};")
        .AppendLine()
        .AppendLine("/// <summary>")
        .AppendLine("/// Implement this interface into an entity class to have a DbSet generated for it automatically.")
        .AppendLine("/// </summary>")
        .AppendLine("public interface IEntity;")
        .ToString();
}