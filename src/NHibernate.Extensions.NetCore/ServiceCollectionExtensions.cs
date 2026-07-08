using System;
using System.Reflection;
using System.Xml;
using Microsoft.Extensions.DependencyInjection;
using NHibernate.Cfg;

namespace NHibernate.Extensions.NetCore;

public static class ServiceCollectionExtensions {

    extension(IServiceCollection services) {

        public void AddHibernate() {
            if (services == null) {
                throw new ArgumentNullException(nameof(services));
            }
            var cfg = new Configuration();
            cfg.Configure();
            services.AddHibernate(cfg, true);
        }

        public void AddHibernate(string path) {
            if (services == null) {
                throw new ArgumentNullException(nameof(services));
            }
            if (path == null) {
                throw new ArgumentNullException(nameof(path));
            }
            if (path.Length == 0) {
                throw new ArgumentException("Path cannot be empty.", nameof(path));
            }
            var cfg = new Configuration();
            cfg.Configure(path);
            services.AddHibernate(cfg, true);
        }

        public void AddHibernate(XmlReader xmlReader) {
            if (services == null) {
                throw new ArgumentNullException(nameof(services));
            }
            if (xmlReader == null) {
                throw new ArgumentNullException(nameof(xmlReader));
            }
            var cfg = new Configuration();
            cfg.Configure(xmlReader);
            services.AddHibernate(cfg, true);
        }

        public void AddHibernate(Assembly assembly, string resourceName) {
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
            services.AddHibernate(cfg, true);
        }

        public void AddHibernate(Configuration cfg, bool buildSessionFactoryOnRegister = false) {
            if (services == null) {
                throw new ArgumentNullException(nameof(services));
            }
            if (cfg == null) {
                throw new ArgumentNullException(nameof(cfg));
            }
            services.AddSingleton(cfg);
            if (buildSessionFactoryOnRegister) {
                services.AddSingleton(cfg.BuildSessionFactory());
            }
            else {
                services.AddSingleton(_ => cfg.BuildSessionFactory());
            }
            // Add ISession as scoped
            services.AddScoped(provider => {
                var factory = provider.GetService<ISessionFactory>();
                if (factory == null) {
                    throw new InvalidOperationException($"Can not get service {typeof(ISessionFactory)}");
                }
                return factory.OpenSession();
            });
        }

        public void AddHibernate(string key, Configuration cfg, bool buildSessionFactoryOnRegister = false) {
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
            if (buildSessionFactoryOnRegister) {
                services.AddKeyedSingleton(key, cfg.BuildSessionFactory());
            }
            else {
                services.AddKeyedSingleton<ISessionFactory>(
                    key,
                    (provider, k) => {
                        var config = provider.GetRequiredKeyedService<Configuration>(k);
                        return config.BuildSessionFactory();
                    }
                );
            }
            // Add ISession as scoped
            services.AddKeyedScoped<ISession>(
                key,
                (provider, o) => {
                    var sessionFactory = provider.GetRequiredKeyedService<ISessionFactory>(o);
                    return sessionFactory.OpenSession();
                }
            );
        }

        public void AddHibernate(string key, string path) {
            if (services == null) {
                throw new ArgumentNullException(nameof(services));
            }
            if (key == null) {
                throw new ArgumentNullException(nameof(key));
            }
            if (key.Length == 0) {
                throw new ArgumentException("Key cannot be empty.", nameof(key));
            }
            var cfg = new Configuration();
            cfg.Configure(path);
            services.AddHibernate(key, cfg, true);
        }

        public void AddHibernate(string key, XmlReader xmlReader) {
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
            services.AddHibernate(key, cfg, true);
        }

        public void AddHibernate(string key, Assembly assembly, string resourceName) {
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
            services.AddHibernate(key, cfg, true);
        }

        public void AddHibernate(Action<Configuration> configure) {
            var cfg = new Configuration();
            configure(cfg);
            services.AddHibernate(cfg, true);
        }

        public void AddHibernate(string key, Action<Configuration> configure) {
            var cfg = new Configuration();
            configure(cfg);
            services.AddHibernate(key, cfg, true);
        }
    }

}
