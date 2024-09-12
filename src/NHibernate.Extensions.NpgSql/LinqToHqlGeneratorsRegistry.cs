using NHibernate.Linq.Functions;

using NHibernate.Extensions.NpgSql.Generators;

namespace NHibernate.Extensions.NpgSql;

public class LinqToHqlGeneratorsRegistry : DefaultLinqToHqlGeneratorsRegistry {

    public LinqToHqlGeneratorsRegistry() {
        this.Merge(new ArrayHqlGenerator());
    }

}
