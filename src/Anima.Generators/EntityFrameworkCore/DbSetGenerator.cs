using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Anima.Generators.Utilities;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Anima.Generators.EntityFrameworkCore;

[Generator]
public class DbSetGenerator : IncrementalClassGenerator
{
    protected override IEnumerable<StaticSourceDefinition> StaticSource => [new GenerateDbSetsAttribute()];

    protected override bool Predicate(ClassDeclarationSyntax declaration) =>
        declaration.HasBaseClass("DbContext")
        || declaration.HasGenericInterface("IEntityTypeConfiguration");

    protected override void BuildSource(
        SourceProductionContext context,
        Compilation compilation,
        ImmutableArray<ClassDefinition> definitions)
    {
        var dbContext = definitions.FirstOrDefault(d => d.Symbol.BaseType?.Name == "DbContext");
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