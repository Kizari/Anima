using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Microsoft.Extensions.DependencyInjection;
using unsafe SwapBuffersDelegate = delegate* unmanaged[Stdcall]<System.IntPtr, System.IntPtr, int>;

namespace Anima.DependencyInjection.SourceGeneration.Sample;

[RegisterService(ServiceLifetime.Singleton)]
public unsafe partial class AbstractSample : AbstractSampleBase
{
    private static DateTime TimeStamp2;
    private static SwapBuffersDelegate _original;
    
    private readonly TestTransientService _transient;

    public override void Test()
    {
        throw new NotImplementedException();
    }

    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvStdcall)])]
    public static int EGL_SwapBuffers_Hook(IntPtr display, IntPtr surface)
    {
        return _original(display, surface);
    }
}

[ServiceBase]
public abstract partial class AbstractSampleBase
{
    protected const uint GL_BGRA = 0x80E1;
    protected const uint GL_UBYTE = 0x1401;
    
    protected static TestScopedService Scoped = null!;
    protected static IUserRepository UserRepository = null!;
    protected static DateTime TimeStamp = DateTime.Now;

    protected readonly TestScopedService _scoped;
    protected readonly IUserRepository _userRepository;
    protected readonly IRepository<int, User> _repository;

    [OnConstruct]
    private void OnConstruct()
    {
        UserRepository = _userRepository;
        Scoped = _scoped;
    }

    public abstract void Test();
}