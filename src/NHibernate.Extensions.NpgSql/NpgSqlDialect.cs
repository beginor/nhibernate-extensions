using System;
using System.Text.Json;
using NHibernate.Dialect;
using NHibernate.Dialect.Function;
using NHibernate.SqlTypes;
using NHibernate.Type;
using NpgsqlTypes;

namespace NHibernate.Extensions.NpgSql;

public class NpgSqlDialect : PostgreSQL83Dialect {

    public NpgSqlDialect() {
        RegisterFunctions();
        RegisterUserTypes();
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

    private void RegisterFunctions() {
        // array_contains(arr, 3) => :num = any(arr)
        RegisterFunction("array_contains", new SQLFunctionTemplate(NHibernateUtil.Boolean, "?2 = any(?1)"));
        // array_intersects => ?1 && ?2
        RegisterFunction("array_intersects", new SQLFunctionTemplate(NHibernateUtil.Boolean, "?1 && ?2"));
    }

    private void RegisterUserTypes() {
        TypeFactory.RegisterType(typeof(bool[]), NHibernateUtil.Custom(typeof(ArrayType<bool>)), ["bool[]"]);
        TypeFactory.RegisterType(typeof(short[]), NHibernateUtil.Custom(typeof(ArrayType<short>)), ["short[]"]);
        TypeFactory.RegisterType(typeof(int[]), NHibernateUtil.Custom(typeof(ArrayType<int>)), ["int[]"]);
        TypeFactory.RegisterType(typeof(long[]), NHibernateUtil.Custom(typeof(ArrayType<long>)), ["long[]"]);
        TypeFactory.RegisterType(typeof(float[]), NHibernateUtil.Custom(typeof(ArrayType<float>)), ["float[]"]);
        TypeFactory.RegisterType(typeof(double[]), NHibernateUtil.Custom(typeof(ArrayType<double>)), ["double[]"]);
        TypeFactory.RegisterType(typeof(decimal[]), NHibernateUtil.Custom(typeof(ArrayType<decimal>)), ["decimal[]"]);
        TypeFactory.RegisterType(typeof(string[]), NHibernateUtil.Custom(typeof(ArrayType<string>)), ["string[]"]);
        TypeFactory.RegisterType(typeof(DateTime[]), NHibernateUtil.Custom(typeof(ArrayType<DateTime>)), ["datetime[]"]);
        TypeFactory.RegisterType(typeof(DateTimeOffset[]), NHibernateUtil.Custom(typeof(ArrayType<DateTimeOffset>)), ["datetimeoffset[]"]);
        TypeFactory.RegisterType(typeof(TimeSpan[]), NHibernateUtil.Custom(typeof(ArrayType<TimeSpan>)), ["timespan[]"]);

        TypeFactory.RegisterType(typeof(JsonElement), NHibernateUtil.Custom(typeof(JsonbType)), ["jsonb"]);
    }
}
