using NHibernate;
using NHibernate.Cfg;

namespace Fastworks.Repositories.NHibernate
{
    /// <summary>
    /// Represents the factory singleton for database session.
    /// </summary>
    public sealed class DatabaseSessionFactory
    {
        #region Private Fields
        /// <summary>
        /// The session factory instance.
        /// </summary>
        private readonly ISessionFactory _sessionFactory = null;
        /// <summary>
        /// The session instance.
        /// </summary>
        private ISession _session = null;

        #endregion

        #region Ctor
       
        private DatabaseSessionFactory()
        {
            _sessionFactory = new Configuration().Configure().BuildSessionFactory();
        }
       
        private DatabaseSessionFactory(Configuration nhibernateConfig)
        {
            _sessionFactory = nhibernateConfig.BuildSessionFactory();
        }
        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the singleton instance of the session. If the session has not been
        /// initialized or opened, it will return a newly opened session from the session factory.
        /// </summary>
        public ISession Session
        {
            get
            {
                ISession result = _session;
                if (result != null && result.IsOpen)
                    return result;
                return OpenSession();
            }
        }

        public ISessionFactory SessionFactory
        {
            get { return _sessionFactory; }
        }

        #endregion

        #region Public Methods
       
        public ISession OpenSession()
        {
            this._session = _sessionFactory.OpenSession();
            return this._session;
        }

        #endregion
    }
}
