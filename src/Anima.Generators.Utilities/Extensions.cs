using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Pluralize.NET;

namespace Anima.Generators.Utilities;

public static class Extensions
{
    public static bool HasInterface(this ClassDeclarationSyntax classDeclaration, string interfaceName)
    {
        if (classDeclaration.BaseList != null)
        {
            var baseTypes = classDeclaration.BaseList.Types.Select(t => t.Type);
            return baseTypes.Any(t => t.ToString() == interfaceName);
        }

        return false;
    }

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

    public static bool HasAttribute(this MemberDeclarationSyntax syntax, string attributeName)
    {
        return syntax.AttributeLists
            .SelectMany(l => l.Attributes)
            .Any(a => a.Name.ToString() == attributeName);
    }

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

    public static string Pluralize(this string value)
    {
        var pluralizer = new Pluralizer();
        return pluralizer.Pluralize(value);
    }

    public static string ToPropertyName(this string fieldName) =>
        fieldName.ToUpperInvariant()[1] + fieldName.Substring(2);

    public static IEnumerable<TFlags> Split<TFlags>(this TFlags value) where TFlags : struct, Enum
    {
        return Enum.GetValues(typeof(TFlags))
            .Cast<TFlags>()
            .Where(f => !f.Equals(default(TFlags)) && value.HasFlag(f));
    }

    // public static IEnumerable<TFlags> Split<TFlags>(this TFlags value) where TFlags : struct, Enum
    // {
    //     var type = Enum.GetUnderlyingType(typeof(TFlags));
    //     var validFlags = ((TFlags[])Enum.GetValues(typeof(TFlags)))
    //         .Select(f => Convert.ChangeType(f, type)).ToList();
    //     var flags = Convert.ChangeType(value, type);
    //
    //     return flags switch
    //     {
    //         ulong u64 => validFlags.Where(f => (u64 & (ulong)f) > 0).Cast<TFlags>(),
    //         long i64 => validFlags.Where(f => (i64 & (long)f) > 0).Cast<TFlags>(),
    //         uint u32 => validFlags.Where(f => (u32 & (uint)f) > 0).Cast<TFlags>(),
    //         int i32 => validFlags.Where(f => (i32 & (int)f) > 0).Cast<TFlags>(),
    //         ushort u16 => validFlags.Where(f => (u16 & (ushort)f) > 0).Cast<TFlags>(),
    //         short i16 => validFlags.Where(f => (i16 & (short)f) > 0).Cast<TFlags>(),
    //         byte u8 => validFlags.Where(f => (u8 & (byte)f) > 0).Cast<TFlags>(),
    //         sbyte i8 => validFlags.Where(f => (i8 & (sbyte)f) > 0).Cast<TFlags>(),
    //         _ => throw new NotSupportedException($"Type {type} is not supported.")
    //     };
    // }

    public static bool HasAnyAttribute(this MemberDeclarationSyntax syntax, params string[] attributeNames)
    {
        return syntax.AttributeLists
            .SelectMany(l => l.Attributes)
            .Any(a => attributeNames
                .Any(n => a.Name.ToString() == n));
    }

    public static bool HasBaseClass(this ClassDeclarationSyntax syntax, string baseClassName)
    {
        return syntax.BaseList?.Types.Any(t => t.Type.ToString() == baseClassName) == true;
    }

    public static bool HasAttribute(this ISymbol symbol, string fullAttributeName)
    {
        return symbol.GetAttributes().Any(a => a.AttributeClass?.Name == fullAttributeName);
    }

    public static IEnumerable<TMember> GetMembersByAttribute<TMember>(this ITypeSymbol symbol, string fullAttributeName)
        where TMember : ISymbol
    {
        return symbol.GetMembers()
            .OfType<TMember>()
            .Where(m => m.HasAttribute(fullAttributeName));
    }

    public static AttributeData? GetAttribute(this ISymbol symbol, string fullAttributeName)
    {
        return symbol.GetAttributes().FirstOrDefault(a => a.AttributeClass?.Name == fullAttributeName);
    }
}