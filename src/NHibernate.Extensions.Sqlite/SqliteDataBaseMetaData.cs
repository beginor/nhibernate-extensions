using System.Data.Common;
using NHibernate.Dialect.Schema;

namespace NHibernate.Extensions.Sqlite;

public class SqliteDataBaseMetaData : SQLiteDataBaseMetaData {

    public SqliteDataBaseMetaData(
        DbConnection connection
    ) : this(connection, new SqliteDialect()) { }

    public SqliteDataBaseMetaData(
        DbConnection connection,
        NHibernate.Dialect.Dialect dialect
    ) : base(connection, dialect) { }

    public override bool IncludeDataTypesInReservedWords => false;

}
