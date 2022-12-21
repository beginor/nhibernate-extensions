using System;
using Microsoft.Extensions.DependencyInjection;

namespace NHibernate.NetCore;

public static class ServiceProviderExtensions {

    public static ISessionFactory GetSessionFactory(
        this IServiceProvider serviceProvider
    ) {
        var cfg = serviceProvider.GetService<IConfigurationProvider>();
        if (cfg == null) {
            throw new InvalidOperationException($"Can not get service {typeof(IConfigurationProvider)} !");
        }
        return cfg.GetSessionFactory();
    }

    public static ISessionFactory GetSessionFactory(
        this IServiceProvider serviceProvider,
        string key
    ) {
        var cfg = serviceProvider.GetService<IConfigurationProvider>();
        if (cfg == null) {
            throw new InvalidOperationException($"Can not get service {typeof(IConfigurationProvider)} !");
        }
        return cfg.GetSessionFactory(key);
    }
}
