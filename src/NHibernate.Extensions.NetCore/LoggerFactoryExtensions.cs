using System;

namespace NHibernate.Extensions.NetCore {

    public static class LoggerFactoryExtensions {

        public static void UseAsNHibernateLoggerFactory(
            this Microsoft.Extensions.Logging.ILoggerFactory loggerFactory
        ) {
            NHibernateLogger.SetLoggersFactory(
                new NetCoreLoggerFactory(loggerFactory)
            );
        }

    }

}
