using System.Messaging;

namespace Fastworks.Bus.MSMQ
{
    public class MSMQBusOptions
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets a <see cref="System.Boolean"/> value which indicates whether
        /// the exclusive read access should be granted to the first application 
        /// that accesses the queue.
        /// </summary>
        public bool SharedModeDenyReceive { get; set; }
        /// <summary>
        /// Gets or sets a <see cref="System.Boolean"/> value which indicates whether
        /// a connection cache should be created and used.
        /// </summary>
        public bool EnableCache { get; set; }
        /// <summary>
        /// Gets or sets a value that indicates the access mode for the queue.
        /// </summary>
        public QueueAccessMode QueueAccessMode { get; set; }
        /// <summary>
        /// Gets or sets the location of the queue referenced by this System.Messaging.MessageQueue,
        /// which can be "." for the local computer.
        /// </summary>
        public string Path { get; set; }
        /// <summary>
        /// Gets or sets a <see cref="System.Boolean"/> value which indicates whether the internal
        /// transaction should be used when sending or receiving messages.
        /// </summary>
        public bool UseInternalTransaction { get; set; }
        /// <summary>
        /// Gets or sets the formatter used to serialize an object into or deserialize
        /// an object from the body of a message read from or written to the queue.
        /// </summary>
        public IMessageFormatter MessageFormatter { get; set; }
        #endregion

        #region Ctor
       
        public MSMQBusOptions(string path, bool sharedModeDenyReceive, bool enableCache, QueueAccessMode queueAccessMode, bool useInternalTransaction, IMessageFormatter messageFormatter)
        {
            this.SharedModeDenyReceive = sharedModeDenyReceive;
            this.EnableCache = enableCache;
            this.QueueAccessMode = queueAccessMode;
            this.Path = path;
            this.UseInternalTransaction = useInternalTransaction;
            this.MessageFormatter = messageFormatter;
        }
        
        public MSMQBusOptions(string path)
            : this(path, false, false, QueueAccessMode.SendAndReceive, false, new XmlMessageFormatter())
        { }
        
        public MSMQBusOptions(string path, bool useInternalTransaction)
            : this(path, false, false, QueueAccessMode.SendAndReceive, useInternalTransaction, new XmlMessageFormatter())
        { }

        #endregion
    }
}
