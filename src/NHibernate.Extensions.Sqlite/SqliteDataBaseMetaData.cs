using System.Data.Common;
using NHibernate.Dialect.Schema;

namespace NHibernate.Extensions.Sqlite;

public class SqliteDataBaseMetaData(
    DbConnection connection,
    NHibernate.Dialect.Dialect dialect
) : SQLiteDataBaseMetaData(connection, dialect) {

    public SqliteDataBaseMetaData(
        DbConnection connection
    ) : this(connection, new SqliteDialect()) { }

    public override bool IncludeDataTypesInReservedWords => false;

}
