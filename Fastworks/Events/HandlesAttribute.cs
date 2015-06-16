using System;

namespace Fastworks.Events
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class HandlesAttribute : System.Attribute
    {
        #region Public Properties
        /// <summary>
        /// Gets or sets the type of the domain event that can be handled by
        /// the decorated method.
        /// </summary>
        public Type DomainEventType { get; set; }
        #endregion

        #region Ctor
        /// <summary>
        /// Initializes a new instance of <c>HandlesAttribute</c> class.
        /// </summary>
        /// <param name="domainEventType">The type of the domain event that can be handled by
        /// the decorated method.</param>
        public HandlesAttribute(Type domainEventType)
        {
            this.DomainEventType = domainEventType;
        }
        #endregion
    }
}
