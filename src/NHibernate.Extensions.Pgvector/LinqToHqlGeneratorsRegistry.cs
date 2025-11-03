using NHibernate.Extensions.Pgvector.Generators;
using NHibernate.Linq.Functions;

namespace NHibernate.Extensions.Pgvector;

public class LinqToHqlGeneratorsRegistry : NHibernate.Extensions.Npgsql.LinqToHqlGeneratorsRegistry {

    public LinqToHqlGeneratorsRegistry() {
        this.Merge(new PgvectorHqlGenerator());
    }

}
