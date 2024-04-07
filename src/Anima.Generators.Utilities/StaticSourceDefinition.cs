namespace Anima.Generators.Utilities;

public abstract class StaticSourceDefinition
{
    public string FileName => $"{GetType().FullName}.g.cs";
    public abstract string Source { get; }
}