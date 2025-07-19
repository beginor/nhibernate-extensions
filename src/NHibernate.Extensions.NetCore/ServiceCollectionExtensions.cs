using System;
using System.Reflection;
using System.Xml;
using Microsoft.Extensions.DependencyInjection;
using NHibernate.Cfg;

namespace NHibernate.Extensions.NetCore;

public static class ServiceCollectionExtensions {

    public static void AddHibernate(
        this IServiceCollection services
    ) {
        if (services == null) {
            throw new ArgumentNullException(nameof(services));
        }
        var cfg = new Configuration();
        cfg.Configure();
        AddHibernate(services, cfg);
    }

    public static void AddHibernate(
        this IServiceCollection services,
        string path
    ) {
        if (services == null) {
            throw new ArgumentNullException(nameof(services));
        }
        if (string.IsNullOrEmpty(path)) {
            throw new ArgumentNullException(nameof(path));
        }
        var cfg = new Configuration();
        cfg.Configure(path);
        AddHibernate(services, cfg);
    }

    public static void AddHibernate(
        this IServiceCollection services,
        XmlReader xmlReader
    ) {
        if (services == null) {
            throw new ArgumentNullException(nameof(services));
        }
        if (xmlReader == null) {
            throw new ArgumentNullException(nameof(xmlReader));
        }
        var cfg = new Configuration();
        cfg.Configure(xmlReader);
        AddHibernate(services, cfg);
    }

    public static void AddHibernate(
        this IServiceCollection services,
        Assembly assembly,
        string resourceName
    ) {
        if (services == null) {
            throw new ArgumentNullException(nameof(services));
        }
        if (assembly == null) {
            throw new ArgumentNullException(nameof(assembly));
        }
        if (resourceName == null) {
            throw new ArgumentNullException(nameof(resourceName));
        }
        var cfg = new Configuration();
        cfg.Configure(assembly, resourceName);
        AddHibernate(services, cfg);
    }

    public static void AddHibernate(
        this IServiceCollection services,
        Configuration cfg
    ) {
        if (services == null) {
            throw new ArgumentNullException(nameof(services));
        }
        if (cfg == null) {
            throw new ArgumentNullException(nameof(cfg));
        }
        // Add Configuration as singleton
        services.AddSingleton(cfg);
        // Add ISessionFactory as singleton
        services.AddSingleton(provider => {
            var config = provider.GetService<Configuration>();
            if (config == null) {
                throw new InvalidOperationException($"Can not get service {typeof(Configuration)}");
            }
            return config.BuildSessionFactory();
        });
        // Add ISession as scoped
        services.AddScoped(provider => {
            var factory = provider.GetService<ISessionFactory>();
            if (factory == null) {
                throw new InvalidOperationException($"Can not get service {typeof(ISessionFactory)}");
            }
            return factory.OpenSession();
        });
    }

    public static void AddHibernate(
        this IServiceCollection services,
        string key,
        Configuration cfg
    ) {
        if (services == null) {
            throw new ArgumentNullException(nameof(services));
        }
        if (key == null) {
            throw new ArgumentNullException(nameof(key));
        }
        if (cfg == null) {
            throw new ArgumentNullException(nameof(cfg));
        }
        services.AddKeyedSingleton(key, cfg);
        // Add ISessionFactory as singleton
        services.AddKeyedSingleton<ISessionFactory>(
            key,
            (provider, o) => {
                var config = provider.GetRequiredKeyedService<Configuration>(o);
                return config.BuildSessionFactory();
            }
        );
        // Add ISession as scoped
        services.AddKeyedScoped<ISession>(
            key,
            (provider, o) => {
                var sessionFactory = provider.GetRequiredKeyedService<ISessionFactory>(o);
                return sessionFactory.OpenSession();
            }
        );
    }

    public static void AddHibernate(
        this IServiceCollection services,
        string key,
        string path
    ) {
        if (services == null) {
            throw new ArgumentNullException(nameof(services));
        }
        if (key == null) {
            throw new ArgumentNullException(nameof(key));
        }
        if (string.IsNullOrEmpty(path)) {
            throw new ArgumentNullException(nameof(path));
        }
        var cfg = new Configuration();
        cfg.Configure(path);
        AddHibernate(services, key, cfg);
    }

    public static void AddHibernate(
        this IServiceCollection services,
        string key,
        XmlReader xmlReader
    ) {
        if (services == null) {
            throw new ArgumentNullException(nameof(services));
        }
        if (key == null) {
            throw new ArgumentNullException(nameof(key));
        }
        if (xmlReader == null) {
            throw new ArgumentNullException(nameof(xmlReader));
        }
        var cfg = new Configuration();
        cfg.Configure(xmlReader);
        AddHibernate(services, key, cfg);
    }

    public static void AddHibernate(
        this IServiceCollection services,
        string key,
        Assembly assembly,
        string resourceName
    ) {
        if (services == null) {
            throw new ArgumentNullException(nameof(services));
        }
        if (key == null) {
            throw new ArgumentNullException(nameof(key));
        }
        if (assembly == null) {
            throw new ArgumentNullException(nameof(assembly));
        }
        if (resourceName == null) {
            throw new ArgumentNullException(nameof(resourceName));
        }
        var cfg = new Configuration();
        cfg.Configure(assembly, resourceName);
        AddHibernate(services, key, cfg);
    }

}
