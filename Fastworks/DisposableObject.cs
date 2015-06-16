using System.Runtime.ConstrainedExecution;
using System;

namespace Fastworks
{
    public abstract class DisposableObject : CriticalFinalizerObject, IDisposable
    {
        ~DisposableObject()
        {
            this.Dispose();
        }

        protected abstract void Dispose(bool disposing = false);

        protected void ExplicitDispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        #region IDisposable Members
        public void Dispose()
        {
            this.ExplicitDispose();
        }
        #endregion 
    }
}
