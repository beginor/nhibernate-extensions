namespace NHibernate.Extensions.NetCore;

public static class LoggerFactoryExtensions {

    public static void UseAsHibernateLoggerFactory(
        this Microsoft.Extensions.Logging.ILoggerFactory loggerFactory
    ) {
        NHibernateLogger.SetLoggersFactory(
            new NetCoreLoggerFactory(loggerFactory)
        );
    }

}
