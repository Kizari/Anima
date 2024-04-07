using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using Anima.Generators.Utilities;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Neurodex.Generators.DependencyInjection;

namespace Anima.Generators.DependencyInjection;

[Generator]
public class RegisterServiceGenerator : IncrementalClassGenerator<ServiceDefinition>
{
    protected override IEnumerable<StaticSourceDefinition> StaticSource => [new RegisterServiceAttribute()];

    protected override bool Predicate(ClassDeclarationSyntax declaration) =>
        declaration.HasAttribute(RegisterServiceAttribute.ShortName);

    protected override ServiceDefinition Transform(
        GeneratorSyntaxContext context,
        CancellationToken cancellationToken)
    {
        var symbol = (INamedTypeSymbol)context.SemanticModel.GetDeclaredSymbol(context.Node)!;
        var dummy = new ClassDefinition(symbol);
        var attribute = symbol.GetAttributes()
            .FirstOrDefault(a => a.AttributeClass!.Name == RegisterServiceAttribute.Name);

        return new ServiceDefinition(symbol)
        {
            Interface = symbol.AllInterfaces.FirstOrDefault(i => i.ToDisplayString()
                    .Split('.').Last().Substring(1) == dummy.NameWithoutGenerics)
                ?.ToDisplayString(), // Get interface that matches class name less the 'I' prefix
            ImplementationType = symbol.IsGenericType
                ? $"{dummy.FullNameWithoutGenerics}<>"
                : dummy.FullName,
            Lifetime =
                (int)(attribute?.ConstructorArguments[0].Value ?? 2) switch
                {
                    0 => "Singleton",
                    1 => "Scoped",
                    2 => "Transient",
                    _ => throw new NotSupportedException("Service lifetime not supported.")
                },
            IsViewModel = symbol.BaseType?.Name is "ViewModelBase" or "ReactiveObject",
            OnConstructMethods = symbol.GetMembers()
                .OfType<IMethodSymbol>()
                .Where(m => m.HasAttribute(OnConstructAttribute.Name))
                .Select(m => m.Name)
                .ToList()
        };
    }

    protected override void BuildSource(
        SourceProductionContext context,
        Compilation compilation,
        ImmutableArray<ServiceDefinition> definitions)
    {
        if (!definitions.Any())
        {
            return;
        }

        foreach (var definition in definitions.Where(d => !d.IsViewModel))
        {
            BuildConstructors(context, definition);
        }

        const string name = "AnimaServiceExtensions";

        var assemblyName = compilation.AssemblyName!
            .Replace(".", "")
            .Replace(" ", "")
            .Replace("_", "");

        var builder = new SourceBuilder()
            .AppendLine("namespace Microsoft.Extensions.DependencyInjection;")
            .AppendLine()
            .AppendLine($"public static class {name}")
            .AppendLine('{')
            .Indent()
            .AppendLine($"public static IServiceCollection Add{assemblyName}(this IServiceCollection services)")
            .AppendLine('{')
            .Indent();

        foreach (var definition in definitions)
        {
            builder.Append($"services.Add{definition.Lifetime}<");

            if (definition.Interface != null)
            {
                builder.Append($"{definition.Interface}, ");
            }

            builder.AppendLine($"{definition.ImplementationType}>();");
        }

        builder
            .AppendLine()
            .AppendLine("return services;")
            .Outdent()
            .AppendLine('}')
            .Outdent()
            .AppendLine('}');

        context.AddSource($"{name}.g.cs", builder.ToString());
    }

    private void BuildConstructors(SourceProductionContext context, ServiceDefinition definition)
    {
        var fields = definition.Symbol.GetMembers()
            .OfType<IFieldSymbol>()
            .Where(f => f.IsReadOnly
                        && f is {DeclaredAccessibility: Accessibility.Private, IsImplicitlyDeclared: false}
                        && f.DeclaringSyntaxReferences[0].GetSyntax()
                            is not VariableDeclaratorSyntax {Initializer: not null})
            .ToArray();

        if (fields.Length == 0)
        {
            return;
        }
        
        var builder = new SourceBuilder()
            .AppendLine($"namespace {definition.Namespace};")
            .AppendLine()
            .AppendLine($"public partial class {definition.Name}")
            .AppendLine('{')
            .Indent()
            .AppendLine($"public {definition.Name}(")
            .Indent()
            .AppendLines(fields, f => $"{f.Type.ToDisplayString()} {f.Name.Substring(1)}", ',')
            .Outdent()
            .AppendLine(')');
        
        builder.AppendLine('{')
            .Indent();

        foreach (var field in fields)
        {
            builder.AppendLine($"{field.Name} = {field.Name.Substring(1)};");
        }

        if (definition.OnConstructMethods.Count > 0)
        {
            builder.AppendLine();
            builder.AppendLines(definition.OnConstructMethods, m => $"{m}();");
        }
        
        builder.Outdent()
            .AppendLine('}')
            .Outdent()
            .AppendLine('}');
        
        context.AddSource(definition.FileName, builder.ToString());
    }
}