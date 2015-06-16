using System;
using System.Collections.Generic;

namespace Fastworks.Bus
{
    public interface IBus : IUnitOfWork, IDisposable
    {
        void Publish<TMessage>(TMessage message);
        void Publish<TMessage>(IEnumerable<TMessage> messages);
        void Clear();
    }
}
