using Anima.Generators.Utilities;

namespace Anima.Generators.DependencyInjection;

public class RegisterServiceAttribute : StaticSourceDefinition
{
    public const string ShortName = "RegisterService";
    public const string Name = $"{ShortName}Attribute";
        
    public override string Source => new SourceBuilder()
        .AppendLine("using System;")
        .AppendLine()
        .AppendLine("namespace Microsoft.Extensions.DependencyInjection;")
        .AppendLine()
        .AppendLine("[AttributeUsage(AttributeTargets.Class)]")
        .AppendLine($"internal class {Name} : Attribute")
        .AppendLine('{')
        .Indent()
        .AppendLine("/// <summary>")
        .AppendLine("/// Adds this class to an extension method for this assembly which will register the class")
        .AppendLine("/// as a service in the <see cref=\"IServiceCollection\"/>. The class will be registered against")
        .AppendLine("/// the interface sharing the same name as the class with the 'I' prefix if implemented,")
        .AppendLine("/// otherwise, it will be registered standalone.")
        .AppendLine("/// <param name=\"lifetime\">The service lifetime to register this service with.</param>")
        .AppendLine("/// </summary>")
        .AppendLine($"public {Name}(ServiceLifetime lifetime) {{ }}")
        .Outdent()
        .AppendLine('}')
        .ToString();
}