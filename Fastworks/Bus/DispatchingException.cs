using System;

namespace Fastworks.Bus
{
    [Serializable]
    public class DispatchingException : InfrastructureException
    {
        public DispatchingException() : base() { }
       
        public DispatchingException(string message) : base(message) { }
       
        public DispatchingException(string message, Exception innerException) : base(message, innerException) { }
       
        public DispatchingException(string format, params object[] args) : base(string.Format(format, args)) { }
    }
}
