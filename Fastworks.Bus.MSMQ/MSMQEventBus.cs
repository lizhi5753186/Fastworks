
namespace Fastworks.Bus.MSMQ
{
    /// <summary>
    /// Represents the event bus which uses the Microsoft Message Queuing technology.
    /// </summary>
    public class MSMQEventBus : MSMQBus, IEventBus
    {
        public MSMQEventBus(string path) : base(path) { }
        
        public MSMQEventBus(string path, bool useInternalTransaction) : base(path, useInternalTransaction) { }
       
        public MSMQEventBus(MSMQBusOptions options) : base(options) { }
      
    }
}
