using Fastworks.Serialization;

namespace Fastworks.Events.Serialization
{
    /// <summary>
    /// Represents the serializer for domain events that serializes/deserializes the domain events
    /// with Json format.
    /// </summary>
    public class DomainEventJsonSerializer : ObjectJsonSerializer, IDomainEventSerializer
    {
    }
}
