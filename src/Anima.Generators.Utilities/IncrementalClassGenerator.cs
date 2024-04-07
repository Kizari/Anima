using System.Collections.Immutable;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace Anima.Generators.Utilities;

public abstract class IncrementalClassGenerator : IncrementalClassGenerator<ClassDefinition>;

public abstract class IncrementalClassGenerator<TDefinition> : IIncrementalGenerator
    where TDefinition : ClassDefinition
{
    /// <summary>
    /// Source code in this dictionary will be added to the compilation automatically.
    /// </summary>
    protected virtual IEnumerable<StaticSourceDefinition> StaticSource => [];

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        // Add any static source to the compilation
        if (StaticSource.Any())
        {
            context.RegisterPostInitializationOutput(c =>
            {
                foreach (var kvp in StaticSource)
                {
                    c.AddSource(kvp.FileName, SourceText.From(kvp.Source, Encoding.UTF8));
                }
            });
        }

        // Collect the filtered classes
        var filteredClasses = context.SyntaxProvider
            .CreateSyntaxProvider(Predicate, Transform)
            .Collect();

        // Register the source generator
        context.RegisterSourceOutput(
            context.CompilationProvider.Combine(filteredClasses),
            (c, t) => BuildSource(c, t.Left, t.Right));
    }

    private bool Predicate(SyntaxNode node, CancellationToken cancellationToken) =>
        node is ClassDeclarationSyntax declaration && Predicate(declaration);

    protected virtual TDefinition Transform(GeneratorSyntaxContext context, CancellationToken cancellationToken)
    {
        var symbol = (INamedTypeSymbol)context.SemanticModel.GetDeclaredSymbol(context.Node)!;
        return (TDefinition)Activator.CreateInstance(typeof(TDefinition), [symbol]);
    }

    protected abstract bool Predicate(ClassDeclarationSyntax declaration);

    protected abstract void BuildSource(
        SourceProductionContext context,
        Compilation compilation,
        ImmutableArray<TDefinition> definitions);
}