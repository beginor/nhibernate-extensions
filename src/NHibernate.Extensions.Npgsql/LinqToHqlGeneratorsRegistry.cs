using NHibernate.Linq.Functions;

using NHibernate.Extensions.Npgsql.Generators;

namespace NHibernate.Extensions.Npgsql;

public class LinqToHqlGeneratorsRegistry : DefaultLinqToHqlGeneratorsRegistry {

    public LinqToHqlGeneratorsRegistry() {
        this.Merge(new ArrayHqlGenerator());
    }

}
