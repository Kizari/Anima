namespace Anima.Utilities.SourceGeneration;

/// <summary>
/// Base class for static source that needs to be added to the compilation, such as marker attribute classes.
/// </summary>
public abstract class StaticSourceDefinition
{
    /// <summary>
    /// The name of the source file that will be generated from this source.
    /// </summary>
    public string FileName => $"{GetType().FullName}.g.cs";
    
    /// <summary>
    /// Builds the source.
    /// </summary>
    public abstract string Source { get; }
}