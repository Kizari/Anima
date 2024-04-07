using System.Collections.Immutable;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace Anima.Utilities.SourceGeneration;

/// <summary>
/// Non-generic implementation of <see cref="IncrementalClassGenerator{TDefinition}"/>
/// </summary>
public abstract class IncrementalClassGenerator : IncrementalClassGenerator<ClassDefinition>;

/// <summary>
/// Base class that simplifies creating source generators for partial classes.
/// </summary>
/// <typeparam name="TDefinition">
/// The class definition type that the <see cref="Transform"/> method will output.
/// </typeparam>
public abstract class IncrementalClassGenerator<TDefinition> : IIncrementalGenerator
    where TDefinition : ClassDefinition
{
    /// <summary>
    /// Source code in this dictionary will be added to the compilation automatically.
    /// </summary>
    protected virtual IEnumerable<StaticSourceDefinition> StaticSource => [];

    /// <inheritdoc />
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

    /// <summary>
    /// Transforms the filtered class symbol into a <see cref="ClassDefinition"/>.
    /// Override this if you need custom transformation logic.
    /// </summary>
    /// <param name="context">The generator syntax context.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The transformed <see cref="ClassDefinition"/>.</returns>
    protected virtual TDefinition Transform(GeneratorSyntaxContext context, CancellationToken cancellationToken)
    {
        var symbol = (INamedTypeSymbol)context.SemanticModel.GetDeclaredSymbol(context.Node)!;
        return (TDefinition)Activator.CreateInstance(typeof(TDefinition), [symbol]);
    }

    /// <summary>
    /// The predicate function used to determine if classes match this generator.
    /// </summary>
    /// <param name="declaration">The class declaration to check.</param>
    /// <returns>True if the class should be matched by this generator.</returns>
    protected abstract bool Predicate(ClassDeclarationSyntax declaration);

    /// <summary>
    /// Implement this method to build source code and add it to the context.
    /// </summary>
    /// <param name="context">The source production context.</param>
    /// <param name="compilation">The active compilation.</param>
    /// <param name="definitions">The class definitions output by the <see cref="Transform"/> method.</param>
    protected abstract void BuildSource(
        SourceProductionContext context,
        Compilation compilation,
        ImmutableArray<TDefinition> definitions);
}