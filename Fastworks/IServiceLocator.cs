using System;


namespace Fastworks
{
    // IServiceProvider: https://msdn.microsoft.com/en-us/library/system.iserviceprovider(v=vs.110).aspx
    public interface IServiceLocator : IServiceProvider
    {
        T GetService<T>() where T : class;

        T GetService<T>(dynamic overridedArguments) where T : class;

        object GetService(Type serviceType, dynamic overridedArguments);

        Array ResolveAll(Type serviceType);

        T[] ResolveAll<T>() where T : class;

        bool Registered<T>();

        bool Registered(Type type);
    }
}
