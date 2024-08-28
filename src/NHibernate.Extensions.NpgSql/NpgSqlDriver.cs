using System.Collections.Generic;
using System.Data.Common;
using NHibernate.Cfg;
using NHibernate.Driver;
using NHibernate.SqlTypes;
using Npgsql;

namespace NHibernate.Extensions.NpgSql;

public class NpgSqlDriver : NpgsqlDriver {

    private NpgsqlDataSource? dataSource;

    public override void Configure(
        IDictionary<string, string> settings
    ) {
        base.Configure(settings);
        var connectionString = settings["connection.connection_string"];
        if (string.IsNullOrEmpty(connectionString)) {
            throw new HibernateConfigException("connection.connection_string is not set!");
        }
        var builder = new NpgsqlDataSourceBuilder(connectionString);
        builder.EnableDynamicJson();
        dataSource = NpgsqlDataSource.Create(connectionString);
    }

    protected override void InitializeParameter(DbParameter dbParam, string name, SqlType sqlType) {
        if (sqlType is NpgSqlType type && dbParam is NpgsqlParameter parameter) {
            InitializeParameter(parameter, name, type);
        }
        else {
            base.InitializeParameter(dbParam, name, sqlType);
        }
    }

    protected virtual void InitializeParameter(NpgsqlParameter dbParam, string name, NpgSqlType sqlType) {
        if (sqlType == null) {
            throw new QueryException($"No type assigned to parameter '{name}'");
        }
        dbParam.ParameterName = FormatNameForParameter(name);
        dbParam.DbType = sqlType.DbType;
        dbParam.NpgsqlDbType = sqlType.NpgDbType;
    }

    public override DbConnection CreateConnection() {
        if (dataSource == null) {
            throw new HibernateException("dataSource is not created!");
        }
        return dataSource.CreateConnection();
    }

}
