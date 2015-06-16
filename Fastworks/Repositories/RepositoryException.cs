using System;

namespace Fastworks.Repositories
{
    [Serializable]
    public class RepositoryException : DomainException
    {
        public RepositoryException() : base() { }
       
        public RepositoryException(string message) : base(message) { }
        
        public RepositoryException(string message, Exception innerException) : base(message, innerException) { }
        
        public RepositoryException(string format, params object[] args) : base(string.Format(format, args)) { }
    }
}
