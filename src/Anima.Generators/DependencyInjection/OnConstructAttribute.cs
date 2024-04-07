using System;
using Anima.Generators.Utilities;

namespace Neurodex.Generators.DependencyInjection;

public class OnConstructAttribute : StaticSourceDefinition
{
    public const string ShortName = "OnConstruct";
    public const string Name = $"{ShortName}Attribute";
    
    public override string Source => AttributeHelper.CreateSource(Name,
        "Methods marked with this attribute will be executed at the end of the generated constructor.",
        attributeTargets: AttributeTargets.Method);
}