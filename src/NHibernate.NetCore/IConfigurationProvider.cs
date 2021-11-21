using NHibernate.Cfg;

namespace NHibernate.NetCore;

public interface IConfigurationProvider {

    ISessionFactory GetSessionFactory();

    ISessionFactory GetSessionFactory(string key);

    void SetSessionFactory(ISessionFactory sessionFactory);

    void SetSessionFactory(string key, ISessionFactory sessionFactory);

    Configuration GetConfiguration();

    Configuration GetConfiguration(string key);

    void SetConfiguration(Configuration configuration);

    void SetConfiguration(string key, Configuration configuration);

}
