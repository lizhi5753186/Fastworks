using System;

namespace Fastworks.Commands
{
    [Serializable]
    public abstract class Command : ICommand
    {
        public Command()
            : this(Guid.NewGuid())
        { 
        }

        public Command(Guid id)
        {
            this.Id = id;
        }

        public Guid Id { get; set; }
    }
}
