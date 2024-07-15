using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using Anima.Utilities.SourceGeneration;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Anima.DependencyInjection.SourceGeneration;

/// <summary>
/// Generates extension method for service container and constructors for service classes marked with [RegisterService].
/// </summary>
[Generator]
public class RegisterServiceGenerator : IncrementalClassGenerator<ServiceDefinition>
{
    /// <inheritdoc/>
    protected override IEnumerable<StaticSourceDefinition> StaticSource =>
    [
        new RegisterServiceAttribute(),
        new OnConstructAttribute()
    ];

    /// <inheritdoc/>
    protected override bool Predicate(ClassDeclarationSyntax declaration) =>
        declaration.HasAttribute(RegisterServiceAttribute.ShortName);

    /// <inheritdoc/>
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
            // Get interfaces whose name is contained within the type (e.g. UserRepository will match IRepository).
            Interfaces = symbol.AllInterfaces
                .Where(i =>
                {
                    var dummy2 = new ClassDefinition(i);
                    return dummy.NameWithoutGenerics
                        .Contains(dummy2.NameWithoutGenerics.Substring(1));
                })
                .Select(s => s.ToDisplayString())
                .ToArray(),
            AbstractBase = symbol.BaseType?.IsAbstract == true ? symbol.BaseType.ToDisplayString() : null,
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
            OnConstructMethods = symbol.GetMembers()
                .OfType<IMethodSymbol>()
                .Where(m => m.HasAttribute(OnConstructAttribute.Name))
                .Select(m => m.Name)
                .ToList(),
            ServiceFields = symbol.GetUnassignedPrivateReadonlyFields().ToArray()
        };
    }

    /// <inheritdoc/>
    protected override void BuildSource(
        SourceProductionContext context,
        Compilation compilation,
        ImmutableArray<ServiceDefinition> definitions)
    {
        if (!definitions.Any())
        {
            return;
        }

        foreach (var definition in definitions)
        {
            BuildConstructors(context, definition);
        }

        var assemblyName = compilation.AssemblyName!
            .Replace(".", "")
            .Replace(" ", "")
            .Replace("_", "");

        var builder = new SourceBuilder()
            .AppendLine("namespace Microsoft.Extensions.DependencyInjection;")
            .AppendLine()
            .AppendLine($"public static class {assemblyName}ServiceExtensions")
            .AppendLine('{')
            .Indent()
            .AppendLine($"public static IServiceCollection Add{assemblyName}(this IServiceCollection services)")
            .AppendLine('{')
            .Indent();

        foreach (var definition in definitions)
        {
            if (definition.Interfaces?.Any() == true)
            {
                foreach (var interfaceType in definition.Interfaces)
                {
                    builder.AppendLine(
                        $"services.Add{definition.Lifetime}<{interfaceType}, {definition.ImplementationType}>();");
                }
            }
            else
            {
                builder.AppendLine(definition.AbstractBase == null
                    ? $"services.Add{definition.Lifetime}<{definition.ImplementationType}>();"
                    : $"services.Add{definition.Lifetime}<{definition.AbstractBase}, {definition.ImplementationType}>();");
            }
        }

        builder
            .AppendLine()
            .AppendLine("return services;")
            .Outdent()
            .AppendLine('}')
            .Outdent()
            .AppendLine('}');

        context.AddSource($"{assemblyName}ServiceExtensions.g.cs", builder.ToString());
    }

    private void BuildConstructors(SourceProductionContext context, ServiceDefinition definition)
    {
        if (definition.ServiceFields?.Length < 1)
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
            .Indent();

        if (definition.HasAbstractBase)
        {
            definition.BaseServiceFields = definition.Symbol.BaseType!
                .GetUnassignedPrivateAndProtectedReadonlyFields()
                .ToArray();
            
            if (definition.ServiceFields?.Length < 1)
            {
                builder.AppendLines(definition.BaseServiceFields, 
                    f => $"{f.Type.ToDisplayString()} {f.Name.Substring(1)}", ',');
            }
            else
            {
                builder.AppendLines(definition.BaseServiceFields, 
                    f => $"{f.Type.ToDisplayString()} {f.Name.Substring(1)},");
            }
        }
        
        builder.AppendLines(definition.ServiceFields!, 
                f => $"{f.Type.ToDisplayString()} {f.Name.Substring(1)}", ',')
            .Outdent()
            .AppendLine(')');

        if (definition.HasAbstractBase)
        {
            builder.AppendLine(" : base")
                .AppendLine('(')
                .Indent()
                .AppendLines(definition.BaseServiceFields!, f => f.Name.Substring(1), ',')
                .Outdent()
                .AppendLine(')');
        }
        
        builder.AppendLine('{')
            .Indent();

        foreach (var field in definition.ServiceFields!)
        {
            builder.AppendLine($"{field.Name} = {field.Name.Substring(1)};");
        }

        if (definition.OnConstructMethods?.Count > 0)
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