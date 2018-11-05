using System;
using Microsoft.Extensions.DependencyInjection;
using NHibernate.Cfg;
using System.Xml;
using System.Reflection;

namespace NHibernate.NetCore {

    public static class ServiceCollectionExtensions {

        private static IConfigurationProvider configProvider
            = new ConfigurationProvider();

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
            AddConfigurationProvider(services);
            configProvider.SetConfiguration(cfg);
            // Add Configuration as singleton
            services.AddSingleton(provider => {
                var cfgProvider = provider.GetService<IConfigurationProvider>();
                return cfgProvider.GetConfiguration();
            });
            // Add ISessionFactory as singleton
            services.AddSingleton(provider => {
                var config = provider.GetService<Configuration>();
                return config.BuildSessionFactory();
            });
            // Add ISession as scoped
            services.AddScoped(provider => {
                var factory = provider.GetService<ISessionFactory>();
                return factory.OpenSession();
            });
        }

        private static void AddConfigurationProvider(
            this IServiceCollection services
        ) {
            var descriptor = new ServiceDescriptor(
                serviceType: typeof(IConfigurationProvider),
                instance: configProvider
            );
            // descriptor.Lifetime = ServiceLifetime.Singleton;
            if (!services.Contains(descriptor)) {
                services.Add(descriptor);
            }
        }

        public static void AddHibernate(
            this IServiceCollection services,
            string key,
            Configuration cfg
        ) {
            if (services == null) {
                throw new ArgumentNullException(nameof(services));
            }
            if (cfg == null) {
                throw new ArgumentNullException(nameof(cfg));
            }
            AddConfigurationProvider(services);
            configProvider.SetConfiguration(key, cfg);
        }

        public static void AddHibernate(
            this IServiceCollection services,
            string key,
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
}
