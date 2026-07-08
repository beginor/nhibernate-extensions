using Microsoft.Extensions.Logging;

namespace NHibernate.Extensions.NetCore;

public class NetCoreLoggerFactory(
    Microsoft.Extensions.Logging.ILoggerFactory loggerFactory
) : INHibernateLoggerFactory {

    public INHibernateLogger LoggerFor(string keyName) {
        return new NetCoreLogger(loggerFactory.CreateLogger(keyName));
    }

    public INHibernateLogger LoggerFor(System.Type type) {
        return new NetCoreLogger(loggerFactory.CreateLogger(type));
    }

}
