using NHibernate.Cfg;

namespace Fastworks.Repositories.NHibernate
{
    public class NHibernateApplicationConfiguration : Configuration
    {
        public NHibernateApplicationConfiguration()
            : base()
        {
            this.Configure();
        }
        
        public NHibernateApplicationConfiguration(string fileName)
            : base()
        {
            this.Configure(fileName);
        }
    }
}
