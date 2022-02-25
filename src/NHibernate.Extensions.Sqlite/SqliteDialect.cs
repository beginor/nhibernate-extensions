using System.Data.Common;
using NHibernate.Dialect.Schema;

namespace NHibernate.Extensions.Sqlite;

public class SqliteDialect : Dialect.SQLiteDialect {

    public override IDataBaseSchema GetDataBaseSchema(DbConnection connection) {
        var schema = new SqliteDataBaseMetaData(connection, this);
        return schema;
    }

}
