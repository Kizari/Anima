using System;
using Anima.Generators.Utilities;

namespace Anima.Generators.EntityFrameworkCore;

public class GenerateDbSetsAttribute : StaticSourceDefinition
{
    public const string ShortName = "GenerateDbSets";
    public const string Name = $"{ShortName}Attribute";
    
    public override string Source => AttributeHelper.CreateSource(Name,
        "Mark a DbContext implementation with this attribute to have DbSet properties generated " +
        "for each entity in the solution automatically.",
        attributeTargets: AttributeTargets.Class);
}