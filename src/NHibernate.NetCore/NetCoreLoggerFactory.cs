using System;
using Microsoft.Extensions.Logging;
using MsILoggerFactory = Microsoft.Extensions.Logging.ILoggerFactory;

namespace NHibernate.NetCore {

    public class NetCoreLoggerFactory : IDisposable, INHibernateLoggerFactory {

        private MsILoggerFactory loggerFactory;

        public NetCoreLoggerFactory(
            MsILoggerFactory loggerFactory
        ) {
            this.loggerFactory = loggerFactory;
        }

        ~NetCoreLoggerFactory() {
            Dispose(false);
        }

        public void Dispose() {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing) {
            if (disposing) {
                loggerFactory?.Dispose();
                loggerFactory = null;
            }
        }

        public INHibernateLogger LoggerFor(string keyName) {
            var logger = loggerFactory.CreateLogger(keyName);
            return new NetCoreLogger(logger);
        }

        public INHibernateLogger LoggerFor(System.Type type) {
            var logger = loggerFactory.CreateLogger(type);
            return new NetCoreLogger(logger);
        }

    }

}
