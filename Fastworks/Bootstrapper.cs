using System;
using System.Collections.Generic;
using System.Configuration;

namespace Fastworks
{
    public sealed class Bootstrapper
    {
        #region Private Static Fields
        private static readonly Bootstrapper instance = new Bootstrapper();
        
        private readonly IObjectContainer objectContainer;
        #endregion 

        #region Public Static Properties
        public static Bootstrapper Instance
        {
            get { return instance; }
        }
        #endregion 

        #region Public Properties
        public IObjectContainer ObjectContainer
        {
            get { return this.objectContainer; }
        }

        #endregion 

        #region Ctor
        private Bootstrapper()
        {
            // Initializer Object Container and Interception
            string objectContainerProviderName = ConfigurationManager.AppSettings["objectContainerProvider"];
            Type objectContainerType = Type.GetType(objectContainerProviderName);
            this.objectContainer = (ObjectContainer)Activator.CreateInstance(objectContainerType);
        }
        #endregion 
    }
}
