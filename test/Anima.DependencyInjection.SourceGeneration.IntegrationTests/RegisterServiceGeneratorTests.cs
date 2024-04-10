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
}

[RegisterService(ServiceLifetime.Scoped)]
public class TestScopedService;

[RegisterService(ServiceLifetime.Singleton)]
public class TestSingletonService;

[RegisterService(ServiceLifetime.Transient)]
public class TestTransientService;