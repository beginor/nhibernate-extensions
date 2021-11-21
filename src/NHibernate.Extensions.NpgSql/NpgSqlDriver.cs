using System.Data;
using System.Data.Common;
using NHibernate.Driver;
using NHibernate.SqlTypes;
using Npgsql;

namespace NHibernate.Extensions.NpgSql;

public class NpgSqlDriver : NpgsqlDriver {

    protected override void InitializeParameter(
        DbParameter dbParam,
        string name,
        SqlType sqlType
    ) {
        if (sqlType is NpgSqlType type && dbParam is NpgsqlParameter parameter) {
            this.InitializeParameter(
                parameter,
                name,
                type
            );
        }
        else {
            base.InitializeParameter(dbParam, name, sqlType);
        }
    }

    protected virtual void InitializeParameter(
        NpgsqlParameter dbParam,
        string name,
        NpgSqlType sqlType
    ) {
        if (sqlType == null) {
            throw new QueryException(
                $"No type assigned to parameter '{name}'"
            );
        }
        dbParam.ParameterName = FormatNameForParameter(name);
        dbParam.DbType = sqlType.DbType;
        dbParam.NpgsqlDbType = sqlType.NpgDbType;
    }
}
