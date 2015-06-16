using System;

namespace Fastworks.Bus
{
    public interface IMessageDispatcher
    {
        void Clear();
        
        void DispatchMessage<T>(T message);

        // Registers a message handler into message dispatcher.
        void Register<T>(IHandler<T> handler);
       
        // Unregisters a message handler from the message dispatcher.
        void UnRegister<T>(IHandler<T> handler);
       
        // Occurs when the message dispatcher is going to dispatch a message.
        event EventHandler<MessageDispatchEventArgs> Dispatching;
      
        // Occurs when the message dispatcher failed to dispatch a message.
        event EventHandler<MessageDispatchEventArgs> DispatchFailed;
      
        // Occurs when the message dispatcher has finished dispatching the message.
        event EventHandler<MessageDispatchEventArgs> Dispatched;
    }
}
