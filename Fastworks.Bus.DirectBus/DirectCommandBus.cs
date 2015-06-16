
namespace Fastworks.Bus.DirectBus
{
    public sealed class DirectCommandBus : DirectBus, ICommandBus
    {
        public DirectCommandBus(IMessageDispatcher dispatcher)
            : base(dispatcher)
        { }
    }
}
