# Anima.EntityFrameworkCore.SourceGeneration

Source generators to reduce boilerplate code when working with EntityFrameworkCore.

## GenerateDbSets

This feature eases the burden of manually adding entities to the `DbContext` every time a new entity
class is created.

### Usage

Set the project's `DbContext` class as `partial` and mark it with the `[GenerateDbSets]`
attribute to automatically generate `DbSet` properties for every class
in the project that implements `IEntityTypeConfiguration`.

### Example

Given the following classes:

```csharp
[GenerateDbSets]
public partial class DatabaseContext : DbContext
{
    public DatabaseContext() { } // Parameterless constructor for dotnet ef
    
    public DatabaseContext(DbContextOptions<DatabaseContext> options)
    {
        // Configure the database here
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Add entity configurations from classes that implement IEntityTypeConfiguration
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetAssembly(typeof(DatabaseContext))!);
        base.OnModelCreating(modelBuilder);
    }
}
```

```csharp
public class User : IEntityTypeConfiguration<User>
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    
    public ICollection<Session> Sessions { get; set; } = new HashSet<Session>();
    
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(e => e.FirstName).HasMaxLength(50);
        builder.Property(e => e.LastName).HasMaxLength(50);
    }
}
```

```csharp
public class Session : IEntityTypeConfiguration<User>
{
    public int Id { get; set; }
    
    public User User { get; set; }
    public int UserId { get; set; }
    
    public string Platform { get; set; }
    public DateTime LoginTime { get; set; }
    public DateTime? LogoutTime { get; set; }
    
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(e => e.Platform).HasMaxLength(20);
    }
}
```

This output will be generated:
```csharp
public partial class DatabaseContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Session> Sessions { get; set; }
}
```