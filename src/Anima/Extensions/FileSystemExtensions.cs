using System.IO.Abstractions;

namespace Anima.Extensions;

/// <summary>
/// Extension methods related to the file system.
/// </summary>
public static class FileSystemExtensions
{
    /// <inheritdoc cref="IPath.Combine(string[])"/>
    /// <remarks>
    /// Equivalent to <see cref="IPath.Combine(string[])"/> except path separators will always be
    /// forward slashes regardless of the strings passed into this method.
    /// </remarks>
    public static string CombineNormalized(this IPath path, params string[] paths)
    {
        var result = path.Combine(paths);
        return result
            .Replace('/', path.DirectorySeparatorChar)
            .Replace('\\', path.DirectorySeparatorChar);
    }
}