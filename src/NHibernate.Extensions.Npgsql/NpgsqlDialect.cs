using System;
using System.Text.Json;
using NHibernate.Dialect.Function;
using NHibernate.SqlTypes;
using NHibernate.Type;
using NpgsqlTypes;
using NHibernate.Extensions.Npgsql.UserTypes;

namespace NHibernate.Extensions.Npgsql;

public class NpgsqlDialect : NHibernate.Dialect.PostgreSQL83Dialect {

    public NpgsqlDialect() {
        RegisterFunctions();
        RegisterUserTypes();
    }

    public override string GetTypeName(
        SqlType sqlType
    ) {
        if (sqlType is NpgsqlType npgSqlType) {
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
                string type = arrayDbType switch {
                    NpgsqlDbType.Boolean => "boolean[]",
                    NpgsqlDbType.Smallint => "smallint[]",
                    NpgsqlDbType.Integer => "integer[]",
                    NpgsqlDbType.Bigint => "bigint[]",
                    NpgsqlDbType.Real => "real[]",
                    NpgsqlDbType.Double => "double precision[]",
                    NpgsqlDbType.Numeric => "numeric[]",
                    NpgsqlDbType.Varchar => "character varying[]",
                    NpgsqlDbType.Timestamp => "timestamp[]",
                    NpgsqlDbType.TimestampTz => "timestamptz[]",
                    _ => ""
                };
                return type;
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
        TypeFactory.RegisterType(typeof(DateTime[]), NHibernateUtil.Custom(typeof(ArrayType<DateTime>)), ["datetime[]", "timestamp[]"]);
        TypeFactory.RegisterType(typeof(DateTimeOffset[]), NHibernateUtil.Custom(typeof(ArrayType<DateTimeOffset>)), ["datetimeoffset[]", "timestamptz[]"]);

        TypeFactory.RegisterType(typeof(JsonElement), NHibernateUtil.Custom(typeof(JsonbType)), ["jsonb"]);
        TypeFactory.RegisterType(typeof(JsonElement), NHibernateUtil.Custom(typeof(JsonType)), ["json"]);
        TypeFactory.RegisterType(typeof(DateTime), NHibernateUtil.Custom(typeof(TimeStampType)), ["timestamp"]);
        TypeFactory.RegisterType(typeof(DateTimeOffset), NHibernateUtil.Custom(typeof(TimeStampTzType)), ["timestamptz"]);
    }
}
