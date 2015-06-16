using System;

namespace Fastworks.Serialization
{
    [Serializable]
    public class SerializationException : InfrastructureException
    {
        #region Ctor
        
        public SerializationException() : base() { }
        
        public SerializationException(string message) : base(message) { }
        
        public SerializationException(string message, Exception innerException) : base(message, innerException) { }
        
        public SerializationException(string format, params object[] args) : base(string.Format(format, args)) { }

        #endregion
    }
}
