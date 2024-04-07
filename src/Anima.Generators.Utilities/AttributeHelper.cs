namespace Anima.Generators.Utilities;

public static class AttributeHelper
{
    public static string CreateSource(string attributeName,
        string? description = null,
        string? nameSpace = "Anima",
        AttributeTargets attributeTargets = AttributeTargets.All,
        bool allowMultiple = false) => CreateSourceBuilder(attributeName,
        description, nameSpace, attributeTargets, allowMultiple).AppendLine("{}").ToString();

    public static SourceBuilder CreateSourceBuilder(string attributeName,
        string? description = null,
        string? nameSpace = "Anima",
        AttributeTargets attributeTargets = AttributeTargets.All,
        bool allowMultiple = false)
    {
        var builder = new SourceBuilder()
            .AppendLine("using System;")
            .AppendLine()
            .AppendLine($"namespace {nameSpace};")
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