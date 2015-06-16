using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fastworks.Events
{
    public interface IEventHandler<in TEvent> : IHandler<TEvent>
        where TEvent : class, IEvent
    {
    }
}
