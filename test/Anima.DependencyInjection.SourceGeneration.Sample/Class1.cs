using Microsoft.Extensions.DependencyInjection;

namespace Anima.DependencyInjection.SourceGeneration.Sample;

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