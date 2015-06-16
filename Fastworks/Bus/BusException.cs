using System;

namespace Fastworks.Bus
{
    public class BusException : InfrastructureException
    {
        public BusException() : base() { }
       
        public BusException(string message) : base(message) { }
       
        public BusException(string message, Exception innerException) : base(message, innerException) { }
       
        public BusException(string format, params object[] args) : base(string.Format(format, args)) { }
    }
}
