using System.Collections.Generic;
using System.Data.Common;
using NHibernate.Cfg;
using NHibernate.SqlTypes;
using Npgsql;

namespace NHibernate.Extensions.Npgsql;

public class NpgsqlDriver : NHibernate.Driver.NpgsqlDriver {

    protected NpgsqlDataSource DataSource { get; private set; } = null!;

    public override void Configure(
        IDictionary<string, string> settings
    ) {
        base.Configure(settings);
        DataSource = SetupDataSourceBuilder(settings).Build();
    }

    protected override void InitializeParameter(DbParameter dbParam, string name, SqlType sqlType) {
        if (sqlType is NpgsqlType type && dbParam is NpgsqlParameter parameter) {
            InitializeParameter(parameter, name, type);
        }
        else {
            base.InitializeParameter(dbParam, name, sqlType);
        }
    }

    protected virtual void InitializeParameter(NpgsqlParameter dbParam, string name, NpgsqlType sqlType) {
        if (sqlType == null) {
            throw new QueryException($"No type assigned to parameter '{name}'");
        }
        dbParam.ParameterName = FormatNameForParameter(name);
        dbParam.DbType = sqlType.DbType;
        dbParam.NpgsqlDbType = sqlType.NpgDbType;
    }

    public override DbConnection CreateConnection() {
        if (DataSource == null) {
            throw new HibernateException("dataSource is not created!");
        }
        return DataSource.CreateConnection();
    }

    protected virtual NpgsqlDataSourceBuilder SetupDataSourceBuilder(
        IDictionary<string, string> settings
    ) {
        var connectionString = settings["connection.connection_string"];
        if (string.IsNullOrEmpty(connectionString)) {
            throw new HibernateConfigException("connection.connection_string is not set!");
        }
        var builder = new NpgsqlDataSourceBuilder(connectionString);
        builder.EnableDynamicJson();
        return builder;
    }

}
