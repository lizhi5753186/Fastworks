namespace Fastworks.Bus.MSMQ
{
    public class MSMQCommandBus : MSMQBus, ICommandBus
    {
        public MSMQCommandBus(string path) : base(path) { }
        
        public MSMQCommandBus(string path, bool useInternalTransaction) : base(path, useInternalTransaction) { }
        
        public MSMQCommandBus(MSMQBusOptions options) : base(options) { }
    }
}
