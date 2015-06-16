using Fastworks.Bus;

namespace Fastworks.Repositories
{
    public abstract class EventPublisherDomainRepository : DomainRepository
    {
        #region Private Fields
        private readonly IEventBus eventBus;
        #endregion

        public IEventBus EventBus
        {
            get { return this.eventBus; }
        }

        #region Ctor
        public EventPublisherDomainRepository(IEventBus eventBus)
        {
            this.eventBus = eventBus;
        }
        #endregion

        #region Override Methods
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                eventBus.Dispose();
            }
            base.Dispose(disposing);
        }
        #endregion 


        #region IUnitOfWork Members

        public override bool DistributedTransactionSupported
        {
            get { return this.eventBus.DistributedTransactionSupported; }
        }

        public override void Rollback()
        {
            eventBus.Rollback();
        }
        #endregion 
    }
}
