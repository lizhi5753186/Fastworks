using System;
using System.Data.Entity;

namespace Fastworks.Repositories.EntityFramework
{
    public class EntityFrameworkRepositoryContext : RepositoryContext, IEntityFrameworkRepositoryContext
    {
        private readonly DbContext efContext;
        private readonly object sync = new object();


        public EntityFrameworkRepositoryContext(DbContext efContext)
        {
            this.efContext = efContext;
        }

        #region IRepositoryContext Members
        public override void RegisterNew<TAggregateRoot>(TAggregateRoot entity)
        {
            this.efContext.Entry(entity).State = EntityState.Added;
            Committed = false;
        }

        public override void RegisterModified<TAggregateRoot>(TAggregateRoot entity)
        {
            this.efContext.Entry(entity).State = EntityState.Modified;
            Committed = false;
        }

        public override void RegisterDeleted<TAggregateRoot>(TAggregateRoot entity)
        {
            this.efContext.Entry(entity).State = EntityState.Deleted;
            Committed = false;
        }

        #region  IUnitOfWork Members
        public override bool DistributedTransactionSupported
        {
            get { return true; }
        }

        public override bool Committed { get; protected set; }
        

        public override void Commit()
        {
            if (!Committed)
            {
                lock (sync)
                {
                    efContext.SaveChanges();
                }

                Committed = true;
            }
        }

        public override void Rollback()
        {
            Committed = false;
        }

        #endregion 
        protected override void Dispose(bool disposing = false)
        {
            if (disposing)
            {
                efContext.Dispose();
            }
            
            base.Dispose(disposing);
        }

        #endregion 
    
        #region IEntityFrameworkRepositoryContext Members
        public DbContext Context
        {
            get { return this.efContext; }
        }
        #endregion 
    
        
    }
}
