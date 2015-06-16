using Fastworks.Bus;

namespace Fastworks.Commands
{
    [RegisterDispatch]
    public interface ICommandHandler<TCommand> : IHandler<TCommand>
        where TCommand : ICommand
    {
    }
}
