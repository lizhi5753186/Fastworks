using System;
using System.Collections.Generic;
using System.Linq;

namespace Fastworks
{
    public abstract class ObjectContainer : IObjectContainer
    {
        #region Protected Methods
        protected virtual T DoGetService<T>()
          where T : class
        {
            return this.DoGetService(typeof(T)) as T;
        }

        protected virtual T DoGetService<T>(object overridedArguments) where T : class
        {
            return this.DoGetService(typeof(T), overridedArguments) as T;
        }

        protected virtual T[] DoResolveAll<T>()
            where T : class
        {
            //return this.DoResolveAll(typeof(T)) as T[];
            var original = this.DoResolveAll(typeof(T));
            var casted = new T[original.Length];
            int index = 0;
            var e = original.GetEnumerator();
            while (e.MoveNext())
            {
                casted[index] = e.Current as T;
                index++;
            }
            return casted;
        }

        protected abstract object DoGetService(Type serviceType);
        protected abstract Array DoResolveAll(Type serviceType);
        protected abstract object DoGetService(Type serviceType, object overridedArguments);

        #endregion 
        
        #region IObjectContainer Members
        public abstract void InitializeFromConfigFile(string configSectionName);

        public abstract T GetContainer<T>();
       
        public T GetService<T>() where T : class
        {
            T serviceImpl = this.DoGetService<T>();
            return serviceImpl;
        }

        public T GetService<T>(dynamic overridedArguments) where T : class
        {
            T serviceImpl = this.DoGetService<T>(overridedArguments);
            return serviceImpl;
        }

        public object GetService(Type serviceType, dynamic overrideArguments)
        {
            throw new NotImplementedException();
        }

        public Array ResolveAll(Type serviceType)
        {
            var serviceImpls = this.DoResolveAll(serviceType);
            return serviceImpls;
        }

        public T[] ResolveAll<T>() where T : class
        {
            var serviceImpls = this.DoResolveAll<T>();
            return serviceImpls;
        }

        public abstract bool Registered<T>();
        
        public abstract bool Registered(Type type);
        
        #endregion

        #region IServiceProvider Members
        public object GetService(Type serviceType)
        {
            object serviceImpl = this.DoGetService(serviceType);
            return serviceImpl;
        }
        #endregion 
    }
}
