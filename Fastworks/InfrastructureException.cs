using System;

namespace Fastworks
{
    // Represents errors that occur in the infrastructure layer of Fastworks framework.
    [Serializable]
    public class InfrastructureException : FastworksException
    {
        public InfrastructureException() : base() { }

        public InfrastructureException(string message) : base(message) { }

        public InfrastructureException(string message, Exception innerException) : base(message, innerException) { }

        public InfrastructureException(string format, params object[] args) : base(string.Format(format, args)) { }
    }
}
