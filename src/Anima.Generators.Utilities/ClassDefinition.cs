using Microsoft.CodeAnalysis;

namespace Anima.Generators.Utilities;

public class ClassDefinition(INamedTypeSymbol symbol)
{
    public INamedTypeSymbol Symbol { get; } = symbol;

    /// <summary>
    /// The namespace this class is a part of
    /// </summary>
    public string Namespace => Symbol.ContainingNamespace.ToDisplayString();

    /// <summary>
    /// The local type name of the class
    /// </summary>
    public string Name => Symbol.Name;

    /// <summary>
    /// The fully qualified name of the class (without assembly information)
    /// </summary>
    public string FullName => Symbol.ToDisplayString();

    /// <summary>
    /// The file name that can be given to source files generated for the class
    /// </summary>
    public string FileName => $"{FullName}.g.cs";

    /// <summary>
    /// True if this type or some containing type has generic type parameters
    /// </summary>
    public bool IsGenericType => Symbol.IsGenericType;

    /// <summary>
    /// The local type name without generic type parameters in it
    /// </summary>
    public string NameWithoutGenerics
    {
        get
        {
            var index = Name.IndexOf('<');
            return index > -1 ? Name.Substring(0, index) : Name;
        }
    }

    /// <summary>
    /// The fully qualified type name without generic type parameters in it
    /// </summary>
    public string FullNameWithoutGenerics
    {
        get
        {
            var index = FullName.IndexOf('<');
            return index > -1 ? FullName.Substring(0, index) : FullName;
        }
    }
}