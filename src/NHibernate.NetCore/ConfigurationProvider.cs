using System.Collections.Concurrent;
using NHibernate.Cfg;

namespace NHibernate.NetCore;

public class ConfigurationProvider : IConfigurationProvider {

    private ConcurrentDictionary<string, ISessionFactory> sessionFactories
        = new ConcurrentDictionary<string, ISessionFactory>();
    private ConcurrentDictionary<string, Configuration> configurations
        = new ConcurrentDictionary<string, Configuration>();

    private static readonly string DefaultConfigurationKey
        = typeof(Configuration).FullName;
    private static readonly string DefaultSessionFactoryKey
        = typeof(ISessionFactory).FullName;

    public ISessionFactory GetSessionFactory() {
        return sessionFactories.GetOrAdd(
            DefaultSessionFactoryKey,
            key => {
                var cfg = configurations[DefaultConfigurationKey];
                if (cfg == null) {
                    throw new System.InvalidOperationException(
                        "Default configuration does not exists!"
                    );
                }
                var sessionFactory = cfg.BuildSessionFactory();
                return sessionFactory;
            }
        );
    }

    public ISessionFactory GetSessionFactory(string key) {
        return sessionFactories.GetOrAdd(
            key,
            k => {
                var cfg = configurations[key];
                if (cfg == null) {
                    throw new System.InvalidOperationException(
                        $"Configuration with {key} does not exists!"
                    );
                }
                var sessionFactory = cfg.BuildSessionFactory();
                return sessionFactory;
            }
        );
    }

    public void SetSessionFactory(ISessionFactory sessionFactory) {
        sessionFactories[DefaultSessionFactoryKey] = sessionFactory;
    }

    public void SetSessionFactory(string key, ISessionFactory sessionFactory) {
        sessionFactories[key] = sessionFactory;
    }

    public Configuration GetConfiguration() {
        return configurations[DefaultConfigurationKey];
    }

    public Configuration GetConfiguration(string key) {
        return configurations[key];
    }

    public void SetConfiguration(Configuration configuration) {
        configurations[DefaultConfigurationKey] = configuration;
    }

    public void SetConfiguration(string key, Configuration configuration) {
        configurations[key] = configuration;
    }
}
