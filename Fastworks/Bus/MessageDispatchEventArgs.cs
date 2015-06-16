using System;

namespace Fastworks.Bus
{
    public class MessageDispatchEventArgs : EventArgs
    {
        #region Public Properties
        public dynamic Message { get; set; }
       
        // Gets or sets the type of the message handler.
        public Type HandlerType { get; set; }
       
        // Gets or sets the handler.
        public object Handler { get; set; }
        #endregion

        #region Ctor
      
        public MessageDispatchEventArgs()
        { }
       
        public MessageDispatchEventArgs(dynamic message, Type handlerType, object handler)
        {
            this.Message = message;
            this.HandlerType = handlerType;
            this.Handler = handler;
        }
        #endregion
    }
}
