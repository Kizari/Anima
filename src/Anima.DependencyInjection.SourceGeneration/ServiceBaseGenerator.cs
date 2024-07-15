using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Anima.Utilities.SourceGeneration;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Anima.DependencyInjection.SourceGeneration;

/// <summary>
/// Generates constructors for abstract classes marked with [ServiceBase].
/// </summary>
[Generator]
public class ServiceBaseGenerator : IncrementalClassGenerator
{
    /// <inheritdoc/>
    protected override IEnumerable<StaticSourceDefinition> StaticSource => [new ServiceBaseAttribute()];

    /// <inheritdoc/>
    protected override bool Predicate(ClassDeclarationSyntax declaration) =>
        declaration.HasAttribute(ServiceBaseAttribute.ShortName);

    /// <inheritdoc/>
    protected override void BuildSource(
        SourceProductionContext context, 
        Compilation compilation, 
        ImmutableArray<ClassDefinition> definitions)
    {
        foreach (var definition in definitions.Where(d => d.IsAbstract))
        {
            var services = definition.Symbol.GetUnassignedPrivateAndProtectedReadonlyFields().ToArray();
            var onConstructs = definition.Symbol.GetMarkedMethods(OnConstructAttribute.Name).ToArray();
            
            var builder = new SourceBuilder()
                .AppendLine($"namespace {definition.Namespace};")
                .AppendLine()
                .AppendLine($"public partial class {definition.Name}")
                .AppendLine('{')
                .Indent()
                .AppendLine($"public {definition.Name}(")
                .Indent()
                .AppendLines(services, f => $"{f.Type.ToDisplayString()} {f.Name.Substring(1)}", ',')
                .Outdent()
                .AppendLine(')')
                .AppendLine('{')
                .Indent()
                .AppendLines(services, f => $"{f.Name} = {f.Name.Substring(1)};");
            
            if (onConstructs.Length != 0)
            {
                builder.AppendLine();
                builder.AppendLines(onConstructs, m => $"{m}();");
            }
            
            builder.Outdent()
                .AppendLine('}')
                .Outdent()
                .AppendLine('}');
        
            context.AddSource(definition.FileName, builder.ToString());
        }
    }
}