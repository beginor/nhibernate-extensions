using System;
using Microsoft.Extensions.Logging;

namespace NHibernate.Extensions.NetCore;

public class NetCoreLogger(ILogger logger) : INHibernateLogger {

    public bool IsEnabled(NHibernateLogLevel logLevel) {
        return logger.IsEnabled(MapLogLevel(logLevel));
    }

    private static LogLevel MapLogLevel(NHibernateLogLevel logLevel) {
      return logLevel switch {
          NHibernateLogLevel.Trace => LogLevel.Trace,
          NHibernateLogLevel.Debug => LogLevel.Debug,
          NHibernateLogLevel.Info => LogLevel.Information,
          NHibernateLogLevel.Warn => LogLevel.Warning,
          NHibernateLogLevel.Error => LogLevel.Error,
          NHibernateLogLevel.Fatal => LogLevel.Critical,
          _ => LogLevel.None
      };
  }

    public void Log(
        NHibernateLogLevel logLevel,
        NHibernateLogValues state,
        Exception exception
    ) {
        var level = (int)logLevel;
        var msLogLevel = (LogLevel)level;
        logger.Log(
            msLogLevel,
            default(EventId),
            state,
            exception,
            (s, _) => s.ToString()
        );
    }
}
