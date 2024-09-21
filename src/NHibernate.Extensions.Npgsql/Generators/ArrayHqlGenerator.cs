using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using NHibernate.Hql.Ast;
using NHibernate.Linq.Functions;
using NHibernate.Linq.Visitors;
using NHibernate.Util;

namespace NHibernate.Extensions.Npgsql.Generators;

public class ArrayHqlGenerator : BaseHqlGeneratorForMethod {

    public ArrayHqlGenerator() {
        SupportedMethods = [
            ReflectHelper.GetMethodDefinition<int[]>(x => x.ArrayContains(0)),
            ReflectHelper.GetMethodDefinition<int[]>(x => x.ArrayIntersects(Array.Empty<int>())),
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
            "ArrayContains" => "array_contains",
            "ArrayIntersects" => "array_intersects",
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
