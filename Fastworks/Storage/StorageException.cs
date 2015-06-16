using System;

namespace Fastworks.Storage
{
    [Serializable]
    public class StorageException : InfrastructureException
    {
        #region Ctor
        
        public StorageException() : base() { }
        
        public StorageException(string message) : base(message) { }
       
        public StorageException(string message, Exception innerException) : base(message, innerException) { }
        
        public StorageException(string format, params object[] args) : base(string.Format(format, args)) { }

        #endregion
    }
}
