﻿using System;
using Microsoft.Extensions.Logging;

namespace NHibernate.Extensions.NetCore;

public class NetCoreLogger(ILogger logger) : IDisposable, INHibernateLogger {

    ~NetCoreLogger() {
        Dispose(false);
    }

    public void Dispose() {
        Dispose(true);
    }

    protected virtual void Dispose(bool disposing) {
        // if (disposing) {
        //     logger = null;
        // }
    }

    public bool IsEnabled(NHibernateLogLevel logLevel) {
        var level = (int)logLevel;
        var msLogLevel = (LogLevel)level;
        return logger.IsEnabled(msLogLevel);
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
            (s, ex) => {
                var message = s.ToString();
                if (ex != null) {
                    message += ex.ToString();
                }
                return message;
            }
        );
    }
}
