using System.Data;
using System.Data.Common;
using NHibernate.Driver;
using NHibernate.SqlTypes;
using Npgsql;
using NpgsqlTypes;

namespace NHibernate.Extensions.NpgSql {

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

    public class NpgSqlDialet : NHibernate.Dialect.PostgreSQL83Dialect {

        public override string GetTypeName(
            SqlType sqlType
        ) {
            if (sqlType is NpgSqlType npgSqlType) {
                var npgDbType = npgSqlType.NpgDbType;
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
}
