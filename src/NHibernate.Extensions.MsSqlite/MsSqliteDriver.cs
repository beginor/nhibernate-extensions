using System;
using System.Data;
using System.Data.Common;
using NHibernate.Driver;
using NHibernate.Engine;

namespace NHibernate.Extensions.MsSqlite {

    public class MsSqliteDriver : ReflectionBasedDriver {

        public MsSqliteDriver() : base(
            "Microsoft.Data.Sqlite",
            "Microsoft.Data.Sqlite",
            "Microsoft.Data.Sqlite.SqliteConnection",
            "Microsoft.Data.Sqlite.SqliteCommand"
        ) {}

        public override DbConnection CreateConnection() {
            var connection = base.CreateConnection();
            connection.StateChange += OnConnectionStateChange;
            return connection;
        }

        private static void OnConnectionStateChange(
            object sender,
            StateChangeEventArgs e
        ) {
            if ((e.OriginalState == ConnectionState.Broken || e.OriginalState == ConnectionState.Closed || e.OriginalState == ConnectionState.Connecting) &&
                e.CurrentState == ConnectionState.Open
            ) {
                var connection = (DbConnection)sender;
                using (var command = connection.CreateCommand()) {
                    // Activated foreign keys if supported by SQLite.
                    // Unknown pragmas are ignored.
                    command.CommandText = "PRAGMA foreign_keys = ON";
                    command.ExecuteNonQuery();
                }
            }
        }

        public override IResultSetsCommand GetResultSetsCommand(ISessionImplementor session) {
            return new BasicResultSetsCommand(session);
        }

        public override bool UseNamedPrefixInSql => true;

        public override bool UseNamedPrefixInParameter => true;

        public override string NamedPrefix => "@";

        public override bool SupportsMultipleOpenReaders => false;

        public override bool SupportsMultipleQueries => true;

        public override bool SupportsNullEnlistment => false;

        public override bool HasDelayedDistributedTransactionCompletion => true;

    }

}
