using Microsoft.Extensions.DependencyInjection;

namespace Anima.DependencyInjection.SourceGeneration.IntegrationTests;

public class RegisterServiceGeneratorTests
{
    [Fact]
    public void Generator_WhenExtensionCalled_RegistersServicesCorrectly()
    {
        var collection = new ServiceCollection()
            .AddAnimaDependencyInjectionSourceGenerationIntegrationTests();
        
        var scoped = collection.FirstOrDefault(d => d.ServiceType == typeof(TestScopedService));
        Assert.NotNull(scoped);
        Assert.Equal(ServiceLifetime.Scoped, scoped.Lifetime);
        
        var singleton = collection.FirstOrDefault(d => d.ServiceType == typeof(TestSingletonService));
        Assert.NotNull(singleton);
        Assert.Equal(ServiceLifetime.Singleton, singleton.Lifetime);
        
        var transient = collection.FirstOrDefault(d => d.ServiceType == typeof(TestTransientService));
        Assert.NotNull(transient);
        Assert.Equal(ServiceLifetime.Transient, transient.Lifetime);
    }

    [Fact]
    public void Generator_WhenExtensionCalled_RegistersMultipleInterfacesCorrectly()
    {
        var collection = new ServiceCollection()
            .AddAnimaDependencyInjectionSourceGenerationIntegrationTests();

        var matches = collection
            .Where(d => d.ImplementationType == typeof(UserRepository))
            .Select(d => d.ServiceType)
            .ToArray();

        Assert.Contains(typeof(IRepository<User, int>), matches);
        Assert.Contains(typeof(IUserRepository), matches);
    }
}

[RegisterService(ServiceLifetime.Scoped)]
public class TestScopedService;

[RegisterService(ServiceLifetime.Singleton)]
public class TestSingletonService;

[RegisterService(ServiceLifetime.Transient)]
public class TestTransientService;

public interface IRepository<TKey, TItem>;
public interface IUserRepository;

public class Repository<TKey, TItem> : IRepository<TKey, TItem>;

public class User;

[RegisterService(ServiceLifetime.Singleton)]
public class UserRepository : Repository<User, int>, IUserRepository;

[RegisterService(ServiceLifetime.Singleton)]
public class TestAbstractService;

public abstract class TestAbstractBase;