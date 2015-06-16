using System;

namespace Fastworks
{
    [Serializable]
    public class FastworksException : Exception
    {
        public FastworksException() : base() { }

        public FastworksException(string message) : base(message) { }

        public FastworksException(string message, Exception innerException) : base(message, innerException) { }

        public FastworksException(string format, params object[] args) : base(string.Format(format, args)) { }
    }
}
