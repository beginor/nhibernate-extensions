using System;
using Microsoft.Extensions.DependencyInjection;

namespace NHibernate.Extensions.NetCore;

public static class ServiceProviderExtensions {

    extension(IServiceProvider serviceProvider) {

        public ISessionFactory GetSessionFactory() {
            return serviceProvider.GetRequiredService<ISessionFactory>();
        }

        public ISessionFactory GetSessionFactory(
            string key
        ) {
            return serviceProvider.GetRequiredKeyedService<ISessionFactory>(key);
        }

    }

}
