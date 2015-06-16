using System;

namespace Fastworks.Events
{
    public interface IEvent : IEntity
    {
        // Gets or sets the date and time on which the event was produced.
        DateTime Timestamp { get; set; }

        // Gets or sets the assembly qualified type name of the event.
        string AssemblyQualifiedEventType { get; set; }
    }
}
