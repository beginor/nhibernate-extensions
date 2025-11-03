using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using NHibernate.Hql.Ast;
using NHibernate.Linq.Functions;
using NHibernate.Linq.Visitors;
using NHibernate.Util;
using Pgvector;

namespace NHibernate.Extensions.Pgvector.Generators;

public class PgvectorHqlGenerator : BaseHqlGeneratorForMethod {

    public PgvectorHqlGenerator() {
        SupportedMethods = [
            ReflectHelper.GetMethodDefinition<Vector>(x => x.L1Distance(new Vector("[0]"))),
            ReflectHelper.GetMethodDefinition<Vector>(x => x.L2Distance(new Vector("[0]"))),
            ReflectHelper.GetMethodDefinition<Vector>(x => x.CosineDistance(new Vector("[0]"))),
        ];
    }

    public override HqlTreeNode BuildHql(
        MethodInfo method,
        Expression targetObject,
        ReadOnlyCollection<Expression> arguments,
        HqlTreeBuilder treeBuilder,
        IHqlExpressionVisitor visitor
    ) {
        var hqlMethod = "";
        var linqMethod = method.Name;
        hqlMethod = linqMethod switch {
            "L1Distance" => "l1_distance",
            "L2Distance" => "l2_distance",
            "CosineDistance" => "cosine_distance",
            _ => hqlMethod
        };
        if (string.IsNullOrEmpty(hqlMethod)) {
            throw new HibernateException($"Method {method.Name} not found");
        }
        return treeBuilder.BooleanMethodCall(
            hqlMethod,
            arguments.Select(visitor.Visit).Cast<HqlExpression>()
        );
    }

}
