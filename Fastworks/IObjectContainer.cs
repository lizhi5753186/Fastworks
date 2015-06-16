using System;

namespace Fastworks
{
    public interface IObjectContainer : IServiceLocator
    {
        // Initializes the object container from config file
        void InitializeFromConfigFile(string configSectionName);

        // Get the container instance
        T GetContainer<T>();
    }
}
