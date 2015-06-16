using System.Collections.Generic;

namespace Fastworks.Bus.DirectBus
{
    public abstract class DirectBus : DisposableObject, IBus
    {
        #region Private Fields
        private volatile bool _committed = true;
        private readonly IMessageDispatcher _dispatcher;
        private readonly Queue<dynamic> _messageQueue = new Queue<dynamic>();
        private readonly object queueLock = new object();
        private object[] _backupMessageArray;
        #endregion 

        #region Ctor
        public DirectBus(IMessageDispatcher dispatcher)
        {
            this._dispatcher = dispatcher;
        }
        #endregion 

        #region IBus Members

        public void Publish<TMessage>(TMessage message)
        {
            lock (queueLock)
            {
                _messageQueue.Enqueue(message);
                _committed = false;
            }
        }

        public void Publish<TMessage>(IEnumerable<TMessage> messages)
        {
            lock (queueLock)
            {
                foreach (var message in messages)
                {
                    _messageQueue.Enqueue(message);
                }

                _committed = false;
            }
        }

        public void Clear()
        {
            lock (queueLock)
            {
                this._messageQueue.Clear();
            }
        }
        #endregion 

        #region IUnitOfWork Members

        public bool DistributedTransactionSupported
        {
            get { return false; }
        }

        public bool Committed
        {
            get { return this._committed; }
        }

        public void Commit()
        {
            lock (queueLock)
            {
                _backupMessageArray = new object[_messageQueue.Count];
                _messageQueue.CopyTo(_backupMessageArray, 0);
                while (_messageQueue.Count > 0)
                {
                    _dispatcher.DispatchMessage(_messageQueue.Dequeue());
                }
                _committed = true;
            }
        }

        public void Rollback()
        {
            lock (queueLock)
            {
                if (_backupMessageArray != null && _backupMessageArray.Length > 0)
                {
                    _messageQueue.Clear();
                    foreach (var msg in _backupMessageArray)
                    {
                        _messageQueue.Enqueue(msg);
                    }
                }
                _committed = false;
            }
        }
        #endregion 

        #region DisposableObject Members

        protected override void Dispose(bool disposing = false)
        {
        }

        #endregion 
    }
}
