using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using NHibernate.Hql.Ast;
using NHibernate.Linq.Functions;
using NHibernate.Linq.Visitors;
using NHibernate.Util;

namespace NHibernate.Extensions.NpgSql.Generators;

public class ArrayContainsGenerator : BaseHqlGeneratorForMethod {

    public ArrayContainsGenerator() {
        SupportedMethods = [
            ReflectHelper.GetMethodDefinition<int[]>(x => x.ArrayContains(0))
        ];
    }

    public override HqlTreeNode BuildHql(
        MethodInfo method,
        Expression targetObject,
        ReadOnlyCollection<Expression> arguments,
        HqlTreeBuilder treeBuilder,
        IHqlExpressionVisitor visitor
    ) {
        return treeBuilder.BooleanMethodCall(
            "array_contains",
            arguments.Select(arg => visitor.Visit(arg)).Cast<HqlExpression>()
        );
    }

}
