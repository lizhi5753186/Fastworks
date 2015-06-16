using System;

namespace Fastworks
{
    public class ServiceLocator : IServiceLocator
    {
        #region Private Fields
        private readonly IObjectContainer objectContainer;
        private static readonly ServiceLocator instance = new ServiceLocator();

        #endregion

        #region Public Static Properties
        public static ServiceLocator Instance
        {
            get { return instance; }
        }
        #endregion

        #region Ctor
        private ServiceLocator()
        {
            objectContainer = Bootstrapper.Instance.ObjectContainer;
        }

        #endregion

        #region IServiceLocator Members


        public T GetService<T>() where T : class
        {
            return objectContainer.GetService<T>();
        }

        public T GetService<T>(dynamic overridedArguments) where T : class
        {
            return objectContainer.GetService<T>(overridedArguments);
        }

        public object GetService(Type serviceType, dynamic overridedArguments)
        {
            return objectContainer.GetService(serviceType, overridedArguments);
        }

        public Array ResolveAll(Type serviceType)
        {
            return objectContainer.ResolveAll(serviceType);
        }

        public T[] ResolveAll<T>() where T : class
        {
            return objectContainer.ResolveAll<T>();
        }

        public bool Registered<T>()
        {
            return objectContainer.Registered<T>();
        }

        public bool Registered(Type type)
        {
            return objectContainer.Registered(type);
        }
        #endregion

        #region IServiceProvider Members
        public object GetService(Type serviceType)
        {
            return objectContainer.GetService(serviceType);
        }
        #endregion 
        
    }
}
