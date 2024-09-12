using NHibernate.Dialect;
using NHibernate.Dialect.Function;
using NHibernate.SqlTypes;
using NpgsqlTypes;

namespace NHibernate.Extensions.NpgSql;

public class NpgSqlDialect : PostgreSQL83Dialect {

    public NpgSqlDialect() {
        // array_contains(arr, 3) => :num = any(arr)
        RegisterFunction("array_contains", new SQLFunctionTemplate(NHibernateUtil.Boolean, "?2 = any(?1)"));
        RegisterFunction("array_intersects", new SQLFunctionTemplate(NHibernateUtil.Boolean, "?1 && ?2"));
    }

    public override string GetTypeName(
        SqlType sqlType
    ) {
        if (sqlType is NpgSqlType npgSqlType) {
            var npgDbType = npgSqlType.NpgDbType;
            if (npgDbType == NpgsqlDbType.Numeric) {
                return "numeric";
            }
            if (npgDbType == NpgsqlDbType.Json) {
                return "json";
            }
            if (npgDbType == NpgsqlDbType.Jsonb) {
                return "jsonb";
            }
            // array type
            if (npgDbType < 0) {
                var arrayDbType = (NpgsqlDbType) (npgDbType - NpgsqlDbType.Array);
                if (arrayDbType == NpgsqlDbType.Boolean) {
                    return "boolean[]";
                }
                if (arrayDbType == NpgsqlDbType.Double) {
                    return "double precision[]";
                }
                if (arrayDbType == NpgsqlDbType.Real) {
                    return "real[]";
                }
                if (arrayDbType == NpgsqlDbType.Smallint) {
                    return "smallint[]";
                }
                if (arrayDbType == NpgsqlDbType.Integer) {
                    return "integer[]";
                }
                if (arrayDbType == NpgsqlDbType.Bigint) {
                    return "bigint[]";
                }
                if (arrayDbType == NpgsqlDbType.Varchar) {
                    return "character varying[]";
                }
            }
        }
        return base.GetTypeName(sqlType);
    }

}
