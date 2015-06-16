using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Fastworks.Bus
{
    public class MessageDispatcher : IMessageDispatcher
    {
        #region Private Fields
        private readonly Dictionary<Type, List<dynamic>> _handlers = new Dictionary<Type, List<dynamic>>();
        #endregion

        #region Private Methods
        /// <summary>
        /// Registers the specified handler type to the message dispatcher.
        /// </summary>
        /// <param name="messageDispatcher">Message dispatcher instance.</param>
        /// <param name="handlerType">The type to be registered.</param>
        private static void RegisterType(IMessageDispatcher messageDispatcher, Type handlerType)
        {
            MethodInfo methodInfo = messageDispatcher.GetType().GetMethod("Register", BindingFlags.Public | BindingFlags.Instance);

            var handlerIntfTypeQuery = from p in handlerType.GetInterfaces()
                                       where p.IsGenericType &&
                                       p.GetGenericTypeDefinition().Equals(typeof(IHandler<>))
                                       select p;
            if (handlerIntfTypeQuery != null)
            {
                foreach (var handlerIntfType in handlerIntfTypeQuery)
                {
                    object handlerInstance = Activator.CreateInstance(handlerType);
                    Type messageType = handlerIntfType.GetGenericArguments().First();
                    MethodInfo genericMethodInfo = methodInfo.MakeGenericMethod(messageType);
                    genericMethodInfo.Invoke(messageDispatcher, new object[] { handlerInstance });
                }
            }
        }
        /// <summary>
        /// Registers all the handler types within a given assembly to the message dispatcher.
        /// </summary>
        /// <param name="messageDispatcher">Message dispatcher instance.</param>
        /// <param name="assembly">The assembly.</param>
        private static void RegisterAssembly(IMessageDispatcher messageDispatcher, Assembly assembly)
        {
            foreach (Type type in assembly.GetExportedTypes())
            {
                var intfs = type.GetInterfaces();
                if (intfs.Any(p =>
                    p.IsGenericType &&
                    p.GetGenericTypeDefinition().Equals(typeof(IHandler<>))) &&
                    intfs.Any(p =>
                    p.IsDefined(typeof(RegisterDispatchAttribute), true)))
                {
                    RegisterType(messageDispatcher, type);
                }
            }
        }
        #endregion

        #region Protected Methods
        /// <summary>
        /// Occurs when the message dispatcher is going to dispatch a message.
        /// </summary>
        /// <param name="e">The event data.</param>
        protected virtual void OnDispatching(MessageDispatchEventArgs e)
        {
            var temp = this.Dispatching;
            if (temp != null)
                temp(this, e);
        }
        /// <summary>
        /// Occurs when the message dispatcher failed to dispatch a message.
        /// </summary>
        /// <param name="e">The event data.</param>
        protected virtual void OnDispatchFailed(MessageDispatchEventArgs e)
        {
            var temp = this.DispatchFailed;
            if (temp != null)
                temp(this, e);
        }
        /// <summary>
        /// Occurs when the message dispatcher has finished dispatching the message.
        /// </summary>
        /// <param name="e">The event data.</param>
        protected virtual void OnDispatched(MessageDispatchEventArgs e)
        {
            var temp = this.Dispatched;
            if (temp != null)
                temp(this, e);
        }
        #endregion

        #region IMessageDispatcher
        public void Clear()
        {
            _handlers.Clear();
        }

        public virtual void DispatchMessage<T>(T message)
        {
            Type messageType = typeof(T);
            if (_handlers.ContainsKey(messageType))
            {
                var messageHandlers = _handlers[messageType];
                foreach (var messageHandler in messageHandlers)
                {
                    var dynMessageHandler = (IHandler<T>)messageHandler;
                    var eventArgs = new MessageDispatchEventArgs(message, messageHandler.GetType(), messageHandler);
                    this.OnDispatching(eventArgs);
                    try
                    {
                        dynMessageHandler.Handle(message);
                        this.OnDispatched(eventArgs);
                    }
                    catch
                    {
                        this.OnDispatchFailed(eventArgs);
                    }
                }
            }
        }

        /// <summary>
        /// Registers a message handler into message dispatcher.
        /// </summary>
        /// <typeparam name="T">The type of the message.</typeparam>
        /// <param name="handler">The handler to be registered.</param>
        public void Register<T>(IHandler<T> handler)
        {
            Type keyType = typeof(T);

            if (_handlers.ContainsKey(keyType))
            {
                List<object> registeredHandlers = _handlers[keyType];
                if (registeredHandlers != null)
                {
                    if (!registeredHandlers.Contains(handler))
                        registeredHandlers.Add(handler);
                }
                else
                {
                    registeredHandlers = new List<object>();
                    registeredHandlers.Add(handler);
                    _handlers.Add(keyType, registeredHandlers);

                }
            }
            else
            {
                List<object> registeredHandlers = new List<object>();
                registeredHandlers.Add(handler);
                _handlers.Add(keyType, registeredHandlers);
            }
        }

        public void UnRegister<T>(IHandler<T> handler)
        {
            var keyType = typeof(T);
            if (_handlers.ContainsKey(keyType) &&
               _handlers[keyType] != null &&
                _handlers[keyType].Count > 0 &&
                _handlers[keyType].Contains(handler))
            {
                _handlers[keyType].Remove(handler);
            }
        }

        public event EventHandler<MessageDispatchEventArgs> Dispatching;

        public event EventHandler<MessageDispatchEventArgs> DispatchFailed;

        public event EventHandler<MessageDispatchEventArgs> Dispatched;
        #endregion 
    }
}
