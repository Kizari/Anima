using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Pluralize.NET;

namespace Anima.Utilities.SourceGeneration;

/// <summary>
/// Extension methods to assist with source generation.
/// </summary>
public static class Extensions
{
    /// <summary>
    /// Checks if a <see cref="ClassDeclarationSyntax"/> implements an interface with the given name.<br/>
    /// Does not work with interfaces that have type parameters—use <see cref="HasGenericInterface"/> instead.
    /// </summary>
    /// <param name="classDeclaration">The class declaration syntax to check for the matching interface.</param>
    /// <param name="interfaceName">The local name of the interface to check for.</param>
    /// <returns>True if the interface is implemented in the class.</returns>
    public static bool HasInterface(this ClassDeclarationSyntax classDeclaration, string interfaceName)
    {
        if (classDeclaration.BaseList != null)
        {
            var baseTypes = classDeclaration.BaseList.Types.Select(t => t.Type);
            return baseTypes.Any(t => t.ToString() == interfaceName);
        }

        return false;
    }

    /// <summary>
    /// Checks if a <see cref="ClassDeclarationSyntax"/> implements a generic interface with the given name.
    /// </summary>
    /// <param name="classDeclaration">The class declaration syntax to check for the matching interface.</param>
    /// <param name="interfaceName">The local name of the interface to check for.</param>
    /// <returns>True if the generic interface is implemented in the class.</returns>
    public static bool HasGenericInterface(this ClassDeclarationSyntax classDeclaration, string interfaceName)
    {
        if (classDeclaration.BaseList != null)
        {
            return classDeclaration.BaseList.Types.Any(t =>
            {
                var name = t.ToString();
                var index = name.IndexOf('<');
                if (index < 0)
                {
                    return false;
                }

                var nameToCheck = name.Substring(0, index);
                return nameToCheck == interfaceName;
            });
        }

        return false;
    }

    /// <summary>
    /// Checks if a <see cref="MemberDeclarationSyntax"/> is decorated with an attribute of the given name.<br/>
    /// Does not work with attributes that have type parameters—use <see cref="HasGenericAttribute"/> instead.
    /// </summary>
    /// <param name="syntax">The member declaration syntax to check for the attribute.</param>
    /// <param name="attributeName">The short name (without "Attribute" suffix) of the attribute to check for.</param>
    /// <returns>True if the attribute is present on the member.</returns>
    public static bool HasAttribute(this MemberDeclarationSyntax syntax, string attributeName)
    {
        return syntax.AttributeLists
            .SelectMany(l => l.Attributes)
            .Any(a => a.Name.ToString() == attributeName);
    }
    
    /// <summary>
    /// Checks if a <see cref="MemberDeclarationSyntax"/> is decorated with a generic attribute of the given name.
    /// </summary>
    /// <param name="syntax">The member declaration syntax to check for the attribute.</param>
    /// <param name="attributeName">The short name (without "Attribute" suffix) of the attribute to check for.</param>
    /// <returns>True if the generic attribute is present on the member.</returns>
    public static bool HasGenericAttribute(this MemberDeclarationSyntax syntax, string attributeName)
    {
        return syntax.AttributeLists
            .SelectMany(l => l.Attributes)
            .Any(a =>
            {
                var name = a.Name.ToString();
                var index = name.IndexOf('<');
                if (index < 0)
                {
                    return false;
                }

                var nameToCheck = name.Substring(0, index);
                return nameToCheck == attributeName;
            });
    }

    /// <summary>
    /// Converts a singular string to a plural string.
    /// </summary>
    /// <param name="value">The singular text to convert.</param>
    /// <returns>The pluralized text.</returns>
    public static string Pluralize(this string value)
    {
        var pluralizer = new Pluralizer();
        return pluralizer.Pluralize(value);
    }

    /// <summary>
    /// Converts a field name that follows standard C# conventions to a matching property name.
    /// For example, "_someValue" will convert to "SomeValue."
    /// </summary>
    /// <param name="fieldName">The name of the backing field.</param>
    /// <returns>The property name.</returns>
    public static string ToPropertyName(this string fieldName) =>
        fieldName.ToUpperInvariant()[1] + fieldName.Substring(2);

    /// <summary>
    /// Splits a bitfield into its individual flags.
    /// </summary>
    /// <param name="value">The bitfield to split.</param>
    /// <typeparam name="TFlags">The type of the flags enum representing the bitfield.</typeparam>
    /// <returns>An enumerable containing the individual flags.</returns>
    public static IEnumerable<TFlags> Split<TFlags>(this TFlags value) where TFlags : struct, Enum
    {
        return Enum.GetValues(typeof(TFlags))
            .Cast<TFlags>()
            .Where(f => !f.Equals(default(TFlags)) && value.HasFlag(f));
    }

    /// <summary>
    /// Checks if the given member has at least one of the given attributes.
    /// </summary>
    /// <param name="syntax">The member to check for attributes.</param>
    /// <param name="attributeNames">The short names (without "Attribute" suffix) of the attributes to check for.</param>
    /// <returns>True if one or more of the attributes are present on the member.</returns>
    public static bool HasAnyAttribute(this MemberDeclarationSyntax syntax, params string[] attributeNames)
    {
        return syntax.AttributeLists
            .SelectMany(l => l.Attributes)
            .Any(a => attributeNames
                .Any(n => a.Name.ToString() == n));
    }

    /// <summary>
    /// Checks if the given class inherits a class with the given base class name.
    /// </summary>
    /// <param name="syntax">The class to check for the base class.</param>
    /// <param name="baseClassName">The local name of the base class to check for.</param>
    /// <returns>True if the class inherits the base class.</returns>
    public static bool HasBaseClass(this ClassDeclarationSyntax syntax, string baseClassName)
    {
        return syntax.BaseList?.Types.Any(t => t.Type.ToString() == baseClassName) == true;
    }

    /// <summary>
    /// Checks if the given symbol is decorated by an attribute with the given name.
    /// </summary>
    /// <param name="symbol">The symbol to check.</param>
    /// <param name="fullAttributeName">The name of the attribute (with "Attribute" suffix) to check for.</param>
    /// <returns>True if the symbol is decorated with the attribute.</returns>
    public static bool HasAttribute(this ISymbol symbol, string fullAttributeName)
    {
        return symbol.GetAttributes().Any(a => a.AttributeClass?.Name == fullAttributeName);
    }

    /// <summary>
    /// Gets all members of the given member type that are decorated by an attribute with the given attribute name.
    /// </summary>
    /// <param name="symbol">The type to get members from.</param>
    /// <param name="fullAttributeName">The name of the attribute (with "Attribute" suffix) to check for.</param>
    /// <typeparam name="TMember">The type of the members to retrieve.</typeparam>
    /// <returns>
    /// Enumerable containing all members of type <b>TMember</b> that are decorated with the given attribute.
    /// </returns>
    public static IEnumerable<TMember> GetMembersByAttribute<TMember>(this ITypeSymbol symbol, string fullAttributeName)
        where TMember : ISymbol
    {
        return symbol.GetMembers()
            .OfType<TMember>()
            .Where(m => m.HasAttribute(fullAttributeName));
    }

    /// <summary>
    /// Gets the attribute with the given name from the given symbol.
    /// </summary>
    /// <param name="symbol">The symbol to get the attribute from.</param>
    /// <param name="fullAttributeName">The name of the attribute (with "Attribute" suffix) to retrieve.</param>
    /// <returns>The data associated with the attribute.</returns>
    public static AttributeData? GetAttribute(this ISymbol symbol, string fullAttributeName)
    {
        return symbol.GetAttributes().FirstOrDefault(a => a.AttributeClass?.Name == fullAttributeName);
    }

    /// <summary>
    /// Retrieves all private readonly fields from the type that aren't assigned a value when declared.
    /// </summary>
    /// <param name="symbol">The symbol to retrieve the fields from.</param>
    /// <returns>An enumerable of private readonly fields from the symbol.</returns>
    public static IEnumerable<IFieldSymbol> GetUnassignedPrivateReadonlyFields(this ITypeSymbol symbol)
    {
        return symbol.GetMembers()
            .OfType<IFieldSymbol>()
            .Where(f => f.IsReadOnly
                        && f is {DeclaredAccessibility: Accessibility.Private, IsImplicitlyDeclared: false}
                        && f.DeclaringSyntaxReferences[0].GetSyntax()
                            is not VariableDeclaratorSyntax {Initializer: not null});
    }
    
    /// <summary>
    /// Retrieves all private and protected readonly fields from the type that aren't assigned a value when declared.
    /// </summary>
    /// <param name="symbol">The symbol to retrieve the fields from.</param>
    /// <returns>An enumerable of private and protected readonly fields from the symbol.</returns>
    public static IEnumerable<IFieldSymbol> GetUnassignedPrivateAndProtectedReadonlyFields(this ITypeSymbol symbol)
    {
        return symbol.GetMembers()
            .OfType<IFieldSymbol>()
            .Where(f => f.IsReadOnly
                        && f is
                        {
                            DeclaredAccessibility: Accessibility.Private or Accessibility.Protected,
                            IsImplicitlyDeclared: false
                        }
                        && f.DeclaringSyntaxReferences[0].GetSyntax()
                            is not VariableDeclaratorSyntax {Initializer: not null});
    }

    /// <summary>
    /// Retrieves all methods from the given type that are marked with the given marker attribute.
    /// </summary>
    /// <param name="symbol">The symbol to retrieve methods from.</param>
    /// <param name="markerAttributeName">Name of the marker attribute class including the "Attribute" suffix.</param>
    /// <returns>An enumerable of the marked methods.</returns>
    public static IEnumerable<string> GetMarkedMethods(this ITypeSymbol symbol, string markerAttributeName)
    {
        return symbol.GetMembers()
            .OfType<IMethodSymbol>()
            .Where(m => m.HasAttribute(markerAttributeName))
            .Select(m => m.Name);
    }
}