using System.Collections.Generic;
using System.Data.Common;
using System.Threading;
using NHibernate.SqlTypes;
using Npgsql;
using NpgsqlTypes;

namespace NHibernate.Extensions.Pgvector;

public class PgvectorDriver : NHibernate.Extensions.Npgsql.NpgsqlDriver {

    protected override NpgsqlDataSourceBuilder SetupDataSourceBuilder(
        IDictionary<string, string> settings
    ) {
        var builder = base.SetupDataSourceBuilder(settings);
        builder.UseVector();
        return builder;
    }

    public override DbConnection CreateConnection() {
        var conn = (NpgsqlConnection)base.CreateConnection();
        conn.Open();
        using var cmd = new NpgsqlCommand("CREATE EXTENSION IF NOT EXISTS vector", conn);
        cmd.ExecuteNonQuery();
        conn.ReloadTypes();
        conn.Close();
        return conn;
    }

    protected override void InitializeParameter(DbParameter dbParam, string name, SqlType sqlType) {
        if (sqlType is PgvectorSqlType vectorSqlType && dbParam is NpgsqlParameter parameter) {
            this.InitializeVectorParameter(parameter, name, vectorSqlType);
        }
        else {
            base.InitializeParameter(dbParam, name, sqlType);
        }
    }

    private void InitializeVectorParameter(NpgsqlParameter dbParam, string name, PgvectorSqlType sqlType) {
        dbParam.ParameterName = FormatNameForParameter(name);
        if (sqlType.PgvectorType == PgvectorType.Vector) {
            dbParam.DataTypeName = "vector";
        }
        else if (sqlType.PgvectorType == PgvectorType.HalfVector) {
            dbParam.DataTypeName = "halfvec";
        }
        else if (sqlType.PgvectorType == PgvectorType.SparseVector) {
            dbParam.DataTypeName = "sparsevec";
        }
    }

}
