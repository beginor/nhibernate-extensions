using System;
using Microsoft.Extensions.DependencyInjection;

namespace NHibernate.NetCore {

    public static class ServiceProviderExtensions {

        public static ISessionFactory ResolveSessionFactory(
            this IServiceProvider serviceProvider
        ) {
            var cfg = serviceProvider.GetService<IConfigurationProvider>();
            return cfg.GetSessionFactory();
        }

        public static ISessionFactory ResolveSessionFactory(
            this IServiceProvider serviceProvider,
            string key
        ) {
            var cfg = serviceProvider.GetService<IConfigurationProvider>();
            return cfg.GetSessionFactory(key);
        }
    }
}
