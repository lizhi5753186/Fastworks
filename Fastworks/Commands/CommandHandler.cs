
using Fastworks.Repositories;
namespace Fastworks.Commands
{
    public abstract class CommandHandler<TCommand> : ICommandHandler<TCommand>
        where TCommand : ICommand
    {
        public abstract void Handle(TCommand message);

        /// <summary>
        /// Gets the instance of the domain repository that could be used
        /// by the current command handler to perform repository operations.
        /// </summary>
        protected virtual IDomainRepository DomainRepository
        {
            get { return Bootstrapper.Instance.ObjectContainer.GetService<IDomainRepository>(); }
        }
    }
}
