using NHibernate.Driver;
using NHibernate.Engine;

namespace NHibernate.Extensions.MsSqlite {

    /// <summary>
    /// NHibernate driver for the Microsoft.Data.Sqlite.Core data provider for .NET.
    /// </summary>
    /// <remarks>
    /// In order to use this driver, you must have the nuget package <a href="https://www.nuget.org/packages/Microsoft.Data.Sqlite.Core/">Microsoft.Data.Sqlite.Core</a>
    /// and <a href="https://www.nuget.org/packages/SQLitePCLRaw.bundle_e_sqlite3/">SQLitePCLRaw.bundle_e_sqlite3</a> installed for NHibernate to load.
    /// ```
    /// </remarks>
    public class MsSqliteDriver : ReflectionBasedDriver {

        public MsSqliteDriver() : base(
            "Microsoft.Data.Sqlite.Core",
            "Microsoft.Data.Sqlite",
            "Microsoft.Data.Sqlite.SqliteConnection",
            "Microsoft.Data.Sqlite.SqliteCommand"
        ) { }

        public override IResultSetsCommand GetResultSetsCommand(ISessionImplementor session) {
            NHibernate.Driver.SQLite20Driver d;
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
