using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Anima.EntityFrameworkCore.SourceGeneration.IntegrationTests;

public class DbSetGeneratorTests
{
    [Fact]
    public void Generator_CreatesDbSets()
    {
        using var context = new TestContext();
        Assert.Empty(context.TestEntity1s);
        Assert.Empty(context.TestEntity2s);
    }
}

[GenerateDbSets]
public partial class TestContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());
    }
}

public class TestEntity1 : IEntity
{
    public int Id { get; set; }
}

public class TestEntity2 : IEntityTypeConfiguration<TestEntity2>
{
    public int Id { get; set; }
    
    public void Configure(EntityTypeBuilder<TestEntity2> builder) { }
}