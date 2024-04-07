namespace Anima.Utilities.SourceGeneration;

/// <summary>
/// Helper methods for building marker attribute source.
/// </summary>
public static class AttributeHelper
{
    /// <summary>
    /// Builds source code for a marker attribute.
    /// </summary>
    /// <param name="attributeName">The class name of the marker attribute.</param>
    /// <param name="namespaceName">The namespace to scope the marker attribute to.</param>
    /// <param name="description">Description of what the attribute does for the XML doc string.</param>
    /// <param name="attributeTargets">The targets the marker attribute can be applied to.</param>
    /// <param name="allowMultiple">
    /// Whether or not the same member can have multiple instances of the marker attribute.
    /// </param>
    /// <returns>String representation of the source code.</returns>
    public static string CreateSource(string attributeName,
        string namespaceName,
        string? description = null,
        AttributeTargets attributeTargets = AttributeTargets.All,
        bool allowMultiple = false) => CreateSourceBuilder(attributeName,
        namespaceName, description, attributeTargets, allowMultiple).AppendLine("{}").ToString();

    /// <summary>
    /// Builds source code for a marker attribute.
    /// </summary>
    /// <param name="attributeName">The class name of the marker attribute.</param>
    /// <param name="namespaceName">The namespace to scope the marker attribute to.</param>
    /// <param name="description">Description of what the attribute does for the XML doc string.</param>
    /// <param name="attributeTargets">The targets the marker attribute can be applied to.</param>
    /// <param name="allowMultiple">
    /// Whether or not the same member can have multiple instances of the marker attribute.
    /// </param>
    /// <returns>
    /// Source builder populated with the incomplete marker attribute source.
    /// The source ends with "internal class {attributeName} : Attribute" so further changes should carry on from there.
    /// </returns>
    public static SourceBuilder CreateSourceBuilder(string attributeName,
        string namespaceName,
        string? description = null,
        AttributeTargets attributeTargets = AttributeTargets.All,
        bool allowMultiple = false)
    {
        var builder = new SourceBuilder()
            .AppendLine("using System;")
            .AppendLine()
            .AppendLine($"namespace {namespaceName};")
            .AppendLine();

        if (description != null)
        {
            builder
                .AppendLine("/// <summary>")
                .AppendLine($"/// {description}")
                .AppendLine("/// </summary>");
        }

        var targets = attributeTargets.Split().Select(t => $"AttributeTargets.{t}");
        return builder
            .AppendLine(
                $"[AttributeUsage({string.Join(" | ", targets)}, AllowMultiple = {allowMultiple.ToString().ToLower()})]")
            .AppendLine($"internal class {attributeName} : Attribute");
    }
}