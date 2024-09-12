using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using NHibernate.Engine;
using NHibernate.Linq;
using NHibernate.Param;
using NHibernate.Type;

namespace NHibernate.Extensions.NpgSql;

public class QueryProvider : DefaultQueryProvider {

    public QueryProvider(ISessionImplementor session) : base(session) { }

    public QueryProvider(ISessionImplementor session, object collection) : base(session, collection) { }

    protected QueryProvider(ISessionImplementor session, object collection, NhQueryableOptions options) : base(session, collection, options) { }

    protected override NhLinqExpression PrepareQuery(
        Expression expression,
        [UnscopedRef] out IQuery query
    ) {
        var nhExpression = base.PrepareQuery(expression, out query);
        foreach (var (key, value) in nhExpression.NamedParameters) {
           var valueType = value.Value.GetType();
           if (valueType.IsArray) {
               var elementType = valueType.GetElementType();
               var arrayUserType = ArrayTypeUtil.GetArrayType(elementType!);
               nhExpression.NamedParameters[key] = new NamedParameter(key, value.Value, new CustomType(arrayUserType, null));
           }
        }
        SetParameters(query, nhExpression.NamedParameters);
        return nhExpression;
    }

}
