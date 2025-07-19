using System;
using Microsoft.Extensions.DependencyInjection;

namespace NHibernate.Extensions.NetCore;

public static class ServiceProviderExtensions {

    public static ISessionFactory GetSessionFactory(
        this IServiceProvider serviceProvider
    ) {
        return serviceProvider.GetRequiredService<ISessionFactory>();
    }

    public static ISessionFactory GetSessionFactory(
        this IServiceProvider serviceProvider,
        string key
    ) {
        return serviceProvider.GetRequiredKeyedService<ISessionFactory>(key);
    }
}
