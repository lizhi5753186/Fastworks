using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity.Configuration;
using Microsoft.Practices.Unity;
using System.Reflection;
using System.Configuration;

namespace Fastworks.ObjectContainers.Unity
{
    public class UnityObjectContainer : ObjectContainer
    {
        private readonly IUnityContainer container;

        public UnityObjectContainer()
        {
            container = new UnityContainer();
        }

        #region  ObjectContainer Members
        protected override object DoGetService(Type serviceType)
        {
            return container.Resolve(serviceType);
        }

        protected override Array DoResolveAll(Type serviceType)
        {
            return container.ResolveAll(serviceType).ToArray();
        }

        protected override object DoGetService(Type serviceType, object overridedArguments)
        {
            List<ParameterOverride> overrides = new List<ParameterOverride>();
            Type argumentsType = overridedArguments.GetType();
            argumentsType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .ToList()
                .ForEach(property =>
                {
                    var propertyValue = property.GetValue(overridedArguments, null);
                    var propertyName = property.Name;
                    overrides.Add(new ParameterOverride(propertyName, propertyValue));
                });

            return container.Resolve(serviceType, overrides.ToArray());
        }

        public override void InitializeFromConfigFile(string configSectionName)
        {
            UnityConfigurationSection section = (UnityConfigurationSection)ConfigurationManager.GetSection(configSectionName);
            section.Configure(container);
        }

        public override T GetContainer<T>()
        {
            if (typeof(T).Equals(typeof(UnityContainer)))
                return (T)this.container;

            throw new InfrastructureException("The wrapped container type provided by the current object container should be '{0}'.", typeof(UnityContainer));
        }

        public override bool Registered<T>()
        {
            return this.container.IsRegistered<T>();
        }

        public override bool Registered(Type type)
        {
            return this.container.IsRegistered(type);
        }
        #endregion 
    }

}
