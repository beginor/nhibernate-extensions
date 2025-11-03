using System.Collections.Generic;
using Npgsql;

namespace NHibernate.Extensions.Pgvector;

public class PgvectorDriver : NHibernate.Extensions.Npgsql.NpgsqlDriver {

    protected override NpgsqlDataSourceBuilder SetupDataSourceBuilder(
        IDictionary<string, string> settings
    ) {
        var builder = base.SetupDataSourceBuilder(settings);
        builder.UseVector();
        return builder;
    }

}
