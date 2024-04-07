using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Anima.Utilities.SourceGeneration;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Anima.EntityFrameworkCore.SourceGeneration;

/// <summary>
/// Automatically generates DbSet properties for the context marked with [GenerateDbSets]
/// from classes that inherit IEntityTypeConfiguration
/// </summary>
[Generator]
public sealed class DbSetGenerator : IncrementalClassGenerator
{
    /// <inheritdoc />
    protected override IEnumerable<StaticSourceDefinition> StaticSource => [new GenerateDbSetsAttribute()];

    /// <summary>
    /// Selects classes that are marked with [GenerateDbSets] or implement IEntityTypeConfiguration.
    /// </summary>
    /// <param name="declaration"></param>
    /// <returns></returns>
    protected override bool Predicate(ClassDeclarationSyntax declaration) =>
        declaration.HasAttribute(GenerateDbSetsAttribute.ShortName)
        || declaration.HasGenericInterface("IEntityTypeConfiguration");

    /// <summary>
    /// Generates the partial class matching the marked DbContext.
    /// </summary>
    protected override void BuildSource(
        SourceProductionContext context,
        Compilation compilation,
        ImmutableArray<ClassDefinition> definitions)
    {
        var dbContext = definitions
            .FirstOrDefault(d => d.Symbol.HasAttribute(GenerateDbSetsAttribute.Name));
        
        if (dbContext != null)
        {
            var builder = new SourceBuilder()
                .AppendLine($"namespace {dbContext.Namespace};")
                .AppendLine()
                .AppendLine($"public partial class {dbContext.Name}")
                .AppendLine('{')
                .Indent();

            foreach (var definition in definitions.Where(d => d.Symbol.BaseType?.Name != "DbContext"))
            {
                builder.AppendLine(
                    $"public DbSet<{definition.FullName}> {definition.Name.Pluralize()} {{ get; set; }}");
            }

            builder.Outdent()
                .AppendLine('}');

            context.AddSource(dbContext.FileName, builder.ToString());
        }
    }
}